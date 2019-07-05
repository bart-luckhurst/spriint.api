using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Core
{
    /// <summary>
    /// Represents an Issue (story or bug).
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// The ID of the Issue.
        /// </summary>
        public int IssueId { get; set; }

        /// <summary>
        /// The public ID of the Issue.
        /// </summary>
        public Guid PublicIssueId { get; set; }

        /// <summary>
        /// The ID of the Project to which the Issue belongs.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The ID of the Epic to which the Issue belongs (if any).
        /// </summary>
        public int? EpicId { get; set; }

        /// <summary>
        /// The type of the Issue.
        /// </summary>
        public IssueType IssueType { get; }

        /// <summary>
        /// The name of the Issue.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the Issue.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Status of the Issue.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// The estimate for the Issue (in story points).
        /// </summary>
        public int Estimate { get; set; }

        /// <summary>
        /// The DateTime at which the Issue was created.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// The Project to which the Issue belongs.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// The Epic to which the Issue belongs (if any).
        /// </summary>
        public Epic Epic { get; set; }
    }
}
