using Moq;
using Spriint.Api.Core;
using Spriint.Api.Exceptions;
using Spriint.Api.Managers;
using Spriint.Api.Repositories;
using Spriint.Api.Test.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Spriint.Api.Test
{
    public class EpicManagerTests
    {
        #region CreateEpicAsync Tests
        [Theory]
        [InlineData(null, "A brief description.")]
        [InlineData(null, null)]
        public async Task CreateEpicAsync_NameNotSet_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            IEpicManager epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await epicManager.CreateEpicAsync(publicProjectId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be set.");
        }

        [Theory]
        [MemberData(nameof(GetNameTooLongData))]
        public async Task CreateEpicAsync_NameTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            IEpicManager epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await epicManager.CreateEpicAsync(publicProjectId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 64 characters.");
        }

        [Theory]
        [MemberData(nameof(GetDescriptionTooLongData))]
        public async Task CreateEpicAsync_DescriptionTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            IEpicManager epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await epicManager.CreateEpicAsync(publicProjectId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "description");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 512 characters.");
        }

        [Theory]
        [InlineData("Project Name", "A brief description.")]
        [InlineData("Project Name", null)]
        public async Task CreateEpicAsync_FicticiousProjectId_ThrowsNotFoundException(string name, string description)
        {
            Guid projectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            IEpicManager epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await epicManager.CreateEpicAsync(projectId, name, description);
            });
        }

        [Theory]
        [InlineData("Project Name", "A brief description.")]
        [InlineData("Project Name", null)]
        public async Task CreateEpicAsync_ValidInput_ReturnsEpic(string name, string description)
        {
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);
            Guid expectedEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = expectedEpicId,
                ProjectId = expectedProject.ProjectId,
                Name = name,
                Description = description,
                DateTimeCreated = DateTime.UtcNow
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.CreateEpicAsync(expectedProject.ProjectId, name, description))
                .ReturnsAsync(expectedEpic);
            IEpicManager epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act
            Epic createdEpic = await epicManager.CreateEpicAsync(publicProjectId, name, description);

            //assert
            Assert.NotNull(createdEpic);
        }
        #endregion

        #region GetEpicsAsync Tests
        [Fact]
        public async Task GetEpicsAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            Guid projectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(projectId))
                .ReturnsAsync(expectedProject);
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            IEpicManager epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await epicManager.GetEpicsAsync(projectId);
            });
        }

        [Fact]
        public async Task GetEpicsAsync_ValidProjectId_ReturnsEpics()
        {
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);
            List<Epic> expectedEpics = new List<Epic>()
            {
                new Epic()
                {
                    EpicId = 1,
                    PublicEpicId = Guid.NewGuid(),
                    ProjectId = expectedProject.ProjectId,
                    Name = "Epic 1",
                    Description = null,
                    DateTimeCreated = DateTime.UtcNow
                },
                new Epic()
                {
                    EpicId = 2,
                    PublicEpicId = Guid.NewGuid(),
                    ProjectId = expectedProject.ProjectId,
                    Name = "Epic 2",
                    Description = null,
                    DateTimeCreated = DateTime.UtcNow
                },
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            IEpicManager epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);
            epicRepositoryMock.Setup(x => x.GetEpicsAsync(expectedProject.ProjectId))
                .ReturnsAsync(expectedEpics);

            //act
            List<Epic> epics = await epicManager.GetEpicsAsync(publicProjectId);

            //assert
            Assert.Equal(expectedEpics, epics);
        }
        #endregion

        #region GetEpicAsync Tests
        [Fact]
        public async Task GetEpicAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = 1,
                Project = expectedProject,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await epicManager.GetEpicAsync(publicProjectId, publicEpicId);
            });
        }

        [Fact]
        public async Task GetEpicAsync_FicticiousEpicId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = null;
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await epicManager.GetEpicAsync(publicProjectId, publicEpicId);
            });
        }

        [Fact]
        public async Task GetEpicAsync_ValidInput_ReturnsEpic()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = expectedProject.ProjectId,
                Project = expectedProject,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act
            Epic returnedEpic = await epicManager.GetEpicAsync(publicProjectId, publicEpicId);

            //assert
            Assert.Equal(expectedEpic, returnedEpic);
        }
        #endregion

        #region UpdateEpicAsync Tests
        [Fact]
        public async Task UpdateEpicAsync_FiciticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = 1,
                Project = expectedProject,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.UpdateEpicAsync(publicEpicId, 
                expectedEpic.Name, 
                expectedEpic.Description))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await epicManager.UpdateEpicAsync(publicProjectId, 
                    publicEpicId, 
                    expectedEpic.Name, 
                    expectedEpic.Description);
            });
        }

        [Fact]
        public async Task UpdateEpicAsync_FicticiousEpicId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = null;
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.UpdateEpicAsync(publicEpicId,
                "name",
                "description"))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await epicManager.UpdateEpicAsync(publicProjectId,
                    publicEpicId,
                    "name",
                    "description");
            });
        }

        [Theory]
        [InlineData(null, "A brief description.")]
        [InlineData(null, null)]
        public async Task UpdateEpicAsync_NameNotSet_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = 1,
                Project = expectedProject,
                Name = name,
                Description = description,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.UpdateEpicAsync(publicEpicId,
                expectedEpic.Name,
                expectedEpic.Description))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await epicManager.UpdateEpicAsync(publicProjectId, publicEpicId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be set.");
        }

        [Theory]
        [MemberData(nameof(GetNameTooLongData))]
        public async Task UpdateEpicAsync_NameTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = 1,
                Project = expectedProject,
                Name = name,
                Description = description,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.UpdateEpicAsync(publicEpicId,
                expectedEpic.Name,
                expectedEpic.Description))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await epicManager.UpdateEpicAsync(publicProjectId, publicEpicId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 64 characters.");
        }

        [Theory]
        [MemberData(nameof(GetDescriptionTooLongData))]
        public async Task UpdateEpicAsync_DescriptionTooLong_ThrowsValidationException(string name, string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = 1,
                Project = expectedProject,
                Name = name,
                Description = description,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.UpdateEpicAsync(publicEpicId,
                expectedEpic.Name,
                expectedEpic.Description))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await epicManager.UpdateEpicAsync(publicProjectId, publicEpicId, name, description);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "description");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 512 characters.");
        }

        [Theory]
        [InlineData("Project Name", "A brief description.")]
        [InlineData("Project Name", null)]
        public async Task UpdateEpicAsync_ValidInput_ReturnsEpic(string name, string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = 1,
                Project = expectedProject,
                Name = name,
                Description = description,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.UpdateEpicAsync(publicEpicId,
                expectedEpic.Name,
                expectedEpic.Description))
                .ReturnsAsync(expectedEpic);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act
            Epic createdEpic = await epicManager.UpdateEpicAsync(publicProjectId, 
                publicEpicId, 
                name, 
                description);

            //assert
            Assert.Equal(expectedEpic, createdEpic);
        }
        #endregion

        #region DeleteEpicAsync Tests
        [Fact]
        public async Task DeleteEpicAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = 1,
                Project = expectedProject,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.DeleteEpicAsync(publicEpicId))
                .Returns(Task.CompletedTask);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await epicManager.DeleteEpicAsync(publicProjectId, publicEpicId);
            });
        }

        [Fact]
        public async Task DeleteEpicAsync_FicticiousEpicId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = null;
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.DeleteEpicAsync(publicEpicId))
                .Returns(Task.CompletedTask);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await epicManager.DeleteEpicAsync(publicProjectId, publicEpicId);
            });
        }

        [Fact]
        public async Task DeleteEpicAsync_ValidInput_NoExceptionsThrown()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                ProjectId = 1,
                PublicProjectId = publicProjectId,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = new Epic()
            {
                EpicId = 1,
                PublicEpicId = publicEpicId,
                ProjectId = expectedProject.ProjectId,
                Project = expectedProject,
                Name = "name",
                Description = null,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);
            epicRepositoryMock.Setup(x => x.DeleteEpicAsync(publicEpicId))
                .Returns(Task.CompletedTask);

            var epicManager = new EpicManager(projectRepositoryMock.Object, epicRepositoryMock.Object);

            //act
            await epicManager.DeleteEpicAsync(publicProjectId, publicEpicId);

            //assert - implicit
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
