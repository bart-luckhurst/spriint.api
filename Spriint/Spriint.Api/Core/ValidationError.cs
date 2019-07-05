using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Core
{
    /// <summary>
    /// Represents an error validating an inputted value.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The property which triggered the ValidationError.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Creates a new instance of ValidationError.
        /// </summary>
        /// <param name="property">The property which triggered the ValidationError.</param>
        /// <param name="errorMessage">The error message.</param>
        public ValidationError(string property, string errorMessage)
        {
            Property = property;
            ErrorMessage = errorMessage;
        }
    }
}
