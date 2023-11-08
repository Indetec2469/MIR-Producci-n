SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Javier Elías
-- Create date: 16/02/2021
-- Modified date: 27/09/2021
-- Description:	Procesador de Inventarios
-- =============================================|
CREATE OR ALTER PROCEDURE [dbo].[ARspProcesadorInventarios]
	@TipoMovimientoId INT,
	@MotivoMovto VARCHAR(MAX),	
	@CreadoPorId INT,
	@InsertarAgrupador BIT,
	@ReferenciaMovtoId INT,
	@PolizaId INT,
	@Movimientos ARudtInventarioMovimiento READONLY
AS
BEGIN
		SET NOCOUNT ON;

		DECLARE @InventarioMovtoAgrupadorId INT = NULL

		--Si se va a usar un agrupador se inserta el registro en la tabla y se obtiene el Id
		IF ( @InsertarAgrupador = 1 AND @ReferenciaMovtoId != -1 )
		BEGIN

				DECLARE @ejercicio INT = ( SELECT YEAR(GETDATE()) )
				DECLARE @autonumerico VARCHAR(20)
				
				--Obtenemos el Autonumerico del Agrupador
				EXECUTE GRspAutonumericoSigIncr 'Inventario Movimiento Agrupador', @ejercicio, @autonumerico OUTPUT
				
				DECLARE @tmp TABLE(id INT)

				--Insertamos el registro Agrupador
				INSERT INTO ARtblInventarioMovimientoAgrupador
				(
					--InventarioMovtoAgrupadorId - this column value is auto-generated
					CodigoAgrupador,
					TipoMovimientoId,
					ReferenciaMovtoId,
					PolizaId,
					FechaCreacion,
					CreadoPorId
				)
				OUTPUT INSERTED.InventarioMovtoAgrupadorId INTO @tmp
				VALUES
				(
					-- InventarioMovtoAgrupadorId - int
					@autonumerico, -- CodigoAgrupador - varchar
					@TipoMovimientoId, -- TipoMovimientoId - int
					@ReferenciaMovtoId, -- ReferenciaMovtoId - int
					@PolizaId, -- PolizaId - int
					GETDATE(), -- FechaCreacion - datetime
					@CreadoPorId -- CreadoPorId - int
				)

				--Obtenemos el Id del Agrupador insertado
				SELECT @InventarioMovtoAgrupadorId = id FROM @tmp
		END
		
		--Tabla temporal para recorrer los Movimientos
		DECLARE @movimientosTemp TABLE ( AlmacenProductoId INT, CantidadMovimiento DECIMAL(28, 10), CostoUnitario MONEY, ReferenciaMovtoId INT, Contador INT )

		INSERT INTO @movimientosTemp
		SELECT AlmacenProductoId, CantidadMovimiento, CostoUnitario, ReferenciaMovtoId, ROW_NUMBER() OVER (ORDER BY AlmacenProductoId) FROM @Movimientos

		DECLARE @contadorMovimientos INT = ( SELECT COUNT(AlmacenProductoId) FROM @movimientosTemp )
		DECLARE @contador INT = 1

		--Recorremos la tabla temporal
		WHILE ( @contador <= @contadorMovimientos )
		BEGIN				
				DECLARE @AlmacenProductoId INT
				DECLARE @CantidadMovimiento DECIMAL(28, 10)
				DECLARE @CostoUnitario MONEY
				DECLARE @ReferenciaMovtoIdDetalle INT

				--Obtenemos los datos del movimiento
				SELECT @AlmacenProductoId = AlmacenProductoId, @CantidadMovimiento = CantidadMovimiento, @CostoUnitario = CostoUnitario, @ReferenciaMovtoIdDetalle = ReferenciaMovtoId FROM @movimientosTemp WHERE Contador = @contador

				DECLARE @Producto VARCHAR(250)
				DECLARE @CuentaPresupuestalId INT
				DECLARE @UnidadMedidaId INT
				DECLARE @TipoCostoArticuloId INT
				DECLARE @CostoPromedio MONEY
				DECLARE @CantidadAntesMovto DECIMAL(28, 10)

				--THROWS
				DECLARE @mensaje VARCHAR(MAX) = 'No es posible procesar el movimiento. '

				DECLARE @51000 VARCHAR(MAX) = @mensaje + 'El artículo [@Producto] con cuenta presupuestal [@CuentaPresupuestalId] no puede tener existencia negativa.'

				--Buscamos los datos del producto a afectar
				SELECT @Producto = producto.Descripcion,
						@CuentaPresupuestalId = CuentaPresupuestalId,
						@UnidadMedidaId = UnidadDeMedidaId,
						@TipoCostoArticuloId = 4, ----------------------------------------------------- ES TEMPORAL, SE DBE CAMBIAR --------------------------------------------------------------------------
						@CostoPromedio = CostoPromedio,
						@CantidadAntesMovto = Cantidad
				FROM ARtblAlmacenProducto AS almacen
						INNER JOIN tblProducto AS producto ON almacen.ProductoId = producto.ProductoId
				WHERE almacen.AlmacenProductoId = @AlmacenProductoId

				--Validamos que el inventario no pueda ajustar a existencia negativa
				IF ( (@CantidadAntesMovto + @CantidadMovimiento) < 0 )
				BEGIN 
					SET @mensaje = (REPLACE(REPLACE(@51000, '@Producto', @Producto), '@CuentaPresupuestalId', @CuentaPresupuestalId));
					THROW 51002, @mensaje, 1;
				END

				--Insertamos el Movimiento
				INSERT INTO ARtblInventarioMovimiento
				(
					--InventarioMovtoId - this column value is auto-generated
					InventarioMovtoAgrupadorId,
					AlmacenProductoId,
					UnidadMedidaId,
					CantidadMovimiento,
					TipoMovimientoId,
					CantidadAntesMovto,
					TipoCostoArticuloId,
					CostoUnitario,
					CostoPromedio,
					MotivoMovto,
					ReferenciaMovtoId,
					FechaCreacion,
					CreadoPorId
				)
				VALUES
				(
					@InventarioMovtoAgrupadorId,
					@AlmacenProductoId,
					@UnidadMedidaId,
					@CantidadMovimiento,
					@TipoMovimientoId,
					@CantidadAntesMovto,
					@TipoCostoArticuloId,
					ISNULL(@CostoUnitario, @CostoPromedio),
					@CostoPromedio,
					ISNULL(@MotivoMovto, ''),
					@ReferenciaMovtoIdDetalle,
					GETDATE(),
					@CreadoPorId
				)

				--Actualizamos la cantidad en la tabla de tblAlmacenProducto
				UPDATE ARtblAlmacenProducto SET Cantidad = @CantidadAntesMovto + @CantidadMovimiento WHERE AlmacenProductoId = @AlmacenProductoId

				--Aumentamos el contador para recorrer
				SET @contador = @contador + 1
		END
END
GO