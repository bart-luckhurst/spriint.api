using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Core
{
    /// <summary>
    /// Represents an Epic within a Project.
    /// </summary>
    public class Epic
    {
        /// <summary>
        /// The (internal) ID of the Epic.
        /// </summary>
        public int EpicId { get; set; }

        /// <summary>
        /// The public ID of the Epic.
        /// </summary>
        public Guid PublicEpicId { get; set; }

        /// <summary>
        /// The ID of the Project to which the Epic belongs.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The name of the Epic.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the Epic.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Status of the Epic. Derived from Issues within the Epic.
        /// </summary>
        public Status Status { get; }

        /// <summary>
        /// The DateTime at which the Epic was created.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// The Project to which the Epic belongs.
        /// </summary>
        public Project Project { get; set; }
    }
}
