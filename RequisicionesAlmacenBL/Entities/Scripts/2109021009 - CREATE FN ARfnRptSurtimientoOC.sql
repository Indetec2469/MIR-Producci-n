SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:		Javier Elías
-- Create date: 02/09/2021
-- Modified date: 
-- Description:	Función para obtener el reporte de Surtimiento
--						de Orden de Compra.
-- ========================================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptSurtimientoOC]
(	
	@reciboId INT,
	@usuarioId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT dbo.RHfnGetNombreCompletoEmpleado(usuarioImpresion.EmpleadoId) AS Usuario,
		   'rptSurtimientoOC' AS Reporte,
		   dbo.GRfnGetFechaConFormato(GETDATE(), 0) AS FechaImpresion,
		   REPLACE(dbo.GRfnGetFechaConFormato(GETDATE(), 1), (dbo.GRfnGetFechaConFormato(GETDATE(), 0) + ' '), '') AS HoraImpresion,
		   dbo.GRfnGetFechaConFormato(recibo.Fecha, 0) AS Fecha,
		   0 AS Requisicion,
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
		   reciboDet.Cantidad * reciboDet.Costo AS Total
	FROM tblCompra AS recibo
		 INNER JOIN tblCompraDet AS reciboDet ON recibo.CompraId = reciboDet.CompraId
		 INNER JOIN tblOrdenCompraDet AS ocDetalle ON reciboDet.RefOrdenCompraDetId = ocDetalle.OrdenCompraDetId
		 INNER JOIN tblOrdenCompra AS oc ON ocDetalle.OrdenCompraId = oc.OrdenCompraId
		 INNER JOIN tblProducto AS producto ON ocDetalle.ProductoId = producto.ProductoId
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
		 INNER JOIN tblProveedor AS proveedor ON recibo.ProveedorId = proveedor.ProveedorId
		 INNER JOIN tblAlmacen AS almacen ON recibo.AlmacenId = almacen.AlmacenId
		 INNER JOIN GRtblUsuario AS usuarioImpresion ON usuarioImpresion.UsuarioId = @usuarioId
	WHERE recibo.CompraId = @reciboId
)
GO