SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoReciboCortesia]
AS
-- ===========================================================
-- Author:		Javier Elías
-- Create date: 23/11/2021
-- Modified date: 
-- Description:	View para obtener el Listado de los Recibos de Cortesía
-- ===========================================================
SELECT cortesia.CortesiaId,
       cortesia.Codigo,
       dbo.GRfnGetFechaConFormato(cortesia.Fecha, 0) AS Fecha,
       proveedor.RazonSocial AS Proveedor,
       almacen.Nombre AS Almacen,
       ISNULL(CONVERT(VARCHAR(6), cortesia.OrdenCompraId), '') AS OC,
       cortesia.TotalCortesia
FROM ARtblCortesia AS cortesia
     INNER JOIN tblProveedor AS proveedor ON cortesia.ProveedorId = proveedor.ProveedorId
	 INNER JOIN tblAlmacen AS almacen ON cortesia.AlmacenId = almacen.AlmacenId
WHERE cortesia.EstatusId = 1 -- Activo
GO