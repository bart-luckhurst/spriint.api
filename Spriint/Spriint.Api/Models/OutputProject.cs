using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Models
{
    /// <summary>
    /// Represents a Project.
    /// </summary>
    public class OutputProject
    {
        /// <summary>
        /// The ID of the Project.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The name of the Project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the Project.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The DateTime at which the Project was created.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }
    }
}
