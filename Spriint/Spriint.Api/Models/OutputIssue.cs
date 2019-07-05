using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Models
{
    /// <summary>
    /// Represents an Issue.
    /// </summary>
    public class OutputIssue
    {
        /// <summary>
        /// The ID of the Issue.
        /// </summary>
        public string IssueId { get; set; }

        /// <summary>
        /// The ID of the Project to which the Issue belongs.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The ID of the Epic to which the Issue belongs (if any).
        /// </summary>
        public string EpicId { get; set; }

        /// <summary>
        /// The type of Issue.
        /// </summary>
        public string IssueType { get; set; }

        /// <summary>
        /// The name of the Issue.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the Issue.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The status of the Issue.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The estimate for the Issue, if any.
        /// </summary>
        public int Estimate { get; set; }

        /// <summary>
        /// The DateTime at which the Issue was created.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }
    }
}
