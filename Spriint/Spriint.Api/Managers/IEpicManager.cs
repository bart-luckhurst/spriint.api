using Spriint.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Managers
{
    /// <summary>
    /// Defines the interface for EpicManagers.
    /// </summary>
    public interface IEpicManager
    {
        /// <summary>
        /// Creates a new Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project to which the Epic will belong.</param>
        /// <param name="name">The name of the Epic.</param>
        /// <param name="description">A brief description of the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        Task<Epic> CreateEpicAsync(Guid publicProjectId, string name, string description);

        /// <summary>
        /// Gets all Epics for a specified Project.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <returns>An awaitable Task of type List of type Epic.</returns>
        Task<List<Epic>> GetEpicsAsync(Guid publicProjectId);

        /// <summary>
        /// Gets a specified Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        Task<Epic> GetEpicAsync(Guid publicProjectId, Guid publicEpicId);

        /// <summary>
        /// Updates a specified Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <param name="name">The new name of the Epic.</param>
        /// <param name="description">The new description for the Epic.</param>
        /// <returns>An awaitable Task of type Epic.</returns>
        Task<Epic> UpdateEpicAsync(Guid publicProjectId, Guid publicEpicId, string name, string description);

        /// <summary>
        /// Deletes a specified Epic.
        /// </summary>
        /// <param name="publicProjectId">The public ID of the Project.</param>
        /// <param name="publicEpicId">The public ID of the Epic.</param>
        /// <returns>An awaitable Task.</returns>
        Task DeleteEpicAsync(Guid publicProjectId, Guid publicEpicId);
    }
}
