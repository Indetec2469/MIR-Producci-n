
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

ALTER VIEW [dbo].[GRvwListadoUsuario]
AS
SELECT usuario.UsuarioId, usuario.NombreUsuario, dbo.RHfnGetNombreCompletoEmpleado(usuario.EmpleadoId) AS NombreEmpleado, rol.Nombre AS NombreRol
FROM GRtblUsuario usuario
INNER JOIN GRtblRol rol ON usuario.RolId = rol.RolId
WHERE usuario.EstatusId = 1
GO


