ALTER TABLE GRtblPermisoFicha ALTER COLUMN CreadoPorId INT NULL
GO

SET IDENTITY_INSERT GRtblPermisoFicha ON
GO
INSERT INTO GRtblPermisoFicha
(
    PermisoFichaId, -- this column value is auto-generated
    Etiqueta,
    Descripcion,
    NodoMenuId,
    EstatusId,
    FechaCreacion,
    Timestamp
)
VALUES
(
    2, -- PermisoFichaId - int
    'Crear solicitudes a nombre de terceros', -- Etiqueta - varchar
    'Permiso para realizar solicitudes a terceros', -- Descripcion - nvarchar
    8, -- NodoMenuId - int
    1, -- EstatusId - int
    GETDATE(), -- FechaCreacion - datetime
    NULL -- Timestamp - timestamp
)
GO
SET IDENTITY_INSERT GRtblPermisoFicha OFF
GO