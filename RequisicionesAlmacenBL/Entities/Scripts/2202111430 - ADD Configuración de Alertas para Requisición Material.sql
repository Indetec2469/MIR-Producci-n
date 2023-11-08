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
(126, 'AlertaEtapa', 'Requisición Material. Nuevo', 1, 1, 0, GETDATE()),
(127, 'AlertaEtapa', 'Requisición Material. Validación', 1, 1, 0, GETDATE()),
(128, 'AlertaEtapaAccion', 'Enviar', 1, 1, 0, GETDATE()),
(129, 'AlertaEtapaAccion', 'Autorizar', 1, 1, 0, GETDATE()),
(130, 'AlertaEtapaAccion', 'Rechazar', 1, 1, 0, GETDATE()),
(131, 'AlertaEtapaAccion', 'Revisió', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF
GO

SET IDENTITY_INSERT GRtblAlertaEtapaAccion ON
INSERT INTO GRtblAlertaEtapaAccion
(
    AlertaEtapaAccionId,
    EtapaId,
    AccionId,
    PermiteAutorizacion,
    PermiteNotificacion,
    Timestamp
)
VALUES
(
    1, -- AlertaEtapaAccionId - int
    126, -- EtapaId - int
    128, -- AccionId - int
    1, -- PermiteAutorizacion - bit
    1, -- PermiteNotificacion - bit
    NULL -- Timestamp - timestamp
),
(
    2, -- AlertaEtapaAccionId - int
    127, -- EtapaId - int
    129, -- AccionId - int
    0, -- PermiteAutorizacion - bit
    1, -- PermiteNotificacion - bit
    NULL -- Timestamp - timestamp
),
(
    3, -- AlertaEtapaAccionId - int
    127, -- EtapaId - int
    130, -- AccionId - int
    0, -- PermiteAutorizacion - bit
    1, -- PermiteNotificacion - bit
    NULL -- Timestamp - timestamp
),
(
    4, -- AlertaEtapaAccionId - int
    127, -- EtapaId - int
    131, -- AccionId - int
    0, -- PermiteAutorizacion - bit
    1, -- PermiteNotificacion - bit
    NULL -- Timestamp - timestamp
)
SET IDENTITY_INSERT GRtblAlertaEtapaAccion OFF
GO

SET IDENTITY_INSERT GRtblAlertaDefinicion ON
INSERT INTO GRtblAlertaDefinicion
(
    AlertaDefinicionId, -- this column value is auto-generated
    NombreCorto,
    Descripcion,
    AlertaEtapaAccionId,
    NodoMenuId,
    RutaAccion,
    TablaReferencia,
    CampoId,
    CampoCodigo,
    CampoEstadoRegistro,
    NuevoEstado,
    CambiarEstatusATramite,
    EtapaAccionAlAutorizarId,
    EtapaAccionAlRechazarId,
    EtapaAccionAlRevisionId,
    Borrado,
    Timestamp,
    RutaAccionNodoMenuId
)
VALUES
(
    1, -- AlertaDefinicionId - int
    'Requisición Material - Autorización', -- NombreCorto - nvarchar
    'Requisición Material - Etapa Nueva - Acción Enviar - Autorización', -- Descripcion - nvarchar
    1, -- AlertaEtapaAccionId - int
    8, -- NodoMenuId - int
    '/compras/requisiciones/requisicionmaterial/editar/{id}', -- RutaAccion - nvarchar
    'ARtblRequisicionMaterial', -- TablaReferencia - nvarchar
    'RequisicionMaterialId', -- CampoId - nvarchar
    'CodigoRequisicion', -- CampoCodigo - nvarchar
    'EstatusId', -- CampoEstadoRegistro - nvarchar
    69, -- NuevoEstado - int
    1, -- CambiarEstatusATramite - bit
    2, -- EtapaAccionAlAutorizarId - int
    3, -- EtapaAccionAlRechazarId - int
    4, -- EtapaAccionAlRevisionId - int
    0, -- Borrado - bit
    NULL, -- Timestamp - timestamp
    8 -- RutaAccionNodoMenuId - int
),
(
    2, -- AlertaDefinicionId - int
    'Requisición Material - Autorizada', -- NombreCorto - nvarchar
    'Requisición Material - Etapa Validación - Acción Autorizar', -- Descripcion - nvarchar
    2, -- AlertaEtapaAccionId - int
    8, -- NodoMenuId - int
    '/compras/requisiciones/requisicionmaterial/editar/{id}', -- RutaAccion - nvarchar
    'ARtblRequisicionMaterial', -- TablaReferencia - nvarchar
    'RequisicionMaterialId', -- CampoId - nvarchar
    'CodigoRequisicion', -- CampoCodigo - nvarchar
    'EstatusId', -- CampoEstadoRegistro - nvarchar
    64, -- NuevoEstado - int
    1, -- CambiarEstatusATramite - bit
    NULL, -- EtapaAccionAlAutorizarId - int
    NULL, -- EtapaAccionAlRechazarId - int
    NULL, -- EtapaAccionAlRevisionId - int
    0, -- Borrado - bit
    NULL, -- Timestamp - timestamp
    8 -- RutaAccionNodoMenuId - int
),
(
    3, -- AlertaDefinicionId - int
    'Requisición Material - Rechazada', -- NombreCorto - nvarchar
    'Requisición Material - Etapa Validación - Acción Rechazar', -- Descripcion - nvarchar
    3, -- AlertaEtapaAccionId - int
    8, -- NodoMenuId - int
    '/compras/requisiciones/requisicionmaterial/editar/{id}', -- RutaAccion - nvarchar
    'ARtblRequisicionMaterial', -- TablaReferencia - nvarchar
    'RequisicionMaterialId', -- CampoId - nvarchar
    'CodigoRequisicion', -- CampoCodigo - nvarchar
    'EstatusId', -- CampoEstadoRegistro - nvarchar
    74, -- NuevoEstado - int
    1, -- CambiarEstatusATramite - bit
    NULL, -- EtapaAccionAlAutorizarId - int
    NULL, -- EtapaAccionAlRechazarId - int
    NULL, -- EtapaAccionAlRevisionId - int
    0, -- Borrado - bit
    NULL, -- Timestamp - timestamp
    8 -- RutaAccionNodoMenuId - int
),
(
    4, -- AlertaDefinicionId - int
    'Requisición Material - En Revisión', -- NombreCorto - nvarchar
    'Requisición Material - Etapa Validación - Acción Revisión', -- Descripcion - nvarchar
    4, -- AlertaEtapaAccionId - int
    8, -- NodoMenuId - int
    '/compras/requisiciones/requisicionmaterial/editar/{id}', -- RutaAccion - nvarchar
    'ARtblRequisicionMaterial', -- TablaReferencia - nvarchar
    'RequisicionMaterialId', -- CampoId - nvarchar
    'CodigoRequisicion', -- CampoCodigo - nvarchar
    'EstatusId', -- CampoEstadoRegistro - nvarchar
    76, -- NuevoEstado - int
    1, -- CambiarEstatusATramite - bit
    NULL, -- EtapaAccionAlAutorizarId - int
    NULL, -- EtapaAccionAlRechazarId - int
    NULL, -- EtapaAccionAlRevisionId - int
    0, -- Borrado - bit
    NULL, -- Timestamp - timestamp
    8 -- RutaAccionNodoMenuId - int
)
SET IDENTITY_INSERT GRtblAlertaDefinicion OFF
GO