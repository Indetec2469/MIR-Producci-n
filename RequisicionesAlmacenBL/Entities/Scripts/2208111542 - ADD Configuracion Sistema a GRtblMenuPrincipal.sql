SET IDENTITY_INSERT GRtblMenuPrincipal ON
INSERT INTO GRtblMenuPrincipal
(
    NodoMenuId, -- this column value is auto-generated
    Etiqueta,
    Descripcion,
    TipoNodoId,
    NodoPadreId,
    SistemaAccesoId,
    Url,
    Icono,
    AdmitePermiso,
    Orden,
    EstatusId,
    Timestamp
)
VALUES
(
    64, --NodoMenuId - int
    'Sistema', -- Etiqueta - varchar
    'SubMenú Sistema', -- Descripcion - varchar
    98, -- TipoNodoId - int
    50, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    NULL, -- Url - varchar
    'icon ion-gear-a', -- Icono - varchar
    0, -- AdmitePermiso - bit
    2, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
),
(
    65, --NodoMenuId - int
    'Configuración', -- Etiqueta - varchar
    'Ficha Configuración', -- Descripcion - varchar
    7, -- TipoNodoId - int
    64, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    'sistemas/sistema/configuracion', -- Url - varchar
    NULL, -- Icono - varchar
    0, -- AdmitePermiso - bit
    1, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
)
SET IDENTITY_INSERT GRtblMenuPrincipal OFF