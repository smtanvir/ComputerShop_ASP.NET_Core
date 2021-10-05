using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Evidence.CustomValidation
{
    public class CustomMfgDateAttribute : ValidationAttribute
    {
        public CustomMfgDateAttribute() : base("{0} Should be less than Current Date")
        {

        }
        public override bool IsValid(object value)
        {
            DateTime proValue = Convert.ToDateTime(value);

            if (proValue < DateTime.Now)
                return true;
            else
                return false;
        }
    }
}
