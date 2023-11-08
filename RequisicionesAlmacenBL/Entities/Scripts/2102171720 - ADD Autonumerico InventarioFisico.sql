INSERT INTO tblAutonumerico
(
    --AutonumericoId - column value is auto-generated
    Nombre,
    Prefijo,
    Siguiente,
    Ceros,
	Ejercicio,
    Activo,
    FechaCreacion,
    CreadoPorId
)
VALUES
(
    -- AutonumericoId - tinyint
    'Inventarios Físicos', -- Nombre - nvarchar
    'IF', -- Prefijo - nvarchar
    1, -- Siguiente - bigint
    6, -- Ceros - int,
	NULL, -- Ejercicio
    1, -- Activo - bit
    GETDATE(), -- FechaCreacion - datetime
    2
)