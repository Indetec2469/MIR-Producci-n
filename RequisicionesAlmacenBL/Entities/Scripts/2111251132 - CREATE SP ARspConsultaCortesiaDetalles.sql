SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaCortesiaDetalles]
	@cortesiaId INT
AS
-- ========================================================
-- Author:		Javier Elías
-- Create date: 25/11/2021
-- Modified date: 
-- Description:	Procedure para obtener los detalles de una Cortesía
-- ========================================================
SELECT detalle.CortesiaDetalleId,
        detalle.CortesiaId,
        NumeroPartida,
        detalle.ProductoId,
        detalle.Descripcion,
        detalle.CuentaPresupuestalEgrId,
        detalle.UnidadMedidaId,
        detalle.Cantidad,
        detalle.PrecioUnitario,
        detalle.TotalPartida,
		ap.AlmacenProductoId,
        um.UnidadDeMedidaId,
        um.Descripcion AS UnidadDeMedida,
        ua.RamoId AS UnidadAdministrativaId,
        ua.Nombre AS UnidadAdministrativa,
        p.ProyectoId,
        p.Nombre AS Proyecto,
        d.DependenciaId AS FuenteFinanciamientoId,
        d.Nombre AS FuenteFinanciamiento,
        tg.TipoGastoId,
        tg.Nombre AS TipoGasto
FROM ARtblCortesia AS cortesia
     INNER JOIN ARtblCortesiaDetalle AS detalle ON cortesia.CortesiaId = detalle.CortesiaId
	 INNER JOIN ARtblAlmacenProducto AS ap ON cortesia.AlmacenId = ap.AlmacenId AND detalle.ProductoId = ap.ProductoId AND detalle.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId
	 INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblCuentaPresupuestalEgr AS cp ON detalle.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
	 INNER JOIN tblRamo AS ua ON cp.RamoId = ua.RamoId
	 INNER JOIN tblProyecto AS p ON cp.ProyectoId = p.ProyectoId
	 INNER JOIN tblDependencia AS d ON cp.DependenciaId = d.DependenciaId
	 INNER JOIN tblTipoGasto AS tg ON cp.TipoGastoId = tg.TipoGastoId
WHERE cortesia.CortesiaId = @cortesiaId
ORDER BY detalle.NumeroPartida
GO