using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Resources.Extensions
{
    public class StringLengthIfNotEmptyAttribute : StringLengthAttribute
    {
        public StringLengthIfNotEmptyAttribute(int maxLength)
            : base(maxLength)
        {
        }

        public override bool IsValid(object value)
        {
            string password = (string)value;
            if (string.IsNullOrEmpty(password))
            {
                return true;
            }

            // Validate the password using the existing validation attributes.
            return base.IsValid(value);
        }
    }
}
