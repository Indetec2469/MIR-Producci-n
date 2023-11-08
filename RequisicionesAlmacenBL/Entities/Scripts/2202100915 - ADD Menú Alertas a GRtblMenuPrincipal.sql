UPDATE GRtblMenuPrincipal SET Orden = Orden + 1 WHERE NodoPadreId IS NULL
GO

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
    56, --NodoMenuId - int
    'Alertas', -- Etiqueta - varchar
    'Menú Alertas', -- Descripcion - varchar
    6, -- TipoNodoId - int
    NULL, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    NULL, -- Url - varchar
    NULL, -- Icono - varchar
    0, -- AdmitePermiso - bit
    1, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
),
(
    57, --NodoMenuId - int
    'Alertas', -- Etiqueta - varchar
    'SubMenú Alertas', -- Descripcion - varchar
    98, -- TipoNodoId - int
    56, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    NULL, -- Url - varchar
    'icon ion-gear-a', -- Icono - varchar
    0, -- AdmitePermiso - bit
    1, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
),
(
    58, --NodoMenuId - int
    'Configuración', -- Etiqueta - varchar
    'Ficha de configuración de alertas', -- Descripcion - varchar
    7, -- TipoNodoId - int
    57, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    'alertas/alertas/configuracion', -- Url - varchar
    NULL, -- Icono - varchar
    1, -- AdmitePermiso - bit
    1, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
),
(
    59, --NodoMenuId - int
    'Notificaciones', -- Etiqueta - varchar
    'Ficha Alertas', -- Descripcion - varchar
    7, -- TipoNodoId - int
    57, -- NodoPadreId - int
    8, -- SistemaAccesoId - int
    'alertas/alertas/notificaciones', -- Url - varchar
    NULL, -- Icono - varchar
    1, -- AdmitePermiso - bit
    2, -- Orden - tinyint
    1, -- EstatusId - int
    NULL -- Timestamp - timestamp
)
SET IDENTITY_INSERT GRtblMenuPrincipal OFF