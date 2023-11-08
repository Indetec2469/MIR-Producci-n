SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ****************************************************************
 * MIspRptLibroConsultaMatrizIndicadorResultado
 * ****************************************************************
 * Descripción: Consulta para obtener los datos de Matriz Indicador Resultado para el reporte.
 * Autor: Rene Carrillo
 * Fecha: 19.08.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: MIRId
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

ALTER   PROCEDURE [dbo].[MIspRptLibroConsultaMatrizIndicadorResultado]
	@MIRId INT
AS
	SELECT mir.*, pg.Nombre AS ProgramaPresupuestario
	FROM MItblMatrizIndicadorResultado mir
		INNER JOIN tblProgramaGobierno pg ON mir.ProgramaPresupuestarioId = pg.ProgramaGobiernoId
	WHERE mir.MIRId = @MIRId
