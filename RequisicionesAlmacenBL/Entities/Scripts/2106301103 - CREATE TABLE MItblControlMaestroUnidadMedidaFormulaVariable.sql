/****** Object:  Table [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable]    Script Date: 30/06/2021 11:16:56 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable](
	[UnidadMedidaFormulaVariableId] [int] IDENTITY(1,1) NOT NULL,
	[UnidadMedidaId] [int] NOT NULL,
	[Variable] [varchar](500) NOT NULL,
	[Borrado] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_MItblControlMaestroUnidadMedidaFormulaVariable] PRIMARY KEY CLUSTERED 
(
	[UnidadMedidaFormulaVariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable] ADD  CONSTRAINT [DF_MItblControlMaestroUnidadMedidaFormulaVariable_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO

ALTER TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable]  WITH CHECK ADD  CONSTRAINT [FK_MItblControlMaestroUnidadMedidaFormulaVariable_GRtblUsuario] FOREIGN KEY([CreadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO

ALTER TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable] CHECK CONSTRAINT [FK_MItblControlMaestroUnidadMedidaFormulaVariable_GRtblUsuario]
GO

ALTER TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable]  WITH CHECK ADD  CONSTRAINT [FK_MItblControlMaestroUnidadMedidaFormulaVariable_GRtblUsuario1] FOREIGN KEY([ModificadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO

ALTER TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable] CHECK CONSTRAINT [FK_MItblControlMaestroUnidadMedidaFormulaVariable_GRtblUsuario1]
GO

ALTER TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable]  WITH CHECK ADD  CONSTRAINT [FK_MItblControlMaestroUnidadMedidaFormulaVariable_MItblControlMaestroUnidadMedida] FOREIGN KEY([UnidadMedidaId])
REFERENCES [dbo].[MItblControlMaestroUnidadMedida] ([UnidadMedidaId])
GO

ALTER TABLE [dbo].[MItblControlMaestroUnidadMedidaFormulaVariable] CHECK CONSTRAINT [FK_MItblControlMaestroUnidadMedidaFormulaVariable_MItblControlMaestroUnidadMedida]
GO


