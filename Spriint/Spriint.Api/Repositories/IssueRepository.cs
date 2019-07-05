using Dapper;
using Spriint.Api.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Repositories
{
    /// <summary>
    /// Default implementation of IIssueRepository.
    /// </summary>
    public class IssueRepository : IIssueRepository
    {
        private readonly string connectionString;

        public IssueRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new Issue.
        /// </summary>
        /// <param name="projectId">The ID of the Project.</param>
        /// <param name="epicId">The ID of the Epic (if any).</param>
        /// <param name="issueType">The IssueType.</param>
        /// <param name="name">The name of the Issue.</param>
        /// <param name="description">A description of the Issue.</param>
        /// <param name="status">The status of the Issue.</param>
        /// <param name="estimate">The estimate for the Issue (if any).</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> CreateIssueAsync(int projectId,
            int? epicId,
            IssueType issueType,
            string name,
            string description,
            Status status,
            int? estimate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Issues_Create";
                var parameters = new
                {
                    projectId,
                    epicId,
                    issueType = (int)issueType,
                    name = new DbString { Value = name, IsFixedLength = false, Length = 64, IsAnsi = false },
                    description = new DbString { Value = description, IsFixedLength = false, Length = 512, IsAnsi = false },
                    status = (int)status,
                    estimate
                };
                Issue createdIssue = await connection.QuerySingleAsync<Issue>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return createdIssue;
            }
        }

        /// <summary>
        /// Gets all Issues for a specified Project.
        /// </summary>
        /// <param name="projectId">The ID of the Project.</param>
        /// <returns>An awaitable Task of type List of Type Issue.</returns>
        public async Task<List<Issue>> GetIssuesAsync(int projectId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Issues_GetAll";
                var parameters = new
                {
                    projectId
                };
                List<Issue> issues = (await connection.QueryAsync<Issue>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure)).ToList();
                return issues;
            }
        }

        /// <summary>
        /// Gets the specified Issue.
        /// </summary>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> GetIssueAsync(Guid publicIssueId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Issues_Get";
                var parameters = new
                {
                    publicIssueId
                };
                Issue issue = await connection.QuerySingleAsync<Issue>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return issue;
            }
        }

        /// <summary>
        /// Updates the specified Issue.
        /// </summary>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <param name="epicId">The Epic ID.</param>
        /// <param name="issueType">The new IssueType.</param>
        /// <param name="name">The new Name.</param>
        /// <param name="description">The new description.</param>
        /// <param name="status">The new status.</param>
        /// <param name="estimate">The new estimate.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> UpdateIssueAsync(Guid publicIssueId,
            int? epicId,
            IssueType issueType,
            string name,
            string description,
            Status status,
            int? estimate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Issues_Update";
                var parameters = new
                {
                    publicIssueId,
                    epicId,
                    issueType = (int)issueType,
                    name = new DbString { Value = name, IsFixedLength = false, Length = 64, IsAnsi = false },
                    description = new DbString { Value = description, IsFixedLength = false, Length = 512, IsAnsi = false },
                    status = (int)status,
                    estimate
                };
                Issue updatedIssue = await connection.QuerySingleAsync<Issue>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return updatedIssue;
            }
        }

        /// <summary>
        /// Updates the status of the specified Issue.
        /// </summary>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <param name="status">The new status.</param>
        /// <returns>An awaitable Task of type Issue.</returns>
        public async Task<Issue> UpdateIssueStatusAsync(Guid publicIssueId,
            Status status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Issues_UpdateStatus";
                var parameters = new
                {
                    publicIssueId,
                    status = (int)status
                };
                Issue updatedIssue = await connection.QuerySingleAsync<Issue>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return updatedIssue;
            }
        }

        /// <summary>
        /// Deletes the specified Issue.
        /// </summary>
        /// <param name="publicIssueId">The public ID of the Issue.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task DeleteIssueAsync(Guid publicIssueId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Issues_Delete";
                var parameters = new
                {
                    publicIssueId
                };
                await connection.ExecuteAsync(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
