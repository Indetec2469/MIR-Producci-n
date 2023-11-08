SET IDENTITY_INSERT tblControlMaestro ON

-- Tipo de Sentido

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	52, -- ControlId
	'Sentido', -- Control
	'Ascendente', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	53, -- ControlId
	'Sentido', -- Control
	'Descendente', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF