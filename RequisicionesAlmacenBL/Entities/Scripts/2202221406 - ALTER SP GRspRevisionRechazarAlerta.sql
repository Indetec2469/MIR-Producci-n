SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: <Create Date,,>
-- Modified date: 21/02/2022
-- Description:	SP para Rechazar/Revision una alerta
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GRspRevisionRechazarAlerta] 
	@accionId int,
	@creadoPorId INT,
	@alertaId BIGINT,
	@motivo NVARCHAR(2000),
	@valorSalida NVARCHAR(MAX) OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC GRspProcesadorDeAlertas @accionId = @accionId,
							    @creadoPorId = @creadoPorId,
							    @alertaId = @alertaId,
								@motivo = @motivo,
							    @valorSalida = @valorSalida OUTPUT
END
GO