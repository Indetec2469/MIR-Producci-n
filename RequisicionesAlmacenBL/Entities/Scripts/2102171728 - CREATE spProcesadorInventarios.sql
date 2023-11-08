DROP PROCEDURE IF EXISTS spProcesadorInventarios
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Javier Elías
-- Create date: 16/02/2021
-- Description:	Procesador de Inventarios
-- =============================================
CREATE PROCEDURE [dbo].[spProcesadorInventarios] 
	@AlmacenProductoId INT,
	@CantidadMovimiento DECIMAL(28, 10),
	@TipoMovimientoId INT,
	@MotivoMovto VARCHAR(MAX),
	@ReferenciaMovtoId INT,
	@CreadoPorId INT
AS
BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		
		DECLARE @Producto VARCHAR(250)
		DECLARE @CuentaPresupuestalId INT
		DECLARE @UnidadMedidaId INT
		DECLARE @TipoCostoArticuloId INT
		DECLARE @ValorContableAntesMovto MONEY
		DECLARE @CantidadAntesMovto DECIMAL(28, 10)

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = 'No es posible procesar el movimiento. '

		DECLARE @51000 VARCHAR(MAX) = @mensaje + 'El artículo [@Producto] con cuenta presupuestal [@CuentaPresupuestalId] no puede tener existencia en negativo.'

		--Buscamos los datos del producto a afectar
		SELECT @Producto = producto.Descripcion,
				@CuentaPresupuestalId = CuentaPresupuestalId,
				@UnidadMedidaId = UnidadDeMedidaId,
				@TipoCostoArticuloId = 0,
				@ValorContableAntesMovto = 0,
				@CantidadAntesMovto = Cantidad
		FROM RequisicionesAlmacenDatos.dbo.tblAlmacenProducto AS almacen
				INNER JOIN tblProducto AS producto ON almacen.ProductoId = producto.ProductoId
		WHERE almacen.AlmacenProductoId = @AlmacenProductoId

		--Validamos que el inventario no pueda ajustar a existencia negativa
		IF ( (@CantidadAntesMovto + @CantidadMovimiento) < 0 )
		BEGIN 
			SET @mensaje = (REPLACE(REPLACE(@51000, '@Producto', @Producto), '@CuentaPresupuestalId', @CuentaPresupuestalId));
			THROW 51002, @mensaje, 1;
		END

		INSERT INTO RequisicionesAlmacenDatos.dbo.tblInventarioMovimiento
		(
			--InventarioMovtoId - this column value is auto-generated
			AlmacenProductoId,
			UnidadMedidaId,
			CantidadMovimiento,
			TipoMovimientoId,
			CantidadAntesMovto,
			TipoCostoArticuloId,
			ValorContableAntesMovto,
			MotivoMovto,
			ReferenciaMovtoId,
			FechaCreacion,
			CreadoPorId
		)
		VALUES
		(
			@AlmacenProductoId,
			@UnidadMedidaId,
			@CantidadMovimiento,
			@TipoMovimientoId,
			@CantidadAntesMovto,
			@TipoCostoArticuloId,
			@ValorContableAntesMovto,
			ISNULL(@MotivoMovto, ''),
			@ReferenciaMovtoId,
			GETDATE(),
			@CreadoPorId
		)

		--Actualizamos la cantidad en la tabla de tblAlmacenProducto
		UPDATE RequisicionesAlmacenDatos.dbo.tblAlmacenProducto SET Cantidad = @CantidadAntesMovto + @CantidadMovimiento WHERE AlmacenProductoId = @AlmacenProductoId
END