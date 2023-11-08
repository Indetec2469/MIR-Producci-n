SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================
-- Author:		Javier Elías
-- Create date: 16/02/2021
-- Modified date: 27/09/2021
-- Description:	Procesador para afecta un Inventario Físico
-- =================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspAfectaInventarioFisico]
	@InventarioId INT,
	@UsuarioId INT
AS
BEGIN
		SET NOCOUNT ON;

		DECLARE @EN_PROCESO INT = 32
		DECLARE @TERMINADO INT = 33
		DECLARE @CANCELADO INT = 34

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
		SELECT detalle.AlmacenProductoId,
				Producto,
				CuentaPresupuestalId,
				Conteo - Existencia AS CantidadMovimiento,
				MotivoAjuste AS MotivoMovto,
				detalle.InventarioFisicoDetalleId,
				ROW_NUMBER() OVER (ORDER BY InventarioFisicoDetalleId) AS Orden
		FROM ARtblInventarioFisico AS inventario
				INNER JOIN ARtblInventarioFisicoDetalle AS detalle ON inventario.InventarioFisicoId = detalle.InventarioFisicoId AND Borrado = 0
				INNER JOIN ARtblAlmacenProducto AS producto ON detalle.AlmacenProductoId = producto.AlmacenProductoId
		WHERE inventario.EstatusId = @EN_PROCESO
				AND inventario.InventarioFisicoId = @InventarioId

		DECLARE @contador INT = 1
		DECLARE @registros INT = ( SELECT COUNT(AlmacenProductoId) FROM @tbl )

		DECLARE @AlmacenProductoId INT
		DECLARE @Producto VARCHAR(250)
		DECLARE @CuentaPresupuestalId INT
		DECLARE @CantidadMovimiento DECIMAL(28, 10)
		DECLARE @TipoMovimientoId INT = 35 --Inventario Físico
		DECLARE @MotivoMovto VARCHAR(MAX)
		DECLARE @ReferenciaDetalleId INT

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = 'No es posible afectar el inventario. '

		DECLARE @51000 VARCHAR(MAX) = @mensaje + 'El inventario ha sido modificado por otro usuario.'
		DECLARE @51001 VARCHAR(MAX) = @mensaje + 'El conteo del artículo [@Producto] con cuenta presupuestal [@CuentaPresupuestalId] no puede estar vacío.'
		DECLARE @51002 VARCHAR(MAX) = @mensaje + 'No existen movimientos por registrar.'

		BEGIN TRY
		--BEGIN TRANSACTION
				DECLARE @validarEstatus INT = ( SELECT COUNT(*) FROM ARtblInventarioFisico WHERE EstatusId = @EN_PROCESO )

				--Validamos que el Inventario no haya sido modificado por otro usuario
				IF ( @validarEstatus = 0 )
						THROW 51000, @mensaje, 1;
				
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
							SET @mensaje = (REPLACE(REPLACE(@51001, '@Producto', @Producto), '@CuentaPresupuestalId', @CuentaPresupuestalId));
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

				--Actualizamos el Estatus del Inventario para ponerlo como Terminados
				UPDATE ARtblInventarioFisico
					SET
						EstatusId = @TERMINADO,
						FechaUltimaModificacion = GETDATE(),
						ModificadoPorId = @UsuarioId
				 WHERE InventarioFisicoId = @InventarioId
		--COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			--ROLLBACK TRANSACTION;
			THROW;
		END CATCH
END
GO