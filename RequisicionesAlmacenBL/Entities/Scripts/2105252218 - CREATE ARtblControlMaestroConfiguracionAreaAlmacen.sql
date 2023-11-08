SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARtblControlMaestroConfiguracionAreaAlmacen](
	[ConfiguracionAreaAlmacenId] [int] IDENTITY(1,1) NOT NULL,
	[ConfiguracionAreaId] [int] NOT NULL,
	[AlmacenId] [varchar](4) NOT NULL,
	[Borrado] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ARtblControlMaestroConfiguracionAreaAlmacen] PRIMARY KEY CLUSTERED 
(
	[ConfiguracionAreaAlmacenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ARtblControlMaestroConfiguracionAreaAlmacen] ADD  CONSTRAINT [DF_ARtblControlMaestroConfiguracionAreaAlmacen_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO


