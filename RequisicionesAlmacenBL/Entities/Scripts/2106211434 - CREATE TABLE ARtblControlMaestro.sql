DROP TABLE IF EXISTS [dbo].[ARtblControlMaestro]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ARtblControlMaestro](
	[ControlId] [int] IDENTITY(5000000,1) NOT NULL,
	[Control] [varchar](100) NOT NULL,
	[Valor] [varchar](max) NOT NULL,
	[Sistema] [bit] NOT NULL,
	[Activo] [bit] NOT NULL,
	[ControlSencillo] [bit] NOT NULL,
	[ModuloId] [int] NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_tblControlMaestro] PRIMARY KEY CLUSTERED 
(
	[ControlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ARtblControlMaestro] ADD  CONSTRAINT [DF_tblControlMaestro_Activo]  DEFAULT ((1)) FOR [Activo]
GO

ALTER TABLE [dbo].[ARtblControlMaestro] ADD  CONSTRAINT [DF_tblControlMaestro_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO

--SET IDENTITY_INSERT SACG0000001.dbo.ARtblControlMaestro ON
--INSERT INTO SACG0000001.dbo.ARtblControlMaestro
--(
--    ControlId,
--    Control,
--    Valor,
--    Sistema,
--    Activo,
--    ControlSencillo,
--    ModuloId,
--    FechaCreacion
--)
--SELECT ControlId,
--       Control,
--       Valor,
--       Sistema,
--       Activo,
--       ControlSencillo,
--       ModuloId,
--       GETDATE()
--FROM RequisicionesAlmacenDatos.dbo.tblControlMaestro
--SET IDENTITY_INSERT SACG0000001.dbo.ARtblControlMaestro OFF
--GO

--UPDATE ARtblControlMaestro SET Valor = 'Guardada' WHERE ControlId = 22
--GO

SET IDENTITY_INSERT SACG0000001.dbo.ARtblControlMaestro ON
INSERT INTO SACG0000001.dbo.ARtblControlMaestro
(
    ControlId,
    Control,
    Valor,
    Sistema,
    Activo,
    ControlSencillo,
    ModuloId,
    FechaCreacion
)
VALUES
(1, 'EstatusRegistro', 'Activo', 1, 1, 0, NULL, GETDATE()),
(2, 'EstatusRegistro', 'Inactivo', 1, 1, 0, NULL, GETDATE()),
(3, 'EstatusRegistro', 'Borrado', 1, 1, 0, NULL, GETDATE()),
(4, 'TipoCompra', 'Compra directa', 1, 1, 0, NULL, GETDATE()),
(5, 'TipoCompra', 'Invitación compra', 1, 1, 0, NULL, GETDATE()),
(6, 'TipoNodoMenu', 'Módulo', 1, 1, 0, NULL, GETDATE()),
(7, 'TipoNodoMenu', 'Ficha', 1, 1, 0, NULL, GETDATE()),
(8, 'SistemaAcceso', 'WEB', 1, 1, 0, NULL, GETDATE()),
(9, 'TipoMovimiento', 'Incrementa', 1, 1, 0, NULL, GETDATE()),
(10, 'TipoMovimiento', 'Disminuye', 1, 1, 0, NULL, GETDATE()),
(11, 'TipoArchivo', 'Documento de texto', 1, 1, 0, NULL, GETDATE()),
(12, 'TipoArchivo', 'Hoja de cálculo', 1, 1, 0, NULL, GETDATE()),
(13, 'TipoArchivo', 'PDF', 1, 1, 0, NULL, GETDATE()),
(14, 'TipoArchivo', 'XML', 1, 1, 0, NULL, GETDATE()),
(15, 'TipoArchivo', 'Imagen', 1, 1, 0, NULL, GETDATE()),
(16, 'TipoArchivo', 'Otro', 1, 1, 0, NULL, GETDATE()),
(17, 'TipoCalculoNombreArchivo', 'Sencillo', 1, 1, 0, NULL, GETDATE()),
(18, 'RutaTemporalArchivo', 'C:/PIXVS Archivos/REQUISICIONES/tmp', 1, 1, 1, NULL, GETDATE()),
(19, 'RutaRaizArchivo', 'C:/PIXVS Archivos/REQUISICIONES/raiz', 1, 1, 1, NULL, GETDATE()),
(20, 'CantidadCerosAutonumericoArchivo', '12', 1, 1, 1, NULL, GETDATE()),
(21, 'IdAutonumericoArchivo', '344', 1, 1, 1, NULL, GETDATE()),
(22, 'EstatusSolicitud', 'Guardada', 1, 1, 0, NULL, GETDATE()),
(23, 'EstatusSolicitud', 'En proceso autorización', 1, 1, 0, NULL, GETDATE()),
(24, 'EstatusSolicitud', 'Rechazada', 1, 1, 0, NULL, GETDATE()),
(25, 'EstatusSolicitud', 'Revisión', 1, 1, 0, NULL, GETDATE()),
(26, 'EstatusSolicitud', 'Pendiente por surtir', 1, 1, 0, NULL, GETDATE()),
(27, 'EstatusSolicitud', 'Surtido parcial', 1, 1, 0, NULL, GETDATE()),
(28, 'EstatusSolicitud', 'Completa', 1, 1, 0, NULL, GETDATE()),
(29, 'EstatusSolicitud', 'Cerrada', 1, 1, 0, NULL, GETDATE()),
(30, 'EstatusSolicitud', 'Activa', 1, 1, 0, NULL, GETDATE()),
(31, 'EstatusSolicitud', 'Borrada', 1, 1, 0, NULL, GETDATE()),
(32, 'EstatusInventarioFisico', 'En Proceso', 1, 1, 0, NULL, GETDATE()),
(33, 'EstatusInventarioFisico', 'Terminado', 1, 1, 0, NULL, GETDATE()),
(34, 'EstatusInventarioFisico', 'Cancelado', 1, 1, 0, NULL, GETDATE()),
(35, 'TipoInventarioMovimiento', 'Inventario Físico', 1, 1, 0, NULL, GETDATE()),
(36, 'TipoInventarioMovimiento', 'Inventario Ajuste', 1, 1, 0, NULL, GETDATE()),
(37, 'NivelGobierno', 'Federal', 1, 1, 0, NULL, GETDATE()),
(38, 'NivelGobierno', 'Estatal', 1, 1, 0, NULL, GETDATE()),
(39, 'NivelGobierno', 'Municipal', 1, 1, 0, NULL, GETDATE()),
(40, 'Nivel', 'Fin', 1, 1, 0, NULL, GETDATE()),
(41, 'Nivel', 'Proposito', 1, 1, 0, NULL, GETDATE()),
(42, 'Nivel', 'Componente', 1, 1, 0, NULL, GETDATE()),
(43, 'Nivel', 'Actividad', 1, 1, 0, NULL, GETDATE()),
(44, 'Nivel', 'Calidad', 1, 1, 0, NULL, GETDATE()),
(45, 'FrecuenciaMedicion', 'Mensual', 1, 1, 0, NULL, GETDATE()),
(46, 'FrecuenciaMedicion', 'Trimestral', 1, 1, 0, NULL, GETDATE()),
(47, 'FrecuenciaMedicion', 'Semestral', 1, 1, 0, NULL, GETDATE()),
(48, 'FrecuenciaMedicion', 'Anual', 1, 1, 0, NULL, GETDATE()),
(49, 'FrecuenciaMedicion', 'Bianual', 1, 1, 0, NULL, GETDATE()),
(50, 'FrecuenciaMedicion', 'Trianual', 1, 1, 0, NULL, GETDATE()),
(51, 'FrecuenciaMedicion', 'Sexenal', 1, 1, 0, NULL, GETDATE()),
(52, 'Sentido', 'Ascendente', 1, 1, 0, NULL, GETDATE()),
(53, 'Sentido', 'Descendente', 1, 1, 0, NULL, GETDATE()),
(54, 'TipoComponente', 'Relación Actividad - Proyecto', 1, 1, 0, NULL, GETDATE()),
(55, 'TipoComponente', 'Relación Componente - Proyecto', 1, 1, 0, NULL, GETDATE()),
(56, 'TipoPresupuesto', 'Por ejercer', 1, 1, 0, NULL, GETDATE()),
(57, 'TipoPresupuesto', 'Devengado', 1, 1, 0, NULL, GETDATE())
SET IDENTITY_INSERT SACG0000001.dbo.ARtblControlMaestro OFF
GO

--SELECT '(' + CAST(ControlId AS NVARCHAR(1000)) + ', ''' +
--       CAST(Control AS NVARCHAR(1000)) + ''', ''' +
--       CAST(Valor AS NVARCHAR(1000)) + ''', ' +
--       CAST(Sistema AS NVARCHAR(1000)) + ', ' +
--       CAST(Activo AS NVARCHAR(1000)) + ', ' +
--       CAST(ControlSencillo AS NVARCHAR(1000)) + ', ' +
--       'NULL, GETDATE()),'
--FROM RequisicionesAlmacenDatos.dbo.tblControlMaestro