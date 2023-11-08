using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Catalogos.Models.ViewModel
{
    public class ImportarAlmacenViewModel
    {
        public ImportarAlmacenProductoItem AlmacenProductoItem { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}