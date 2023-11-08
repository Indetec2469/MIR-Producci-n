UPDATE GRtblMenuPrincipal
  SET
      Etiqueta = REPLACE(Etiqueta, 'Inventarios', 'Almacenes'),
      Descripcion = REPLACE(Descripcion, 'Inventarios', 'Almacenes')
WHERE NodoMenuId IN(15, 18)
GO

UPDATE GRtblMenuPrincipal
  SET
      URL = REPLACE(URL, 'inventarios/', 'almacenes/')
WHERE URL LIKE '%inventarios/%'
GO