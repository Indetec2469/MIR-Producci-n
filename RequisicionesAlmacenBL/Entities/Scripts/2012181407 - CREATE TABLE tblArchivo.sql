DROP TABLE IF EXISTS tblArchivo
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblArchivo](
	[ArchivoId] [uniqueidentifier] NOT NULL,
	[NombreOriginal] [nvarchar](250) NOT NULL,
	[NombreFisico] [nvarchar](100) NOT NULL,
	[ReferenciaId] [int] NOT NULL,
	[OrigenArchivoId] [int] NOT NULL,
	[RutaFisica] [nvarchar](max) NOT NULL,
	[TipoArchivoId] [int] NOT NULL,
	[Vigente] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_tblArchivo] PRIMARY KEY CLUSTERED 
(
	[ArchivoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblArchivo] ADD  CONSTRAINT [DF_tblArchivo_ArchivoId]  DEFAULT (newid()) FOR [ArchivoId]
GO

ALTER TABLE [dbo].[tblArchivo] ADD  CONSTRAINT [DF_tblArchivo_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO

--ALTER TABLE [dbo].[tblArchivo]  WITH CHECK ADD  CONSTRAINT [DF_tblArchivo_tblUsuario] FOREIGN KEY([CreadoPorId])
--REFERENCES [dbo].[tblUsuario] ([UsuarioId])
--GO

--ALTER TABLE [dbo].[tblArchivo] CHECK CONSTRAINT [DF_tblArchivo_tblUsuario]
--GO

--ALTER TABLE [dbo].[tblArchivo]  WITH CHECK ADD  CONSTRAINT [DF_tblArchivo_tblUsuario1] FOREIGN KEY([ModificadoPorId])
--REFERENCES [dbo].[tblUsuario] ([UsuarioId])
--GO

--ALTER TABLE [dbo].[tblArchivo] CHECK CONSTRAINT [DF_tblArchivo_tblUsuario1]
--GO

ALTER TABLE [dbo].[tblArchivo]  WITH CHECK ADD  CONSTRAINT [FK_tblArchivo_tblControlMaestro] FOREIGN KEY([TipoArchivoId])
REFERENCES [dbo].[tblControlMaestro] ([ControlId])
GO

ALTER TABLE [dbo].[tblArchivo] CHECK CONSTRAINT [FK_tblArchivo_tblControlMaestro]
GO

ALTER TABLE [dbo].[tblArchivo]  WITH CHECK ADD  CONSTRAINT [FK_tblArchivo_tblListadoCMOA] FOREIGN KEY([OrigenArchivoId])
REFERENCES [dbo].[tblListadoCMOA] ([ControlOrigenArchivoId])
GO

ALTER TABLE [dbo].[tblArchivo] CHECK CONSTRAINT [FK_tblArchivo_tblListadoCMOA]
GO