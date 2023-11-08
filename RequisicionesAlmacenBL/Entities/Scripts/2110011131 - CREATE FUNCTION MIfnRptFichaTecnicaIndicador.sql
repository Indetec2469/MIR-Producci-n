SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 01/10/2021
-- Description:	Funcion para obtener el reporte de Ficha Tecnica de Indicadores
-- =============================================
CREATE FUNCTION MIfnRptFichaTecnicaIndicador
(	
	@MIRId INT
	
)
RETURNS TABLE 
AS
RETURN 
(
		-- Add the SELECT statement with parameter references here
	SELECT  '' AS UnidadResponsable,
	        PG.ProgramaGobiernoId + ' - ' + PG.Nombre AS ProgramaPresupuestario,
			MIRI.NombreIndicador AS NombreIndicador,
			MIRI.ResumenNarrativo AS ResumenNarrativo,
			MIRI.DefinicionIndicador AS DefinicionIndicador,
			MIRI.TipoIndicadorId AS TipoIndicadorId,
			MIRI.DimensionId AS DimensionId,
			MIRI.NivelIndicadorId AS NivelIndicadorId,
			CMUM.Formula + '<br>' + MIRI.DescripcionFormula AS MetodoCalculo,
			CMUM.Nombre AS UnidadMedida,
			CMFM.Descripcion AS FrecuenciaMedicion,
			MIRI.ValorBase AS LineaBase,
			Meta.Valor AS Meta,
			MIRI.MedioVerificacion,
			Variables.Descripcion AS DescripcionVariables,
			MIRI.SentidoId
	FROM MItblMatrizIndicadorResultado MIR
	INNER JOIN MItblMatrizIndicadorResultadoIndicador MIRI ON MIR.MIRId = MIRI.MIRId
	INNER JOIN MItblControlMaestroUnidadMedida CMUM ON MIRI.UnidadMedidaId = CMUM.UnidadMedidaId
	INNER JOIN MItblControlMaestroFrecuenciaMedicion CMFM ON MIRI.FrecuenciaMedicionId = CMFM.FrecuenciaMedicionId
	INNER JOIN tblProgramaGobierno PG ON  MIR.ProgramaPresupuestarioId = PG.ProgramaGobiernoId
	INNER JOIN (
		SELECT MIRIM.MIRIndicadorId, MIRIM.Valor 
		FROM MItblMatrizIndicadorResultadoIndicadorMeta MIRIM
		INNER JOIN (
						SELECT MIRIndicadorId, MAX(Orden) As Orden
						FROM MItblMatrizIndicadorResultadoIndicadorMeta
						WHERE EstatusId = 1
						GROUP BY MIRIndicadorId
		) AS MIRIMM ON MIRIM.MIRIndicadorId = MIRIMM.MIRIndicadorId AND MIRIM.Orden = MIRIMM.Orden
		WHERE MIRIM.EstatusId = 1
	) AS Meta ON MIRI.MIRIndicadorId = Meta.MIRIndicadorId
	INNER JOIN (
					SELECT T.MIRIndicadorId, Descripcion = STUFF(
					  (SELECT ',<br> ' +  '<b>Variable </b> ' +  CMUMFV.Variable + '  =>  ' + MIRIFV.DescripcionVariable
					   FROM MItblMatrizIndicadorResultadoIndicadorFormulaVariable MIRIFV
					   INNER JOIN MItblControlMaestroUnidadMedidaFormulaVariable CMUMFV ON MIRIFV.UnidadMedidaFormulaVariableId = CMUMFV.UnidadMedidaFormulaVariableId
					   WHERE MIRIFV.MIRIndicadorId = T.MIRIndicadorId AND MIRIFV.EstatusId = 1
					   FOR XML PATH, TYPE).value(N'.[1]', N'nvarchar(max)'),1, 5, N'')
					FROM MItblMatrizIndicadorResultadoIndicadorFormulaVariable T 
					GROUP BY T.MIRIndicadorId
	) as Variables ON MIRI.MIRIndicadorId = Variables.MIRIndicadorId
	WHERE MIRI.EstatusId = 1 AND MIRI.MIRId = @MIRId

)
GO
