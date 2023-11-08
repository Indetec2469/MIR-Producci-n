SET IDENTITY_INSERT GRtblControlMaestro ON
INSERT INTO GRtblControlMaestro
(
    ControlId,
    Control,
    Valor,
    Sistema,
    Activo,
    ControlSencillo,
    FechaCreacion
)
VALUES
(132, 'TipoCompra', 'Licitación', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF
GO

ALTER TABLE ARtblInvitacionArticulo ADD TipoCompraId INT NULL
GO

UPDATE ARtblInvitacionArticulo SET TipoCompraId = 5 -- Invitación compra
GO

ALTER TABLE ARtblInvitacionArticulo ALTER COLUMN TipoCompraId INT NOT NULL
GO

ALTER TABLE ARtblInvitacionArticulo  WITH CHECK ADD  CONSTRAINT FK_TipoCompraId FOREIGN KEY(TipoCompraId)
REFERENCES GRtblControlMaestro (ControlId)
GO

ALTER TABLE ARtblInvitacionArticulo CHECK CONSTRAINT FK_TipoCompraId
GO