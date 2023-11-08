UPDATE GRtblMenuPrincipal SET Orden = 1 WHERE NodoMenuId = 1
GO
UPDATE GRtblMenuPrincipal SET Orden = 2 WHERE NodoMenuId = 15
GO
UPDATE GRtblMenuPrincipal SET Orden = 3 WHERE NodoMenuId = 24
GO
UPDATE GRtblMenuPrincipal SET Orden = 4 WHERE NodoMenuId = 56
GO
UPDATE GRtblMenuPrincipal SET Orden = 5 WHERE NodoMenuId = 47
GO
UPDATE GRtblMenuPrincipal SET Orden = 6 WHERE NodoMenuId = 50
GO

UPDATE GRtblMenuPrincipal SET Orden = 2 WHERE NodoMenuId = 43
GO
UPDATE GRtblMenuPrincipal SET Orden = 3 WHERE NodoMenuId = 44
GO

UPDATE GRtblMenuPrincipal SET NodoPadreId = 18, Url = REPLACE(Url, 'compras/', 'almacenes/'), Orden = 5 WHERE NodoMenuId = 13
GO
UPDATE GRtblMenuPrincipal SET NodoPadreId = 18, Url = REPLACE(Url, 'compras/', 'almacenes/'), Orden = 6 WHERE NodoMenuId = 14
GO
UPDATE GRtblMenuPrincipal SET NodoPadreId = 18, Url = REPLACE(Url, 'compras/', 'almacenes/'), Orden = 7 WHERE NodoMenuId = 45
GO