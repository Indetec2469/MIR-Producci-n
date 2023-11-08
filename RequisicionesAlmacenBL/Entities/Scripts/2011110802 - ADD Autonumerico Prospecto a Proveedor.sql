INSERT INTO tblAutonumerico
(
    --AutonumericoId - column value is auto-generated
    Nombre,
    Prefijo,
    Siguiente,
    Ceros,
    Activo,
    FechaCreacion,
    CreadoPorId
)
VALUES
(
    -- AutonumericoId - tinyint
    'Prospecto a Proveedor', -- Nombre - nvarchar
    'PAP', -- Prefijo - nvarchar
    1, -- Siguiente - bigint
    6, -- Ceros - int
    1, -- Activo - bit
    GETDATE(), -- FechaCreacion - datetime
    2
)