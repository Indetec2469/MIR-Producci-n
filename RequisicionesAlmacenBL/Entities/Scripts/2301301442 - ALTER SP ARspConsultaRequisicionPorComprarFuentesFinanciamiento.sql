SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionPorComprarFuentesFinanciamiento]
AS
-- =======================================================================
-- Author:			Javier Elías
-- Create date:		29/07/2021
-- Modified date:	27/01/2023
-- Description:		Procedimiento para Obtener las Fuentes de Financiamiento
--					para una Requisición Pendiente por Comprar.
-- =======================================================================
SELECT DISTINCT
       CuentaPresupuestalEgrId,
       detalle.ProductoId,
       areaAlmacen.AlmacenId,
       detalle.UnidadAdministrativaId,
       detalle.ProyectoId,
       detalle.TipoGastoId,
       ff.RamoId,
       ff.Nombre
FROM ARtblControlMaestroConfiguracionArea AS area
     INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen AS areaAlmacen ON area.ConfiguracionAreaId = areaAlmacen.ConfiguracionAreaId AND areaAlmacen.Borrado = 0
     INNER JOIN ARtblRequisicionMaterial AS requisicion ON area.AreaId = requisicion.AreaId
     INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId AND detalle.EstatusId IN(84, 90, 83) -- Por surtir, Surtido parcial, Por Comprar
     INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
     INNER JOIN tblCuentaPresupuestalEgr AS cp ON producto.ObjetoGastoId = cp.ObjetoGastoId
                                                  AND detalle.UnidadAdministrativaId = cp.DependenciaId
                                                  AND detalle.ProyectoId = cp.ProyectoId
                                                  AND detalle.TipoGastoId = cp.TipoGastoId
     INNER JOIN tblRamo AS ff ON cp.RamoId = ff.RamoId
WHERE area.Borrado = 0
ORDER BY detalle.ProductoId,
         areaAlmacen.AlmacenId,
         detalle.UnidadAdministrativaId,
         detalle.ProyectoId,
         detalle.TipoGastoId,
         ff.RamoId