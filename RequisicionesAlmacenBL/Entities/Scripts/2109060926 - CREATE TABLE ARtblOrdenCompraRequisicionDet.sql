SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARtblOrdenCompraRequisicionDet](
	[OrdenCompraRequisicionDetId] [int] IDENTITY(1,1) NOT NULL,
	[OrdenCompraDetId] [int] NOT NULL,
	[RequisicionMaterialDetalleId] [int] NOT NULL,
	[Cantidad] [decimal](28, 10) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ARtblOrdenCompraRequisicionDet] PRIMARY KEY CLUSTERED 
(
	[OrdenCompraRequisicionDetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ARtblOrdenCompraRequisicionDet] ADD  CONSTRAINT [DF_ARtblOrdenCompraRequisicionDet_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO


