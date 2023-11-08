ALTER TABLE ARtblRequisicionMaterial ADD EmpleadoId INT
GO

ALTER TABLE [dbo].[ARtblRequisicionMaterial]  WITH CHECK ADD  CONSTRAINT [FK_ARtblRequisicionMaterial_Empleado] FOREIGN KEY([EmpleadoId])
REFERENCES [dbo].[RHtblEmpleado] ([EmpleadoId])
GO

ALTER TABLE [dbo].[ARtblRequisicionMaterial] CHECK CONSTRAINT [FK_ARtblRequisicionMaterial_Empleado]
GO

UPDATE requisiciones SET requisiciones.EmpleadoId = empleado.EmpleadoId
FROM ARtblRequisicionMaterial AS requisiciones
     INNER JOIN GRtblUsuario AS usuario ON requisiciones.CreadoPorId = usuario.UsuarioId
     INNER JOIN RHtblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
GO

ALTER TABLE ARtblRequisicionMaterial ALTER COLUMN EmpleadoId INT NOT NULL
GO