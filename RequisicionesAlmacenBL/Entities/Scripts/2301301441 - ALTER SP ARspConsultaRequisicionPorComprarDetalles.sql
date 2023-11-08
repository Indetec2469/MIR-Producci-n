SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionPorComprarDetalles]
AS
-- =======================================================================
-- Author:			Javier Elías
-- Create date:		29/07/2021
-- Modified date:	27/01/2023
-- Description:		Procedimiento para Obtener los detalles de una Requisición
--					Pendiente por Comprar de la Ficha Requisición Material.
-- =======================================================================
SELECT RequisicionMaterialDetalleId,
       detalle.RequisicionMaterialId,
	   CodigoRequisicion AS Solicitud,
       dbo.GRfnGetFechaConFormato(FechaRequisicion, 0) AS Fecha,
	   FechaRequisicion,
       dbo.RHfnGetNombreCompletoEmpleado(usuario.EmpleadoId) AS Usuario,
	   requisicion.AreaId AS AreaId,
       area.DependenciaId + ' - ' + area.Nombre AS Area,
	   CONVERT(VARCHAR (4), NULL) AS AlmacenId,
       CONVERT(VARCHAR (100), NULL) AS Almacen,
	   unidadAdministrativa.DependenciaId AS UnidadAdministrativaId,
       unidadAdministrativa.Nombre AS UnidadAdministrativa,
	   proyecto.ProyectoId,
       proyecto.Nombre AS Proyecto,
	   tipoGasto.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto,
	   producto.ProductoId,
	   detalle.Descripcion AS Descripcion,
	   um.Descripcion AS UM,
	   CostoUnitario,
	   CONVERT(VARCHAR (10), CASE WHEN detalle.EstatusId = 83 THEN producto.TarifaImpuestoId ELSE NULL END) AS TarifaImpuestoId, -- Si es Por comprar
	   Cantidad AS CantidadSolicitada,
	   -1 * ISNULL(Surtida, 0) AS CantidadSurtida,
	   ISNULL(existencia.Existencia, CONVERT(DECIMAL(38, 10), 0)) AS Existencia,
	   CONVERT(DECIMAL(28, 10), producto.StockMinimo) AS ExistenciaMinima,
	   CONVERT(FLOAT, NULL) AS CantidadComprar,
	   CONVERT(FLOAT, NULL) AS Total,
	   CONVERT(FLOAT, CASE WHEN detalle.EstatusId = 83 THEN 0 ELSE NULL END) AS Ajuste, -- Si es Por comprar
	   Comentarios,
	   CONVERT(VARCHAR (6), NULL) AS FuenteFinanciamientoId,
	   CONVERT(INT, NULL) AS ProveedorId,
	   detalle.EstatusId,
	   estatus.Valor AS Estatus,	  
	   CONVERT(BIT, 0) AS Revision,
	   CONVERT(BIT, 0) AS Comprar,
	   CONVERT(BIT, 0) AS Rechazar,
	   CONVERT(VARCHAR(2000), NULL) AS Motivo,

	   CONVERT(INT, NULL) AS OrdenCompraDetId,
	   CONVERT(INT, NULL) AS OrdenCompraId,
	   CONVERT(INT, NULL) AS CuentaPresupuestalEgrId,
	   CONVERT(DECIMAL(28, 10), NULL) AS Importe,
	   CONVERT(FLOAT, CASE WHEN detalle.EstatusId = 83 THEN 0 ELSE NULL END) AS IEPS, -- Si es Por comprar
	   CONVERT(FLOAT, NULL) AS IVA,
	   CONVERT(FLOAT, NULL) AS ISH,
	   CONVERT(FLOAT, NULL) AS RetencionISR,
	   CONVERT(FLOAT, NULL) AS RetencionCedular,
	   CONVERT(FLOAT, NULL) AS RetencionIVA,
	   CONVERT(FLOAT, NULL) AS TotalPresupuesto,

	   CONVERT(INT, NULL) AS InvitacionCompraDetalleId,
	   CONVERT(INT, NULL) AS InvitacionCompraId,

	   requisicion.Timestamp AS RequisicionTimestamp,
	   detalle.Timestamp AS DetalleTimestamp
FROM ARtblRequisicionMaterial AS requisicion
     INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId AND detalle.EstatusId IN (84, 90, 83) -- Por surtir, Surtido parcial, Por Comprar
	 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
	 INNER JOIN tblTarifaImpuesto AS impuesto ON producto.TarifaImpuestoId = impuesto.TarifaImpuestoId
     INNER JOIN GRtblUsuario AS usuario ON requisicion.CreadoPorId = UsuarioId
     INNER JOIN tblDependencia AS area ON requisicion.AreaId = area.DependenciaId
     INNER JOIN tblDependencia AS unidadAdministrativa ON detalle.UnidadAdministrativaId = unidadAdministrativa.DependenciaId
     INNER JOIN tblProyecto AS proyecto ON detalle.ProyectoId = proyecto.ProyectoId
     INNER JOIN tblTipoGasto AS tipoGasto ON detalle.TipoGastoId = tipoGasto.TipoGastoId
	 INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
	 INNER JOIN GRtblControlMaestro AS estatus ON detalle.EstatusId = estatus.ControlId
	 OUTER APPLY 
	 (
		SELECT SUM(CantidadMovimiento) AS Surtida
		FROM ARtblInventarioMovimiento
		WHERE TipoMovimientoId = 63 -- Requisición Material Surtimiento
				AND ReferenciaMovtoId = detalle.RequisicionMaterialDetalleId
	 ) AS surtimiento
	 OUTER APPLY 
	 (
			SELECT SUM(Cantidad) AS Existencia
			FROM ARtblAlmacenProducto AS almacenProducto
				 INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen AS areaAlmacen ON almacenProducto.AlmacenId = areaAlmacen.AlmacenId AND areaAlmacen.Borrado = 0
				 INNER JOIN ARtblControlMaestroConfiguracionArea AS area ON requisicion.AreaId = area.AreaId AND area.Borrado = 0
				 INNER JOIN tblCuentaPresupuestalEgr AS cuentaPresupuestal ON almacenProducto.CuentaPresupuestalId = cuentaPresupuestal.CuentaPresupuestalEgrId
																			  AND cuentaPresupuestal.DependenciaId = detalle.UnidadAdministrativaId
																			  AND cuentaPresupuestal.ProyectoId = detalle.ProyectoId
																			  AND cuentaPresupuestal.TipoGastoId = detalle.TipoGastoId
			WHERE almacenProducto.ProductoId = detalle.ProductoId
				  AND almacenProducto.Borrado = 0
	  ) AS existencia
WHERE requisicion.EstatusId IN (64, 68, 73) -- Autorizada, En Proceso, Por Comprar