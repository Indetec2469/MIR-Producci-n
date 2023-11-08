SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoOrdenCompraRecibo]
AS
-- ====================================================================
-- Author:		Javier Elías
-- Create date: 24/08/2021
-- Modified date: 08/03/2022
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
     OUTER APPLY
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
GO