SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===========================================================
-- Author:		Javier Elías
-- Create date: 13/12/2021
-- Modified date: 
-- Description:	Procesador para afectar un Cargar un Inventario Inicial
-- ===========================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspCargaInventarioInicial]
	@ImportarAlmacenProducto ARudtImportarAlmacenProducto READONLY,
	@UsuarioId INT
AS
BEGIN
		SET NOCOUNT ON;

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = 'No es posible cargar el inventario. '

		DECLARE @51000 VARCHAR(MAX) = @mensaje + 'El Producto [@ProductoId] no existe.'
		DECLARE @51001 VARCHAR(MAX) = @mensaje + 'El Almacén [@AlmacenId] no existe.'
		DECLARE @51002 VARCHAR(MAX) = @mensaje + 'La cantidad para el Producto [@ProductoId] debe ser mayor a 0.'
		DECLARE @51003 VARCHAR(MAX) = @mensaje + 'No existe cuenta presupuestal para el Artículo [@ProductoId] con Fuente de FinanciamientoId [@FuenteFinanciamientoId], Proyecto [@ProyectoId], Unidad AdministrativaId [@UnidadAdministrativaId] y Tipo de Gasto [@TipoGastoId].'
		DECLARE @51004 VARCHAR(MAX) = @mensaje + 'No existen movimientos por registrar.'

		BEGIN TRY
		--BEGIN TRANSACTION
				
				--Construimos los movimientos para el procesador
				DECLARE @Movimientos ARudtInventarioMovimiento

				--Declaramos las variables para las validaciones
				DECLARE @ProductoId VARCHAR(MAX)
				DECLARE @AlmacenId VARCHAR(MAX)
				DECLARE @FuenteFinanciamientoId VARCHAR(MAX)
				DECLARE @ProyectoId VARCHAR(MAX)
				DECLARE @UnidadAdministrativaId VARCHAR(MAX)
				DECLARE @TipoGastoId VARCHAR(MAX)
				DECLARE @Cantidad DECIMAL(28, 10)

				--Validamos que todos los productos existan
				SELECT TOP 1 @ProductoId = importado.ProductoId
				FROM @ImportarAlmacenProducto AS importado
						LEFT JOIN tblProducto AS producto ON importado.ProductoId = producto.ProductoId
				WHERE producto.ProductoId IS NULL

				IF ( @ProductoId IS NOT NULL )
				BEGIN
					SET @mensaje = (REPLACE(@51000, '@ProductoId', @ProductoId));
					THROW 51000, @mensaje, 1;
				END

				--Validamos que todos los almacenes existan
				SELECT TOP 1 @AlmacenId = importado.AlmacenId
				FROM @ImportarAlmacenProducto AS importado
						LEFT JOIN tblAlmacen AS almacen ON importado.AlmacenId = almacen.AlmacenId
				WHERE almacen.AlmacenId IS NULL

				IF ( @AlmacenId IS NOT NULL )
				BEGIN
					SET @mensaje = (REPLACE(@51001, '@AlmacenId', @AlmacenId));
					THROW 51001, @mensaje, 1;
				END

				--Validamos que todos los productos tengan cantidad mayor a 0
				SELECT TOP 1 @ProductoId = importado.ProductoId
				FROM @ImportarAlmacenProducto AS importado
				WHERE importado.Cantidad IS NULL OR importado.Cantidad <= 0

				IF ( @ProductoId IS NOT NULL )
				BEGIN
					SET @mensaje = (REPLACE(@51002, '@ProductoId', @ProductoId));
					THROW 51002, @mensaje, 1;
				END

				--Validamos que exista la Cuenta Presupuestal para todos los registros
				SELECT TOP 1 @ProductoId = importado.ProductoId,
						@FuenteFinanciamientoId = importado.FuenteFinanciamientoId,
						@ProyectoId = importado.ProyectoId,
						@UnidadAdministrativaId = importado.UnidadAdministrativaId,
						@TipoGastoId = importado.TipoGastoId
				FROM @ImportarAlmacenProducto AS importado
						INNER JOIN tblProducto AS producto ON importado.ProductoId = producto.ProductoId
						LEFT JOIN tblCuentaPresupuestalEgr AS cp ON producto.ObjetoGastoId = cp.ObjetoGastoId
																	AND importado.FuenteFinanciamientoId = cp.RamoId
																	AND importado.ProyectoId = cp.ProyectoId
																	AND importado.UnidadAdministrativaId = cp.DependenciaId
																	AND importado.TipoGastoId = cp.TipoGastoId
				WHERE cp.CuentaPresupuestalEgrId IS NULL

				IF ( @ProductoId IS NOT NULL )
				BEGIN
					SET @mensaje = (REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@51003, '@ProductoId', @ProductoId), '@FuenteFinanciamientoId', @FuenteFinanciamientoId), '@ProyectoId', @ProyectoId), '@UnidadAdministrativaId', @UnidadAdministrativaId), '@TipoGastoId', @TipoGastoId));
					THROW 51003, @mensaje, 1;
				END

				--Insertamos los registros que no existen en la tabla de Almacén/Producto
				INSERT INTO ARtblAlmacenProducto
				(
					--AlmacenProductoId - this column value is auto-generated
					ProductoId,
					AlmacenId,
					CuentaPresupuestalId,
					Cantidad,
					Borrado,
					FechaCreacion,
					CreadoPorId
				)
				SELECT importado.ProductoId,
						importado.AlmacenId,
						cp.CuentaPresupuestalEgrId,
						0,
						0,
						GETDATE(),
						@UsuarioId
				FROM @ImportarAlmacenProducto AS importado
						INNER JOIN tblProducto AS producto ON importado.ProductoId = producto.ProductoId
						INNER JOIN tblCuentaPresupuestalEgr AS cp ON producto.ObjetoGastoId = cp.ObjetoGastoId
																	AND importado.FuenteFinanciamientoId = cp.RamoId
																	AND importado.ProyectoId = cp.ProyectoId
																	AND importado.UnidadAdministrativaId = cp.DependenciaId
																	AND importado.TipoGastoId = cp.TipoGastoId
						LEFT JOIN ARtblAlmacenProducto AS ap ON importado.AlmacenId = ap.AlmacenId
																AND importado.ProductoId = ap.ProductoId
																AND cp.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId
																AND ap.Borrado = 0
				WHERE ap.AlmacenProductoId IS NULL

				--Creamos los movimientos para el procesador
				INSERT INTO @Movimientos
				SELECT ap.AlmacenProductoId,
						importado.Cantidad - ap.Cantidad,
						importado.CostoUnitario,
						NULL
				FROM @ImportarAlmacenProducto AS importado
						INNER JOIN tblProducto AS producto ON importado.ProductoId = producto.ProductoId
						INNER JOIN tblCuentaPresupuestalEgr AS cp ON producto.ObjetoGastoId = cp.ObjetoGastoId
																	AND importado.FuenteFinanciamientoId = cp.RamoId
																	AND importado.ProyectoId = cp.ProyectoId
																	AND importado.UnidadAdministrativaId = cp.DependenciaId
																	AND importado.TipoGastoId = cp.TipoGastoId
						INNER JOIN ARtblAlmacenProducto AS ap ON importado.AlmacenId = ap.AlmacenId
																AND importado.ProductoId = ap.ProductoId
																AND cp.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId
																AND ap.Borrado = 0
				WHERE importado.Cantidad - ap.Cantidad != 0

				--Validamos que existan movimientos por registrar en el inventario
				DECLARE @contadorMovimientos INT = ( SELECT COUNT(AlmacenProductoId) FROM @Movimientos )
				
				IF ( @contadorMovimientos = 0 )
				BEGIN
					SET @mensaje = @51004;
					THROW 51004, @51004, 1;
				END

				--Si pasó todas las validaciones mandamos llamar al Procesador de Inventarios
				EXEC ARspProcesadorInventarios
								109, -- TipoMovimiento Carga Inventario Inicial
								'Carga Inventario Inicial', --Motivo Movto,
								@UsuarioId,
								1, -- Insertar agrupador
								NULL, -- Referencia Movto Id
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