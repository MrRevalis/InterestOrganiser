using InterestOrganiser.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InterestOrganiser.ValidationRules
{
    public class IsValidEmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            Regex emailPattern = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (emailPattern.IsMatch($"{value}"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
