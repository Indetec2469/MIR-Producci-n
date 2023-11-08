using RequisicionesAlmacenBL.Helpers;
using System.Data.Entity;

namespace RequisicionesAlmacenBL.Entities
{
    public partial class SAACGContext : DbContext
    {

        public SAACGContext(string cadenaConexion) : base (cadenaConexion){ }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && Database.CurrentTransaction == null)
        //    {
        //        Database.Connection.Dispose();
        //        SAACGContextHelper.Dispose();
        //    }
        //}
    }
}
