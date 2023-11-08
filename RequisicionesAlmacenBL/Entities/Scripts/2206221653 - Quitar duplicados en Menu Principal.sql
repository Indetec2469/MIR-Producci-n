UPDATE padre SET EstatusId = 3
FROM GRtblMenuPrincipal AS padre
     LEFT JOIN
	 (
			SELECT NodoPadreId,
				   COUNT(NodoMenuId) AS Contador
			FROM GRtblMenuPrincipal AS hijo
			WHERE EstatusId = 1
			GROUP BY NodoPadreId
	  ) AS hijos ON hijos.NodoPadreId = padre.NodoMenuId
WHERE padre.TipoNodoId != 7
      AND hijos.Contador IS NULL
GO

UPDATE rm SET EstatusId = 3
FROM GRtblRolMenu AS rm
     INNER JOIN GRtblMenuPrincipal AS mp ON rm.NodoMenuId = mp.NodoMenuId
WHERE mp.TipoNodoId != 7
GO