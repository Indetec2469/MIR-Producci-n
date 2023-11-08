UPDATE GRtblMenuPrincipal SET Etiqueta = 'Solicitud de Materiales', Descripcion = 'Ficha de Solicitud de Materiales' WHERE NodoMenuId = 8
GO

UPDATE GRtblMenuPrincipal SET Etiqueta = REPLACE(Etiqueta, 'Requisición', 'Solicitudes'), Descripcion = REPLACE(Descripcion, 'Requisición', 'Solicitud') WHERE NodoMenuId IN (9, 10)
GO