using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spriint.Api.Test.Utilities
{
    public static class Strings
    {
        public static string GenerateRandomString(int length)
        {
            Random random = new Random();
            return new string(
                Enumerable.Range(0, length)
                .Select(x => (char)(random.Next(128)))
                .ToArray());
        }
    }
}
