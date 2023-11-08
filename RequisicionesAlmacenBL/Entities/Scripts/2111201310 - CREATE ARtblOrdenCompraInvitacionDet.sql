DROP TABLE IF EXISTS [ARtblOrdenCompraInvitacionDet]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ARtblOrdenCompraInvitacionDet](
	[OrdenCompraInvitacionDetId] [int] IDENTITY(1,1) NOT NULL,
	[OrdenCompraDetId] [int] NOT NULL,
	[InvitacionCompraDetalleId] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ARtblOrdenCompraInvitacionDet] PRIMARY KEY CLUSTERED 
(
	[OrdenCompraInvitacionDetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ARtblOrdenCompraInvitacionDet] ADD  CONSTRAINT [DF_ARtblOrdenCompraInvitacionDet_FechaCreacion]  DEFAULT (GETDATE()) FOR [FechaCreacion]
GO

ALTER TABLE [dbo].[ARtblOrdenCompraInvitacionDet]  WITH CHECK ADD  CONSTRAINT [FK_ARtblOrdenCompraInvitacionDet_tblOrdenCompraDet] FOREIGN KEY([OrdenCompraDetId])
REFERENCES [dbo].[tblOrdenCompraDet] ([OrdenCompraDetId])
GO

ALTER TABLE [dbo].[ARtblOrdenCompraInvitacionDet] CHECK CONSTRAINT [FK_ARtblOrdenCompraInvitacionDet_tblOrdenCompraDet]
GO

ALTER TABLE [dbo].[ARtblOrdenCompraInvitacionDet]  WITH CHECK ADD  CONSTRAINT [FK_ARtblOrdenCompraInvitacionDet_ARtblInvitacionCompraDetalle] FOREIGN KEY([InvitacionCompraDetalleId])
REFERENCES [dbo].[ARtblInvitacionCompraDetalle] ([InvitacionCompraDetalleId])
GO

ALTER TABLE [dbo].[ARtblOrdenCompraInvitacionDet] CHECK CONSTRAINT [FK_ARtblOrdenCompraInvitacionDet_ARtblInvitacionCompraDetalle]
GO