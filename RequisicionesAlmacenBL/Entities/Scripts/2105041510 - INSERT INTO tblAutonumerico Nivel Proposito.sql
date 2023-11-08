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
    'Nivel Proposito', -- Nombre - nvarchar
    'INPRO', -- Prefijo - nvarchar
    1, -- Siguiente - bigint
    6, -- Ceros - int,
    1, -- Activo - bit
    GETDATE(), -- FechaCreacion - datetime
    2
)
GO