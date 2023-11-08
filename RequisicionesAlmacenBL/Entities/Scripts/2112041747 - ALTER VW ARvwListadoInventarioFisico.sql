SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoInventarioFisico]
AS
-- ======================================================
-- Author:		Javier Elías
-- Create date: 21/01/2021
-- Modified date: 04/12/2021
-- Description:	View para obtener el Listado de Inventarios Físicos
-- ======================================================
SELECT inventario.InventarioFisicoId, 
       Codigo, 
       dbo.GRfnGetFechaConFormato(inventario.FechaCreacion, 0) AS Fecha,
       Almacen,
	   empleado.Nombre + ' ' + empleado.PrimerApellido + ISNULL(' ' + empleado.SegundoApellido, '') AS Usuario,
	   inventario.MontoAjuste,
	   estatus.Valor AS Estatus
FROM ARtblInventarioFisico AS inventario
     INNER JOIN GRtblUsuario AS usuario ON inventario.CreadoPorId = usuario.UsuarioId
     INNER JOIN RHtblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
	 INNER JOIN GRtblControlMaestro AS estatus ON inventario.EstatusId = estatus.ControlId
WHERE inventario.EstatusId != 34 -- Estatus En Proceso o Terminado
GO