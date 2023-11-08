SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 11/01/2022
-- Modified date: 21/02/2022
-- Description:	SP para iniciar una alerta
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GRspIniciarAlerta] 
	@accionId int,
	@alertaDefinicionId int = null,
	@referenciaProcesoId int = null,
	@codigoTramite nvarchar(50) = null,
	@textoRepresentativo nvarchar(255) = null,
	@creadoPorId INT = NULL,
	@valorSalida NVARCHAR(500) OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC GRspProcesadorDeAlertas @accionId = @accionId
							   ,@alertaDefinicionId = @alertaDefinicionId
							   ,@referenciaProcesoId = @referenciaProcesoId
							   ,@codigoTramite = @codigoTramite
							   ,@textoRepresentativo = @textoRepresentativo
							   ,@creadoPorId = @creadoPorId
							   ,@valorSalida = @valorSalida OUTPUT
END
GO