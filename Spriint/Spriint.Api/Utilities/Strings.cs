using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Utilities
{
    public static class Strings
    {
        public static bool ValidateMaxLength(this string input, int maxLength)
        {
            return (input?.Length ?? 0) <= maxLength;
        }

        public static bool ValidateIsSet(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}
