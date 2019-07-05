using Spriint.Api.Core;
using Spriint.Api.Exceptions;
using Spriint.Api.Repositories;
using Spriint.Api.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Managers
{
    /// <summary>
    /// Default implementation of IProjectManager.
    /// </summary>
    public class ProjectManager : IProjectManager
    {
        private readonly IProjectRepository projectRepository;

        public ProjectManager(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        /// <summary>
        /// Creates a new Project.
        /// </summary>
        /// <param name="name">The name of the Project.</param>
        /// <param name="description">A brief description of the Project.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        public async Task<Project> CreateProjectAsync(string name, string description)
        {
            List<ValidationError> validationErrors = new List<ValidationError>();
            //validate name is set
            if (!name.ValidateIsSet())
            {
                validationErrors.Add(new ValidationError("name", "Must be set."));
            }
            //validate name max length 64
            if (!name.ValidateMaxLength(64))
            {
                validationErrors.Add(new ValidationError("name", "Must be shorter than 64 characters."));
            }
            //validate description max length 512
            if (!description.ValidateMaxLength(512))
            {
                validationErrors.Add(new ValidationError("description", "Must be shorter than 512 characters."));
            }
            //throw ValidationException if necessary
            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }
            //create project
            Project output = await projectRepository.CreateProjectAsync(name, description);
            return output;
        }

        /// <summary>
        /// Gets a list of all Projects.
        /// </summary>
        /// <returns>An awaitable Task of type List of type Project.</returns>
        public async Task<List<Project>> GetProjectsAsync()
        {
            //get all projects
            List<Project> output = await projectRepository.GetProjectsAsync();
            return output;
        }

        /// <summary>
        /// Gets a specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public Project ID.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        public async Task<Project> GetProjectAsync(Guid publicProjectId)
        {
            //get specified Project
            Project output = await projectRepository.GetProjectAsync(publicProjectId);
            //check Project exists
            if (output == null)
            {
                throw new NotFoundException();
            }
            return output;
        }

        /// <summary>
        /// Gets the counts of Epics, Stories, and Bugs for the specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task of type ProjectCountCollection.</returns>
        public async Task<ProjectCountCollection> GetProjectCountCollectionAsync(Guid publicProjectId)
        {
            //check Project exists
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get counts
            ProjectCountCollection projectCountCollection = await projectRepository.GetProjectCountCollectionAsync(project.ProjectId);
            return projectCountCollection;
        }

        /// <summary>
        /// Updates the specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="name">The new name of the Project.</param>
        /// <param name="description">The new description for the Project.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        public async Task<Project> UpdateProjectAsync(Guid publicProjectId, string name, string description)
        {
            //check Project exists
            Project projectToUpdate = await projectRepository.GetProjectAsync(publicProjectId);
            if (projectToUpdate == null)
            {
                throw new NotFoundException();
            }
            //validate input
            List<ValidationError> validationErrors = new List<ValidationError>();
            //validate name is set
            if (!name.ValidateIsSet())
            {
                validationErrors.Add(new ValidationError("name", "Must be set."));
            }
            //validate name max length 64
            if (!name.ValidateMaxLength(64))
            {
                validationErrors.Add(new ValidationError("name", "Must be shorter than 64 characters."));
            }
            //validate description max length 512
            if (!description.ValidateMaxLength(512))
            {
                validationErrors.Add(new ValidationError("description", "Must be shorter than 512 characters."));
            }
            //throw ValidationException if necessary
            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }
            //create project
            Project output = await projectRepository.UpdateProjectAsync(publicProjectId, name, description);
            return output;
        }

        /// <summary>
        /// Deletes the specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task DeleteProjectAsync(Guid publicProjectId)
        {
            //get specified Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            //check Project exists
            if (project == null)
            {
                throw new NotFoundException();
            }
            //delete Project
            await projectRepository.DeleteProjectAsync(publicProjectId);
        }
    }
}
