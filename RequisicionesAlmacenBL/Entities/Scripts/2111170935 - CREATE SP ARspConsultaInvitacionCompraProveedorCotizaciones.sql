SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaInvitacionCompraProveedorCotizaciones]
	@invitacionId INT
AS
-- ================================================
-- Author:		Javier Elías
-- Create date: 16/11/2021
-- Modified date: 
-- Description:	Procedure para obtener las Cotizaciones de
--                     Proveedores de una Invitación de Compra
-- ================================================
SELECT cotizacion.InvitacionCompraProveedorCotizacionId,
       cotizacion.InvitacionCompraProveedorId,
       cotizacion.CotizacionId,
       cotizacion.FechaCreacion AS FechaCotizacion,
	   proveedor.RazonSocial AS Proveedor,
	   archivo.NombreOriginal AS NombreArchivo,
	   '' AS CreadoPorUsuario,
	   tipoArchivo.Valor AS Tipo,
	   NULL AS NombreArchivoTmp
FROM ARtblInvitacionCompra AS invitacion
     INNER JOIN ARtblInvitacionCompraProveedor AS invitacionProveedor ON invitacion.InvitacionCompraId = invitacionProveedor.InvitacionCompraId AND invitacionProveedor.EstatusId = 1 -- Activo
	 INNER JOIN tblProveedor AS proveedor ON invitacionProveedor.ProveedorId = proveedor.ProveedorId
     INNER JOIN ArtblInvitacionCompraProveedorCotizacion AS cotizacion ON invitacionProveedor.InvitacionCompraProveedorId = cotizacion.InvitacionCompraProveedorId AND cotizacion.EstatusId = 1 -- Activo
	 INNER JOIN GRtblArchivo AS archivo ON cotizacion.CotizacionId = archivo.ArchivoId AND archivo.Vigente = 1
	 INNER JOIN GRtblControlMaestro AS tipoArchivo ON archivo.TipoArchivoId = tipoArchivo.ControlId
WHERE invitacion.InvitacionCompraId = @invitacionId
ORDER BY cotizacion.InvitacionCompraProveedorCotizacionId
GO