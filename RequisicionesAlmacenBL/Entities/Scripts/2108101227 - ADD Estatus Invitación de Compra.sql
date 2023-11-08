INSERT INTO GRtblAutonumerico
(
    --AutonumericoId - column value is auto-generated
    Nombre,
    Prefijo,
    Siguiente,
    Ceros,
	Ejercicio,
    Activo,
    FechaCreacion
)
VALUES
(
    -- AutonumericoId - tinyint
    'Invitación de Compra', -- Nombre - nvarchar
    'INC', -- Prefijo - nvarchar
    1, -- Siguiente - bigint
    6, -- Ceros - int,
	2021, -- Ejercicio
    1, -- Activo - bit
    GETDATE() -- FechaCreacion - datetime
)

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
(91, 'AREstatusInvitacionCompra', 'Cancelada', 1, 1, 0, GETDATE()),
(92, 'AREstatusInvitacionCompra', 'Guardada', 1, 1, 0, GETDATE()),
(93, 'AREstatusInvitacionCompraDetalle', 'Activo', 1, 1, 0, GETDATE()),
(94, 'AREstatusInvitacionCompraDetalle', 'Cancelado', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF