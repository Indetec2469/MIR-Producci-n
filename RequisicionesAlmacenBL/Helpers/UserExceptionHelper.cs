using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Reflection;

namespace RequisicionesAlmacenBL.Helpers
{
    public abstract class UserExceptionHelper
    {
        public static string GetMessage(Exception ex)
        {
            string message = "";

            string t = ex.GetType().ToString();

            switch (ex.GetType().ToString())
            {
                case "System.Data.Entity.Validation.DbEntityValidationException":
                    //Validamos las propiedades
                    foreach (var validationErrors in ((DbEntityValidationException) ex).EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                        }
                    }
                break;

                default:
                    message = ex.InnerException != null 
                        ? ex.InnerException.InnerException != null 
                            ? ex.InnerException.InnerException.Message 
                            : ex.InnerException.Message 
                        : ex.Message;
                break;
            }

            return message;
        }
    }
}