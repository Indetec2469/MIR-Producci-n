SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MItblMatrizIndicadorResultadoIndicadorFormulaVariable](
	[MIRIndicadorFormulaVariableId] [int] IDENTITY(1,1) NOT NULL,
	[MIRIndicadorId] [int] NOT NULL,
	[UnidadMedidaFormulaVariableId] [int] NOT NULL,
	[DescripcionVariable] [varchar](500) NOT NULL,
	[EstatusId] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_MItblMatrizIndicadorResultadoIndicadorVariable] PRIMARY KEY CLUSTERED 
(
	[MIRIndicadorFormulaVariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MItblMatrizIndicadorResultadoIndicadorFormulaVariable] ADD  CONSTRAINT [DF_MItblMatrizIndicadorResultadoIndicadorVariable_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO


