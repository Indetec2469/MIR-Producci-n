SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:		Javier Elías
-- Create date: 20/08/2021
-- Modified date: 
-- Description:	Función para obtener el reporte de Surtimiento
--						de Requisición Material.
-- ========================================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptSurtidoSolicitud] 
(	
	@requisicionId INT,
	@usuarioId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT dbo.RHfnGetNombreCompletoEmpleado(usuarioImpresion.EmpleadoId) AS Usuario,
		   'rptSurtidoSolicitud' AS Reporte,
		   dbo.GRfnGetFechaConFormato(GETDATE(), 0) AS FechaImpresion,
		   REPLACE(dbo.GRfnGetFechaConFormato(GETDATE(), 1), (dbo.GRfnGetFechaConFormato(GETDATE(), 0) + ' '), '') AS HoraImpresion,
		   CodigoRequisicion AS Requisicion,
		   ProductoId,
		   detalle.Descripcion AS Producto,
		   um.Descripcion AS UnidadDeMedida,
		   ISNULL(Surtida, 0) AS Cantidad,
		   detalle.UnidadAdministrativaId AS UnidadAdministrativa,
		   detalle.ProyectoId AS Proyecto
	FROM ARtblRequisicionMaterial AS requisicion
		 INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId
		 INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
		 INNER JOIN GRtblUsuario AS usuario ON requisicion.CreadoPorId = usuario.UsuarioId
		 INNER JOIN RHtblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
		 INNER JOIN GRtblUsuario AS usuarioImpresion ON usuarioImpresion.UsuarioId = @usuarioId
		 CROSS APPLY 
		 (
				SELECT SUM(CantidadMovimiento) AS Surtida
				FROM ARtblInventarioMovimiento
				WHERE TipoMovimientoId = 63 -- Requisición Material Surtimiento
					  AND ReferenciaMovtoId = detalle.RequisicionMaterialDetalleId
				GROUP BY ReferenciaMovtoId
		 ) AS surtimiento
	WHERE detalle.RequisicionMaterialId = @requisicionId
)
GO