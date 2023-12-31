SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:		Javier El�as
-- Create date: 02/09/2021
-- Modified date: 30/05/2022
-- Description:	Funci�n para obtener el reporte de Surtimiento
--						de Orden de Compra.
-- ========================================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptSurtimientoOC]
(	
	@reciboId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT dbo.GRfnGetFechaConFormato(recibo.Fecha, 0) AS Fecha,
		   Requisicion = STUFF(
			   (
					SELECT DISTINCT
						   ', ' + CONVERT(VARCHAR(10), rd.RequisicionMaterialId)
					FROM tblCompra AS c
						 INNER JOIN tblCompraDet AS cd ON c.CompraId = cd.CompraId
						 INNER JOIN tblOrdenCompraDet AS ocd ON cd.RefOrdenCompraDetId = ocd.OrdenCompraDetId
						 INNER JOIN ARtblOrdenCompraRequisicionDet AS ocr ON ocd.OrdenCompraDetId = ocr.OrdenCompraDetId
						 INNER JOIN ARtblRequisicionMaterialDetalle AS rd ON ocr.RequisicionMaterialDetalleId = rd.RequisicionMaterialDetalleId
					WHERE c.CompraId = @reciboId 
					ORDER BY ', ' + CONVERT(VARCHAR(10), rd.RequisicionMaterialId)
					FOR XML PATH('')
			   ), 1, 1, ''),
		   oc.OrdenCompraId AS OrdenCompra,
		   FolioFactura AS Factura,
		   almacen.Nombre AS Almacen,
		   UnidadAdministrativa = STUFF(
			   (
					SELECT DISTINCT
						   ', ' + p.DependenciaId
					FROM tblCompra AS r
						 INNER JOIN tblCompraDet AS d ON r.CompraId = d.CompraId
						 INNER JOIN tblCuentaPresupuestalEgr AS cp ON d.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
						 INNER JOIN tblDependencia AS p ON cp.DependenciaId = p.DependenciaId
					WHERE r.CompraId = @reciboId 
					ORDER BY ', ' + p.DependenciaId 
					FOR XML PATH('')
			   ), 1, 1, ''),
		   Proyecto = STUFF(
			   (
					SELECT DISTINCT
						   ', ' + p.ProyectoId
					FROM tblCompra AS r
						 INNER JOIN tblCompraDet AS d ON r.CompraId = d.CompraId
						 INNER JOIN tblCuentaPresupuestalEgr AS cp ON d.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
						 INNER JOIN tblProyecto AS p ON cp.ProyectoId = p.ProyectoId
					WHERE r.CompraId = @reciboId 
					ORDER BY ', ' + p.ProyectoId 
					FOR XML PATH('')
			   ), 1, 1, ''),
			FuenteFinanciamiento = STUFF(
			   (
					SELECT DISTINCT
						   ', ' + p.RamoId
					FROM tblCompra AS r
						 INNER JOIN tblCompraDet AS d ON r.CompraId = d.CompraId
						 INNER JOIN tblCuentaPresupuestalEgr AS cp ON d.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
						 INNER JOIN tblRamo AS p ON cp.RamoId = p.RamoId
					WHERE r.CompraId = @reciboId 
					ORDER BY ', ' + p.RamoId 
					FOR XML PATH('')
			   ), 1, 1, ''),			
		   ISNULL(recibo.Observaciones, '') AS Observaciones,
		   proveedor.RFC,
		   proveedor.RazonSocial,
		   ISNULL(proveedor.Telefono1, '') AS Telefono,
		   ISNULL(proveedor.Domicilio, '') AS Domicilio,
		   ISNULL(proveedor.Email, '') AS Correo,
		   ISNULL(proveedor.Observaciones, '') AS ProveedorObservaciones,
		   producto.ProductoId,
		   producto.Descripcion AS Producto,
		   um.Descripcion AS UnidadDeMedida,
		   reciboDet.Cantidad,
		   reciboDet.Costo,
		   reciboDet.Cantidad * reciboDet.Costo AS Total,
		   im.InventarioMovtoAgrupadorId AS Entrada
	FROM tblCompra AS recibo
		 INNER JOIN tblCompraDet AS reciboDet ON recibo.CompraId = reciboDet.CompraId
		 INNER JOIN tblOrdenCompraDet AS ocDetalle ON reciboDet.RefOrdenCompraDetId = ocDetalle.OrdenCompraDetId
		 INNER JOIN tblOrdenCompra AS oc ON ocDetalle.OrdenCompraId = oc.OrdenCompraId
		 INNER JOIN tblProducto AS producto ON ocDetalle.ProductoId = producto.ProductoId
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
		 INNER JOIN tblProveedor AS proveedor ON recibo.ProveedorId = proveedor.ProveedorId
		 INNER JOIN tblAlmacen AS almacen ON recibo.AlmacenId = almacen.AlmacenId
		 INNER JOIN ARtblInventarioMovimientoAgrupador AS im ON recibo.CompraId = im.ReferenciaMovtoId AND im.TipoMovimientoId = 95 -- Orden de Compra Recibo
	WHERE recibo.CompraId = @reciboId
)
GO