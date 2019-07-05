using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spriint.Api.Core;
using Spriint.Api.Managers;
using Spriint.Api.Mappers;
using Spriint.Api.Models;

namespace Spriint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IExceptionManager exceptionManager;
        private readonly IProjectManager projectManager;
        private readonly IProjectMapper projectMapper;
        private readonly ILogger<ProjectsController> logger;

        public ProjectsController(IExceptionManager exceptionManager,
            IProjectManager projectManager,
            IProjectMapper projectMapper,
            ILogger<ProjectsController> logger)
        {
            this.exceptionManager = exceptionManager;
            this.projectManager = projectManager;
            this.projectMapper = projectMapper;
            this.logger = logger;
        }


        // POST api/projects
        [HttpPost]
        public async Task<ActionResult<OutputProject>> Post([FromBody] CreateProject createProject)
        {
            try
            {
                logger.LogInformation("Beginning request: /api/projects POST");
                Project createdProject = await projectManager.CreateProjectAsync(createProject.Name,
                    createProject.Description);
                string createdProjectUrl = $"{Utilities.Web.GetBaseUrl(HttpContext)}/api/projects/{createdProject.PublicProjectId}";
                OutputProject output = projectMapper.MapOutputProject(createdProject);
                logger.LogInformation("Request complete: /api/projects POST");
                return Created(createdProjectUrl, output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // GET api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutputProject>>> Get()
        {
            try
            {
                logger.LogInformation("Beginning request: /api/projects GET");
                List<Project> projects = await projectManager.GetProjectsAsync();
                IEnumerable<OutputProject> output = projects.Select(x => projectMapper.MapOutputProject(x));
                logger.LogInformation("Request complete: /api/projects GET");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // GET api/projects/{projectId}
        [HttpGet("{projectId}")]
        public async Task<ActionResult<OutputProject>> Get(Guid projectId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId} GET");
                Project project = await projectManager.GetProjectAsync(projectId);
                OutputProject output = projectMapper.MapOutputProject(project);
                logger.LogInformation($"Request complete: /api/projects/{projectId} GET");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // GET api/projects/{projectId}/counts
        [HttpGet("{projectId}/counts")]
        public async Task<ActionResult<OutputProjectCountCollection>> GetCounts(Guid projectId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId}/counts GET");
                ProjectCountCollection projectCountCollection = await projectManager.GetProjectCountCollectionAsync(projectId);
                OutputProjectCountCollection output = projectMapper.MapOutputProjectCountCollection(projectCountCollection);
                logger.LogInformation($"Request complete: /api/projects/{projectId}/counts GET");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // PUT api/projects/{projectId}
        [HttpPut("{projectId}")]
        public async Task<ActionResult<OutputProject>> Put(Guid projectId, [FromBody] UpdateProject updateProject)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId} PUT");
                Project updatedProject = await projectManager.UpdateProjectAsync(projectId, updateProject.Name,
                    updateProject.Description);
                OutputProject output = projectMapper.MapOutputProject(updatedProject);
                logger.LogInformation($"Request complete: /api/projects/{projectId} PUT");
                return Ok(output);
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }

        // DELETE api/projects/{projectId}
        [HttpDelete("{projectId}")]
        public async Task<ActionResult> Delete(Guid projectId)
        {
            try
            {
                logger.LogInformation($"Beginning request: /api/projects/{projectId} DELETE");
                await projectManager.DeleteProjectAsync(projectId);
                logger.LogInformation($"Request complete: /api/projects/{projectId} DELETE");
                return NoContent();
            }
            catch (Exception ex)
            {
                return exceptionManager.Handle(ex);
            }
        }
    }
}
