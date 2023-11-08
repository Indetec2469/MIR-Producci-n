
DELETE from MItblMatrizConfiguracionPresupuestalDetalle
DELETE FROM MItblMatrizConfiguracionPresupuestal
GO

ALTER TABLE dbo.MItblMatrizConfiguracionPresupuestalDetalle
DROP CONSTRAINT FK_MItblMatrizConfiguracionPresupuestalDetalle_GRtblControlMaestro
GO


ALTER TABLE dbo.MItblMatrizConfiguracionPresupuestalDetalle
DROP COLUMN ClasificadorId
GO

ALTER TABLE MItblMatrizConfiguracionPresupuestal
ADD ClasificadorId INT NOT NULL
GO

ALTER TABLE [dbo].[MItblMatrizConfiguracionPresupuestal] WITH NOCHECK
ADD CONSTRAINT [FK_MItblMatrizConfiguracionPresupuestal_GRtblControlMaestro1] FOREIGN KEY ([ClasificadorId]) REFERENCES [dbo].[GRtblControlMaestro] ([ControlId]);
GO
