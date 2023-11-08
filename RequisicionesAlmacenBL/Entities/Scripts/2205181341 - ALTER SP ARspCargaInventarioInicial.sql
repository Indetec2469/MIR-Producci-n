SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===========================================================
-- Author:		Javier Elías
-- Create date: 13/12/2021
-- Modified date: 18/05/2022
-- Description:	Procesador para afectar un Cargar un Inventario Inicial
-- ===========================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspCargaInventarioInicial]
	@ImportarAlmacenProducto ARudtImportarAlmacenProducto READONLY,
	@UsuarioId INT
AS
BEGIN
		SET NOCOUNT ON;

		BEGIN TRY
		--BEGIN TRANSACTION
				
				--Construimos los movimientos para el procesador
				DECLARE @Movimientos ARudtInventarioMovimiento

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

				--Actualizamos el CostoPromedio y CostoUltimo del Producto si es necesario
				UPDATE producto
					SET
						producto.CostoPromedio = importado.CostoUnitario,
						producto.CostoUltimo = importado.CostoUnitario
				FROM @ImportarAlmacenProducto AS importado
						INNER JOIN tblProducto AS producto ON importado.ProductoId = producto.ProductoId AND producto.CostoPromedio = 0 AND producto.CostoUltimo = 0

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

				--Mandamos llamar al Procesador de Inventarios
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