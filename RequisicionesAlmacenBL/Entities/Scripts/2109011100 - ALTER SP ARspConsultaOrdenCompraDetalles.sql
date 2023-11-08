SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOrdenCompraDetalles]
	@ordenCompraId INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 12/08/2021
-- Modified date: 26/08/2021
-- Description:	Procedure para obtener los Detalles de una OC
-- ============================================================
SELECT ap.AlmacenProductoId,
       detalle.OrdenCompraDetId,
	   detalle.OrdenCompraId,
       detalle.TarifaImpuestoId,
	   impuesto.Nombre AS Impuesto,
       detalle.ProductoId,
	   detalle.Descripcion,
	   um.UnidadDeMedidaId AS UnidadDeMedidaId,
	   um.Descripcion AS UnidadDeMedida,
       detalle.CuentaPresupuestalEgrId,
	   unidadAdministrativa.DependenciaId AS UnidadAdministrativaId,
	   unidadAdministrativa.Nombre AS UnidadAdministrativa,
	   proyecto.ProyectoId,
	   proyecto.Nombre AS Proyecto,
	   fuenteFinanciamiento.RamoId AS FuenteFinanciamientoId,
	   fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
	   tipoGasto.TipoGastoId,
	   tipoGasto.Nombre AS TipoGasto,
       detalle.Cantidad,
	   CONVERT(FLOAT, ISNULL(recibos.CantidadRecibida, 0)) AS CantidadRecibida,
	   CONVERT(FLOAT, detalle.Cantidad - ISNULL(recibos.CantidadRecibida, 0)) AS CantidadPorRecibir,
       Costo,
       Importe,
       detalle.IEPS,
       Ajuste,
       IVA,
       ISH,
       RetencionISR,
       RetencionCedular,
       RetencionIVA,
       TotalPresupuesto,
       Total,	   
	   detalle.Status,
	   CASE detalle.Status WHEN 'A' THEN 'Activo' WHEN 'I' THEN 'Recibo Parcial' WHEN 'R' THEN 'Recibido' WHEN 'C' THEN 'Cancelado' ELSE '' END AS Estatus,
	   CONVERT(BIT, CASE WHEN detalle.Status = 'A' THEN 1 ELSE 0 END) AS PermiteEditar
FROM tblOrdenCompra AS oc
     INNER JOIN tblOrdenCompraDet AS detalle ON oc.OrdenCompraId = detalle.OrdenCompraId AND detalle.Status != 'C'
	 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
	 INNER JOIN tblTarifaImpuesto AS impuesto ON detalle.TarifaImpuestoId = impuesto.TarifaImpuestoId
	 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblCuentaPresupuestalEgr AS cp ON detalle.CuentaPresupuestalEgrId = cp.CuentaPresupuestalEgrId
	 INNER JOIN tblDependencia AS unidadAdministrativa ON cp.DependenciaId = unidadAdministrativa.DependenciaId
	 INNER JOIN tblProyecto AS proyecto ON cp.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblRamo AS fuenteFinanciamiento ON cp.RamoId = fuenteFinanciamiento.RamoId
	 INNER JOIN tblTipoGasto AS tipoGasto ON cp.TipoGastoId = tipoGasto.TipoGastoId
	 INNER JOIN ARtblAlmacenProducto AS ap ON detalle.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId AND oc.AlmacenId = ap.AlmacenId AND detalle.ProductoId = ap.ProductoId AND ap.Borrado = 0
	 LEFT JOIN
	 (
		SELECT detalle.OrdenCompraDetId,
			   SUM(reciboDet.Cantidad) AS CantidadRecibida
		FROM tblCompra AS recibo
			 INNER JOIN tblCompraDet AS reciboDet ON recibo.CompraId = reciboDet.CompraId
			 INNER JOIN tblOrdenCompraDet AS detalle ON reciboDet.RefOrdenCompraDetId = detalle.OrdenCompraDetId AND detalle.Status != 'C' -- Cancelado
		WHERE recibo.Status != 'C' -- Cancelado
		GROUP BY detalle.OrdenCompraDetId
	 ) AS recibos ON detalle.OrdenCompraDetId = recibos.OrdenCompraDetId
WHERE oc.OrdenCompraId = @ordenCompraId
ORDER BY detalle.OrdenCompraDetId
GO