SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoOrdenCompraRecibo]
AS
-- ====================================================================
-- Author:		Javier Elías
-- Create date: 24/08/2021
-- Modified date: 27/05/2022
-- Description:	View para obtener el Listado de los Recibos de Ordenes de Compra
-- ====================================================================
SELECT recibo.CompraId,
       dbo.GRfnGetFechaConFormato(recibo.Fecha, 0) AS FechaRecibo,
	   CASE recibo.Status WHEN 'A' THEN 'Activo' WHEN 'P' THEN 'Orden de Pago parcial' WHEN 'O' THEN 'Orden de Pago' WHEN 'C' THEN 'Cancelado' ELSE '' END AS Estatus,
	   detalles.OrdenCompraId AS CodigoOC,
       dbo.GRfnGetFechaConFormato(detalles.Fecha, 0) AS FechaOC,
	   proveedor.RazonSocial AS Proveedor,
       CASE detalles.Status WHEN 'A' THEN 'Activa' WHEN 'I' THEN 'Parcialmente Recibida' WHEN 'R' THEN 'Recibida' WHEN 'C' THEN 'Cancelada' ELSE '' END AS EstatusOC,
	   recibo.Status
FROM tblCompra AS recibo
     INNER JOIN tblProveedor AS proveedor ON recibo.ProveedorId = proveedor.ProveedorId
     CROSS APPLY
	 (
		SELECT oc.OrdenCompraId,
			   oc.Fecha,
			   oc.Status,
			   SUM(detalle.Cantidad) AS CantidadOC,
			   SUM(detRecibo.Cantidad) AS CantidadRecibida
		FROM tblCompraDet AS detRecibo
			 INNER JOIN tblOrdenCompraDet AS detalle ON detalle.OrdenCompraDetId = detRecibo.RefOrdenCompraDetId AND detalle.Status != 'C' -- Cancelado
			 INNER JOIN tblOrdenCompra AS oc ON detalle.OrdenCompraId = oc.OrdenCompraId AND oc.Status != 'C' -- Cancelada
		WHERE detRecibo.CompraId = recibo.CompraId
		GROUP BY oc.OrdenCompraId,
			   oc.Fecha,
			   oc.Status
	 ) AS detalles
	 LEFT JOIN
	 (
			SELECT DISTINCT
				   oc.OrdenCompraId AS Id
			FROM tblOrdenCompra AS oc
				 INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
				 INNER JOIN ARtblOrdenCompraRequisicionDet AS ocRequisicion ON ocDetalle.OrdenCompraDetId = ocRequisicion.OrdenCompraDetId
				 INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON ocRequisicion.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId AND requisicionDetalle.EstatusId != 78 --AREstatusRequisicionDetalle	Cancelado
				 INNER JOIN ARtblRequisicionMaterial AS requisicion ON requisicionDetalle.RequisicionMaterialId = requisicion.RequisicionMaterialId AND requisicion.EstatusId != 65 -- AREstatusRequisicion Cancelada
	 ) AS ocRequisicion ON detalles.OrdenCompraId = ocRequisicion.Id
	 LEFT JOIN
	 (
			SELECT DISTINCT
				   oc.OrdenCompraId AS Id
			FROM tblOrdenCompra AS oc
				 INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
				 INNER JOIN ARtblOrdenCompraInvitacionDet AS ocInvitacion ON ocDetalle.OrdenCompraDetId = ocInvitacion.OrdenCompraDetId
				 INNER JOIN ARtblInvitacionCompraDetalle AS invitacionDetalle ON ocInvitacion.InvitacionCompraDetalleId = invitacionDetalle.InvitacionCompraDetalleId AND invitacionDetalle.EstatusId != 94 --AREstatusInvitacionCompraDetalle	Cancelado
				 INNER JOIN ARtblInvitacionCompra AS invitacion ON invitacionDetalle.InvitacionCompraId = invitacion.InvitacionCompraId AND invitacion.EstatusId != 91 --AREstatusInvitacionCompra	Cancelada
	 ) AS ocInvitacion ON detalles.OrdenCompraId = ocInvitacion.Id
WHERE ocRequisicion.Id IS NOT NULL OR ocInvitacion.Id IS NOT NULL
GO