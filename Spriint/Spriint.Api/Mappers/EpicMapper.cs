using Spriint.Api.Core;
using Spriint.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Mappers
{
    /// <summary>
    /// Defualt implementation of IEpicMapper.
    /// </summary>
    public class EpicMapper : IEpicMapper
    {
        /// <summary>
        /// Maps the given Epic to an OutputEpic.
        /// </summary>
        /// <param name="epic">The Epic to map.</param>
        /// <returns>An OutputEpic.</returns>
        public OutputEpic MapOutputEpic(Epic epic)
        {
            return new OutputEpic()
            {
                EpicId = epic.PublicEpicId,
                ProjectId = epic.Project.PublicProjectId,
                Name = epic.Name,
                Description = epic.Description,
                DateTimeCreated = epic.DateTimeCreated
            };
        }
    }
}
