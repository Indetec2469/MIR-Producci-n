SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ARtblInventarioMovimiento](
	[InventarioMovtoId] [bigint] IDENTITY(1,1) NOT NULL,
	[AlmacenProductoId] [int] NOT NULL,
	[UnidadMedidaId] [int] NOT NULL,
	[CantidadMovimiento] [decimal](28, 10) NOT NULL,
	[TipoMovimientoId] [int] NOT NULL,
	[CantidadAntesMovto] [decimal](28, 10) NOT NULL,
	[TipoCostoArticuloId] [int] NOT NULL,
	[ValorContableAntesMovto] [money] NOT NULL,
	[MotivoMovto] [varchar](max) NOT NULL,
	[ReferenciaMovtoId] [bigint] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ARtblInventarioMovimiento] PRIMARY KEY CLUSTERED 
(
	[InventarioMovtoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ARtblInventarioMovimiento] ADD  CONSTRAINT [DF_ARtblInventarioMovimiento_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO