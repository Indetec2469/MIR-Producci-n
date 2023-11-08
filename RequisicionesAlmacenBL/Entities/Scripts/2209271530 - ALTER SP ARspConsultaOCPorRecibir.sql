SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOCPorRecibir]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 25/08/2021
-- Modified date: 27/09/2022
-- Description:	Procedimiento para Obtener las Ordenes de Compra para
--						agregar al combo de Productos de la Ficha de Recibo de OC.
-- ============================================================
SELECT oc.OrdenCompraId,
       dbo.GRfnGetFechaConFormato(oc.Fecha, 0) AS FechaOC,
	   oc.ProveedorId,
	   oc.TipoComprobanteFiscalId,
	   oc.AlmacenId,
	   TipoOperacionId,
	   oc.Status,
	   CASE oc.Status WHEN 'A' THEN 'Activa' WHEN 'I' THEN 'Parcialmente Recibida' WHEN 'R' THEN 'Recibida' WHEN 'C' THEN 'Cancelada' ELSE '' END AS EstatusOC,
	   CantidadSolicitada,
	   ISNULL(recibos.CantidadRecibida, 0) AS CantidadRecibida,
	   poliza.Poliza AS PolizaOC
FROM tblOrdenCompra AS oc
    INNER JOIN
	(
			SELECT detalle.OrdenCompraId,
					SUM(detalle.Cantidad) AS CantidadSolicitada
			FROM tblOrdenCompraDet AS detalle
			WHERE detalle.Status != 'C' -- Cancelado
			GROUP BY detalle.OrdenCompraId
	) AS detalles ON oc.OrdenCompraId = detalles.OrdenCompraId
	LEFT JOIN 
	(
			SELECT detalle.OrdenCompraId,
					SUM(reciboDet.Cantidad) AS CantidadRecibida
			FROM tblCompra AS recibo
					INNER JOIN tblCompraDet AS reciboDet ON recibo.CompraId = reciboDet.CompraId
					INNER JOIN tblOrdenCompraDet AS detalle ON reciboDet.RefOrdenCompraDetId = detalle.OrdenCompraDetId AND detalle.Status != 'C' -- Cancelado
			WHERE recibo.Status != 'C' -- Cancelado
			GROUP BY detalle.OrdenCompraId
	) AS recibos ON oc.OrdenCompraId = recibos.OrdenCompraId
	LEFT JOIN
	(
			SELECT DISTINCT
				   oc.OrdenCompraId AS Id
			FROM tblOrdenCompra AS oc
				 INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
				 INNER JOIN ARtblOrdenCompraRequisicionDet AS ocRequisicion ON ocDetalle.OrdenCompraDetId = ocRequisicion.OrdenCompraDetId
				 INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON ocRequisicion.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId AND requisicionDetalle.EstatusId != 78 --AREstatusRequisicionDetalle	Cancelado
				 INNER JOIN ARtblRequisicionMaterial AS requisicion ON requisicionDetalle.RequisicionMaterialId = requisicion.RequisicionMaterialId AND requisicion.EstatusId != 65 -- AREstatusRequisicion Cancelada
	) AS ocRequisicion ON oc.OrdenCompraId = ocRequisicion.Id
	LEFT JOIN
	(
			SELECT DISTINCT
				   oc.OrdenCompraId AS Id
			FROM tblOrdenCompra AS oc
				 INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
				 INNER JOIN ARtblOrdenCompraInvitacionDet AS ocInvitacion ON ocDetalle.OrdenCompraDetId = ocInvitacion.OrdenCompraDetId
				 INNER JOIN ARtblInvitacionCompraDetalle AS invitacionDetalle ON ocInvitacion.InvitacionCompraDetalleId = invitacionDetalle.InvitacionCompraDetalleId AND invitacionDetalle.EstatusId != 94 --AREstatusInvitacionCompraDetalle	Cancelado
				 INNER JOIN ARtblInvitacionCompra AS invitacion ON invitacionDetalle.InvitacionCompraId = invitacion.InvitacionCompraId AND invitacion.EstatusId != 91 --AREstatusInvitacionCompra	Cancelada
	) AS ocInvitacion ON oc.OrdenCompraId = ocInvitacion.Id
	OUTER APPLY
	(
			SELECT TOP 1 Poliza
			FROM tblPolizaRef AS ref
				 INNER JOIN tblPoliza AS pol ON ref.PolizaId = pol.PolizaId
			WHERE Referencia = oc.OrdenCompraId
				  AND TipoMovimientoId = 1
	) AS poliza
WHERE oc.Status NOT IN ('R', 'C') -- Recibida, Cancelada
      AND detalles.CantidadSolicitada - ISNULL(recibos.CantidadRecibida, 0) > 0
	  AND (ocRequisicion.Id IS NOT NULL OR ocInvitacion.Id IS NOT NULL)
ORDER BY oc.OrdenCompraId
GO