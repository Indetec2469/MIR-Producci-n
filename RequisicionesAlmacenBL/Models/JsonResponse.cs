using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Models
{
    class JsonResponse
    {

        /**
         *  Estatus de la respuesta
        */
        public static int STATUS_OK = 200;

        /**
         * Codigo Estatus para identificar la respuesta enviada al Cliente.
         */
        private int Status { get; set; }

        /**
         * Mensaje informativo el Cliente.
         */
        private String Message { get; set; } 

        /**
         * Objeto por lo regular de tipo <b>JSON</b> con datos necesarios para cargar alguna Ficha, listado o
         * un registro en particular.
         */
        private Object Data { get; set; }

        /**
         * Nombre de la ficha o proceso que genera el Response.
         */
        private String Title { get; set; }

        public JsonResponse() { }

        public JsonResponse(Object data) : this(data, null, "OK", STATUS_OK)
        {

        }

        public JsonResponse(Object data, String message) : this(data, message, null, STATUS_OK)
        {
            
        }

        public JsonResponse(Object data, String message, int status) : this(data, message, null, status)
        {
            
        }

        public JsonResponse(Object data, String message, String title) : this(data, message, title, STATUS_OK)
        {
            
        }

        public JsonResponse(Object data, String message, String title, int status)
        {
            this.Status = status;
            this.Message = message;
            this.Data = data;
            this.Title = title;
        }

    }
}
