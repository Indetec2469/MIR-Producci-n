SET IDENTITY_INSERT ARtblControlMaestro ON
INSERT INTO ARtblControlMaestro
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
(
    61, -- ControlId - int
    'EstatusSolicitud', -- Control - varchar
    'Autorizada', -- Valor - varchar
    1, -- Sistema - bit
    1, -- Activo - bit
    0, -- ControlSencillo - bit
    GETDATE()
),
(
    62, -- ControlId - int
    'EstatusSolicitud', -- Control - varchar
    'Por Comprar', -- Valor - varchar
    1, -- Sistema - bit
    1, -- Activo - bit
    0, -- ControlSencillo - bit
    GETDATE()
),
(
    63, -- ControlId - int
    'TipoInventarioMovimiento', -- Control - varchar
    'Requisición Material Surtimiento', -- Valor - varchar
    1, -- Sistema - bit
    1, -- Activo - bit
    0, -- ControlSencillo - bit
    GETDATE()
)
SET IDENTITY_INSERT ARtblControlMaestro OFF