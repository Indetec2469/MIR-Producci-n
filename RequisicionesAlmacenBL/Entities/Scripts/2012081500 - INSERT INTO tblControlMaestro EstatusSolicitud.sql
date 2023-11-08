SET IDENTITY_INSERT tblControlMaestro ON

-- Tipo de Estatus Solicitud

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
    CreadoPorId
) VALUES(
	22, -- ControlId
	'EstatusSolicitud', -- Control
	'En edición', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	23, -- ControlId
	'EstatusSolicitud', -- Control
	'En proceso autorización', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	24, -- ControlId
	'EstatusSolicitud', -- Control
	'Rechazada', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	25, -- ControlId
	'EstatusSolicitud', -- Control
	'Revisión', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	26, -- ControlId
	'EstatusSolicitud', -- Control
	'Pendiente por surtir', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	27, -- ControlId
	'EstatusSolicitud', -- Control
	'Surtido parcial', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	28, -- ControlId
	'EstatusSolicitud', -- Control
	'Completa', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	29, -- ControlId
	'EstatusSolicitud', -- Control
	'Cerrada', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
),
(
	30, -- ControlId
	'EstatusSolicitud', -- Control
	'Activa', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
), (
	31, -- ControlId
	'EstatusSolicitud', -- Control
	'Borrada', -- Valor
	1, -- Sistema
	1, -- Activo
	0, -- ControlSencillo
	1 -- CreadoPorId
)

SET IDENTITY_INSERT tblControlMaestro OFF