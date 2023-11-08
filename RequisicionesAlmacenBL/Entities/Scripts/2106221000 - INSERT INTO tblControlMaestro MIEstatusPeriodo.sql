SET IDENTITY_INSERT tblControlMaestro ON

-- MIEstatusPeriodo

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	58, -- ControlId
	'MIEstatusPeriodo', -- Control
	'Abierto', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	59, -- ControlId
	'MIEstatusPeriodo', -- Control
	'Cerrado', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	60, -- ControlId
	'MIEstatusPeriodo', -- Control
	'Auditado', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF