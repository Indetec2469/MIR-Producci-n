using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class AlmacenProductoService : BaseService<ARtblAlmacenProducto>
    {
        public override ARtblAlmacenProducto BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblAlmacenProducto.Where(m => m.AlmacenProductoId == id && !m.Borrado).FirstOrDefault();
            }
        }

        public override ARtblAlmacenProducto Inserta(ARtblAlmacenProducto entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblAlmacenProducto modelo = context.ARtblAlmacenProducto.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos si guardó correctamente
            return modelo;
        }

        public ARtblAlmacenProducto Inserta(
            string productoId,
            string almacenId,
            int cuentaPresupuestalId,
            int creadoPorId,
            SAACGContext context)
        {
            ARtblAlmacenProducto almacenProducto = new ARtblAlmacenProducto();

            almacenProducto.ProductoId = productoId;
            almacenProducto.AlmacenId = almacenId;
            almacenProducto.CuentaPresupuestalId = cuentaPresupuestalId;
            almacenProducto.Cantidad = 0;
            almacenProducto.Borrado = false;
            almacenProducto.CreadoPorId = creadoPorId;
            
            return Inserta(almacenProducto, context);
        }

        public override bool Actualiza(ARtblAlmacenProducto entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }
    }
}