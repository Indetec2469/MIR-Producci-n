SET IDENTITY_INSERT tblControlMaestro ON

-- Tipo de Nivel

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	40, -- ControlId
	'Nivel', -- Control
	'Fin', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	41, -- ControlId
	'Nivel', -- Control
	'Proposito', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	42, -- ControlId
	'Nivel', -- Control
	'Componente', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	43, -- ControlId
	'Nivel', -- Control
	'Actividad', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF