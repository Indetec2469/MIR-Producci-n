SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER FUNCTION [dbo].[GRfnGetFechaConFormatoCompleto](@date DATETIME) 
RETURNS VARCHAR(3000)
AS
BEGIN
		RETURN CAST(DAY(@date) AS VARCHAR(2)) + ' de ' + CASE MONTH(@date) 
				WHEN 1 THEN 'Enero'
				WHEN 2 THEN 'Febrero'
				WHEN 3 THEN 'Marzo'
				WHEN 4 THEN 'Abril'
				WHEN 5 THEN 'Mayo'
				WHEN 6 THEN 'Junio'
				WHEN 7 THEN 'Julio'
				WHEN 8 THEN 'Agosto'
				WHEN 9 THEN 'Septiembre'
				WHEN 10 THEN 'Octubre'
				WHEN 11 THEN 'Noviembre'
				WHEN 12 THEN 'Diciembre'
		END + ' de ' + CAST(YEAR(@date) AS VARCHAR(4))
END