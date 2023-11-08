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
    'Solicitud de Materiales y Consumibles', -- Nombre - nvarchar
    'SMC', -- Prefijo - nvarchar
    1, -- Siguiente - bigint
    6, -- Ceros - int,
	2021, -- Ejercicio
    1, -- Activo - bit
    GETDATE(), -- FechaCreacion - datetime
    2
)