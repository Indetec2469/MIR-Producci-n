SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblControlMaestroConceptoAjusteInventario](
	[ConceptoAjusteInventarioId] [int] IDENTITY(1,1) NOT NULL,
	[ConceptoAjuste] [varchar](150) NOT NULL,
	[TipoMovimientoId] [int] NOT NULL,
	[CatalogoCuentaId] [int] NOT NULL,
	[SolicitaEvidencia] [bit] NOT NULL,
	[EstatusId] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_tblControlMaestroConceptoAjusteInventario] PRIMARY KEY CLUSTERED 
(
	[ConceptoAjusteInventarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblControlMaestroConceptoAjusteInventario] ADD  CONSTRAINT [DF_tblControlMaestroConceptoAjusteInventario_SolicitaEvidencia]  DEFAULT ((0)) FOR [SolicitaEvidencia]
GO

ALTER TABLE [dbo].[tblControlMaestroConceptoAjusteInventario] ADD  CONSTRAINT [DF_tblControlMaestroConceptoAjusteInventario_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO


