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
    public class IssuesController : ControllerBase
    {
        private readonly IExceptionManager exceptionManager;
        private readonly IIssueManager issueManager;
        private readonly IIssueMapper issueMapper;
        private readonly ILogger<IssuesController> logger;

        public IssuesController(IExceptionManager exceptionManager,
            IIssueManager issueManager,
            IIssueMapper issueMapper,
            ILogger<IssuesController> logger)
        {
            this.exceptionManager = exceptionManager;
            this.issueManager = issueManager;
            this.issueMapper = issueMapper;
            this.logger = logger;
        }

        // POST api/projects/{projectId}/issues
        [HttpPost("api/projects/{projectId}/issues")]
        public async Task<ActionResult<OutputIssue>> Post(Guid projectId, [FromBody] CreateIssue createIssue)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/issues POST");
                Issue createdIssue = await issueManager.CreateIssueAsync(projectId,
                    createIssue.EpicId,
                    createIssue.IssueType,
                    createIssue.Name,
                    createIssue.Description,
                    createIssue.Status,
                    createIssue.Estimate);
                string createdProjectUrl = $"{Utilities.Web.GetBaseUrl(HttpContext)}/api/projects/{projectId}/issues{createdIssue.PublicIssueId}";
                OutputIssue output = issueMapper.MapOutputIssue(createdIssue);
                logger.LogInformation("Request complete: /api/projects/{projectId}/issues POST");
                return Created(createdProjectUrl, output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // GET api/projects/{projectId}/issues
        [HttpGet("api/projects/{projectId}/issues")]
        public async Task<ActionResult<IEnumerable<OutputIssue>>> Get(Guid projectId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/issues GET");
                List<Issue> issues = await issueManager.GetIssuesAsync(projectId);
                IEnumerable<OutputIssue> output = issues.Select(x => issueMapper.MapOutputIssue(x));
                logger.LogInformation("Request complete: /api/projects/{projectId}/issues GET");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // GET api/projects/{projectId}/issues/{issueId}
        [HttpGet("api/projects/{projectId}/issues/{issueId}")]
        public async Task<ActionResult<OutputIssue>> Get(Guid projectId, Guid issueId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/issues/{issueId} GET");
                Issue issue = await issueManager.GetIssueAsync(projectId, issueId);
                OutputIssue output = issueMapper.MapOutputIssue(issue);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/issues/{issueId} GET");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // PUT api/projects/{projectId}/issues/{issueId}
        [HttpPut("api/projects/{projectId}/issues/{issueId}")]
        public async Task<ActionResult<OutputIssue>> Put(Guid projectId, Guid issueId, [FromBody] UpdateIssue updateIssue)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/issues/{issueId} PUT");
                Issue updatedIssue = await issueManager.UpdateIssueAsync(projectId,
                    issueId,
                    updateIssue.EpicId,
                    updateIssue.IssueType,
                    updateIssue.Name,
                    updateIssue.Description,
                    updateIssue.Status,
                    updateIssue.Estimate);
                OutputIssue output = issueMapper.MapOutputIssue(updatedIssue);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/issues/{issueId} PUT");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        //PATCH /api/projects/{projectId}/issues/{issueId}
        [HttpPatch("api/projects/{projectId}/issues/{issueId}")]
        public async Task<ActionResult<OutputIssue>> PatchStatus(Guid projectId, Guid issueId, [FromBody] string status)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/issues/{issueId} PATCH");
                Issue updatedIssue = await issueManager.UpdateIssueStatusAsync(projectId,
                    issueId,
                    status);
                OutputIssue output = issueMapper.MapOutputIssue(updatedIssue);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/issues/{issueId} PATCH");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // DELETE api/projects/{projectId}/issues/{issueId}
        [HttpDelete("api/projects/{projectId}/issues/{issueId}")]
        public async Task<ActionResult> Delete(Guid projectId, Guid issueId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/issue/{issueId} DELETE");
                await issueManager.DeleteIssueAsync(projectId, issueId);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/issue/{issueId} DELETE");
                return NoContent();
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }
    }
}