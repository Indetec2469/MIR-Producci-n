SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspValidarRequisicionOC]
	@ocId INT
AS
-- ====================================================
-- Author:		Javier Elías
-- Create date: 23/02/2022
-- Modified date: 27/05/2022
-- Description:	Procedure para validar si una OC viene de 
--						una Requisición o Invitación
-- ====================================================
SELECT DISTINCT
       ISNULL(ocRequisicion.Id, ocInvitacion.Id) AS Id
FROM tblOrdenCompra AS oc
     INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
	 LEFT JOIN
	 (
			SELECT DISTINCT
				   oc.OrdenCompraId AS Id
			FROM tblOrdenCompra AS oc
				 INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
				 INNER JOIN ARtblOrdenCompraRequisicionDet AS ocRequisicion ON ocDetalle.OrdenCompraDetId = ocRequisicion.OrdenCompraDetId
				 INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON ocRequisicion.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId AND requisicionDetalle.EstatusId != 78 --AREstatusRequisicionDetalle	Cancelado
				 INNER JOIN ARtblRequisicionMaterial AS requisicion ON requisicionDetalle.RequisicionMaterialId = requisicion.RequisicionMaterialId AND requisicion.EstatusId != 65 -- AREstatusRequisicion Cancelada
	 ) AS ocRequisicion ON oc.OrdenCompraId = ocRequisicion.Id
	 LEFT JOIN
	 (
			SELECT DISTINCT
				   oc.OrdenCompraId AS Id
			FROM tblOrdenCompra AS oc
				 INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
				 INNER JOIN ARtblOrdenCompraInvitacionDet AS ocInvitacion ON ocDetalle.OrdenCompraDetId = ocInvitacion.OrdenCompraDetId
				 INNER JOIN ARtblInvitacionCompraDetalle AS invitacionDetalle ON ocInvitacion.InvitacionCompraDetalleId = invitacionDetalle.InvitacionCompraDetalleId AND invitacionDetalle.EstatusId != 94 --AREstatusInvitacionCompraDetalle	Cancelado
				 INNER JOIN ARtblInvitacionCompra AS invitacion ON invitacionDetalle.InvitacionCompraId = invitacion.InvitacionCompraId AND invitacion.EstatusId != 91 --AREstatusInvitacionCompra	Cancelada
	 ) AS ocInvitacion ON oc.OrdenCompraId = ocInvitacion.Id
WHERE oc.OrdenCompraId = @ocId
GO