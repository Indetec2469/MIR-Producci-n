SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaInvitacionCompraListadoProveedores]
	@invitacionId INT
AS
-- =======================================================
-- Author:		Javier Elías
-- Create date: 03/11/2021
-- Modified date: 
-- Description:	Procedure para obtener el listado de los
--                     Proveedores para la Ficha de Invitación de Compra
-- =======================================================
SELECT CONVERT(BIT, CASE WHEN InvitacionCompraProveedorId IS NOT NULL THEN 1 ELSE 0 END) AS Seleccionado,
	   ISNULL(InvitacionCompraProveedorId, -1 * ROW_NUMBER() OVER (ORDER BY RazonSocial, RFC)) AS InvitacionCompraProveedorId,
	   @invitacionId AS InvitacionCompraId,
	   proveedor.ProveedorId,
       REPLACE(RazonSocial, '.', ' ') AS RazonSocial,
       RFC,
	   CONVERT(VARCHAR(5), ISNULL(CotizacionesGanadas, 0)) + '/' + CONVERT(VARCHAR(5), ISNULL(NoCotizaciones, 0)) AS Invitaciones,
	   TipoOperacionId,
	   TipoComprobanteFiscalId
FROM tblProveedor AS proveedor
	LEFT JOIN ARtblInvitacionCompraProveedor AS invitacionProveedor ON proveedor.ProveedorId = invitacionProveedor.ProveedorId AND invitacionProveedor.InvitacionCompraId = @invitacionId AND invitacionProveedor.EstatusId = 1	 -- EstatusRegistro Activo
	LEFT JOIN ArtblInvitacionCompra AS invitacion ON invitacionProveedor.InvitacionCompraId = invitacion.InvitacionCompraId
	OUTER APPLY
	(
			SELECT COUNT(precioTmp.InvitacionCompraDetallePrecioProveedorId) AS NoCotizaciones
			FROM ARtblInvitacionCompra AS invitacionTmp
				 INNER JOIN ARtblInvitacionCompraDetalle AS detalleTmp ON invitacionTmp.InvitacionCompraId = detalleTmp.InvitacionCompraId AND detalleTmp.EstatusId != 94	 -- EstatusRegistro No Cancelados
				 INNER JOIN ArtblInvitacionCompraDetallePrecioProveedor AS precioTmp ON detalleTmp.InvitacionCompraDetalleId = precioTmp.InvitacionCompraDetalleId AND precioTmp.EstatusId = 1	 -- EstatusRegistro Activo
				 INNER JOIN ARtblInvitacionCompra AS invitacionActual ON invitacionActual.InvitacionCompraId = @invitacionId
			WHERE invitacionTmp.Fecha < invitacionActual.Fecha
				  AND precioTmp.ProveedorId = proveedor.ProveedorId
				  AND invitacionTmp.EstatusId != 91 -- No canceladas
	) AS NoCotizaciones
	OUTER APPLY
	(
			SELECT COUNT(precioTmp.InvitacionCompraDetallePrecioProveedorId) AS CotizacionesGanadas
			FROM ARtblInvitacionCompra AS invitacionTmp
				 INNER JOIN ARtblInvitacionCompraDetalle AS detalleTmp ON invitacionTmp.InvitacionCompraId = detalleTmp.InvitacionCompraId AND detalleTmp.EstatusId != 94	 -- EstatusRegistro No Cancelados
				 INNER JOIN ArtblInvitacionCompraDetallePrecioProveedor AS precioTmp ON detalleTmp.InvitacionCompraDetalleId = precioTmp.InvitacionCompraDetalleId AND precioTmp.EstatusId = 1	 -- EstatusRegistro Activo
				 INNER JOIN ARtblInvitacionCompra AS invitacionActual ON invitacionActual.InvitacionCompraId = @invitacionId
			WHERE invitacionTmp.Fecha < invitacionActual.Fecha
				  AND precioTmp.ProveedorId = proveedor.ProveedorId
				  AND precioTmp.Ganador = 1
				  AND invitacionTmp.EstatusId != 91 -- No canceladas
	) AS CotizacionesGanadas
WHERE Status = 'A'
ORDER BY RazonSocial,
              RFC
GO