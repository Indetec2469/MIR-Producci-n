SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOrdenCompraReciboDetalles]
		@compraId INT
AS
-- ================================================================
-- Author:		Javier Elías
-- Create date: 26/08/2021
-- Modified date: 22/09/2021
-- Description:	Procedure para obtener los Detalles del Recibo de una OC
-- ================================================================
SELECT reciboDet.*,
       um.Descripcion AS UM,
	   impuesto.Nombre AS Impuesto,
	   detalle.OrdenCompraId,
       detalle.Cantidad AS CantidadSolicitada,
       ISNULL(CONVERT(FLOAT, ISNULL(recibos.CantidadRecibida, 0)), 0) AS CantidadRecibida,
       ISNULL(detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0), 0) AS CantidadPorRecibir,
	   ISNULL(AlmacenProductoId, -1) AS AlmacenProductoId,
	   RequisicionMaterialId,
	   Solicitud,
	   ISNULL(RequisicionTimestamp, 0x0000000000000000) AS RequisicionTimestamp,
	   RequisicionMaterialDetalleId,
	   ISNULL(DetalleTimestamp, 0x0000000000000000) AS DetalleTimestamp,
	   unidadAdministrativa.DependenciaId AS UnidadAdministrativaId,
	   unidadAdministrativa.Nombre AS UnidadAdministrativa,
	   proyecto. ProyectoId,
	   proyecto.Nombre AS Proyecto,
	   fuenteFinanciamiento.RamoId AS FuenteFinanciamientoId,
	   fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
	   tipoGasto.TipoGastoId,
	   tipoGasto.Nombre AS TipoGasto
FROM tblCompra AS recibo
     INNER JOIN tblCompraDet AS reciboDet ON recibo.CompraId = reciboDet.CompraId
     INNER JOIN tblOrdenCompraDet AS detalle ON reciboDet.RefOrdenCompraDetId = detalle.OrdenCompraDetId
	 INNER JOIN tblOrdenCompra AS oc ON detalle.OrdenCompraId = oc.OrdenCompraId
	 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
	 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblTarifaImpuesto AS impuesto ON reciboDet.TarifaImpuestoId = impuesto.TarifaImpuestoId
	 LEFT JOIN ARtblAlmacenProducto AS ap ON oc.AlmacenId = ap.AlmacenId AND detalle.ProductoId = ap.ProductoId AND detalle.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId AND ap.Borrado = 0
	 INNER JOIN tblCuentaPresupuestalEgr AS cp ON detalle.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
	 INNER JOIN tblDependencia AS unidadAdministrativa ON cp.DependenciaId = unidadAdministrativa.DependenciaId
     INNER JOIN tblProyecto AS proyecto ON cp.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblRamo AS fuenteFinanciamiento ON cp.RamoId = fuenteFinanciamiento.RamoId
     INNER JOIN tblTipoGasto AS tipoGasto ON cp.TipoGastoId = tipoGasto.TipoGastoId
     OUTER APPLY
	 (
		SELECT RefOrdenCompraDetId,
			   SUM(CantidadMovimiento) AS CantidadRecibida
		FROM ARtblInventarioMovimientoAgrupador AS agrupador
			 INNER JOIN ARtblInventarioMovimiento AS movimiento ON agrupador.InventarioMovtoAgrupadorId = movimiento.InventarioMovtoAgrupadorId
			 INNER JOIN tblCompraDet AS reciboDetTemp ON movimiento.ReferenciaMovtoId = reciboDetTemp.CompraDetId
														 AND reciboDetTemp.RefOrdenCompraDetId = detalle.OrdenCompraDetId
														 AND reciboDetTemp.CompraId < @compraId
		GROUP BY RefOrdenCompraDetId
	 ) AS recibos
	 OUTER APPLY
	 (
		 SELECT DISTINCT
			   r.RequisicionMaterialId,
			   r.Timestamp AS RequisicionTimestamp,
			   r.CodigoRequisicion AS Solicitud,
			   rd.RequisicionMaterialDetalleId,
			   rd.Timestamp AS DetalleTimestamp
		FROM ARtblRequisicionMaterialDetalle AS rd
			 INNER JOIN ARtblRequisicionMaterial AS r ON rd.RequisicionMaterialId = r.RequisicionMaterialId
			 INNER JOIN ARtblOrdenCompraRequisicionDet AS ocr ON rd.RequisicionMaterialDetalleId = ocr.RequisicionMaterialDetalleId
																 AND ocr.OrdenCompraDetId = detalle.OrdenCompraDetId
	 ) AS requisicion
WHERE reciboDet.CompraId = @compraId
ORDER BY reciboDet.CompraDetId
GO