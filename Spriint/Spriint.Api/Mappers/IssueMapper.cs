using Spriint.Api.Core;
using Spriint.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Mappers
{
    /// <summary>
    /// Default implementation of IIssueMapper.
    /// </summary>
    public class IssueMapper : IIssueMapper
    {
        /// <summary>
        /// Maps the given Issue to an OutputIssue.
        /// </summary>
        /// <param name="issueToMap">The Issue to map.</param>
        /// <returns>An OutputIssue.</returns>
        public OutputIssue MapOutputIssue(Issue issueToMap)
        {
            return new OutputIssue()
            {
                IssueId = issueToMap.PublicIssueId.ToString(),
                ProjectId = issueToMap.Project?.PublicProjectId.ToString(),
                EpicId = issueToMap.Epic?.PublicEpicId.ToString(),
                IssueType = issueToMap.IssueType.ToString(),
                Name = issueToMap.Name,
                Description = issueToMap.Description,
                Status = issueToMap.Status.ToString(),
                DateTimeCreated = issueToMap.DateTimeCreated
            };
        }
    }
}
