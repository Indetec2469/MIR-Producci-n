SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaDatosOCReciboPorId]
		@ordenCompraId INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 25/08/2021
-- Modified date: 22/09/2021
-- Description:	Procedimiento para Obtener las Ordenes de Compra para
--						agregar al combo de Productos de la Ficha de Recibo de OC.
-- ============================================================
SELECT oc.OrdenCompraId,
       dbo.GRfnGetFechaConFormato(oc.Fecha, 0) AS FechaOC,
	   oc.ProveedorId,
	   oc.TipoComprobanteFiscalId,
	   oc.AlmacenId,
	   TipoOperacionId,
	   oc.Status,
	   CASE oc.Status WHEN 'A' THEN 'Activa' WHEN 'I' THEN 'Parcialmente Recibida' WHEN 'R' THEN 'Recibida' WHEN 'C' THEN 'Cancelada' ELSE '' END AS EstatusOC
FROM tblOrdenCompra AS oc
WHERE oc.OrdenCompraId = @ordenCompraId
ORDER BY oc.OrdenCompraId
GO