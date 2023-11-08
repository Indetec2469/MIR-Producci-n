SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* ****************************************************************
 * GRvwListadoUsuario
 * ****************************************************************
 * Descripción: View para el listado de Usuario.
 * Autor: Rene Carrillo
 * Fecha: 04.11.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: 
 * PARAMETROS DE SALIDA: UsuarioId, NombreUsuario, NombreEmpleado, NombreRol
 *****************************************************************
*/

CREATE VIEW [dbo].[GRvwListadoUsuario]
AS
SELECT usuario.UsuarioId, usuario.NombreUsuario, empleado.Nombre AS NombreEmpleado, rol.Nombre AS NombreRol
FROM GRtblUsuario usuario
	INNER JOIN RHtblEmpleado empleado ON usuario.EmpleadoId = empleado.EmpleadoId
	INNER JOIN GRtblRol rol ON usuario.RolId = rol.RolId
WHERE usuario.EstatusId = 1
GO