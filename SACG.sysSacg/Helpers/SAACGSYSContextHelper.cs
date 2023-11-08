using SACG.sysSacg.Entities;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace SACG.sysSacg.Helpers
{
    public abstract class SAACGSYSContextHelper
    {
        private static SACGSYSContext Context;
        
        public static SACGSYSContext GetContext()
        {
            if (Context == null)
            {
                string cadenaConexion = GetCadenaConexion();
                Context = new SACGSYSContext(cadenaConexion);
                Context.Configuration.LazyLoadingEnabled = false;
            }

            return Context;
        }

        public static string GetCadenaConexion()
        {
            RegistroWindows registroWindows = RegistroWindowsHelper.GetDatosRegistro();
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder
            {
                InitialCatalog = registroWindows.InitialCatalog,
                DataSource = registroWindows.DataSource,
                UserID = registroWindows.SQLUser,
                Password = registroWindows.SQLPassword,
                PersistSecurityInfo = registroWindows.PersistSecurityInfo,
                MultipleActiveResultSets = false
            };

            EntityConnectionStringBuilder efConnection = new EntityConnectionStringBuilder();
            efConnection.ProviderConnectionString = scsb.ConnectionString;
            efConnection.Provider = "System.Data.SqlClient";            
            efConnection.Metadata = "res://*/Entities.EntityDataModelSAACGSYS.csdl|res://*/Entities.EntityDataModelSAACGSYS.ssdl|res://*/Entities.EntityDataModelSAACGSYS.msl";
            return efConnection.ToString();
        }

        public static void Dispose()
        {
            Context = null;
        }
    }
}