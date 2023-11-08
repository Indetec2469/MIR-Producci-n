SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOCPorRecibir]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 25/08/2021
-- Modified date: 
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
	   ISNULL(recibos.CantidadRecibida, 0) AS CantidadRecibida
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
WHERE oc.Status NOT IN ('R', 'C') -- Recibida, Cancelada
      AND detalles.CantidadSolicitada - ISNULL(recibos.CantidadRecibida, 0) > 0
ORDER BY oc.OrdenCompraId
GO