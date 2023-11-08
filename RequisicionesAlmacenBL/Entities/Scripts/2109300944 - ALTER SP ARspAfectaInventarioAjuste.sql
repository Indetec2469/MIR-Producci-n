SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================
-- Author:		Javier Elías
-- Create date: 09/03/2021
-- Modified date: 27/09/2021
-- Description:	Procesador para afectar un Inventario Ajuste
-- =================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspAfectaInventarioAjuste]
	@InventarioId INT,
	@UsuarioId INT
AS
BEGIN
		SET NOCOUNT ON;

		DECLARE @INCREMENTA INT = 9
		DECLARE @DISMINUYE INT = 10

		DECLARE @tbl TABLE (
			AlmacenProductoId INT,
			Producto VARCHAR(250),
			CuentaPresupuestalId INT,
			CantidadMovimiento DECIMAL(28, 10),
			MotivoMovto VARCHAR(MAX),
			DetalleId INT,
			Orden INT
		)

		--Buscamos los datos del producto a afectar
		INSERT INTO @tbl
		SELECT almacenProducto.AlmacenProductoId,
				Descripcion,
				detalle.CuentaPresupuestalId,
				detalle.Cantidad * CASE WHEN TipoMovimientoId = @DISMINUYE THEN -1 ELSE 1 END AS CantidadMovimiento,
				Comentarios AS MotivoMovto,
				detalle.InventarioAjusteDetalleId,
				ROW_NUMBER() OVER (ORDER BY InventarioAjusteDetalleId) AS Orden
		FROM ARtblInventarioAjuste AS inventario
				INNER JOIN ARtblInventarioAjusteDetalle AS detalle ON inventario.InventarioAjusteId = detalle.InventarioAjusteId
				INNER JOIN ARtblAlmacenProducto AS almacenProducto ON detalle.AlmacenId = almacenProducto.AlmacenId AND detalle.ProductoId = almacenProducto.ProductoId AND detalle.CuentaPresupuestalId = almacenProducto.CuentaPresupuestalId
		WHERE inventario.InventarioAjusteId = @InventarioId

		DECLARE @contador INT = 1
		DECLARE @registros INT = ( SELECT COUNT(AlmacenProductoId) FROM @tbl )

		DECLARE @AlmacenProductoId INT
		DECLARE @Producto VARCHAR(250)
		DECLARE @CuentaPresupuestalId INT
		DECLARE @CantidadMovimiento DECIMAL(28, 10)
		DECLARE @TipoMovimientoId INT = 36 --Inventario Ajuste
		DECLARE @MotivoMovto VARCHAR(MAX)
		DECLARE @ReferenciaDetalleId INT

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = 'No es posible afectar el inventario. '

		DECLARE @51000 VARCHAR(MAX) = @mensaje + 'La cantidad de ajuste en el artículo [@Producto] con cuenta presupuestal [@CuentaPresupuestalId] no puede estar vacío.'
		DECLARE @51002 VARCHAR(MAX) = @mensaje + 'No existen movimientos por registrar.'

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
							   @MotivoMovto = MotivoMovto,
							   @ReferenciaDetalleId = DetalleId
						FROM @tbl
						WHERE Orden = @contador

						--Validamos que se haya llenado la columna de conteo
						IF ( @CantidadMovimiento IS NULL )
						BEGIN 
							SET @mensaje = (REPLACE(REPLACE(@51000, '@Producto', @Producto), '@CuentaPresupuestalId', @CuentaPresupuestalId));
							THROW 51001, @mensaje, 1;
						END

						--Verificamos que la cantidad de movimiento sea diferente de 0
						IF ( @CantidadMovimiento != 0 )
						BEGIN
							--Si pasó las validaciones agregamos el movimiento
							INSERT INTO @Movimientos
							SELECT @AlmacenProductoId,
										 @CantidadMovimiento,
										 NULL, -- Costo Unitario
										 @ReferenciaDetalleId
						END

						SET  @contador = @contador + 1
				END

				--Validamos que se existan movimientos por registrar
				DECLARE @contadorMovimientos INT = ( SELECT COUNT(AlmacenProductoId) FROM @Movimientos )

				IF ( @contadorMovimientos = 0 )
						THROW 51002, @mensaje, 1;

				--Si pasó las validaciones mandamos llamar al Procesador de Inventarios
				EXEC ARspProcesadorInventarios
								@TipoMovimientoId,
								@MotivoMovto,
								@UsuarioId,
								1, -- Insertar agrupador
								@InventarioId,
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