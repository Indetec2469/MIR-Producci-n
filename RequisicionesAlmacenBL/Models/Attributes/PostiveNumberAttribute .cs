using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Models.Attributes
{
    public class PostiveNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            decimal getal;
            if (Decimal.TryParse(value.ToString(), out getal))
            {

                if (getal == 0)
                    return false;

                if (getal > 0)
                    return true;
            }
            return false;
        }
    }
}
