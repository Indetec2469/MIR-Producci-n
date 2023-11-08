SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoInvitacionCompra]
AS
-- ====================================================================
-- Author:		Javier Elías
-- Create date: 29/10/2021
-- Modified date: 22/11/2021
-- Description:	View para obtener el Listado de las Invitaciones de Compra
-- ====================================================================
SELECT invitacion.InvitacionCompraId,
       CodigoInvitacion,
       dbo.GRfnGetFechaConFormato(Fecha, 0) AS Fecha,
       almacen.AlmacenId,
       almacen.Nombre AS Almacen,
       ISNULL(proveedores.Contador, 0) AS Proveedores,
	   ISNULL(cotizaciones.Contador, 0) AS Cotizaciones,
       MontoInvitacion,
       invitacion.EstatusId,
       estatus.Valor AS Estatus,
       invitacion.Timestamp,
	   CONVERT(BIT, CASE WHEN invitacion.EstatusId IN (92, 105) THEN 1 ELSE 0 END) AS PermiteEditar, -- Guardada, Convertida Parcialmente
	   CONVERT(BIT, CASE WHEN invitacion.EstatusId = 92 THEN 1 ELSE 0 END) AS PermiteCancelar -- Guardada
FROM ARtblInvitacionCompra AS invitacion
     INNER JOIN tblAlmacen AS almacen ON invitacion.AlmacenId = almacen.AlmacenId
	 INNER JOIN GRtblControlMaestro AS estatus ON invitacion.EstatusId = estatus.ControlId
	 LEFT JOIN
	 (
			SELECT InvitacionCompraId, COUNT(InvitacionCompraProveedorId) AS Contador
			FROM ARtblInvitacionCompraProveedor
			WHERE EstatusId = 1 -- Activo
			GROUP BY InvitacionCompraId
	 ) AS proveedores ON invitacion.InvitacionCompraId = proveedores.InvitacionCompraId
	 LEFT JOIN
	 (
			SELECT InvitacionCompraId, COUNT(InvitacionCompraProveedorCotizacionId) AS Contador
			FROM ArtblInvitacionCompraProveedorCotizacion AS cotizacion
					INNER JOIN ARtblInvitacionCompraProveedor AS proveedor ON cotizacion.InvitacionCompraProveedorId = proveedor.InvitacionCompraProveedorId AND proveedor.EstatusId = 1 -- Activo
			WHERE cotizacion.EstatusId = 1 -- Activo
			GROUP BY InvitacionCompraId
	 ) AS cotizaciones ON invitacion.InvitacionCompraId = cotizaciones.InvitacionCompraId
WHERE invitacion.EstatusId != 91 -- No Canceladas
GO