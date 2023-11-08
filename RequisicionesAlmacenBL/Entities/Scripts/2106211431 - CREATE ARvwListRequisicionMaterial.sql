SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListRequisicionMaterial]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 27/05/2021
-- Modified date: 
-- Description:	View para obtener el Listado de Requisicion de Material
-- ============================================================
SELECT requisicion.RequisicionMaterialId,
       CodigoRequisicion,
       dbo.getFechaConFormato(FechaRequisicion, 0) AS Fecha,
       empleado.Nombre+' '+PrimerApellido+ISNULL(' '+SegundoApellido, '') AS CreadoPor, --- ESTO SE TIENE QUE CAMBIAR
       area.Nombre AS Area,
       Total AS Monto,
       requisicion.EstatusId,
       estatus.Valor AS Estatus,
	   requisicion.Timestamp
FROM ARtblRequisicionMaterial AS requisicion   
     INNER JOIN RequisicionesAlmacenDatos.dbo.tblUsuario AS usuario ON requisicion.CreadoPorId = UsuarioId --- ESTO SE TIENE QUE CAMBIAR
	 INNER JOIN RequisicionesAlmacenDatos.dbo.tblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId --- ESTO SE TIENE QUE CAMBIAR
     INNER JOIN tblDependencia AS area ON AreaId = DependenciaId
	 INNER JOIN ARtblControlMaestro AS estatus ON requisicion.EstatusId = ControlId
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