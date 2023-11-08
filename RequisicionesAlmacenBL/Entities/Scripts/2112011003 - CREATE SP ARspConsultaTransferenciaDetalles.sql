SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaTransferenciaDetalles]
	@transferenciaId INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 27/11/2021
-- Modified date: 
-- Description:	Procedure para obtener los detalles de una Transferencia
-- ============================================================
SELECT detalle.TransferenciaMovtoId,
        detalle.TransferenciaId,
        detalle.NumeroLinea,
        detalle.ProductoId,
        detalle.Descripcion,
        detalle.UnidadMedidaId,
        um.Descripcion AS UnidadDeMedida,
		detalle.Cantidad,

		detalle.AlmacenProductoOrigenId,
		apOrigen.CuentaPresupuestalId AS CuentaPresupuestalOrigenId,
		apOrigen.UnidadAdministrativaId AS UnidadAdministrativaOrigenId,
		apOrigen.UnidadAdministrativa AS UnidadAdministrativaOrigen,
		apOrigen.ProyectoId AS ProyectoOrigenId,
		apOrigen.Proyecto AS ProyectoOrigen,
		apOrigen.FuenteFinanciamientoId AS FuenteFinanciamientoOrigenId,
		apOrigen.FuenteFinanciamiento AS FuenteFinanciamientoOrigen,
		apOrigen.TipoGastoId AS TipoGastoOrigenId,
		apOrigen.TipoGasto AS TipoGastoOrigen,

		detalle.AlmacenProductoDestinoId,
		apDestino.CuentaPresupuestalId AS CuentaPresupuestalDestinoId,
		apDestino.UnidadAdministrativaId AS UnidadAdministrativaDestinoId,
		apDestino.UnidadAdministrativa AS UnidadAdministrativaDestino,
		apDestino.ProyectoId AS ProyectoDestinoId,
		apDestino.Proyecto AS ProyectoDestino,
		apDestino.FuenteFinanciamientoId AS FuenteFinanciamientoDestinoId,
		apDestino.FuenteFinanciamiento AS FuenteFinanciamientoDestino,
		apDestino.TipoGastoId AS TipoGastoDestinoId,
		apDestino.TipoGasto AS TipoGastoDestino,
		almacenDestino.AlmacenId AS AlmacenDestinoId,
		almacenDestino.Nombre AS AlmacenDestino
FROM ARtblTransferencia AS transferencia
     INNER JOIN ARtblTransferenciaMovto AS detalle ON transferencia.TransferenciaId = detalle.TransferenciaId
	 INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
	 INNER JOIN
	 (
			SELECT ap.AlmacenProductoId,
				   ap.AlmacenId,
				   ap.CuentaPresupuestalId,
				   ua.RamoId AS UnidadAdministrativaId,
				   ua.Nombre AS UnidadAdministrativa,
				   p.ProyectoId,
				   p.Nombre AS Proyecto,
				   d.DependenciaId AS FuenteFinanciamientoId,
				   d.Nombre AS FuenteFinanciamiento,
				   tg.TipoGastoId,
				   tg.Nombre AS TipoGasto
			FROM ARtblAlmacenProducto AS ap
				 INNER JOIN tblCuentaPresupuestalEgr AS cp ON ap.CuentaPresupuestalId = cp.CuentaPresupuestalEgrId
				 INNER JOIN tblRamo AS ua ON cp.RamoId = ua.RamoId
				 INNER JOIN tblProyecto AS p ON cp.ProyectoId = p.ProyectoId
				 INNER JOIN tblDependencia AS d ON cp.DependenciaId = d.DependenciaId
				 INNER JOIN tblTipoGasto AS tg ON cp.TipoGastoId = tg.TipoGastoId
	 ) AS apOrigen ON detalle.AlmacenProductoOrigenId = apOrigen.AlmacenProductoId
	 INNER JOIN
	 (
			SELECT ap.AlmacenProductoId,
				   ap.AlmacenId,
				   ap.CuentaPresupuestalId,
				   ua.RamoId AS UnidadAdministrativaId,
				   ua.Nombre AS UnidadAdministrativa,
				   p.ProyectoId,
				   p.Nombre AS Proyecto,
				   d.DependenciaId AS FuenteFinanciamientoId,
				   d.Nombre AS FuenteFinanciamiento,
				   tg.TipoGastoId,
				   tg.Nombre AS TipoGasto
			FROM ARtblAlmacenProducto AS ap
				 INNER JOIN tblCuentaPresupuestalEgr AS cp ON ap.CuentaPresupuestalId = cp.CuentaPresupuestalEgrId
				 INNER JOIN tblRamo AS ua ON cp.RamoId = ua.RamoId
				 INNER JOIN tblProyecto AS p ON cp.ProyectoId = p.ProyectoId
				 INNER JOIN tblDependencia AS d ON cp.DependenciaId = d.DependenciaId
				 INNER JOIN tblTipoGasto AS tg ON cp.TipoGastoId = tg.TipoGastoId
	 ) AS apDestino ON detalle.AlmacenProductoDestinoId = apDestino.AlmacenProductoId
	 INNER JOIN tblAlmacen AS almacenDestino ON apDestino.AlmacenId = almacenDestino.AlmacenId
WHERE transferencia.TransferenciaId = @transferenciaId
ORDER BY detalle.NumeroLinea
GO