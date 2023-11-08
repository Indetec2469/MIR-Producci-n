SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspGetNombreEtapaAccion]
	@etapaAccion INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 27/06/2022
-- Modified date: 
-- Description:	Procedure para obtener el nombre de una EtapaAccion
-- ============================================================
SELECT e.Valor+' - '+a.Valor
FROM GRtblAlertaEtapaAccion AS ea
     INNER JOIN GRtblControlMaestro AS e ON ea.EtapaId = e.ControlId
     INNER JOIN GRtblControlMaestro AS a ON ea.AccionId = a.ControlId
WHERE ea.AlertaEtapaAccionId = @etapaAccion