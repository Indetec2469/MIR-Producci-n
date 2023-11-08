USE [RequisicionesAlmacen]
GO

/****** Object:  Table [dbo].[MItblMatrizConfiguracionPresupuestalDetalle]    Script Date: 6/15/2021 8:45:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[MItblMatrizConfiguracionPresupuestalDetalle]
GO

CREATE TABLE [dbo].[MItblMatrizConfiguracionPresupuestalDetalle](
	[ConfiguracionPresupuestoDetalleId] [int] IDENTITY(1,1) NOT NULL,
	[ConfiguracionPresupuestoId] [int] NOT NULL,
	[ClasificadorId] [int] NOT NULL,
	[MIRIndicadorId] [int] NOT NULL,
	[Enero] [money] NOT NULL,
	[Febrero] [money] NOT NULL,
	[Marzo] [money] NOT NULL,
	[Abril] [money] NOT NULL,
	[Mayo] [money] NOT NULL,
	[Junio] [money] NOT NULL,
	[Julio] [money] NOT NULL,
	[Agosto] [money] NOT NULL,
	[Septiembre] [money] NOT NULL,
	[Octubre] [money] NOT NULL,
	[Noviembre] [money] NOT NULL,
	[Diciembre] [money] NOT NULL,
	[Anual] [money] NOT NULL,
	[Porcentaje] [decimal](18, 2) NOT NULL,
	[EstatusId] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_MItblMatrizConfiguracionPresupuestalDetalle] PRIMARY KEY CLUSTERED 
(
	[ConfiguracionPresupuestoDetalleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MItblMatrizConfiguracionPresupuestalDetalle] ADD  CONSTRAINT [DF_MItblMatrizConfiguracionPresupuestalDetalle_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO


