SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================
-- Author:		Javier Elías
-- Create date: 25/11/2021
-- Modified date: 
-- Description:	Procesador para afecta el inventario
--						por un Recibo de Cortesía
-- ============================================
CREATE OR ALTER PROCEDURE [dbo].[ARspAfectaReciboCortesia]
	@CortesiaId INT,
	@UsuarioId INT
AS
BEGIN
		SET NOCOUNT ON;

		DECLARE @tbl TABLE (
			AlmacenProductoId INT,
			Producto VARCHAR(250),
			CuentaPresupuestalId INT,
			CantidadMovimiento DECIMAL(28, 10),
			CostoUnitario MONEY,
			MotivoMovto VARCHAR(MAX),
			DetalleId INT,
			Orden INT
		)

		--Buscamos los datos del producto a afectar
		INSERT INTO @tbl
		SELECT ap.AlmacenProductoId,
				CONVERT(VARCHAR(10), detalle.ProductoId) + ' - ' + detalle.Descripcion,
				detalle.CuentaPresupuestalEgrId,
				detalle.Cantidad AS CantidadMovimiento,
				detalle.PrecioUnitario,
				'Recibo de Cortesía: ' + cortesia.Codigo AS MotivoMovto,
				detalle.CortesiaDetalleId,
				ROW_NUMBER() OVER (ORDER BY CortesiaDetalleId) AS Orden
		FROM ARtblCortesia AS cortesia
				INNER JOIN ARtblCortesiaDetalle AS detalle ON cortesia.CortesiaId = detalle.CortesiaId
				INNER JOIN ARtblAlmacenProducto AS ap ON cortesia.AlmacenId = ap.AlmacenId AND detalle.ProductoId = ap.ProductoId AND detalle.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId
		WHERE cortesia.CortesiaId = @CortesiaId

		DECLARE @contador INT = 1
		DECLARE @registros INT = ( SELECT COUNT(AlmacenProductoId) FROM @tbl )

		DECLARE @AlmacenProductoId INT
		DECLARE @Producto VARCHAR(250)
		DECLARE @CuentaPresupuestalId INT
		DECLARE @CantidadMovimiento DECIMAL(28, 10)
		DECLARE @CostoUnitario MONEY
		DECLARE @TipoMovimientoId INT = 107 -- Recibo de Cortesía
		DECLARE @MotivoMovto VARCHAR(MAX)
		DECLARE @ReferenciaDetalleId INT

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = 'No es posible afectar el inventario. '

		DECLARE @51000 VARCHAR(MAX) = @mensaje + 'El conteo del artículo [@Producto] con cuenta presupuestal [@CuentaPresupuestalId] no puede estar vacío.'
		DECLARE @51001 VARCHAR(MAX) = @mensaje + 'No existen movimientos por registrar.'

		BEGIN TRY
		--BEGIN TRANSACTION
				--Construimos los movimientos para el procesador
				DECLARE @Movimientos ARudtInventarioMovimiento
				
				WHILE ( @contador <= @registros )
				BEGIN
						SELECT @AlmacenProductoId = AlmacenProductoId,
							   @Producto = Producto,
							   @CuentaPresupuestalId = CuentaPresupuestalId,
							   @CantidadMovimiento = CantidadMovimiento,
							   @CostoUnitario = CostoUnitario,
							   @MotivoMovto = MotivoMovto,
							   @ReferenciaDetalleId = DetalleId
						FROM @tbl
						WHERE Orden = @contador

						--Validamos que se haya llenado la columna de conteo
						IF ( @CantidadMovimiento IS NULL )
						BEGIN 
							SET @mensaje = (REPLACE(REPLACE(@51000, '@Producto', @Producto), '@CuentaPresupuestalId', @CuentaPresupuestalId));
							THROW 51000, @mensaje, 1;
						END

						--Verificamos que la cantidad de movimiento sea diferente de 0
						IF ( @CantidadMovimiento != 0 )
						BEGIN
							--Si pasó las validaciones agregamos el movimiento
							INSERT INTO @Movimientos
							SELECT @AlmacenProductoId,
										 @CantidadMovimiento,
										 @CostoUnitario,
										 @ReferenciaDetalleId
						END

						SET  @contador = @contador + 1
				END

				--Validamos que se existan movimientos por registrar
				DECLARE @contadorMovimientos INT = ( SELECT COUNT(AlmacenProductoId) FROM @Movimientos )

				IF ( @contadorMovimientos = 0 )
						THROW 51001, @mensaje, 1;

				--Si pasó las validaciones mandamos llamar al Procesador de Inventarios
				EXEC ARspProcesadorInventarios
								@TipoMovimientoId,
								@MotivoMovto,
								@UsuarioId,
								1, -- Insertar agrupador
								@CortesiaId,
								NULL, -- Poliza Id
								@Movimientos
		--COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			--ROLLBACK TRANSACTION;
			THROW;
		END CATCH
END
GO