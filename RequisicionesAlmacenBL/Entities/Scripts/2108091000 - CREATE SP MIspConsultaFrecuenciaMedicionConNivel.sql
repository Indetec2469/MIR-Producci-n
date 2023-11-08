SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[MIspConsultaFrecuenciaMedicionConNivel]
AS
/* ****************************************************************
 * MIspConsultaFrecuenciaMedicionConNivel
 * ****************************************************************
 * Descripción: Buscamos los objetos de Frecuencia Medicion con el Nivel.
 * Autor: Rene Carrillo
 * Fecha: 09.08.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA:
 * PARAMETROS DE SALIDA: FrecuenciaMedicionId, Descripcion, NivelId
 *****************************************************************
*/ 
SELECT 
	fm.FrecuenciaMedicionId, 
	fm.Descripcion,
	fmn.NivelId
FROM 
	MItblControlMaestroFrecuenciaMedicion fm
	INNER JOIN MItblControlMaestroFrecuenciaMedicionNivel fmn ON fm.FrecuenciaMedicionId = fmn.FrecuenciaMedicionId AND fmn.Borrado = 0
WHERE 
	fm.Borrado = 0