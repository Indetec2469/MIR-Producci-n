SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoRequisicionMaterial]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 27/05/2021
-- Modified date: 16/08/2022
-- Description:	View para obtener el Listado de Requisicion de Material
-- ============================================================
SELECT requisicion.RequisicionMaterialId,
       CodigoRequisicion,
       dbo.GRfnGetFechaConFormato(FechaRequisicion, 0) AS Fecha,
	   dbo.RHfnGetNombreCompletoEmpleado(empleadoCreadoPor.EmpleadoId) AS CreadoPor,
       area.DependenciaId + ' - ' + area.Nombre AS Area,
       Total AS Monto,
       requisicion.EstatusId,
       estatus.Valor AS Estatus,
	   requisicion.Timestamp,
	   CONVERT(BIT, CASE WHEN requisicion.EstatusId IN (71, 76, 68) THEN 1 ELSE 0 END) PermiteEditar, -- Guardada, Revisión, En proceso
	   CONVERT(BIT, CASE WHEN requisicion.EstatusId IN (71, 76) THEN 1 ELSE 0 END) PermiteCancelar, -- Guardada, Revisión
	   requisicion.CreadoPorId,
	   ISNULL(usuarioEmpleado.UsuarioId, -1) AS UsuarioId
FROM ARtblRequisicionMaterial AS requisicion   
     LEFT JOIN GRtblUsuario AS usuarioEmpleado ON requisicion.EmpleadoId = usuarioEmpleado.EmpleadoId
	 INNER JOIN GRtblUsuario AS usuarioCreadoPor ON requisicion.CreadoPorId = usuarioCreadoPor.UsuarioId
	 INNER JOIN RHtblEmpleado AS empleadoCreadoPor ON usuarioCreadoPor.EmpleadoId = empleadoCreadoPor.EmpleadoId
	 INNER JOIN tblDependencia AS area ON AreaId = DependenciaId
	 INNER JOIN GRtblControlMaestro AS estatus ON requisicion.EstatusId = ControlId
     INNER JOIN
	 (
		SELECT RequisicionMaterialId,
				SUM(TotalPartida) AS Total
		FROM ARtblRequisicionMaterialDetalle
		WHERE EstatusId NOT IN (78)  -- Cancelado, Rechazado
		GROUP BY RequisicionMaterialId
	 ) AS detalles ON requisicion.RequisicionMaterialId = detalles.RequisicionMaterialId
WHERE requisicion.EstatusId != 65 -- Cancelada
GO