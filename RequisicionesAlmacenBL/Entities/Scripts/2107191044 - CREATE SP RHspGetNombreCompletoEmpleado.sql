SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[RHspGetNombreCompletoEmpleado] @empleadoId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT dbo.RHfnGetNombreCompletoEmpleado(@empleadoId)

END