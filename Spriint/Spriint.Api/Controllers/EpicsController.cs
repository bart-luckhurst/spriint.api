using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spriint.Api.Core;
using Spriint.Api.Managers;
using Spriint.Api.Mappers;
using Spriint.Api.Models;

namespace Spriint.Api.Controllers
{
    [ApiController]
    public class EpicsController : ControllerBase
    {
        private readonly IExceptionManager exceptionManager;
        private readonly IEpicManager epicManager;
        private readonly IEpicMapper epicMapper;
        private readonly ILogger<EpicsController> logger;

        public EpicsController(IExceptionManager exceptionManager,
            IEpicManager epicManager,
            IEpicMapper epicMapper,
            ILogger<EpicsController> logger)
        {
            this.exceptionManager = exceptionManager;
            this.epicManager = epicManager;
            this.epicMapper = epicMapper;
            this.logger = logger;
        }


        // POST api/projects/{projectId}/epics
        [HttpPost("api/projects/{projectId}/epics")]
        public async Task<ActionResult<OutputEpic>> Post(Guid projectId, [FromBody] CreateEpic createEpic)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/epics POST");
                Epic createdEpic = await epicManager.CreateEpicAsync(projectId,
                    createEpic.Name,
                    createEpic.Description);
                string createdProjectUrl = $"{Utilities.Web.GetBaseUrl(HttpContext)}/api/projects/{projectId}/epics{createdEpic.PublicEpicId}";
                OutputEpic output = epicMapper.MapOutputEpic(createdEpic);
                logger.LogInformation("Request complete: /api/projects/{projectId}/epics POST");
                return Created(createdProjectUrl, output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // GET api/projects/{projectId}/epics
        [HttpGet("api/projects/{projectId}/epics")]
        public async Task<ActionResult<IEnumerable<OutputEpic>>> Get(Guid projectId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/epics GET");
                List<Epic> epics = await epicManager.GetEpicsAsync(projectId);
                IEnumerable<OutputEpic> output = epics.Select(x => epicMapper.MapOutputEpic(x));
                logger.LogInformation("Request complete: /api/projects/{projectId}/epics GET");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // GET api/projects/{projectId}/epics/{epicId}
        [HttpGet("api/projects/{projectId}/epics/{epicId}")]
        public async Task<ActionResult<OutputEpic>> Get(Guid projectId, Guid epicId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/epics/{epicId} GET");
                Epic epic = await epicManager.GetEpicAsync(projectId, epicId);
                OutputEpic output = epicMapper.MapOutputEpic(epic);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/epics/{epicId} GET");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // PUT api/projects/{projectId}/epics/{epicId}
        [HttpPut("api/projects/{projectId}/epics/{epicId}")]
        public async Task<ActionResult<OutputEpic>> Put(Guid projectId, Guid epicId, [FromBody] UpdateEpic updateEpic)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/epics/{epicId} PUT");
                Epic updatedEpic = await epicManager.UpdateEpicAsync(projectId, 
                    epicId,
                    updateEpic.Name,
                    updateEpic.Description);
                OutputEpic output = epicMapper.MapOutputEpic(updatedEpic);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/epics/{epicId} PUT");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // DELETE api/projects/{projectId}/epics/{epicId}
        [HttpDelete("api/projects/{projectId}/epics/{epicId}")]
        public async Task<ActionResult> Delete(Guid projectId, Guid epicId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/epic/{epicId} DELETE");
                await epicManager.DeleteEpicAsync(projectId, epicId);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/epic/{epicId} DELETE");
                return NoContent();
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }
    }
}