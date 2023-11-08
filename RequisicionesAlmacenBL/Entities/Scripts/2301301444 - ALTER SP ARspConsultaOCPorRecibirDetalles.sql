SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOCPorRecibirDetalles]
AS
-- ==========================================================
-- Author:			Javier Elías
-- Create date:		26/08/2021
-- Modified date:	27/01/2023
-- Description:		Procedure para obtener los Detalles para 
--					el Recibo de una OC
-- ==========================================================
SELECT CONVERT(INT, -1 * ROW_NUMBER() OVER (ORDER BY detalle.OrdenCompraDetId)) AS CompraDetId,
	   CONVERT(INT, -1) AS CompraId,
       detalle.ProductoId,
       detalle.TarifaImpuestoId,
       detalle.OrdenCompraDetId AS RefOrdenCompraDetId,
       detalle.CuentaPresupuestalEgrId,
       detalle.Descripcion,
       detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0) AS Cantidad,
	   detalle.Costo,
	   detalle.Importe, 
	   detalle.IEPS, 
	   detalle.Ajuste, 
	   detalle.IVA, 
	   detalle.ISH, 
	   detalle.RetencionISR, 
	   detalle.RetencionCedular, 
	   detalle.RetencionIVA, 
	   detalle.TotalPresupuesto, 
	   detalle.Total, 
	   um.Descripcion AS UM,
	   impuesto.Nombre AS Impuesto,
       detalle.OrdenCompraId,
	   detalle.Cantidad AS CantidadSolicitada,
	   ISNULL(recibos.CantidadRecibida, 0) AS CantidadRecibida,
	   detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0) AS CantidadPorRecibir,
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
FROM tblOrdenCompra AS oc
     INNER JOIN tblOrdenCompraDet AS detalle ON oc.OrdenCompraId = detalle.OrdenCompraId AND detalle.Status != 'C' -- Cancelado
	 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
	 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblTarifaImpuesto AS impuesto ON detalle.TarifaImpuestoId = impuesto.TarifaImpuestoId
	 LEFT JOIN ARtblAlmacenProducto AS ap ON oc.AlmacenId = ap.AlmacenId AND detalle.ProductoId = ap.ProductoId AND detalle.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId AND ap.Borrado = 0
	 INNER JOIN tblCuentaPresupuestalEgr AS cp ON detalle.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
	 INNER JOIN tblDependencia AS unidadAdministrativa ON cp.DependenciaId = unidadAdministrativa.DependenciaId
     INNER JOIN tblProyecto AS proyecto ON cp.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblRamo AS fuenteFinanciamiento ON cp.RamoId = fuenteFinanciamiento.RamoId
     INNER JOIN tblTipoGasto AS tipoGasto ON cp.TipoGastoId = tipoGasto.TipoGastoId
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
WHERE oc.Status != 'C' -- Cancelada
      AND detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0) > 0
	  AND oc.OrdenCompraId = 1275
ORDER BY oc.OrdenCompraId,
         detalle.OrdenCompraDetId