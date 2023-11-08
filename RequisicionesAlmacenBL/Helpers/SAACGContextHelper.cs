using RequisicionesAlmacenBL.Entities;
using SACG.sysSacg.Helpers;
using System.Text.Json;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Text;
using SACG.GDALib;

namespace RequisicionesAlmacenBL.Helpers
{
    public abstract class SAACGContextHelper
    {
        //private static DbContextTransaction DbContextTransaction;
        //private static SAACGContext Context;
        
        public static SAACGContext GetContext()
        {
            return GetContext(SessionHelper.GetUsuario().EnteId);
        }

        public static SAACGContext GetContext(string entidadId)
        {
            string cadenaConexion = GetCadenaConexion(entidadId);
            SAACGContext Context = new SAACGContext(cadenaConexion);
            Context.Configuration.LazyLoadingEnabled = false;
            return Context;
        }

        //public static void BeginTransaction()
        //{
        //    if (DbContextTransaction == null && Context.Database.CurrentTransaction == null)
        //    {
        //        DbContextTransaction = Context.Database.BeginTransaction();
        //    }
        //}

        //public static void Commit()
        //{
        //    DbContextTransaction.Commit();
        //}

        //public static void Rollback()
        //{
        //    DbContextTransaction.Rollback();
        //}

        //public static void Dispose()
        //{
        //    Context = null;
        //    DbContextTransaction = null;
        //}

        public static string GetCadenaConexion(string enteId)
        {
            RegistroWindows registroWindows = RegistroWindowsHelper.GetDatosRegistro();
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder
            {
                InitialCatalog = "SACG" + enteId,
                DataSource = registroWindows.DataSource,
                UserID = registroWindows.SQLUser,
                Password = registroWindows.SQLPassword ,
                PersistSecurityInfo = registroWindows.PersistSecurityInfo,
                MultipleActiveResultSets = false
            };

            EntityConnectionStringBuilder efConnection = new EntityConnectionStringBuilder();
            efConnection.ProviderConnectionString = scsb.ConnectionString;
            efConnection.Provider = "System.Data.SqlClient";            
            efConnection.Metadata = "res://*/Entities.EntityDataModelSAACG.csdl|res://*/Entities.EntityDataModelSAACG.ssdl|res://*/Entities.EntityDataModelSAACG.msl";
            return efConnection.ToString();
        }

        public static string GetCadenaConexionReportes(string enteId)
        {
            RegistroWindows registroWindows = RegistroWindowsHelper.GetDatosRegistro();

            StringBuilder cadenaConexion = new StringBuilder();
            cadenaConexion.Append("Data Source=").Append(registroWindows.DataSource).Append(";");
            cadenaConexion.Append("AttachDbFilename=;");
            cadenaConexion.Append("Initial Catalog=SACG").Append(enteId).Append(";");
            cadenaConexion.Append("Integrated Security=False;");
            cadenaConexion.Append("Persist Security Info=").Append( registroWindows.PersistSecurityInfo ? bool.TrueString :  bool.FalseString).Append(";");
            cadenaConexion.Append("User ID=").Append(registroWindows.SQLUser).Append(";");
            cadenaConexion.Append("Password=").Append(registroWindows.SQLPassword);

            return cadenaConexion.ToString();
        }
    }
}