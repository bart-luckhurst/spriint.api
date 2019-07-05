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
    /// Default implementation of IEpicRepository.
    /// </summary>
    public class EpicRepository : IEpicRepository
    {
        private readonly string connectionString;

        public EpicRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new Epic.
        /// </summary>
        /// <param name="projectId">The ID of the Project to which the Epic will belong.</param>
        /// <param name="name">The name of the Epic.</param>
        /// <param name="description">A brief description of the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        public async Task<Epic> CreateEpicAsync(int projectId, string name, string description)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Epics_Create";
                var parameters = new
                {
                    projectId,
                    name = new DbString { Value = name, IsFixedLength = false, Length = 64, IsAnsi = false },
                    description = new DbString { Value = description, IsFixedLength = false, Length = 512, IsAnsi = false }
                };
                Epic createdEpic = await connection.QuerySingleAsync<Epic>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return createdEpic;
            }
        }

        /// <summary>
        /// Gets all Epics for a specified Project.
        /// </summary>
        /// <param name="projectId">The ID of the Project.</param>
        /// <returns>An awaitable Task of type List of type Epic.</returns>
        public async Task<List<Epic>> GetEpicsAsync(int projectId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Epics_GetAll";
                var parameters = new
                {
                    projectId
                };
                List<Epic> epics = (await connection.QueryAsync<Epic>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure)).ToList();
                return epics;
            }
        }

        /// <summary>
        /// Gets a specified Epic.
        /// </summary>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        public async Task<Epic> GetEpicAsync(Guid publicEpicId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Epics_Get";
                var parameters = new
                {
                    publicEpicId
                };
                Epic epic = await connection.QuerySingleAsync<Epic>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return epic;
            }
        }

        /// <summary>
        /// Gets a specified Epic by its private ID.
        /// </summary>
        /// <param name="privateEpicId">The private ID of the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        public async Task<Epic> GetEpicAsync(int privateEpicId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Epics_GetByPrivateId";
                var parameters = new
                {
                    privateEpicId
                };
                Epic epic = await connection.QuerySingleAsync<Epic>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return epic;
            }
        }

        /// <summary>
        /// Updates a specified Epic.
        /// </summary>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <param name="name">The new name of the Epic.</param>
        /// <param name="description">The new description for the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        public async Task<Epic> UpdateEpicAsync(Guid publicEpicId, string name, string description)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Epics_Update";
                var parameters = new
                {
                    publicEpicId,
                    name = new DbString { Value = name, IsFixedLength = false, Length = 64, IsAnsi = false },
                    description = new DbString { Value = description, IsFixedLength = false, Length = 512, IsAnsi = false }
                };
                Epic updatedEpic = await connection.QuerySingleAsync<Epic>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return updatedEpic;
            }
        }

        /// <summary>
        /// Deletes a specified Epic.
        /// </summary>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task DeleteEpicAsync(Guid publicEpicId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "Epics_Delete";
                var parameters = new
                {
                    publicEpicId
                };
                await connection.ExecuteAsync(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
