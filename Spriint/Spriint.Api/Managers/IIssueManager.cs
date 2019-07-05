using Spriint.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Managers
{
    /// <summary>
    /// Defines the interface for IssueManagers.
    /// </summary>
    public interface IIssueManager
    {
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
        Task<Issue> CreateIssueAsync(Guid publicProjectId,
            Guid? publicEpicId,
            string issueType,
            string name,
            string description,
            string status,
            int? estimate);

        /// <summary>
        /// Gets all Issues for a specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task of type List of Type Issue.</returns>
        Task<List<Issue>> GetIssuesAsync(Guid publicProjectId);

        /// <summary>
        /// Gets the specified Issue.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        Task<Issue> GetIssueAsync(Guid publicProjectId, 
            Guid publicIssueId);

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
        Task<Issue> UpdateIssueAsync(Guid publicProjectId,
            Guid publicIssueId,
            Guid? publicEpicId,
            string issueType,
            string name,
            string description,
            string status,
            int? estimate);

        /// <summary>
        /// Updates the status of the specified Issue.
        /// </summary>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <param name="status">The new status.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        Task<Issue> UpdateIssueStatusAsync(Guid publicProjectId,
            Guid publicIssueId,
            string status);

        /// <summary>
        /// Deletes the specified Issue.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <returns>An awaitable Task.</returns>
        Task DeleteIssueAsync(Guid publicProjectId,
            Guid publicIssueId);
    }
}
