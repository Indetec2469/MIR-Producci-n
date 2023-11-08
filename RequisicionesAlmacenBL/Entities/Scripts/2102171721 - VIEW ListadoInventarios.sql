SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwListInventarioFisico]
AS
-- ======================================================
-- Author:		Javier Elías
-- Create date: 21/01/2021
-- Modified date: 
-- Description:	View para obtener el Listado de Inventarios Físicos
-- ======================================================
SELECT inventario.InventarioFisicoId, 
       Codigo, 
       inventario.FechaCreacion AS Fecha,
       Almacen,
	   empleado.Nombre + ' ' + empleado.PrimerApellido + ISNULL(' ' + empleado.SegundoApellido, '') AS Usuario,
	   '$ ' + CAST(CAST(inventario.MontoAjuste AS DECIMAL(28, 2)) AS VARCHAR(20)) AS MontoAjuste,
	   estatus.Valor AS Estatus
FROM tblInventarioFisico AS inventario
     INNER JOIN tblUsuario AS usuario ON inventario.CreadoPorId = usuario.UsuarioId
     INNER JOIN tblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId	
	 --LEFT JOIN  
	 --(
		--	SELECT InventarioFisicoId, 
		--		   SUM((Conteo - Existencia) * CostoPromedio) AS MontoAjuste
		--	FROM tblInventarioFisicoDetalle
		--	WHERE Borrado = 0
		--	GROUP BY InventarioFisicoId
	 --) AS detalles ON inventario.InventarioFisicoId = detalles.InventarioFisicoId
	 INNER JOIN tblControlMaestro AS estatus ON inventario.EstatusId = estatus.ControlId
WHERE inventario.EstatusId != 34 -- Estatus En Proceso o Terminado
GO