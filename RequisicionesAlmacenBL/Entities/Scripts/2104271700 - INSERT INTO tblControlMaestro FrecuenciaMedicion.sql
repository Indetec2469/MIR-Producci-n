SET IDENTITY_INSERT tblControlMaestro ON

-- Tipo de Frencuencia Medicion

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	45, -- ControlId
	'FrecuenciaMedicion', -- Control
	'Mensual', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	46, -- ControlId
	'FrecuenciaMedicion', -- Control
	'Trimestral', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	47, -- ControlId
	'FrecuenciaMedicion', -- Control
	'Semestral', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	48, -- ControlId
	'FrecuenciaMedicion', -- Control
	'Anual', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	49, -- ControlId
	'FrecuenciaMedicion', -- Control
	'Bianual', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	50, -- ControlId
	'FrecuenciaMedicion', -- Control
	'Trianual', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	51, -- ControlId
	'FrecuenciaMedicion', -- Control
	'Sexenal', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF