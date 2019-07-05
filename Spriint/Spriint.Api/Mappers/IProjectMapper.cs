using Spriint.Api.Core;
using Spriint.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Mappers
{
    /// <summary>
    /// Defines the interface for ProjectMappers.
    /// </summary>
    public interface IProjectMapper
    {
        /// <summary>
        /// Maps the given Project to an OutputProject.
        /// </summary>
        /// <param name="project">The Project to map.</param>
        /// <returns>An OutputProject.</returns>
        OutputProject MapOutputProject(Project project);

        /// <summary>
        /// Maps the given ProjectCountCollection to an OutputProjectCountCollection.
        /// </summary>
        /// <param name="projectCountCollection">The ProjectCountCollection to convert.</param>
        /// <returns>An OutputProjectCountCollection.</returns>
        OutputProjectCountCollection MapOutputProjectCountCollection(ProjectCountCollection projectCountCollection);
    }
}
