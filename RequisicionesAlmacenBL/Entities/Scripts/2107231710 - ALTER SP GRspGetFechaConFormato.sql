SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GRspGetFechaConFormato] @date DATETIME, @mostrarHoras BIT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT dbo.GRfnGetFechaConFormato(@date, @mostrarHoras) AS ValorSalida

END