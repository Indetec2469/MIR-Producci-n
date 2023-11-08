SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER FUNCTION [dbo].[GRfnGetFechaConFormato](@date DATETIME, @mostrarHoras BIT)
RETURNS VARCHAR(30)
AS
	BEGIN
		DECLARE @fecha NVARCHAR(30)
		--SELECT @fecha = FORMAT(@date, 'MMMM dd, yyyy ', 'es-es')+CASE WHEN @mostrarHoras = 1 THEN LEFT(RIGHT(CONVERT(VARCHAR, @date, 22), 11), 5) + RIGHT(RIGHT(CONVERT(VARCHAR, @date, 22), 11), 3) ELSE '' END
		SELECT @fecha = FORMAT(@date, 'dd/MM/yyyy ', 'es-es')+CASE WHEN @mostrarHoras = 1 THEN LEFT(RIGHT(CONVERT(VARCHAR, @date, 22), 11), 5) + RIGHT(RIGHT(CONVERT(VARCHAR, @date, 22), 11), 3) ELSE '' END
		SET @fecha = UPPER(SUBSTRING(@fecha, 1, 1)) + SUBSTRING(@fecha, 2, LEN(@fecha))
		SET @fecha = REPLACE(@fecha, '  ', ' ')
		RETURN CASE WHEN @mostrarHoras = 0 THEN SUBSTRING(@fecha, 0, LEN(@fecha)+1) ELSE @fecha END
	END