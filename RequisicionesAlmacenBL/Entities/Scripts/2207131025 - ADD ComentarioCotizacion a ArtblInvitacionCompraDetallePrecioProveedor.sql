EXEC sp_rename 'dbo.ArtblInvitacionCompraDetallePrecioProveedor.Comentario', 'ComentarioGanador', 'COLUMN';

ALTER TABLE ArtblInvitacionCompraDetallePrecioProveedor ADD ComentarioCotizacion VARCHAR(1000)
GO