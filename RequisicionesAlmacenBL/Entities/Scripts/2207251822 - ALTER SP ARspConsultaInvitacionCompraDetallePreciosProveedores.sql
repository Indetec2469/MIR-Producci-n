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
-- Modified date: 25/07/2022
-- Description:	Procedure para obtener los Precios de
--                     una Invitación de Compra
-- ================================================
SELECT InvitacionCompraDetallePrecioProveedorId,
       precio.InvitacionCompraDetalleId,
       detalle.InvitacionCompraId,	   
	   ProductoId,
	   Descripcion,
	   CONVERT(NVARCHAR(10), ProductoId) + ' - ' + Descripcion AS Producto,
	   ISNULL(proveedor.ProveedorId, (2000000 + prospecto.ProveedorProspectoId)) AS ProveedorId,
       ISNULL(proveedor.RazonSocial, prospecto.RazonSocial) AS RazonSocial,
	   CONVERT(NVARCHAR(10), ISNULL(proveedor.ProveedorId, (2000000 + prospecto.ProveedorProspectoId))) + ' - ' + ISNULL(proveedor.RazonSocial, prospecto.RazonSocial) AS Proveedor,
	   PrecioArticulo,
	   ISNULL(Ganador, CONVERT(BIT, 0)) AS Ganador,
	   ComentarioGanador,
	   ComentarioCotizacion,
	   precio.EstatusId
FROM ArtblInvitacionCompraDetallePrecioProveedor AS precio
     INNER JOIN ARtblInvitacionCompraDetalle AS detalle ON precio.InvitacionCompraDetalleId = detalle.InvitacionCompraDetalleId
     LEFT JOIN tblProveedor AS proveedor ON precio.ProveedorId = proveedor.ProveedorId
	 LEFT JOIN ARtblProveedorProspecto AS prospecto ON precio.ProveedorId = (2000000 + prospecto.ProveedorProspectoId)
WHERE detalle.InvitacionCompraId = @invitacionId
	AND precio.EstatusId = 1 -- EstatusRegistro Activo
ORDER BY ProductoId,
         proveedor.ProveedorId
GO