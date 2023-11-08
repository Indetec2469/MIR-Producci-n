SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOCPorRecibirDetalles]
AS
-- ================================================================
-- Author:		Javier Elías
-- Create date: 26/08/2021
-- Modified date: 
-- Description:	Procedure para obtener los Detalles para el Recibo de una OC
-- ================================================================
SELECT CONVERT(INT, -1 * ROW_NUMBER() OVER (ORDER BY detalle.OrdenCompraDetId)) AS CompraDetId,
	   CONVERT(INT, -1) AS CompraId,
       detalle.ProductoId,
       detalle.TarifaImpuestoId,
       detalle.OrdenCompraDetId AS RefOrdenCompraDetId,
       CuentaPresupuestalEgrId,
       detalle.Descripcion,
       CONVERT(FLOAT, NULL) AS Cantidad,
       Costo,
       CONVERT(MONEY, NULL) AS Importe,
       CONVERT(FLOAT, NULL) AS IEPS,
       CONVERT(FLOAT, NULL) AS Ajuste,
	   um.Descripcion AS UM,
	   impuesto.Nombre AS Impuesto,
       detalle.OrdenCompraId,
	   detalle.Cantidad AS CantidadSolicitada,
	   ISNULL(recibos.CantidadRecibida, 0) AS CantidadRecibida,
	   detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0) AS CantidadPorRecibir,
	   AlmacenProductoId
FROM tblOrdenCompra AS oc
     INNER JOIN tblOrdenCompraDet AS detalle ON oc.OrdenCompraId = detalle.OrdenCompraId AND detalle.Status != 'C' -- Cancelado
	 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
	 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblTarifaImpuesto AS impuesto ON detalle.TarifaImpuestoId = impuesto.TarifaImpuestoId
	 INNER JOIN ARtblAlmacenProducto AS ap ON oc.AlmacenId = ap.AlmacenId AND detalle.ProductoId = ap.ProductoId AND detalle.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId AND ap.Borrado = 0
     LEFT JOIN
	 (
		SELECT detalle.OrdenCompraDetId,
			   SUM(reciboDet.Cantidad) AS CantidadRecibida
		FROM tblCompra AS recibo
			 INNER JOIN tblCompraDet AS reciboDet ON recibo.CompraId = reciboDet.CompraId
			 INNER JOIN tblOrdenCompraDet AS detalle ON reciboDet.RefOrdenCompraDetId = detalle.OrdenCompraDetId AND detalle.Status != 'C' -- Cancelado
		WHERE recibo.Status != 'C' -- Cancelado
		GROUP BY detalle.OrdenCompraDetId
	 ) AS recibos ON detalle.OrdenCompraDetId = recibos.OrdenCompraDetId
WHERE oc.Status != 'C' -- Cancelada
      AND detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0) > 0
ORDER BY oc.OrdenCompraId,
         detalle.OrdenCompraDetId
GO