using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown if a resource is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// The error message.
        /// </summary>
        public override string Message => "The requested resource could not be found.";
    }
}
