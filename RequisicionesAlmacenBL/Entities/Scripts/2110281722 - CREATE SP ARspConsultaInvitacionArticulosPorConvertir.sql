SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaInvitacionArticulosPorConvertir]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 27/10/2021
-- Modified date: 
-- Description:	Procedimiento para Obtener las Invitaciones y los Artículos
--						que se van a convertir en una Invitación de Compra.
-- ============================================================
SELECT CONVERT(INT, -1 * ROW_NUMBER() OVER(ORDER BY invitacion.InvitacionArticuloId, detalle.InvitacionArticuloDetalleId)) AS InvitacionCompraDetalleId,
       -1 * CONVERT(INT, CONVERT(VARCHAR(10), invitacion.ProveedorId) + invitacion.AlmacenId) AS InvitacionCompraId,
	   invitacion.InvitacionArticuloId,
	   InvitacionArticuloDetalleId,

	   requisicion.CodigoRequisicion AS Requisicion,
	   dbo.GRfnGetFechaConFormato(invitacion.FechaCreacion, 0) AS FechaRegistro,

	   proveedor.ProveedorId,
	   proveedor.RazonSocial AS Proveedor,
	   almacen.AlmacenId,
	   almacen.Nombre AS Almacen,
       
       TarifaImpuestoId,
       detalle.ProductoId,
       detalle.Descripcion,
	   detalle.ProductoId + ' - ' + detalle.Descripcion AS Producto,
	   CuentaPresupuestalEgrId,
       detalle.Cantidad,
       Costo,
       Importe,
       IEPS,
       Ajuste,
       IVA,
       ISH,
       RetencionISR,
       RetencionCedular,
       RetencionIVA,
       TotalPresupuesto,
       Total,

	   invitacion.Timestamp AS InvitacionTimestamp,
	   detalle.Timestamp AS DetalleTimestamp,
	   CONVERT(BIT, 0) AS Seleccionado
FROM ARtblInvitacionArticulo AS invitacion
     INNER JOIN ARtblInvitacionArticuloDetalle AS detalle ON invitacion.InvitacionArticuloId = detalle.InvitacionArticuloId AND detalle.EstatusId = 101	-- Por Invitar
	 INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON detalle.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId
	 INNER JOIN ARtblRequisicionMaterial AS requisicion ON requisicionDetalle.RequisicionMaterialId = requisicion.RequisicionMaterialId
	 INNER JOIN tblProveedor AS proveedor ON invitacion.ProveedorId = proveedor.ProveedorId	
	 INNER JOIN tblAlmacen AS almacen ON invitacion.AlmacenId = almacen.AlmacenId
WHERE invitacion.EstatusId = 99 -- Activa
ORDER BY invitacion.InvitacionArticuloId,
         detalle.InvitacionArticuloDetalleId
GO