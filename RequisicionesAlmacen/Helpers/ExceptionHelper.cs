using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Helpers
{
    public class ExceptionHelper : JsonException
    {

        Test TestError { get; set; }

        public ExceptionHelper()
        {
        }

        public ExceptionHelper(string message)
        {

            Test test = new Test();
            test.Codigo = 1000;
            test.Mensaje = message;
            TestError = test;

            
            //this.Data = JsonSerializer.Serialize(test);
        }

        //public ExceptionHelper(object data)
        //{
        //    this.Data = data;
        //}

        //public override void ExecuteResult(ControllerContext context)
        //{
        //    context.RequestContext.HttpContext.Response.StatusCode = 500;
        //    base.ExecuteResult(context);
        //}

    }


    public class Test
    {

        public int Codigo { get; set; }
        public String Mensaje { get; set; }

    }

}