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
    /// Default implementation of IEpicManager.
    /// </summary>
    public class EpicManager : IEpicManager
    {
        private readonly IProjectRepository projectRepository;
        private readonly IEpicRepository epicRepository;

        public EpicManager(IProjectRepository projectRepository,
            IEpicRepository epicRepository)
        {
            this.projectRepository = projectRepository;
            this.epicRepository = epicRepository;
        }

        /// <summary>
        /// Creates a new Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project to which the Epic will belong.</param>
        /// <param name="name">The name of the Epic.</param>
        /// <param name="description">A brief description of the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        public async Task<Epic> CreateEpicAsync(Guid publicProjectId, 
            string name,
            string description)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
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
            //create Epic
            Epic output = await epicRepository.CreateEpicAsync(project.ProjectId, name, description);
            //attach Project
            output.Project = project;
            return output;
        }

        /// <summary>
        /// Gets all Epics for a specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task of type List of type Epic.</returns>
        public async Task<List<Epic>> GetEpicsAsync(Guid publicProjectId)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Epics
            List<Epic> output = await epicRepository.GetEpicsAsync(project.ProjectId);
            //attach Project
            output.ForEach(x => x.Project = project);
            return output;
        }

        /// <summary>
        /// Gets a specified Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        public async Task<Epic> GetEpicAsync(Guid publicProjectId, Guid publicEpicId)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Epic
            Epic output = await epicRepository.GetEpicAsync(publicEpicId);
            if (output == null)
            {
                throw new NotFoundException();
            }
            return output;
        }

        /// <summary>
        /// Updates a specified Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <param name="name">The new name of the Epic.</param>
        /// <param name="description">The new description for the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        public async Task<Epic> UpdateEpicAsync(Guid publicProjectId, 
            Guid publicEpicId, 
            string name, 
            string description)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Epic
            Epic epicToUpdate = await epicRepository.GetEpicAsync(publicEpicId);
            if (epicToUpdate == null)
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
            //update Epic
            Epic output = await epicRepository.UpdateEpicAsync(publicEpicId, name, description);
            //attach Project
            output.Project = project;
            return output;
        }

        /// <summary>
        /// Deletes a specified Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task DeleteEpicAsync(Guid publicProjectId, Guid publicEpicId)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Epic
            Epic epicToUpdate = await epicRepository.GetEpicAsync(publicEpicId);
            if (epicToUpdate == null)
            {
                throw new NotFoundException();
            }
            //delete Epic
            await epicRepository.DeleteEpicAsync(publicEpicId);
        }
    }
}
