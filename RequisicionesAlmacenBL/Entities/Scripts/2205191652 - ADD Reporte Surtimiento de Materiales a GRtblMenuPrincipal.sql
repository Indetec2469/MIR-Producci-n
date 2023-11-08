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
    60, --NodoMenuId - int
    'Reportes', -- Etiqueta - varchar
    'SubMenú Reportes', -- Descripcion - varchar
    98, -- TipoNodoId - int
    1, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    NULL, -- Url - varchar
    'icon ion-gear-a', -- Icono - varchar
    0, -- AdmitePermiso - bit
    4, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
),
(
    61, --NodoMenuId - int
    'Surtimiento de Materiales', -- Etiqueta - varchar
    'Reporte Surtimiento de Materiales', -- Descripcion - varchar
    7, -- TipoNodoId - int
    60, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    'compras/reportes/reportesurtidomateriales', -- Url - varchar
    NULL, -- Icono - varchar
    0, -- AdmitePermiso - bit
    1, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
)
SET IDENTITY_INSERT GRtblMenuPrincipal OFF