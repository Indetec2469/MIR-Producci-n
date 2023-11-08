using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace RequisicionesAlmacenBL.Services
{
    public abstract class BaseService<T>
    {

        public BaseService(){}
        
        //protected DbSet<object> EntitySet { get { return Context.Set<object>(); } }

        /// <summary>Metodo para guardar una nueva Entidad en Base de datos </summary>
        /// <param name="entidad"> Entidad que se esta intentando guardar</param>
        /// <returns> <c>Objeto Entidad</c> guardado</returns>
        public abstract T Inserta(T entidad, SAACGContext context);

        /// <summary>Metodo para actualizar una Entidad en Base de datos </summary>
        /// <param name="entidad"> Entidad que se esta intentando actualizar</param>
        /// <returns> <c>True</c> en caso de que la actualización se realice con exito; <c>False</c> de lo contrario</returns>
        public abstract bool Actualiza(T entidad, SAACGContext context);

        /// <summary>Metodo para eliminar de forma logica una Entidad en Base de datos </summary>
        /// <param name="id"> ID de la Entidad que se esta intentando eliminar</param>
        /// <param name="eliminadoPorId"> ID del usuario que esta intentando eliminar la Entidad</param>
        /// <returns> <c>True</c> en caso de que la eliminacion se realice con exito; <c>False</c> de lo contrario</returns>
        public abstract bool Elimina(int id, int eliminadoPorId, SAACGContext context);
        
        /// <summary>Metodo para buscar una Entidad en Base de datos por medio de su ID</summary>
        /// <param name="id"> Id de la Entidad que estamos buscando</param>
        /// <returns><c>Objeto Entidad</c> en caso de existir; <c>NULL</c> de lo contrario</returns>
        public abstract T BuscaPorId(int id);

        /// <summary>Metodo para buscar una Entidad en Base de datos por medio de algun criterio</summary>
        /// <param name="criterio"> Criterio por el que se buscara la Entidad</param>
        /// <returns><c>Objeto Entidad</c> en caso de existir; <c>NULL</c> de lo contrario</returns>
        public virtual List<T> BuscaPorCriterio(Expression<Func<T, bool>> criterio) { throw new NotImplementedException(); }

    }
}
