ALTER TABLE ARtblInvitacionArticulo ADD ConfiguracionMontoId TINYINT
GO

UPDATE ia SET ia.ConfiguracionMontoId = ca.ConfiguracionMontoId
FROM ARtblInvitacionArticulo AS ia
     INNER JOIN ARtblControlMaestroConfiguracionMontoCompra AS ca ON ia.TipoCompraId = ca.TipoCompraId AND ca.EstatusId = 1
GO

ALTER TABLE ARtblInvitacionArticulo ALTER COLUMN ConfiguracionMontoId TINYINT NOT NULL
GO

ALTER TABLE ARtblInvitacionArticulo  WITH CHECK ADD  CONSTRAINT FK_ConfiguracionMontoId FOREIGN KEY(ConfiguracionMontoId)
REFERENCES ARtblControlMaestroConfiguracionMontoCompra (ConfiguracionMontoId)
GO

ALTER TABLE ARtblInvitacionArticulo CHECK CONSTRAINT FK_ConfiguracionMontoId
GO