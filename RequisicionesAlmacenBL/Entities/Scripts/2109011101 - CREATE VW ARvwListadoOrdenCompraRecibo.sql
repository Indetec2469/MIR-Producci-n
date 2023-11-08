SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoOrdenCompraRecibo]
AS
-- ====================================================================
-- Author:		Javier Elías
-- Create date: 24/08/2021
-- Modified date: 
-- Description:	View para obtener el Listado de los Recibos de Ordenes de Compra
-- ====================================================================
SELECT recibo.CompraId,
       dbo.GRfnGetFechaConFormato(detalles.Fecha, 0) AS FechaRecibo,
	   detalles.OrdenCompraId AS CodigoOC,	   
       dbo.GRfnGetFechaConFormato(detalles.Fecha, 0) AS FechaOC,
	   proveedor.RazonSocial AS Proveedor,
       CASE detalles.Status WHEN 'A' THEN 'Activa' WHEN 'I' THEN 'Parcialmente Recibida' WHEN 'R' THEN 'Recibida' WHEN 'C' THEN 'Cancelada' ELSE '' END AS Estatus
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
WHERE recibo.Status != 'C' -- Cancelada
GO