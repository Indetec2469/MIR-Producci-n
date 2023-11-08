SET IDENTITY_INSERT tblControlMaestro ON

-- Tipo de Componente

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	54, -- ControlId
	'TipoComponente', -- Control
	'Relación Actividad - Proyecto', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),(
	55, -- ControlId
	'TipoComponente', -- Control
	'Relación Componente - Proyecto', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF