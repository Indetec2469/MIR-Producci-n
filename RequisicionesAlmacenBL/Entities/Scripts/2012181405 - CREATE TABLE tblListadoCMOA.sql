DROP TABLE IF EXISTS tblListadoCMOA
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblListadoCMOA](
	[ControlOrigenArchivoId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](150) NOT NULL,
	[Vigente] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_tblListadoCMOA] PRIMARY KEY CLUSTERED 
(
	[ControlOrigenArchivoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblListadoCMOA] ADD  CONSTRAINT [DF_tblListadoCMOA_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO

--ALTER TABLE [dbo].[tblListadoCMOA]  WITH CHECK ADD  CONSTRAINT [DF_tblListadoCMOA_tblUsuario] FOREIGN KEY([CreadoPorId])
--REFERENCES [dbo].[tblUsuario] ([UsuarioId])
--GO

--ALTER TABLE [dbo].[tblListadoCMOA] CHECK CONSTRAINT [DF_tblListadoCMOA_tblUsuario]
--GO