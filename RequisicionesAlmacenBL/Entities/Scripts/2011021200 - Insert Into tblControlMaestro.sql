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
	1, -- ControlId
	'EstatusRegistro', -- Control
	'Activo', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	2, -- ControlId
	'EstatusRegistro', -- Control
	'Inactivo', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	3, -- ControlId
	'EstatusRegistro', -- Control
	'Borrado', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	4, -- ControlId
	'TipoCompra', -- Control
	'Compra directa', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	5, -- ControlId
	'TipoCompra', -- Control
	'Invitación compra', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF


