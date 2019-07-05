using Spriint.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Exceptions
{
    /// <summary>
    /// Represents an exception thrown due to an invalid input.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// The error message.
        /// </summary>
        public override string Message => $"The input was not valid. Errors: [ {string.Join(", ", ValidationErrors.Select(x => $"{x.Property}: {x.ErrorMessage}"))} ]";

        /// <summary>
        /// A list of individual validation errors.
        /// </summary>
        public List<ValidationError> ValidationErrors { get; set; }

        /// <summary>
        /// Creates a new instance of the ValidationError class.
        /// </summary>
        /// <param name="validationErrors">The individual ValidationErrors which triggered the ValidationException.</param>
        public ValidationException(List<ValidationError> validationErrors)
        {
            ValidationErrors = validationErrors;
        }
    }
}
