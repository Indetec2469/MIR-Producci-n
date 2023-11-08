SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoInventarioAjuste]
AS
-- ======================================================
-- Author:		Javier Elías
-- Create date: 22/02/2021
-- Modified date: 04/12/2021
-- Description:	View para obtener el Listado de Inventarios Ajustes
-- ======================================================
SELECT inventario.InventarioAjusteId,
       CodigoAjusteInventario AS Codigo,
       dbo.GRfnGetFechaConFormato(inventario.FechaCreacion, 0) AS Fecha,
       empleado.Nombre + ' ' + empleado.PrimerApellido + ISNULL(' ' + empleado.SegundoApellido, '') AS Usuario,
       CantidadArticulosAfectados AS NoArticulos,
       inventario.MontoAjuste AS MontoAjuste
FROM ARtblInventarioAjuste AS inventario
     INNER JOIN GRtblUsuario AS usuario ON inventario.CreadoPorId = usuario.UsuarioId
     INNER JOIN RHtblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
GO