SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoOrdenCompra]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 11/08/2021
-- Modified date: 23/02/2022
-- Description:	View para obtener el Listado de Ordenes de Compra
-- ============================================================
SELECT OrdenCompraId,
       dbo.GRfnGetFechaConFormato(Fecha, 0) AS FechaOC,
       proveedor.RazonSocial AS Proveedor,
       ISNULL(MontoOC, 0) AS MontoOC,
	   oc.Status,
	   CASE oc.Status WHEN 'A' THEN 'Activa' WHEN 'I' THEN 'Parcialmente Recibida' WHEN 'R' THEN 'Recibida' WHEN 'C' THEN 'Cancelada' ELSE '' END AS Estatus,
	   CONVERT(BIT, CASE WHEN oc.Status NOT IN ('R', 'C') THEN 1 ELSE 0 END) AS PermiteEditar, -- Activa o Parcialmente Recibida
	   CONVERT(BIT, CASE WHEN oc.Status = 'A' THEN 1 ELSE 0 END) AS PermiteCancelar -- Activa o Parcialmente Recibida
FROM tblOrdenCompra AS oc
     INNER JOIN tblAlmacen AS almacen ON oc.AlmacenId = almacen.AlmacenId
     INNER JOIN tblProveedor AS proveedor ON oc.ProveedorId = proveedor.ProveedorId
     OUTER APPLY 
	 (
		SELECT SUM(Total) AS MontoOC
		FROM tblOrdenCompraDet AS detalle
		WHERE detalle.OrdenCompraId = oc.OrdenCompraId
			  AND detalle.Status != 'C' -- No Cancelado
		GROUP BY detalle.OrdenCompraId
	 ) AS detalles
	 INNER JOIN
	 (
			SELECT DISTINCT
				   oc.OrdenCompraId AS Id
			FROM tblOrdenCompra AS oc
				 INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
				 INNER JOIN ARtblOrdenCompraRequisicionDet AS ocRequisicion ON ocDetalle.OrdenCompraDetId = ocRequisicion.OrdenCompraDetId
				 INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON ocRequisicion.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId
				 INNER JOIN ARtblRequisicionMaterial AS requisicion ON requisicionDetalle.RequisicionMaterialId = requisicion.RequisicionMaterialId AND requisicion.EstatusId != 65 -- AREstatusRequisicion Cancelada
	 ) AS ocRequisicion ON oc.OrdenCompraId = ocRequisicion.Id
WHERE oc.Status != 'C' -- No Cancelada
GO