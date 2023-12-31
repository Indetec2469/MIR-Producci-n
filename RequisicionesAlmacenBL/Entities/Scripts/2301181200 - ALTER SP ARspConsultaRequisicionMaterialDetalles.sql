SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[ARspConsultaRequisicionMaterialDetalles]
	@requisicionId INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 21/06/2021
-- Modified date: 18/01/2023
-- Description:	Procedimiento para Obtener los detalles de una Requisición
--						de la Ficha Requisición Material.
-- ============================================================
SELECT CONCAT(ProductoId, UnidadAdministrativaId, proyecto.ProyectoId, tipoGasto.TipoGastoId) AS ProductoDetalleId,
       detalle.ProductoId,
       detalle.Descripcion,
	   detalle.ProductoId + ' - ' + detalle.Descripcion AS Producto,
       um.UnidadDeMedidaId,
       um.Descripcion AS UnidadDeMedida,
       CostoUnitario,
       UnidadAdministrativaId,
       unidadAdministrativa.Nombre AS UnidadAdministrativa,
       detalle.ProyectoId,
       proyecto.Nombre AS Proyecto,
       tipoGasto.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto,
       RequisicionMaterialDetalleId,
       detalle.RequisicionMaterialId,
       Cantidad,
       TotalPartida,
       Comentarios,
       detalle.EstatusId,
	   estatus.Valor AS Estatus,
	   MotivoRechazo
FROM ARtblRequisicionMaterialDetalle AS detalle
     INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblDependencia AS unidadAdministrativa ON detalle.UnidadAdministrativaId = unidadAdministrativa.DependenciaId
	 INNER JOIN tblProyecto AS proyecto ON detalle.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblTipoGasto AS tipoGasto ON detalle.TipoGastoId = tipoGasto.TipoGastoId
	 INNER JOIN GRtblControlMaestro AS estatus ON detalle.EstatusId = estatus.ControlId
WHERE detalle.RequisicionMaterialId = @requisicionId
      AND detalle.EstatusId NOT IN (78)  -- Cancelado
