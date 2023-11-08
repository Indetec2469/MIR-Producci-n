DELETE FROM GRtblControlMaestro WHERE Control IN ('EstatusSolicitud', 'EstatusSolicitudDetalle')
GO

SET IDENTITY_INSERT GRtblControlMaestro ON
INSERT INTO GRtblControlMaestro
(
    ControlId,
    Control,
    Valor,
    Sistema,
    Activo,
    ControlSencillo,
    FechaCreacion
)
VALUES
(64, 'AREstatusRequisicion', 'Autorizada', 1, 1, 0, GETDATE()),
(65, 'AREstatusRequisicion', 'Cancelada', 1, 1, 0, GETDATE()),
(66, 'AREstatusRequisicion', 'Cerrada', 1, 1, 0, GETDATE()),
(67, 'AREstatusRequisicion', 'En almacén', 1, 1, 0, GETDATE()),
(68, 'AREstatusRequisicion', 'En proceso', 1, 1, 0, GETDATE()),
(69, 'AREstatusRequisicion', 'Enviada', 1, 1, 0, GETDATE()),
(70, 'AREstatusRequisicion', 'Finalizada', 1, 1, 0, GETDATE()),
(71, 'AREstatusRequisicion', 'Guardada', 1, 1, 0, GETDATE()),
(72, 'AREstatusRequisicion', 'Orden Compra', 1, 1, 0, GETDATE()),
(73, 'AREstatusRequisicion', 'Por comprar', 1, 1, 0, GETDATE()),
(74, 'AREstatusRequisicion', 'Rechazada', 1, 1, 0, GETDATE()),
(75, 'AREstatusRequisicion', 'Requisición Compra', 1, 1, 0, GETDATE()),
(76, 'AREstatusRequisicion', 'Revisión', 1, 1, 0, GETDATE()),
(77, 'AREstatusRequisicionDetalle', 'Activo', 1, 1, 0, GETDATE()),
(78, 'AREstatusRequisicionDetalle', 'Cancelado', 1, 1, 0, GETDATE()),
(79, 'AREstatusRequisicionDetalle', 'Cerrado', 1, 1, 0, GETDATE()),
(80, 'AREstatusRequisicionDetalle', 'En almacén', 1, 1, 0, GETDATE()),
(81, 'AREstatusRequisicionDetalle', 'Enviado', 1, 1, 0, GETDATE()),
(82, 'AREstatusRequisicionDetalle', 'Modificado', 1, 1, 0, GETDATE()),
(83, 'AREstatusRequisicionDetalle', 'Por comprar', 1, 1, 0, GETDATE()),
(84, 'AREstatusRequisicionDetalle', 'Por surtir', 1, 1, 0, GETDATE()),
(85, 'AREstatusRequisicionDetalle', 'Rechazado', 1, 1, 0, GETDATE()),
(86, 'AREstatusRequisicionDetalle', 'Relacionado a OC', 1, 1, 0, GETDATE()),
(87, 'AREstatusRequisicionDetalle', 'Relacionado a RC', 1, 1, 0, GETDATE()),
(88, 'AREstatusRequisicionDetalle', 'Revisión', 1, 1, 0, GETDATE()),
(89, 'AREstatusRequisicionDetalle', 'Surtido', 1, 1, 0, GETDATE()),
(90, 'AREstatusRequisicionDetalle', 'Surtido parcial', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF