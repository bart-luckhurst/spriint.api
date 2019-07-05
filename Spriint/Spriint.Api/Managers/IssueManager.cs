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
    /// Default implementation of IIssueManager.
    /// </summary>
    public class IssueManager : IIssueManager
    {
        private readonly IIssueRepository issueRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IEpicRepository epicRepository;

        public IssueManager(IIssueRepository issueRepository,
            IProjectRepository projectRepository,
            IEpicRepository epicRepository)
        {
            this.issueRepository = issueRepository;
            this.projectRepository = projectRepository;
            this.epicRepository = epicRepository;
        }

        /// <summary>
        /// Creates a new Issue.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicEpicId">The public ID of the Epic (if any).</param>
        /// <param name="issueType">The IssueType.</param>
        /// <param name="name">The name of the Issue.</param>
        /// <param name="description">A description of the Issue.</param>
        /// <param name="status">The status of the Issue.</param>
        /// <param name="estimate">The estimate for the Issue (if any).</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> CreateIssueAsync(Guid publicProjectId,
            Guid? publicEpicId,
            string issueType,
            string name,
            string description,
            string status,
            int? estimate)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //validate input
            List<ValidationError> validationErrors = new List<ValidationError>();
            //get Epic
            Epic epic = null;
            if (publicEpicId != null)
            {
                epic = await epicRepository.GetEpicAsync((Guid)publicEpicId);
                if (epic == null)
                {
                    validationErrors.Add(new ValidationError("epicId", "Must be a valid epic ID."));
                }
            }
            //validate type
            if (!Enum.TryParse(issueType, out IssueType parsedIssueType))
            {
                validationErrors.Add(new ValidationError("issueType", "Must be a valid issue type."));
            }
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
            //validate status
            if (!Enum.TryParse(status, out Status parsedStatus))
            {
                validationErrors.Add(new ValidationError("status", "Must be a valid status."));
            }
            //throw ValidationException if necessary
            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }
            //create Issue
            Issue output = await issueRepository.CreateIssueAsync(project.ProjectId,
                epic?.EpicId,
                parsedIssueType,
                name, 
                description,
                parsedStatus,
                estimate);
            return output;
        }

        /// <summary>
        /// Gets all Issues for a specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task of type List of Type Issue.</returns>
        public async Task<List<Issue>> GetIssuesAsync(Guid publicProjectId)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Issues
            List<Issue> output = await issueRepository.GetIssuesAsync(project.ProjectId);
            //attach Project
            output.ForEach(x => x.Project = project);
            return output;
        }

        /// <summary>
        /// Gets the specified Issue.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> GetIssueAsync(Guid publicProjectId,
            Guid publicIssueId)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Issue
            Issue output = await issueRepository.GetIssueAsync(publicIssueId);
            if (output == null)
            {
                throw new NotFoundException();
            }
            //get Epic
            if (output.EpicId != null)
            {
                output.Epic = await epicRepository.GetEpicAsync((int)output.EpicId);
            }
            //attach Project
            output.Project = project;
            return output;
        }

        /// <summary>
        /// Updates the specified Issue.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <param name="issueType">The new IssueType.</param>
        /// <param name="name">The new Name.</param>
        /// <param name="description">The new description.</param>
        /// <param name="status">The new status.</param>
        /// <param name="estimate">The new estimate.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> UpdateIssueAsync(Guid publicProjectId,
            Guid publicIssueId,
            Guid? publicEpicId,
            string issueType,
            string name,
            string description,
            string status,
            int? estimate)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Issue
            Issue issueToUpdate = await issueRepository.GetIssueAsync(publicIssueId);
            if (issueToUpdate == null)
            {
                throw new NotFoundException();
            }
            //validate input
            List<ValidationError> validationErrors = new List<ValidationError>();
            //get Epic
            Epic epic = null;
            if (publicEpicId != null)
            {
                epic = await epicRepository.GetEpicAsync((Guid)publicEpicId);
                if (epic == null)
                {
                    validationErrors.Add(new ValidationError("epicId", "Must be a valid epic ID."));
                }
            }
            //validate IssueType
            if (!Enum.TryParse(issueType, out IssueType parsedIssueType))
            {
                validationErrors.Add(new ValidationError("issueType", "Must be a valid issue type."));
            }
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
            //validate Status
            if (!Enum.TryParse(status, out Status parsedStatus))
            {
                validationErrors.Add(new ValidationError("status", "Must be a valid status."));
            }
            //throw ValidationException if necessary
            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }
            //udate Issue
            Issue output = await issueRepository.UpdateIssueAsync(publicIssueId,
                epic?.EpicId,
                parsedIssueType,
                name,
                description,
                parsedStatus,
                estimate);
            //attach Project
            output.Project = project;
            return output;
        }

        /// <summary>
        /// Updates the status of the specified Issue.
        /// </summary>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <param name="status">The new status.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> UpdateIssueStatusAsync(Guid publicProjectId,
            Guid publicIssueId,
            string status)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Issue
            Issue issueToUpdate = await issueRepository.GetIssueAsync(publicIssueId);
            if (issueToUpdate == null)
            {
                throw new NotFoundException();
            }
            //validate input
            List<ValidationError> validationErrors = new List<ValidationError>();
            if (!Enum.TryParse(status, out Status parsedStatus))
            {
                validationErrors.Add(new ValidationError("status", "Must be a valid status."));
            }
            //throw ValidationException if necessary
            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }
            //udate Issue
            Issue output = await issueRepository.UpdateIssueStatusAsync(publicIssueId,
                parsedStatus);
            //attach Project
            output.Project = project;
            return output;
        }

        /// <summary>
        /// Deletes the specified Issue.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task DeleteIssueAsync(Guid publicProjectId,
            Guid publicIssueId)
        {
            //get Project
            Project project = await projectRepository.GetProjectAsync(publicProjectId);
            if (project == null)
            {
                throw new NotFoundException();
            }
            //get Issue
            Issue issueToDelete = await issueRepository.GetIssueAsync(publicIssueId);
            if (issueToDelete == null)
            {
                throw new NotFoundException();
            }
            //delete Issue
            await issueRepository.DeleteIssueAsync(publicIssueId);
        }
    }
}
