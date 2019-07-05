using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spriint.Api.Core;
using Spriint.Api.Models;

namespace Spriint.Api.Mappers
{
    /// <summary>
    /// Default implementation of IProjectMapper.
    /// </summary>
    public class ProjectMapper : IProjectMapper
    {
        /// <summary>
        /// Maps the given Project to an OutputProject.
        /// </summary>
        /// <param name="project">The Project to map.</param>
        /// <returns>An OutputProject.</returns>
        public OutputProject MapOutputProject(Project project)
        {
            return new OutputProject()
            {
                ProjectId = project.PublicProjectId.ToString(),
                Name = project.Name,
                Description = project.Description,
                DateTimeCreated = project.DateTimeCreated
            };
        }

        /// <summary>
        /// Maps the given ProjectCountCollection to an OutputProjectCountCollection.
        /// </summary>
        /// <param name="projectCountCollection">The ProjectCountCollection to convert.</param>
        /// <returns>An OutputProjectCountCollection.</returns>
        public OutputProjectCountCollection MapOutputProjectCountCollection(ProjectCountCollection projectCountCollection)
        {
            return new OutputProjectCountCollection()
            {
                ProjectId = projectCountCollection.ProjectId,
                EpicCount = projectCountCollection.EpicCount,
                StoryCount = projectCountCollection.StoryCount,
                BugCount = projectCountCollection.BugCount
            };
        }
    }
}
