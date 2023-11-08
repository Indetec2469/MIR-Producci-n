SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[GRvwListadoRol]
AS
/* ****************************************************************
 * GRvwListadoRol
 * ****************************************************************
 * Descripción: View para obtener el listado de ROL
 * Autor: Rene Carrillo
 * Fecha: 22.10.2021
 * Versión: 1.0.0
 *****************************************************************
*/ 
SELECT 
	rol.RolId,
	rol.Nombre,
	rol.Descripcion,
	CASE WHEN rol.EstatusId = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estatus
FROM 
	GRtblRol rol