SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionMaterialDetalles]
	@requisicionId INT
AS
/* ****************************************************************
 * ARspConsultaRequisicionMaterialProductos
 * ****************************************************************
 * Descripción: Procedimiento para Obtener los detalles de una Requisición
 *						 de la Ficha Requisición Material.
 *
 * autor: 	Javier Elías
 * Fecha: 	21.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: RequisicionId
 * PARAMETROS DE SALIDA:
 *****************************************************************
*/ 
SELECT CONCAT(ProductoId, almacen.AlmacenId, UnidadAdministrativaId, proyecto.ProyectoId, tipoGasto.TipoGastoId) AS ProductoDetalleId,
       ProductoId,
       detalle.Descripcion AS Producto,
       um.UnidadDeMedidaId,
       um.Descripcion AS UnidadDeMedida,
       CostoUnitario,
       almacen.AlmacenId,
       almacen.Nombre AS Almacen,
       UnidadAdministrativaId,
       unidadAdministrativa.Nombre AS UnidadAdministrativa,
       detalle.ProyectoId,
       proyecto.Nombre AS Proyecto,
       tipoGasto.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto,
       RequisicionMaterialDetalleId,
       detalle.RequisicionMaterialId,
       Cantidad,
       TotalPartida,
       Comentarios,
       detalle.EstatusId
FROM ARtblRequisicionMaterialDetalle AS detalle
     INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblAlmacen AS almacen ON detalle.AlmacenId  = almacen.AlmacenId
	 INNER JOIN tblDependencia AS unidadAdministrativa ON detalle.UnidadAdministrativaId = unidadAdministrativa.DependenciaId
	 INNER JOIN tblProyecto AS proyecto ON detalle.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblTipoGasto AS tipoGasto ON detalle.TipoGastoId = tipoGasto.TipoGastoId
WHERE detalle.RequisicionMaterialId = @requisicionId
      AND detalle.EstatusId != 31
GO