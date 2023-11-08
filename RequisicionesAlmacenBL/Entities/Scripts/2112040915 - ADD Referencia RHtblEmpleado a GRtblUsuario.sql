ALTER TABLE [dbo].[GRtblUsuario]  WITH CHECK ADD  CONSTRAINT [FK_GRtblUsuario_RHtblEmpleado] FOREIGN KEY([EmpleadoId])
REFERENCES [dbo].[RHtblEmpleado] ([EmpleadoId])
GO

ALTER TABLE [dbo].[GRtblUsuario] CHECK CONSTRAINT [FK_GRtblUsuario_RHtblEmpleado]
GO