using Spriint.Api.Core;
using Spriint.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Mappers
{
    /// <summary>
    /// Defines the interface for IssueMappers.
    /// </summary>
    public interface IIssueMapper
    {
        /// <summary>
        /// Maps the given Issue to an OutputIssue.
        /// </summary>
        /// <param name="issueToMap">The Issue to map.</param>
        /// <returns>An OutputIssue.</returns>
        OutputIssue MapOutputIssue(Issue issueToMap);
    }
}
