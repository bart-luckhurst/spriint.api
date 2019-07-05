using Spriint.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Repositories
{
    /// <summary>
    /// Defines the interface for ProjectRepositories.
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// Creates a new Project.
        /// </summary>
        /// <param name="name">The name of the Project.</param>
        /// <param name="description">A brief description of the Project.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        Task<Project> CreateProjectAsync(string name, string description);

        /// <summary>
        /// Gets a list of all Projects.
        /// </summary>
        /// <returns>An awaitable Task of type List of type Project.</returns>
        Task<List<Project>> GetProjectsAsync();

        /// <summary>
        /// Gets a specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public Project ID.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        Task<Project> GetProjectAsync(Guid publicProjectId);

        /// <summary>
        /// Gets the counts of Epics, Stories, and Bugs for the specified Project.
        /// </summary>
        /// <param name="projectId">The ID of the Project.</param>
        /// <returns>An awaitable Task of type ProjectCountCollection.</returns>
        Task<ProjectCountCollection> GetProjectCountCollectionAsync(int projectId);

        /// <summary>
        /// Updates the specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="name">The new name of the Project.</param>
        /// <param name="description">The new description for the Project.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        Task<Project> UpdateProjectAsync(Guid publicProjectId, string name, string description);

        /// <summary>
        /// Deletes the specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task.</returns>
        Task DeleteProjectAsync(Guid publicProjectId);
    }
}
