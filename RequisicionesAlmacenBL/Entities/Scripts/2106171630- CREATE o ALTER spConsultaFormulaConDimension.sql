SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[spConsultaFormulaConDimension]
AS
/* ****************************************************************
 * spConsultaFormulaConDimension
 * ****************************************************************
 * Descripción: Buscamos los objetos de la Formula con Dimension.
 * Autor: Rene Carrillo
 * Fecha: 17.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA:
 * PARAMETROS DE SALIDA: Todos de Formula y DimensionId
 *****************************************************************
*/
SELECT
	f.*, fd.DimensionId
FROM 
	MItblFormula f
	INNER JOIN MItblFormulaDimension fd ON f.FormulaId = fd.FormulaId AND fd.Borrado = 0
WHERE 
	f.Borrado = 0