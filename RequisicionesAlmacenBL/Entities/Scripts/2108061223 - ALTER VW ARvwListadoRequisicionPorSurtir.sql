SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoRequisicionPorSurtir]
AS
-- =======================================================================
-- Author:		Javier Elías
-- Create date: 22/06/2021
-- Modified date: 04/08/2021
-- Description:	View para obtener el Listado de Requisiciones Pendientes por Autorizar
-- =======================================================================
SELECT requisicion.RequisicionMaterialId,
       CodigoRequisicion,
       dbo.GRfnGetFechaConFormato(FechaRequisicion, 0) AS Fecha,
       dbo.RHfnGetNombreCompletoEmpleado(empleado.EmpleadoId) AS CreadoPor,
       area.Nombre AS Area,
       Total AS Monto,
       requisicion.EstatusId,
       estatus.Valor AS Estatus
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
		WHERE EstatusId NOT IN (78, 85)  -- Cancelado, Rechazado
		GROUP BY RequisicionMaterialId
	 ) AS detalles ON requisicion.RequisicionMaterialId = detalles.RequisicionMaterialId
WHERE requisicion.EstatusId IN (64, 68) -- Autorizada, En Proceso
GO