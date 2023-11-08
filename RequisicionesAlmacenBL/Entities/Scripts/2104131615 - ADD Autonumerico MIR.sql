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
    'MIR', -- Nombre - nvarchar
    'MIR', -- Prefijo - nvarchar
    1, -- Siguiente - bigint
    6, -- Ceros - int,
	YEAR(GETDATE()), -- Ejercicio
    1, -- Activo - bit
    GETDATE(), -- FechaCreacion - datetime
    2
)
GO

ALTER TABLE MItblMatrizIndicadorResultado ADD Codigo VARCHAR(13) NOT NULL
GO