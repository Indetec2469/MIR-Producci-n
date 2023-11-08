UPDATE GRtblMenuPrincipal
  SET
      NodoPadreId = 18,
      Descripcion = 'Ficha de Solicitudes por surtir',
      Url = 'almacenes/almacenes/solicitudporsurtir/listar',
	  Orden = 4
WHERE NodoMenuId = 9
GO