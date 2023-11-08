SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:		Rene Carrillo
-- Create date: 04/11/2021
-- Modified: Javier Elías
-- Modified date: 01/03/2022
-- Description:	Consulta para obtener el Empelado ligado al Usuario
--						y el listado de los empleados no ligados
-- ========================================================
CREATE OR ALTER PROCEDURE [dbo].[RHspConsultaEmpleadoLigado]
	@UsuarioId INT
AS
BEGIN
		SET NOCOUNT ON;

		SELECT *
		FROM
		(
			SELECT empleado.*
			FROM RHtblEmpleado AS empleado
					LEFT JOIN GRtblUsuario AS usuario ON empleado.EmpleadoId = usuario.EmpleadoId AND usuario.Activo = 1 AND usuario.Borrado = 0
			WHERE empleado.EstatusId = 1 -- Activo
					AND usuario.UsuarioId IS NULL

			UNION ALL
    
			SELECT empleado.*
			FROM RHtblEmpleado AS empleado
					INNER JOIN GRtblUsuario AS usuario ON empleado.EmpleadoId = usuario.EmpleadoId AND usuario.Activo = 1 AND usuario.Borrado = 0 AND usuario.UsuarioId = @UsuarioId
			WHERE empleado.EstatusId = 1 -- Activo
		) AS Empleado
		ORDER BY Empleado.EmpleadoId ASC
END
GO