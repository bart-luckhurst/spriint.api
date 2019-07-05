using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Models
{
    /// <summary>
    /// Represents an Epic.
    /// </summary>
    public class OutputEpic
    {
        /// <summary>
        /// The ID of the Epic.
        /// </summary>
        public Guid EpicId { get; set; }

        /// <summary>
        /// The ID of the Project to which the Epic belongs.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// The name of the Epic.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the Epic.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The DateTime at which the Epic was created.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }
    }
}
