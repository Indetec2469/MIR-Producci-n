SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionPorComprarAlmacenes]
AS
-- ==========================================================
-- Author:			Javier Elías
-- Create date:		27/01/2023
-- Modified date:	
-- Description:		Procedimiento para Obtener los Almacenes
--					para una Requisición Pendiente por Comprar.
-- ==========================================================
SELECT DISTINCT
       almacen.AlmacenId,
       almacen.Nombre,
       area.AreaId
FROM ARtblControlMaestroConfiguracionArea AS area
     INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen AS areaAlmacen ON area.ConfiguracionAreaId = areaAlmacen.ConfiguracionAreaId AND areaAlmacen.Borrado = 0
     INNER JOIN tblAlmacen AS almacen ON areaAlmacen.AlmacenId = almacen.AlmacenId
     INNER JOIN ARtblRequisicionMaterial AS requisicion ON area.AreaId = requisicion.AreaId
     INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId AND detalle.EstatusId IN(84, 90, 83) -- Por surtir, Surtido parcial, Por Comprar
WHERE area.Borrado = 0
ORDER BY almacen.Nombre