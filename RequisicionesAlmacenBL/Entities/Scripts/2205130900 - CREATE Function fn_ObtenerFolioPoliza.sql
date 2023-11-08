-------------------------------------------------------------------------------

--// Versión:       Versión 0.0.0.0

--// Descripción:   AGREGAR FUNCION fn_ObtenerFolioPoliza

--// Fecha:                29.03.2021

--// Autor:                Oscar Martínez

-------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObtenerFolioPoliza]') AND type in (N'TF'))

BEGIN

       EXEC dbo.sp_executesql @statement = N'

       CREATE function [dbo].[fn_ObtenerFolioPoliza](@Ejercicio int, @Tipo VARCHAR(1))

       returns @table TABLE(Poliza VARCHAR(10))

       as

       begin

                           INSERT INTO @table

                           SELECT @Tipo + REPLICATE(''0'',(5 - LEN(Folio))) + Folio

                           FROM

                           (SELECT CAST((COALESCE(MAX(SUBSTRING(Poliza, 2,LEN(Poliza)-1)), 0) + 1) AS VARCHAR) AS Folio FROM tblPoliza

                           WHERE Ejercicio = @Ejercicio AND SUBSTRING(Poliza, 1,1) = @Tipo) T0

             return

       end'  

END