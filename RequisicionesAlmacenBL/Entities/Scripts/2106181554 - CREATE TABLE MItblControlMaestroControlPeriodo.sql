SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MItblControlMaestroControlPeriodo](
	[ControlPeriodoId] [TINYINT] IDENTITY(1,1) NOT NULL,
	[Codigo] [VARCHAR](3) NOT NULL,
	[Periodo] [VARCHAR](50) NOT NULL,
	[EstatusPeriodoId] [INT] NOT NULL,
	[FechaCreacion] [DATETIME] NOT NULL,
	[CreadoPorId] [INT] NOT NULL,
	[FechaUltimaModificacion] [DATETIME] NULL,
	[ModificadoPorId] [INT] NULL,
	[Timestamp] [TIMESTAMP] NOT NULL,
 CONSTRAINT [PK_MItblControlMaestroControlPeriodo] PRIMARY KEY CLUSTERED 
(
	[ControlPeriodoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MItblControlMaestroControlPeriodo] ADD  CONSTRAINT [DF_MItblControlMaestroControlPeriodo_FechaCreacion]  DEFAULT (GETDATE()) FOR [FechaCreacion]
GO


