using Spriint.Api.Core;
using Spriint.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spriint.Api.Mappers
{
    /// <summary>
    /// Defines the interface for EpicMappers.
    /// </summary>
    public interface IEpicMapper
    {
        /// <summary>
        /// Maps the given Epic to an OutputEpic.
        /// </summary>
        /// <param name="epic">The Epic to map.</param>
        /// <returns>An OutputEpic.</returns>
        OutputEpic MapOutputEpic(Epic epic);
    }
}
