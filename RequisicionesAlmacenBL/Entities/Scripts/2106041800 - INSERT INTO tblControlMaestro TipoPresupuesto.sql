SET IDENTITY_INSERT tblControlMaestro ON

-- Tipo de Presupuesto

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	56, -- ControlId
	'TipoPresupuesto', -- Control
	'Por ejercer', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	57, -- ControlId
	'TipoPresupuesto', -- Control
	'Devengado', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF