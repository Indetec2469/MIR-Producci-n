SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MItblFormulaVariable](
	[FormulaVariableId] [INT] IDENTITY(1,1) NOT NULL,
	[FormulaId] [INT] NOT NULL,
	[Variable] [VARCHAR](500) NOT NULL,
	[Borrado] [BIT] NOT NULL,
	[FechaCreacion] [DATETIME] NOT NULL,
	[CreadoPorId] [INT] NOT NULL,
	[FechaUltimaModificacion] [DATETIME] NULL,
	[ModificadoPorId] [INT] NULL,
	[Timestamp] [TIMESTAMP] NOT NULL,
 CONSTRAINT [PK_MItblFormulaVariable] PRIMARY KEY CLUSTERED 
(
	[FormulaVariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MItblFormulaVariable] ADD  CONSTRAINT [DF_MItblFormulaVariable_FechaCreacion]  DEFAULT (GETDATE()) FOR [FechaCreacion]
GO


