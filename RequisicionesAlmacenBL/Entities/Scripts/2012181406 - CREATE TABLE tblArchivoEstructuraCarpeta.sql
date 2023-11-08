DROP TABLE IF EXISTS tblArchivoEstructuraCarpeta
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblArchivoEstructuraCarpeta](
	[EstructuraId] [int] IDENTITY(1,1) NOT NULL,
	[NombreCarpeta] [nvarchar](50) NULL,
	[DescripcionCorta] [nvarchar](100) NOT NULL,
	[EstructuraReferenciaId] [int] NULL,
	[OrigenArchivoId] [int] NULL,
	[NombreCalculado] [bit] NULL,
	[TipoCalculo] [int] NULL,
	[Vigente] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_tblArchivoEstructuraCarpeta] PRIMARY KEY CLUSTERED 
(
	[EstructuraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblArchivoEstructuraCarpeta] ADD  CONSTRAINT [DF_tblArchivoEstructuraCarpeta_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO

ALTER TABLE [dbo].[tblArchivoEstructuraCarpeta]  WITH CHECK ADD  CONSTRAINT [DF_tblArchivoEstructuraCarpeta_tblControlMaestro] FOREIGN KEY([TipoCalculo])
REFERENCES [dbo].[tblControlMaestro] ([ControlId])
GO

ALTER TABLE [dbo].[tblArchivoEstructuraCarpeta] CHECK CONSTRAINT [DF_tblArchivoEstructuraCarpeta_tblControlMaestro]
GO

ALTER TABLE [dbo].[tblArchivoEstructuraCarpeta]  WITH CHECK ADD  CONSTRAINT [DF_tblArchivoEstructuraCarpeta_tblListadoCMOA] FOREIGN KEY([OrigenArchivoId])
REFERENCES [dbo].[tblListadoCMOA] ([ControlOrigenArchivoId])
GO

ALTER TABLE [dbo].[tblArchivoEstructuraCarpeta] CHECK CONSTRAINT [DF_tblArchivoEstructuraCarpeta_tblListadoCMOA]
GO

--ALTER TABLE [dbo].[tblArchivoEstructuraCarpeta]  WITH CHECK ADD  CONSTRAINT [DF_tblArchivoEstructuraCarpeta_tblUsuario] FOREIGN KEY([CreadoPorId])
--REFERENCES [dbo].[tblUsuario] ([UsuarioId])
--GO

--ALTER TABLE [dbo].[tblArchivoEstructuraCarpeta] CHECK CONSTRAINT [DF_tblArchivoEstructuraCarpeta_tblUsuario]
--GO