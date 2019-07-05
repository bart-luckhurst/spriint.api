using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Models
{
    /// <summary>
    /// Represents the Epic, Story, and Bug counts for a Project.
    /// </summary>
    public class OutputProjectCountCollection
    {
        /// <summary>
        /// The ID of the Project.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The number of Epics.
        /// </summary>
        public int EpicCount { get; set; }

        /// <summary>
        /// The number of Stories.
        /// </summary>
        public int StoryCount { get; set; }

        /// <summary>
        /// The number of Bugs.
        /// </summary>
        public int BugCount { get; set; }
    }
}
