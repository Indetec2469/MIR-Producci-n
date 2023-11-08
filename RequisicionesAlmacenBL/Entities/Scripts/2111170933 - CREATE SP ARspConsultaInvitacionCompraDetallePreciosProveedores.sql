SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaInvitacionCompraDetallePreciosProveedores]
	@invitacionId INT
AS
-- ================================================
-- Author:		Javier Elías
-- Create date: 11/11/2021
-- Modified date: 
-- Description:	Procedure para obtener los Precios de
--                     una Invitación de Compra
-- ================================================
SELECT InvitacionCompraDetallePrecioProveedorId,
       precio.InvitacionCompraDetalleId,
       detalle.InvitacionCompraId,	   
	   ProductoId,
	   Descripcion,
	   CONVERT(NVARCHAR(10), ProductoId) + ' - ' + Descripcion AS Producto,
	   proveedor.ProveedorId,
       proveedor.RazonSocial,
	   CONVERT(NVARCHAR(10), proveedor.ProveedorId) + ' - ' + proveedor.RazonSocial AS Proveedor,
	   PrecioArticulo,
	   ISNULL(Ganador, CONVERT(BIT, 0)) AS Ganador,
	   Comentario,
	   precio.EstatusId
FROM ArtblInvitacionCompraDetallePrecioProveedor AS precio
     INNER JOIN ARtblInvitacionCompraDetalle AS detalle ON precio.InvitacionCompraDetalleId = detalle.InvitacionCompraDetalleId
     INNER JOIN tblProveedor AS proveedor ON precio.ProveedorId = proveedor.ProveedorId
WHERE detalle.InvitacionCompraId = @invitacionId
	AND precio.EstatusId = 1 -- EstatusRegistro Activo
ORDER BY ProductoId,
         proveedor.ProveedorId
GO