SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOrdenCompraReciboDetalles]
		@compraId INT= 1
AS
-- ================================================================
-- Author:		Javier Elías
-- Create date: 26/08/2021
-- Modified date: 
-- Description:	Procedure para obtener los Detalles del Recibo de una OC
-- ================================================================
SELECT reciboDet.*,
       um.Descripcion AS UM,
	   impuesto.Nombre AS Impuesto,
	   detalle.OrdenCompraId,
       detalle.Cantidad AS CantidadSolicitada,
       ISNULL(recibos.CantidadRecibida, 0) AS CantidadRecibida,
       detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0) AS CantidadPorRecibir,
	   AlmacenProductoId
FROM tblCompra AS recibo
     INNER JOIN tblCompraDet AS reciboDet ON recibo.CompraId = reciboDet.CompraId
     INNER JOIN tblOrdenCompraDet AS detalle ON reciboDet.RefOrdenCompraDetId = detalle.OrdenCompraDetId
	 INNER JOIN tblOrdenCompra AS oc ON detalle.OrdenCompraId = oc.OrdenCompraId
	 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
	 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblTarifaImpuesto AS impuesto ON reciboDet.TarifaImpuestoId = impuesto.TarifaImpuestoId
	 INNER JOIN ARtblAlmacenProducto AS ap ON oc.AlmacenId = ap.AlmacenId AND detalle.ProductoId = ap.ProductoId AND detalle.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId AND ap.Borrado = 0
     OUTER APPLY
	 (
		SELECT reciboDetTemp.RefOrdenCompraDetId,
				SUM(reciboDetTemp.Cantidad) AS CantidadRecibida
		FROM tblCompra AS reciboTemp
				INNER JOIN tblCompraDet AS reciboDetTemp ON reciboTemp.CompraId = reciboDetTemp.CompraId AND reciboDetTemp.RefOrdenCompraDetId = detalle.OrdenCompraDetId
		WHERE reciboTemp.Status != 'C' -- Cancelado
				AND reciboTemp.Fecha < recibo.Fecha
		GROUP BY reciboDetTemp.RefOrdenCompraDetId
	 ) AS recibos
WHERE reciboDet.CompraId = @compraId
ORDER BY reciboDet.CompraDetId
GO