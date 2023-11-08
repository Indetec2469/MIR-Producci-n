using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACG.sysSacg.Entities
{
    using SACG.sysSacg.Helpers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class SACGSYSContext : DbContext
    {

        public SACGSYSContext(string cadenaConexion) : base(cadenaConexion) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Database.CurrentTransaction == null)
            {
                Database.Connection.Dispose();
                SAACGSYSContextHelper.Dispose();
            }
        }
    }
}