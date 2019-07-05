using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Models
{
    /// <summary>
    /// Represents a Project to be created.
    /// </summary>
    public class CreateProject
    {
        /// <summary>
        /// The name of the Project.
        /// Max length: 64 characters.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the Project.
        /// Max length: 512 characters.
        /// </summary>
        public string Description { get; set; }
    }
}
