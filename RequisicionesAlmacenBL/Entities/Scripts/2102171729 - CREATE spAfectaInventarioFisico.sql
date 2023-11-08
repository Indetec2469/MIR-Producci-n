DROP PROCEDURE IF EXISTS spAfectaInventarioFisico
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================
-- Author:		Javier Elías
-- Create date: 16/02/2021
-- Description:	Procesador para afecta un Inventario Físico
-- =================================================
CREATE PROCEDURE [dbo].[spAfectaInventarioFisico] 
	@InventarioId INT,
	@UsuarioId INT
AS
BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
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
			Orden INT
		)

		--Buscamos los datos del producto a afectar
		INSERT INTO @tbl
		SELECT detalle.AlmacenProductoId,
				Producto,
				CuentaPresupuestalId,
				Conteo - Existencia AS CantidadMovimiento,
				MotivoAjuste AS MotivoMovto,
				ROW_NUMBER() OVER (ORDER BY InventarioFisicoDetalleId) AS Orden
		FROM RequisicionesAlmacenDatos.dbo.tblInventarioFisico AS inventario
				INNER JOIN RequisicionesAlmacenDatos.dbo.tblInventarioFisicoDetalle AS detalle ON inventario.InventarioFisicoId = detalle.InventarioFisicoId AND Borrado = 0
				INNER JOIN RequisicionesAlmacenDatos.dbo.tblAlmacenProducto AS producto ON detalle.AlmacenProductoId = producto.AlmacenProductoId
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

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = 'No es posible afectar el inventario. '

		DECLARE @51000 VARCHAR(MAX) = @mensaje + 'El inventario ha sido modificado por otro usuario.'
		DECLARE @51001 VARCHAR(MAX) = @mensaje + 'El conteo del artículo [@Producto] con cuenta presupuestal [@CuentaPresupuestalId] no puede estar vacío.'

		BEGIN TRY
		--BEGIN TRANSACTION
				DECLARE @validarEstatus INT = ( SELECT COUNT(*) FROM RequisicionesAlmacenDatos.dbo.tblInventarioFisico WHERE EstatusId = @EN_PROCESO )

				--Validamos que el Inventario no haya sido modificado por otro usuario
				IF ( @validarEstatus = 0 )
						THROW 51000, @mensaje, 1;

				WHILE ( @contador <= @registros )
				BEGIN
						SELECT @AlmacenProductoId = AlmacenProductoId,
							   @Producto = Producto,
							   @CuentaPresupuestalId = CuentaPresupuestalId,
							   @CantidadMovimiento = CantidadMovimiento,
							   @MotivoMovto = MotivoMovto
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
							--Si pasó las validaciones mandamos llamar al Procesador de Inventarios
							EXEC spProcesadorInventarios
											@AlmacenProductoId,
											@CantidadMovimiento,
											@TipoMovimientoId,
											@MotivoMovto,
											@InventarioId,
											@UsuarioId
						END

						SET  @contador = @contador + 1
				END

				--Actualizamos el Estatus del Inventario para ponerlo como Terminados
				UPDATE RequisicionesAlmacenDatos.dbo.tblInventarioFisico
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