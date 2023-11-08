SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaDatosFinanciamientoOrdenCompra]
	@ordenCompraId INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 11/08/2021
-- Modified date: 
-- Description:	Procedure para obtener los Datos de Financiamiento
--						de una Orden de Compra
-- ============================================================
SELECT DISTINCT
       CASE WHEN COUNT(OrdenCompraDetId) OVER (PARTITION BY OrdenCompraId) - COUNT(DependenciaId) OVER (PARTITION BY DependenciaId) = 0 THEN DependenciaId ELSE NULL END AS DependenciaId,
       CASE WHEN COUNT(OrdenCompraDetId) OVER (PARTITION BY OrdenCompraId) - COUNT(ProyectoId) OVER (PARTITION BY ProyectoId) = 0 THEN ProyectoId ELSE NULL END AS ProyectoId,
	   CASE WHEN COUNT(OrdenCompraDetId) OVER (PARTITION BY OrdenCompraId) - COUNT(RamoId) OVER (PARTITION BY RamoId) = 0 THEN RamoId ELSE NULL END AS RamoId,
	   CONVERT(BIT, CASE WHEN SUM(Ajuste) OVER (PARTITION BY OrdenCompraId) > 0 THEN 1 ELSE 0 END) AS Ajuste
FROM tblOrdenCompraDet AS detalle
     INNER JOIN tblCuentaPresupuestalEgr AS cp ON detalle.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
WHERE detalle.OrdenCompraId = @ordenCompraId
GO