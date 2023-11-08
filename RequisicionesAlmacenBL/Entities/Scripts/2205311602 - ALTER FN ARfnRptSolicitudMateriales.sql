SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- Author:		Javier Elías
-- Create date: 18/08/2021
-- Modified date: 30/05/2022
-- Description:	Función para obtener el reporte de Requisición Material.
-- ============================================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptSolicitudMateriales] 
(	
	@requisicionId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT dependencia.Nombre AS AreaSolicitante,
		   ProyectoPresupuestario = STUFF(
		   (
				SELECT DISTINCT
					   ', ' + p.ProyectoId
				FROM ARtblRequisicionMaterial AS r
					 INNER JOIN ARtblRequisicionMaterialDetalle AS d ON r.RequisicionMaterialId = d.RequisicionMaterialId
					 INNER JOIN tblProyecto AS p ON d.ProyectoId = p.ProyectoId
				WHERE r.RequisicionMaterialId = @requisicionId 
				ORDER BY ', ' + p.ProyectoId 
				FOR XML PATH('')
		   ), 1, 1, ''),
		   detalle.ProductoId,
		   um.Descripcion AS UnidadMedida,
		   detalle.Descripcion,
		   Cantidad AS CantidadSolicitada,
		   ISNULL(Surtida, 0) AS CantidadEntregada,
		   ISNULL(Comentarios, '') AS Observaciones,
		   dbo.RHfnGetNombreCompletoEmpleado(usuario.EmpleadoId) AS SolicitadoPor,
		   CONVERT(VARCHAR, requisicion.FechaCreacion, 3) AS FechaSolicitada,
		   '' AS AutorizadoPor,
		   '' AS FechaAutorizada,
		   '' AS EntregadoPor,
		   '' AS FechaEntregada,
		   '' AS RecibidoPor,
		   '' AS FechaRecibida
	FROM ARtblRequisicionMaterial AS requisicion
		 INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId AND detalle.EstatusId NOT IN(78, 85)
		 INNER JOIN tblUnidadDeMedida AS um ON um.UnidadDeMedidaId = detalle.UnidadMedidaId
		 INNER JOIN GRtblUsuario AS usuario ON requisicion.CreadoPorId = usuario.UsuarioId
		 INNER JOIN RHtblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
		 INNER JOIN tblDependencia AS dependencia ON empleado.AreaAdscripcionId = dependencia.DependenciaId
		 OUTER APPLY 
		 (
				SELECT SUM(CantidadMovimiento) AS Surtida
				FROM ARtblInventarioMovimiento
				WHERE TipoMovimientoId = 63 -- Requisición Material Surtimiento
					  AND ReferenciaMovtoId = detalle.RequisicionMaterialDetalleId
		 ) AS surtimiento
	WHERE requisicion.RequisicionMaterialId = @requisicionId	
)
GO