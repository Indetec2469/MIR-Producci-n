SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspValidarRequisicionOC]
	@ocId INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 23/02/2022
-- Modified date: 
-- Description:	Procedure para validar si una OC viene de una Requisición
-- ============================================================
SELECT DISTINCT
       oc.OrdenCompraId AS Id
FROM tblOrdenCompra AS oc
     INNER JOIN tblOrdenCompraDet AS ocDetalle ON oc.OrdenCompraId = ocDetalle.OrdenCompraId
     INNER JOIN ARtblOrdenCompraRequisicionDet AS ocRequisicion ON ocDetalle.OrdenCompraDetId = ocRequisicion.OrdenCompraDetId
     INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON ocRequisicion.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId
     INNER JOIN ARtblRequisicionMaterial AS requisicion ON requisicionDetalle.RequisicionMaterialId = requisicion.RequisicionMaterialId AND requisicion.EstatusId != 65 -- AREstatusRequisicion Cancelada
WHERE oc.OrdenCompraId = @ocId
GO