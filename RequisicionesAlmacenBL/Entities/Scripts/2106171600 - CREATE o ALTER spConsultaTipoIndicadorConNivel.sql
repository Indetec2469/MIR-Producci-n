SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[spConsultaTipoIndicadorConNivel]
AS
/* ****************************************************************
 * spConsultaTipoIndicadorConNivel
 * ****************************************************************
 * Descripción: Buscamos los objetos de Tipo Indicador con el Nivel.
 * Autor: Rene Carrillo
 * Fecha: 17.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA:
 * PARAMETROS DE SALIDA: TipoIndicadorId, Descripcion, NivelId
 *****************************************************************
*/ 

SELECT 
	ti.TipoIndicadorId, 
	ti.Descripcion,
	tin.NivelId
FROM 
	tblControlMaestroTipoIndicador ti
	INNER JOIN tblControlMaestroTipoIndicadorNivel tin ON ti.TipoIndicadorId = tin.TipoIndicadorId AND tin.Borrado = 0
WHERE 
	ti.Borrado = 0