SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwListInventarioAjuste]
AS
-- ======================================================
-- Author:		Javier Elías
-- Create date: 22/02/2021
-- Modified date: 
-- Description:	View para obtener el Listado de Inventarios Ajustes
-- ======================================================
SELECT inventario.InventarioAjusteId,
       CodigoAjusteInventario AS Codigo,
       inventario.FechaCreacion AS Fecha,
       empleado.Nombre + ' ' + empleado.PrimerApellido + ISNULL(' ' + empleado.SegundoApellido, '') AS Usuario,
       CantidadArticulosAfectados AS NoArticulos,
       inventario.MontoAjuste AS MontoAjuste
FROM tblInventarioAjuste AS inventario
     INNER JOIN tblUsuario AS usuario ON inventario.CreadoPorId = usuario.UsuarioId
     INNER JOIN tblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
GO