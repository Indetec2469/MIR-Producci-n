SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaInvitacionCompraOrdenesCompra]
	@invitacionId INT
AS
-- ================================================
-- Author:		Javier Elías
-- Create date: 20/11/2021
-- Modified date: 
-- Description:	Procedure para obtener las OC de
--                     una Invitación de Compra
-- ================================================
SELECT oc.OrdenCompraId,
       proveedor.ProveedorId,
       proveedor.RazonSocial AS Proveedor,
       almacen.AlmacenId,
       almacen.Nombre AS Almacen,
       CONVERT(varchar,oc.FechaRecepcion,111) AS FechaRecepcion,
       oc.Referencia,
       oc.Observacion,
       SUM(ocDetalle.Total) AS Monto
FROM ARtblInvitacionCompra AS invitacion
     INNER JOIN ARtblInvitacionCompraDetalle AS invitacionDetalle ON invitacion.InvitacionCompraId = invitacionDetalle.InvitacionCompraId
     INNER JOIN ARtblOrdenCompraInvitacionDet AS ocInvitacionDetalle ON invitacionDetalle.InvitacionCompraDetalleId = ocInvitacionDetalle.InvitacionCompraDetalleId
     INNER JOIN tblOrdenCompraDet AS ocDetalle ON ocInvitacionDetalle.OrdenCompraDetId = ocDetalle.OrdenCompraDetId
     INNER JOIN tblOrdenCompra AS oc ON ocDetalle.OrdenCompraId = oc.OrdenCompraId
     INNER JOIN tblProveedor AS proveedor ON oc.ProveedorId = proveedor.ProveedorId
     INNER JOIN tblAlmacen AS almacen ON oc.AlmacenId = almacen.AlmacenId
WHERE invitacion.InvitacionCompraId = @invitacionId
GROUP BY oc.OrdenCompraId,
         proveedor.ProveedorId,
         proveedor.RazonSocial,
         almacen.AlmacenId,
         almacen.Nombre,
         oc.FechaRecepcion,
         oc.Referencia,
         oc.Observacion
ORDER BY oc.OrdenCompraId
GO