SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaInvitacionCompraDetalles]
	@invitacionId INT
AS
-- ================================================
-- Author:		Javier Elías
-- Create date: 01/11/2021
-- Modified date: 20/11/2021
-- Description:	Procedure para obtener los Detalles de
--                     una Invitación de Compra
-- ================================================
SELECT InvitacionCompraDetalleId,
       InvitacionCompraId,
       requisicion.RequisicionMaterialId,
       CodigoRequisicion,
       dbo.GRfnGetFechaConFormato(FechaRequisicion, 0) AS FechaRequisicion,
       invitacionCompraDet.ProductoId,
       invitacionCompraDet.Descripcion,
       invitacionCompraDet.Cantidad,
       invitacionCompraDet.Costo,
       invitacionCompraDet.Importe,
	   invitacionCompraDet.Timestamp,
	   invitacionCompraDet.TarifaImpuestoId,
	   invitacionCompraDet.CuentaPresupuestalEgrId,
	   invitacionCompraDet.IEPS,
	   invitacionCompraDet.Ajuste,
	   invitacionCompraDet.IVA,
	   invitacionCompraDet.ISH,
	   invitacionCompraDet.RetencionISR,
	   invitacionCompraDet.RetencionCedular,
	   invitacionCompraDet.RetencionIVA,
	   invitacionCompraDet.TotalPresupuesto,
	   invitacionCompraDet.Total,
	   invitacionCompraDet.EstatusId,
	   invitacionCompraDet.InvitacionArticuloDetalleId,
	   CONVERT(BIT, CASE WHEN invitacionCompraDet.EstatusId = 93 THEN 1 ELSE 0 END) AS PermiteEditar -- Activos
FROM ARtblInvitacionCompraDetalle AS invitacionCompraDet
     INNER JOIN ARtblInvitacionArticuloDetalle AS invitacionArticuloDet ON invitacionCompraDet.InvitacionArticuloDetalleId = invitacionArticuloDet.InvitacionArticuloDetalleId
     INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDet ON invitacionArticuloDet.RequisicionMaterialDetalleId = requisicionDet.RequisicionMaterialDetalleId
     INNER JOIN ARtblRequisicionMaterial AS requisicion ON requisicionDet.RequisicionMaterialId = requisicion.RequisicionMaterialId
WHERE InvitacionCompraId = @invitacionId
	AND invitacionCompraDet.EstatusId != 94	 --No Cancelados
ORDER BY invitacionCompraDet.InvitacionCompraDetalleId
GO