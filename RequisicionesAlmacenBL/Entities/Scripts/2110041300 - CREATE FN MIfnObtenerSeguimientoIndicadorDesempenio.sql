SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ****************************************************************
 * MIfnObtenerSeguimientoIndicadorDesempenio
 * ****************************************************************
 * Descripción: Funcion para obtener Seguimiento Indicador Desempenio.
 * Autor: Rene Carrillo
 * Fecha: 04.10.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: MIRIndicadorId
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE FUNCTION [dbo].[MIfnObtenerSeguimientoIndicadorDesempenio] 
(	
	@MIRIndicadorId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT umfv.Variable, sv.Enero, sv.Febrero, sv.Marzo, sv.Abril, sv.Mayo, sv.Junio, sv.Julio, sv.Agosto, sv.Septiembre, sv.Octubre, sv.Noviembre, sv.Diciembre FROM MItblMatrizIndicadorResultadoIndicador miri
		INNER JOIN MItblMatrizIndicadorResultadoIndicadorFormulaVariable fv ON miri.MIRIndicadorId = fv.MIRIndicadorId
		INNER JOIN MItblControlMaestroUnidadMedidaFormulaVariable umfv ON fv.UnidadMedidaFormulaVariableId = umfv.UnidadMedidaFormulaVariableId
		INNER JOIN MItblMatrizConfiguracionPresupuestalSeguimientoVariable sv ON fv.MIRIndicadorFormulaVariableId = sv.MIRIndicadorFormulaVariableId
	WHERE miri.MIRIndicadorId = @MIRIndicadorId
)