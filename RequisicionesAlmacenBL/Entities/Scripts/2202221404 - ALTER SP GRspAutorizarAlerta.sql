SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 11/01/2022
-- Modified date: 21/02/2022
-- Description:	SP para autorizar una Alerta
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GRspAutorizarAlerta] 
	@accionId int,
	@creadoPorId INT,
	@alertaId BIGINT,
	@valorSalida NVARCHAR(MAX) OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC GRspProcesadorDeAlertas @accionId = @accionId,
							    @creadoPorId = @creadoPorId,
							    @alertaId = @alertaId,
							    @valorSalida = @valorSalida OUTPUT
END
GO