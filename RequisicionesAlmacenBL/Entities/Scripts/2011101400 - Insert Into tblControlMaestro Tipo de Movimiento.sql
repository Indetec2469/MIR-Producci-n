SET IDENTITY_INSERT tblControlMaestro ON

-- Activo, Inactivo, Borrado, Compra directa y Invitación compra

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	9, -- ControlId
	'TipoMovimiento', -- Control
	'Incrementa', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	10, -- ControlId
	'TipoMovimiento', -- Control
	'Disminuye', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF