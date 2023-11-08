INSERT INTO GRtblAutonumerico
(
    --AutonumericoId - this column value is auto-generated
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
    'Inventario Movimiento Agrupador', -- Nombre - nvarchar
    'IMA', -- Prefijo - nvarchar
    1, -- Siguiente - bigint
    6, -- Ceros - int
    2021, -- Ejercicio - int
    1, -- Activo - bit
    GETDATE(), -- FechaCreacion - datetime
    NULL -- CreadoPorId - INT 
)