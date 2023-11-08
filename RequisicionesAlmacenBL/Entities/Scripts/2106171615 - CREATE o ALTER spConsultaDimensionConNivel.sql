SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[spConsultaDimensionConNivel]
AS
/* ****************************************************************
 * spConsultaDimensionConNivel
 * ****************************************************************
 * Descripción: Buscamos los objetos de Dimension con el Nivel.
 * Autor: Rene Carrillo
 * Fecha: 17.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA:
 * PARAMETROS DE SALIDA: DimensionId, Descripcion, NivelId
 *****************************************************************
*/ 
SELECT
	cmd.DimensionId,
	cmd.Descripcion,
	cmdn.NivelId
FROM
	tblControlMaestroDimension cmd
	INNER JOIN tblControlMaestroDimensionNivel cmdn ON cmd.DimensionId = cmdn.DimensionId AND cmdn.Borrado = 0
WHERE 
	cmd.Borrado = 0