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
    /// Default implementation of IProjectRepository.
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        private readonly string connectionString;

        public ProjectRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new Project.
        /// </summary>
        /// <param name="name">The name of the Project.</param>
        /// <param name="description">A brief description of the Project.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        public async Task<Project> CreateProjectAsync(string name, string description)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Projects_Create";
                var parameters = new
                {
                    name = new DbString { Value = name, IsFixedLength = false, Length = 64, IsAnsi = false },
                    description = new DbString { Value = description, IsFixedLength = false, Length = 512, IsAnsi = false }
                };
                Project createdProject = await connection.QuerySingleAsync<Project>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return createdProject;
            }
        }

        /// <summary>
        /// Gets a list of all Projects.
        /// </summary>
        /// <returns>An awaitable Task of type List of type Project.</returns>
        public async Task<List<Project>> GetProjectsAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Projects_GetAll";
                List<Project> projects = (await connection.QueryAsync<Project>(storedProcedureName,
                    commandType: CommandType.StoredProcedure)).ToList();
                return projects;
            }
        }

        /// <summary>
        /// Gets a specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public Project ID.</param>
        /// <returns>An awaitable Task of type Project.</returns>
        public async Task<Project> GetProjectAsync(Guid publicProjectId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Projects_Get";
                var parameters = new
                {
                    publicProjectId
                };
                Project project = await connection.QuerySingleAsync<Project>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return project;
            }
        }

        /// <summary>
        /// Gets the counts of Epics, Stories, and Bugs for the specified Project.
        /// </summary>
        /// <param name="projectId">The ID of the Project.</param>
        /// <returns>An awaitable Task of type ProjectCountCollection.</returns>
        public async Task<ProjectCountCollection> GetProjectCountCollectionAsync(int projectId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Projects_GetCounts";
                var parameters = new
                {
                    projectId
                };
                ProjectCountCollection projectCountCollection = await connection.QuerySingleAsync<ProjectCountCollection>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return projectCountCollection;
            }
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Projects_Update";
                var parameters = new
                {
                    publicProjectId,
                    name = new DbString { Value = name, IsFixedLength = false, Length = 64, IsAnsi = false },
                    description = new DbString { Value = description, IsFixedLength = false, Length = 512, IsAnsi = false }
                };
                Project updatedProject = await connection.QuerySingleAsync<Project>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return updatedProject;
            }
        }

        /// <summary>
        /// Deletes the specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task DeleteProjectAsync(Guid publicProjectId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Projects_Delete";
                var parameters = new
                {
                    publicProjectId
                };
                await connection.ExecuteAsync(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
