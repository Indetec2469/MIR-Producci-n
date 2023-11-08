ALTER TABLE GRtblAlertaDefinicion ADD CampoUsuarioRegistro VARCHAR (100) NULL
GO

UPDATE GRtblAlertaDefinicion SET CampoUsuarioRegistro = 'CreadoPorId'
GO

ALTER TABLE GRtblAlertaDefinicion ALTER COLUMN CampoUsuarioRegistro VARCHAR (100) NOT NULL
GO