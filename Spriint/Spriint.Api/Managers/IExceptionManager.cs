using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spriint.Api.Managers
{
    /// <summary>
    /// Defines the interface for ExceptionManagers.
    /// </summary>
    public interface IExceptionManager
    {
        /// <summary>
        /// Handles the given exception and returns an appropriate HTTP status code, and error message.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>An ActionResult.</returns>
        ActionResult Handle(Exception ex);
    }
}
