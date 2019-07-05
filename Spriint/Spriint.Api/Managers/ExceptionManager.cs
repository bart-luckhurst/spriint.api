using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spriint.Api.Exceptions;

namespace Spriint.Api.Managers
{
    /// <summary>
    /// Default implemenatation of IExceptionManager.
    /// </summary>
    public class ExceptionManager : IExceptionManager
    {
        private readonly ILogger<ExceptionManager> logger;

        public ExceptionManager(ILogger<ExceptionManager> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Handles the given exception and returns an appropriate HTTP status code, and error message.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>An ActionResult.</returns>
        public ActionResult Handle(Exception ex)
        {
            ActionResult output;
            //return appropraite status code
            if (ex.GetType() == typeof(ValidationException))
            {
                output = new StatusCodeResult(400);
            }
            else
            {
                output = new StatusCodeResult(500);
            }
            //log
            logger.LogError(ex, $"Exception being handled. Status code: {(output as StatusCodeResult).StatusCode}");
            //return
            return output;
        }
    }
}
