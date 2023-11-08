using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services
{
    public class ArchivoService : BaseService<GRtblArchivo>
    {
        public override GRtblArchivo Inserta(GRtblArchivo entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            GRtblArchivo Archivo = context.GRtblArchivo.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos la entidad que se acaba de guardar en la Base de Datos
            return Archivo;
        }

        public override bool Actualiza(GRtblArchivo entidad, SAACGContext context)
        {
            //Agregamos la entidad que vamos a actualizar al Context
            context.GRtblArchivo.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in GRtblArchivo.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Retornamos true o false si se realizo correctamente la operacion
            return context.SaveChanges() > 0;
        }

        public override GRtblArchivo BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public GRtblArchivo BuscaArchivoPorId(Nullable<Guid> id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.GRtblArchivo.Find(id);
            }
        }

        public GRtblArchivo BuscaPorReferenciaOrienTipo(int referenciaId, int origenArchivoId, int tipoArchivoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                List<GRtblArchivo> listadoArchivos = Context.GRtblArchivo.Where(m => m.ReferenciaId == referenciaId
                    && m.OrigenArchivoId == origenArchivoId
                    && m.TipoArchivoId == tipoArchivoId
                ).ToList();

                if (listadoArchivos.Count() > 0)
                {
                    return listadoArchivos.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public void EliminaArchivo(Nullable<Guid> id, Nullable<int> eliminadoPorId, SAACGContext context)
        {
            GRtblArchivo archivoBorrar = BuscaArchivoPorId(id);

            if (archivoBorrar != null)
            {
                archivoBorrar.Vigente = false;
                archivoBorrar.ModificadoPorId = eliminadoPorId;
                archivoBorrar.FechaUltimaModificacion = DateTime.Now;

                Actualiza(archivoBorrar, context);

                DeleteArchivo(GetRutaRaizCMM() + GetFileSeparator() + archivoBorrar.RutaFisica);
            }
        }

        public Guid GuardaArchivo(Nullable<int> usuarioId,
                                    Nullable<Guid> archivoAnteriorId,
                                    string nombreOriginalArchivo,
                                    Stream fileContent,
                                    int origenId, 
                                    List<string> ids, 
                                    int referenciaId, 
                                    int tipoArchivoId,
                                    SAACGContext context)
        {

            if (archivoAnteriorId != null)
            {
                EliminaArchivo(archivoAnteriorId, usuarioId, context);
            }

            string nombreOriginalArchivoTemp = fileContent != null ? nombreOriginalArchivo : nombreOriginalArchivo.Substring(nombreOriginalArchivo.IndexOf("_") + 1);
            string nombreFisicoArchivo = GetSiguienteAutonumericoTemporal() + "_";
            string rutaGuardar = GetRutaArchivo(origenId, ids);
            string nombreArchivoTmp = nombreFisicoArchivo + nombreOriginalArchivoTemp;
            string rutaFisicaArchivo = rutaGuardar + GetFileSeparator() + nombreArchivoTmp;

            GRtblArchivo archivo = new GRtblArchivo();
            archivo.NombreOriginal = nombreOriginalArchivoTemp;
            archivo.NombreFisico = nombreFisicoArchivo;
            archivo.ReferenciaId = referenciaId;
            archivo.OrigenArchivoId = origenId;
            archivo.RutaFisica = rutaFisicaArchivo;
            archivo.TipoArchivoId = tipoArchivoId;
            archivo.Vigente = true;
            archivo.FechaCreacion = DateTime.Now;
            archivo.CreadoPorId = usuarioId;

            Guid archivoId = Inserta(archivo, context).ArchivoId;

            string fullPath = GetRutaRaizCMM() + GetFileSeparator() + rutaGuardar;

            CreateDir(fullPath);

            if (fileContent != null)
            {
                UploadArchivo(fullPath + GetFileSeparator() + nombreArchivoTmp, fileContent);
            }
            else
            {
                MoveArchivo(GetRutaTmpCMM() + GetFileSeparator() + nombreOriginalArchivo, GetRutaRaizCMM() + GetFileSeparator() + rutaFisicaArchivo);
            }

            return archivoId;
        }

        public string GetSiguienteAutonumericoTemporal()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRspAutonumericoArcSigIncr().FirstOrDefault();
            }
        }

        public string GetRutaArchivo(int origen, List<string> ids)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.Database.SqlQuery<string>(
                    "SELECT dbo.GRfnGetRutaArchivo(@origen, @ids)",
                    new SqlParameter("origen", origen),
                    new SqlParameter("ids", "[" + String.Join(",", ids) + "]")
                ).FirstOrDefault().Replace("/", GetFileSeparator());
            }
        }

        public string GetFileSeparator()
        {
            return Path.DirectorySeparatorChar.ToString();
        }

        public string GetRutaTmpCMM()
        {
            return new ControlMaestroService().BuscaPorId(TipoRutaArchivo.RUTA_TEMPORAL_ARCHIVO).Valor.Replace("/", GetFileSeparator());
        }

        public string GetRutaRaizCMM()
        {
            return new ControlMaestroService().BuscaPorId(TipoRutaArchivo.RUTA_RAIZ_ARCHIVO).Valor.Replace("/", GetFileSeparator());
        }

        public int ObtenerTipoArchivo(string extension)
        {
            switch (extension)
            {
                case ".doc":
                case ".docm":
                case ".docx":
                case ".dot":
                case ".dotm":
                case ".dotx":
                case ".txt":
                    return TipoArchivo.DOCUMENTOTEXTO;
                case ".xlsx":
                case ".xlsm":
                case ".xlsb":
                case ".xltx":
                case ".xltm":
                case ".xls":
                case ".xlt":
                case ".xlam":
                case ".xla":
                case ".xlw":
                case ".xlr":
                case ".csv":
                    return TipoArchivo.HOJACALCULO;
                case ".xml":
                    return TipoArchivo.XML;
                case ".pdf":
                    return TipoArchivo.PDF;
                case ".bmp":
                case ".gif":
                case ".jpg":
                case ".jpeg":
                case ".tif":
                case ".tiff":
                case ".png":
                    return TipoArchivo.IMAGEN;
                default:
                    return TipoArchivo.OTRO;
            }
        }        

        public void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public bool UploadArchivo(string path, Stream fileContent)
        {
            FileStream fileStream = File.Create(path);
            fileContent.Seek(0, SeekOrigin.Begin);
            fileContent.CopyTo(fileStream);
            fileStream.Dispose();

            return true;
        }
        
        public Dictionary<string, byte[]> Download(Nullable<Guid> archivoId)
        {
            return Download(archivoId, null, null);
        }

        public Dictionary<string, byte[]> Download(string nombreArchivoTmp, string nombreOriginal)
        {
            return Download(null, nombreArchivoTmp, nombreOriginal);
        }

        private Dictionary<string, byte[]> Download(Nullable<Guid> archivoId, string nombreArchivoTmp, string nombreOriginal)
        {
            Dictionary<string, byte[]> diccionarioArchivo = null;

            if (archivoId != null)
            {
                GRtblArchivo archivo = BuscaArchivoPorId(archivoId);

                if (archivo != null)
                {
                    string path = GetRutaRaizCMM() + GetFileSeparator() + archivo.RutaFisica;

                    diccionarioArchivo = new Dictionary<string, byte[]>();
                    diccionarioArchivo.Add(archivo.NombreOriginal, File.ReadAllBytes(path));
                }
            }
            else if (nombreArchivoTmp != null)
            {
                string path = GetRutaTmpCMM() + GetFileSeparator() + nombreArchivoTmp;

                if (File.Exists(path))
                {
                    diccionarioArchivo = new Dictionary<string, byte[]>();
                    diccionarioArchivo.Add(nombreOriginal, File.ReadAllBytes(path));
                }
            }

            return diccionarioArchivo;
        }

        public string GetImageSrc(Nullable<Guid> archivoId)
        {
            GRtblArchivo archivo = BuscaArchivoPorId(archivoId);

            if (archivo != null)
            {
                string path = GetRutaRaizCMM() + GetFileSeparator() + archivo.RutaFisica;

                if (!File.Exists(path))
                {
                    return null;
                }

                byte[] fileBytes = File.ReadAllBytes(path);

                string extension = archivo.NombreOriginal.Substring(archivo.NombreOriginal.IndexOf(".") + 1);

                return String.Format("data:image/" + extension + ";base64,{0}", Convert.ToBase64String(fileBytes));
            }
            else
            {
                return null;
            }
        }

        public bool MoveArchivo(string pathOrigen, string pathDestino)
        {
            File.Move(pathOrigen, pathDestino);

            return true;
        }

        public bool DeleteArchivo(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            File.Delete(path);

            return true;
        }

        public string GuardaArchivoTemporal(string nombreOriginalArchivo, Stream fileContent)
        {
            string nombreArchivoTmp = GetSiguienteAutonumericoTemporal() + "_" + nombreOriginalArchivo;
            string ruta = GetRutaTmpCMM() + GetFileSeparator() + nombreArchivoTmp;

            CreateDir(GetRutaTmpCMM());

            UploadArchivo(ruta, fileContent);

            return nombreArchivoTmp;
        }

        public FileInfo GetImagenLogotipo(string nombreLogotipo)
        {
            string fullPath = GetRutaRaizCMM() + GetFileSeparator() + new ArchivoEstructuraCarpetaService().GetCarpetaPorOrigenArchivoId(3);

            CreateDir(fullPath);

            try
            {
                return new FileInfo(fullPath + GetFileSeparator() + nombreLogotipo);
            }
            catch
            {
                return null;
            }
        }
    }
}