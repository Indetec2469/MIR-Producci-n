SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ARtblInvitacionArticulo](
	[InvitacionArticuloId] [int] IDENTITY(1,1) NOT NULL,
	[ProveedorId] [int] NOT NULL,
	[AlmacenId] [varchar](4) NOT NULL,
	[MontoInvitacion] [money] NOT NULL,
	[EstatusId] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ARtblInvitacionArticulos] PRIMARY KEY CLUSTERED 
(
	[InvitacionArticuloId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ARtblInvitacionArticuloDetalle]    Script Date: 26/10/2021 09:31:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ARtblInvitacionArticuloDetalle](
	[InvitacionArticuloDetalleId] [int] IDENTITY(1,1) NOT NULL,
	[InvitacionArticuloId] [int] NOT NULL,
	[RequisicionMaterialDetalleId] [int] NOT NULL,
	[ProductoId] [varchar](10) NOT NULL,
	[CuentaPresupuestalEgrId] [int] NOT NULL,
	[TarifaImpuestoId] [varchar](10) NOT NULL,
	[Descripcion] [varchar](250) NOT NULL,
	[Cantidad] [float] NOT NULL,
	[Costo] [money] NOT NULL,
	[Importe] [money] NOT NULL,
	[IEPS] [float] NOT NULL,
	[Ajuste] [float] NOT NULL,
	[IVA] [float] NOT NULL,
	[ISH] [float] NOT NULL,
	[RetencionISR] [float] NOT NULL,
	[RetencionCedular] [float] NOT NULL,
	[RetencionIVA] [float] NOT NULL,
	[TotalPresupuesto] [float] NOT NULL,
	[Total] [float] NOT NULL,
	[EstatusId] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NOT NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ARtblInvitacionArticuloDetalle] PRIMARY KEY CLUSTERED 
(
	[InvitacionArticuloDetalleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo] ADD  CONSTRAINT [DF_ARtblInvitacionArticulos_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] ADD  CONSTRAINT [DF_ARtblInvitacionArticulo_Cantidad]  DEFAULT ((0)) FOR [Cantidad]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] ADD  CONSTRAINT [DF_ARtblInvitacionArticulo_Costo]  DEFAULT ((0)) FOR [Costo]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] ADD  CONSTRAINT [DF_ARtblInvitacionArticulo_Importe]  DEFAULT ((0.00)) FOR [Importe]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] ADD  CONSTRAINT [DF_ARtblInvitacionArticulo_IEPS]  DEFAULT ((0)) FOR [IEPS]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] ADD  CONSTRAINT [DF_ARtblInvitacionArticulo_Ajuste]  DEFAULT ((0)) FOR [Ajuste]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] ADD  CONSTRAINT [DF_ARtblInvitacionArticulo_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblControlMaestro1] FOREIGN KEY([EstatusId])
REFERENCES [dbo].[GRtblControlMaestro] ([ControlId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblControlMaestro1]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario2] FOREIGN KEY([CreadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario2]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario3] FOREIGN KEY([ModificadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario3]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_tblAlmacen] FOREIGN KEY([AlmacenId])
REFERENCES [dbo].[tblAlmacen] ([AlmacenId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_tblAlmacen]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_tblProveedor] FOREIGN KEY([ProveedorId])
REFERENCES [dbo].[tblProveedor] ([ProveedorId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticulo] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_tblProveedor]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_ARtblRequisicionMaterialDetalle] FOREIGN KEY([RequisicionMaterialDetalleId])
REFERENCES [dbo].[ARtblRequisicionMaterialDetalle] ([RequisicionMaterialDetalleId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_ARtblRequisicionMaterialDetalle]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblControlMaestro] FOREIGN KEY([EstatusId])
REFERENCES [dbo].[GRtblControlMaestro] ([ControlId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblControlMaestro]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario] FOREIGN KEY([CreadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario1] FOREIGN KEY([ModificadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_GRtblUsuario1]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_tblCuentaPresupuestalEgr] FOREIGN KEY([CuentaPresupuestalEgrId])
REFERENCES [dbo].[tblCuentaPresupuestalEgr] ([CuentaPresupuestalEgrId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_tblCuentaPresupuestalEgr]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_tblProducto] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[tblProducto] ([ProductoId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_tblProducto]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticulo_tblTarifaImpuesto] FOREIGN KEY([TarifaImpuestoId])
REFERENCES [dbo].[tblTarifaImpuesto] ([TarifaImpuestoId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticulo_tblTarifaImpuesto]
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ARtblInvitacionArticuloDetalle_ARtblInvitacionArticulo] FOREIGN KEY([InvitacionArticuloId])
REFERENCES [dbo].[ARtblInvitacionArticulo] ([InvitacionArticuloId])
GO
ALTER TABLE [dbo].[ARtblInvitacionArticuloDetalle] CHECK CONSTRAINT [FK_ARtblInvitacionArticuloDetalle_ARtblInvitacionArticulo]
GO
