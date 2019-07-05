using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Spriint.Api.Core;
using Spriint.Api.Exceptions;
using Spriint.Api.Managers;
using Spriint.Api.Repositories;
using Spriint.Api.Test.Utilities;
using Xunit;

namespace Spriint.Api.Test
{
    public class ProjectManagerTests
    {
        #region CreateProjectAsync Tests
        [Theory]
        [InlineData(null, "A brief description.")]
        [InlineData(null, null)]
        public async Task CreateProjectAsync_NameNotSet_ThrowsValidationException(string name, string description)
        {
            //arrange
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await projectManager.CreateProjectAsync(name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be set.");
        }

        [Theory]
        [MemberData(nameof(GetNameTooLongData))]
        public async Task CreateProjectAsync_NameTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await projectManager.CreateProjectAsync(name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 64 characters.");
        }

        [Theory]
        [MemberData(nameof(GetDescriptionTooLongData))]
        public async Task CreateProjectAsync_DescriptionTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await projectManager.CreateProjectAsync(name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "description");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 512 characters.");
        }

        [Theory]
        [InlineData("Project Name", "A brief description.")]
        [InlineData("Project Name", null)]
        public async Task CreateProjectAsync_ValidInput_ReturnsProject(string name, string description)
        {
            //arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(x => x.CreateProjectAsync(name, description))
                .ReturnsAsync(new Project()
                {
                    Name = name,
                    Description = description,
                    DateTimeCreated = DateTime.UtcNow,
                    ProjectId = 1,
                    PublicProjectId = Guid.NewGuid()
                });
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act
            Project createdProject = await projectManager.CreateProjectAsync(name, description);

            //assert
            Assert.NotNull(createdProject);
        }
        #endregion

        #region GetProjectsAsync Tests
        [Fact]
        public async Task GetProjectsAsync_ReturnsAllProjects()
        {
            //arrange
            List<Project> expectedProjects = new List<Project>()
            {
                new Project()
                {
                    Name = "Project 1",
                    Description = "Project 1 description.",
                    DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                    ProjectId = 1,
                    PublicProjectId = Guid.NewGuid()
                },
                new Project()
                {
                    Name = "Project 2",
                    Description = "Project 2 description.",
                    DateTimeCreated = DateTime.UtcNow,
                    ProjectId = 2,
                    PublicProjectId = Guid.NewGuid()
                }
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectsAsync())
                .ReturnsAsync(expectedProjects);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act
            List<Project> projects = await projectManager.GetProjectsAsync();

            //assert
            Assert.Equal(projects, expectedProjects);
        }
        #endregion

        #region GetProjectAsync Tests
        [Fact]
        public async Task GetProjectAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid projectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await projectManager.GetProjectAsync(projectId);
            });
        }

        [Fact]
        public async Task GetProjectAsync_ValidInput_ReturnsProject()
        {
            //arrange
            Guid projectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = projectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act
            Project project = await projectManager.GetProjectAsync(projectId);

            //assert
            Assert.Equal(project, expectedProject);
        }
        #endregion

        #region GetProjectCountCollectionAsync Tests
        [Fact]
        public async Task GetProjectCountCollectionAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid projectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await projectManager.GetProjectCountCollectionAsync(projectId);
            });
        }

        [Fact]
        public async Task GetProjectCountCollectionAsync_ValidInput_ReturnsProjectCountCollection()
        {
            //arrange
            Guid projectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = projectId
            };
            ProjectCountCollection expectedProjectCountCollection = new ProjectCountCollection()
            {
                ProjectId = projectId.ToString(),
                BugCount = 4,
                EpicCount = 3,
                StoryCount = 7
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.GetProjectCountCollectionAsync(expectedProject.ProjectId))
                .ReturnsAsync(expectedProjectCountCollection);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act
            ProjectCountCollection projectCountCollection = await projectManager.GetProjectCountCollectionAsync(projectId);

            //assert
            Assert.Equal(projectCountCollection, expectedProjectCountCollection);
        }
        #endregion

        #region UpdateProjectAsync Tests
        [Theory]
        [InlineData(null, "A brief description.")]
        [InlineData(null, null)]
        public async Task UpdateProjectAsync_NameNotSet_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid projectId = new Guid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = projectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                 .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.UpdateProjectAsync(projectId, name, description))
                 .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await projectManager.UpdateProjectAsync(projectId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be set.");
        }

        [Theory]
        [MemberData(nameof(GetNameTooLongData))]
        public async Task UpdateProjectAsync_NameTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid projectId = new Guid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = projectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                 .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.UpdateProjectAsync(projectId, name, description))
                 .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await projectManager.UpdateProjectAsync(projectId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 64 characters.");
        }

        [Theory]
        [MemberData(nameof(GetDescriptionTooLongData))]
        public async Task UpdateProjectAsync_DescriptionTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid projectId = new Guid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = projectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                 .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.UpdateProjectAsync(projectId, name, description))
                 .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await projectManager.UpdateProjectAsync(projectId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "description");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 512 characters.");
        }

        [Theory]
        [InlineData("Project Name", "A brief description.")]
        [InlineData("Project Name", null)]
        public async Task UpdateProjectAsync_FicticiousProjectId_ThrowsNotFoundException(string name, string description)
        {
            //arrange
            Guid projectId = new Guid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                 .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.UpdateProjectAsync(projectId, name, description))
                 .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await projectManager.UpdateProjectAsync(projectId, name, description);
            });
        }

        [Theory]
        [InlineData("Project Name", "A brief description.")]
        [InlineData("Project Name", null)]
        public async Task UpdateProjectAsync_ValidInput_ReturnsProject(string name, string description)
        {
            //arrange
            Guid projectId = new Guid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = projectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                 .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.UpdateProjectAsync(projectId, name, description))
                .ReturnsAsync(expectedProject);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act
            Project updatedProject = await projectManager.UpdateProjectAsync(projectId, name, description);

            //assert
            Assert.NotNull(updatedProject);
        }
        #endregion

        #region DeleteProjectAsync Tests
        [Fact]
        public async Task DeleteProjectAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid projectId = new Guid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.DeleteProjectAsync(projectId))
                .Returns(Task.CompletedTask);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await projectManager.DeleteProjectAsync(projectId);
            });
        }

        [Fact]
        public async Task DeleteProjectAsync_ValidProjectId_CallsRepository()
        {
            //arrange
            Guid projectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = projectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            projectRepositoryMock.Setup(x => x.DeleteProjectAsync(projectId))
                .Returns(Task.CompletedTask);
            IProjectManager projectManager = new ProjectManager(projectRepositoryMock.Object);

            //act (no additional asserts required)
            await projectManager.DeleteProjectAsync(projectId);
        }
        #endregion

        #region Helper Methods
        public static IEnumerable<object[]> GetNameTooLongData()
        {
            return new List<object[]>
            {
                new object[] { Strings.GenerateRandomString(65), "A brief description." },
                new object[] { Strings.GenerateRandomString(200), "A brief description." }
            };
        }
        public static IEnumerable<object[]> GetDescriptionTooLongData()
        {
            return new List<object[]>
            {
                new object[] { "Project Name", Strings.GenerateRandomString(513) },
                new object[] { "Project Name", Strings.GenerateRandomString(800) }
            };
        }
        #endregion
    }
}
