SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GRspGetFechaOperacion]
AS
-- =============================================
-- Author:		Javier Elías
-- Create date: 10/08/2022
-- Modified date: 
-- Description:	Procedimiento para obtener la
--						Fecha de Operación configurada
-- =============================================

-- Obtenemos la Fecha de Operación configurada
DECLARE @fechaOperacion DATE = ( SELECT TOP 1 CASE WHEN TipoConfiguracionFechaId = 137 THEN FechaOperacion ELSE GETDATE() END FROM GRtblConfiguracionEnte WHERE EstatusId = 1 )

SELECT @fechaOperacion