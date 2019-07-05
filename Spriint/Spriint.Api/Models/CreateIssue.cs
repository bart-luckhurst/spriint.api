using Spriint.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Models
{
    /// <summary>
    /// Represents an Issue to be created.
    /// </summary>
    public class CreateIssue
    {
        /// <summary>
        /// The ID of the Epic (if any).
        /// </summary>
        public Guid? EpicId { get; set; }

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
        /// The estimate for the Issue.
        /// </summary>
        public int? Estimate { get; set; }
    }
}
