SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoRequisicionMaterial]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 27/05/2021
-- Modified date: 23/07/2021
-- Description:	View para obtener el Listado de Requisicion de Material
-- ============================================================
SELECT requisicion.RequisicionMaterialId,
       CodigoRequisicion,
       dbo.GRfnGetFechaConFormato(FechaRequisicion, 0) AS Fecha,
	   dbo.RHfnGetNombreCompletoEmpleado(empleado.EmpleadoId) AS CreadoPor,
       area.Nombre AS Area,
       Total AS Monto,
       requisicion.EstatusId,
       estatus.Valor AS Estatus,
	   requisicion.Timestamp
FROM ARtblRequisicionMaterial AS requisicion   
     INNER JOIN GRtblUsuario AS usuario ON requisicion.CreadoPorId = UsuarioId
	 INNER JOIN RHtblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
     INNER JOIN tblDependencia AS area ON AreaId = DependenciaId
	 INNER JOIN GRtblControlMaestro AS estatus ON requisicion.EstatusId = ControlId
     INNER JOIN
	 (
		SELECT RequisicionMaterialId,
				SUM(TotalPartida) AS Total
		FROM ARtblRequisicionMaterialDetalle
		WHERE EstatusId != 3 -- No borrados
		GROUP BY RequisicionMaterialId
	 ) AS detalles ON requisicion.RequisicionMaterialId = detalles.RequisicionMaterialId
WHERE requisicion.EstatusId != 31 -- No borradas
GO