SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionPorSurtirExistencias]
	@almacenId NVARCHAR(4),
	@productoId NVARCHAR(10),
	@dependenciaId NVARCHAR(6),
	@proyectoId NVARCHAR(6),
	@tipoGastoId NVARCHAR(1)
AS
/* ****************************************************************
 * ARspConsultaRequisicionPorSurtirExistencias
 * ****************************************************************
 * Descripción: Procedimiento para Obtener la existenica de Almacén / Producto
						 para una Requisición Pendiente por Surtir.
 *
 * autor: 	Javier Elías
 * Fecha: 	23.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: AlmacenId, ProductoId, DependenciaId, 
												   ProyectoId, TipoGastoId
 * PARAMETROS DE SALIDA:
 *****************************************************************
*/ 
SELECT almacenProducto.AlmacenProductoId,
       fuenteFinanciamiento.RamoId AS FuenteFinanciamientoId,
	   fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
       Cantidad AS Existencia,
	   CONVERT(DECIMAL(28, 10), NULL) AS CantidadSurtir
FROM ARtblAlmacenProducto AS almacenProducto
     INNER JOIN tblCuentaPresupuestalEgr AS cuentaPresupuestal ON almacenProducto.CuentaPresupuestalId = cuentaPresupuestal.CuentaPresupuestalEgrId
     INNER JOIN tblRamo AS fuenteFinanciamiento ON cuentaPresupuestal.RamoId = fuenteFinanciamiento.RamoId
WHERE almacenProducto.Borrado = 0
      AND almacenProducto.Cantidad > 0
	  AND almacenProducto.AlmacenId = @almacenId
	  AND almacenProducto.ProductoId = @productoId
      AND cuentaPresupuestal.DependenciaId = @dependenciaId
      AND cuentaPresupuestal.ProyectoId = @proyectoId
      AND cuentaPresupuestal.TipoGastoId = @tipoGastoId
GO