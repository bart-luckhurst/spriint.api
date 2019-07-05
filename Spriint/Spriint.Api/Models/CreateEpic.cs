using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Models
{
    /// <summary>
    /// Represents an Epic to be created.
    /// </summary>
    public class CreateEpic
    {
        /// <summary>
        /// The name of the Epic. 
        /// Max length: 64 characters.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the Epic.
        /// Max length: 1024 characters.
        /// </summary>
        public string Description { get; set; }
    }
}
