using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisNetwork.Models
{
    public class EnumShouldNotContainEmptyString : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int valueAsInt = Convert.ToInt32(value);
            if (valueAsInt == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
