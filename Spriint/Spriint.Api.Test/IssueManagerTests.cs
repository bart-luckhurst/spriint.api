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
    public class IssueManagerTests
    {
        #region CreateIssueAsync Tests
        [Fact]
        public async Task CreateIssueAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);
            });
        }

        [Fact]
        public async Task CreateIssueAsync_FicticiousEpicId_ThrowsValidationException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = null;
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "epicId");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be a valid epic ID.");
        }

        [Theory]
        [InlineData("Insect")]
        [InlineData("Feature")]
        [InlineData("Stegosaurus")]
        public async Task CreateIssueAsync_FicticiousIssueType_ThrowsValidationException(string issueType)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                issueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "issueType");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be a valid issue type.");
        }

        [Fact]
        public async Task CreateIssueAsync_NameNotSet_ThrowsValidationException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                null,
                expectedIssue.Description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be set.");
        }

        [Theory]
        [MemberData(nameof(GetNameTooLongData))]
        public async Task CreateIssueAsync_NameTooLong_ThrowsValidationException(string name)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                name,
                expectedIssue.Description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 64 characters.");
        }

        [Theory]
        [MemberData(nameof(GetDescriptionTooLongData))]
        public async Task CreateIssueAsync_DescriptionTooLong_ThrowsValidationException(string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                expectedIssue.Name,
                description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "description");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 512 characters.");
        }

        [Theory]
        [InlineData("ToDone")]
        [InlineData("Running")]
        [InlineData("Completed")]
        public async Task CreateIssueAsync_FicticiousStatus_ThrowsValidationException(string status)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                expectedIssue.Name,
                expectedIssue.Description,
                status,
                expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "status");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be a valid status.");
        }

        [Fact]
        public async Task CreateIssueAsync_ValidInput_ReturnsIssue()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.CreateIssueAsync(expectedIssue.ProjectId, 
                expectedIssue.EpicId, 
                expectedIssue.IssueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status,
                expectedIssue.Estimate))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act
            Issue createdIssue = await issueManager.CreateIssueAsync(publicProjectId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);

            //assert
            Assert.Equal(expectedIssue, createdIssue);
        }
        #endregion

        #region GetIssuesAsync Tests
        [Fact]
        public async Task GetIssuesAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            List<Issue> expectedIssues = new List<Issue>()
            {
                new Issue()
                {
                    IssueId = 1,
                    PublicIssueId = Guid.NewGuid(),
                    ProjectId = 1,
                    Project = expectedProject,
                    EpicId = 1,
                    Name = "name",
                    Description = null,
                    Status = Status.ToDo,
                    Estimate = 13,
                    DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
                },
                new Issue()
                {
                    IssueId = 2,
                    PublicIssueId = Guid.NewGuid(),
                    ProjectId = 1,
                    Project = expectedProject,
                    EpicId = 1,
                    Name = "name",
                    Description = null,
                    Status = Status.Complete,
                    Estimate = 8,
                    DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
                },
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssuesAsync(1))
                .ReturnsAsync(expectedIssues);

            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await issueManager.GetIssuesAsync(publicProjectId);
            });
        }

        [Fact]
        public async Task GetIssuesAsync_ValidInput_NoExceptionsThrown()
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

            List<Issue> expectedIssues = new List<Issue>()
            {
                new Issue()
                {
                    IssueId = 1,
                    PublicIssueId = Guid.NewGuid(),
                    ProjectId = 1,
                    Project = expectedProject,
                    EpicId = 1,
                    Name = "name",
                    Description = null,
                    Status = Status.ToDo,
                    Estimate = 13,
                    DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
                },
                new Issue()
                {
                    IssueId = 2,
                    PublicIssueId = Guid.NewGuid(),
                    ProjectId = 1,
                    Project = expectedProject,
                    EpicId = 1,
                    Name = "name",
                    Description = null,
                    Status = Status.Complete,
                    Estimate = 8,
                    DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
                },
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssuesAsync(expectedProject.ProjectId))
                .ReturnsAsync(expectedIssues);

            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act
            List<Issue> returnedIssues = await issueManager.GetIssuesAsync(publicProjectId);

            //assert
            Assert.Equal(expectedIssues, returnedIssues);
        }
        #endregion

        #region GetIssueAsyncTests
        [Fact]
        public async Task GetIssueAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);

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
            epicRepositoryMock.Setup(x => x.GetEpicAsync(1))
                .ReturnsAsync(expectedEpic);

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await issueManager.GetIssueAsync(publicProjectId, publicIssueId);
            });
        }

        [Fact]
        public async Task GetIssueAsync_FicticiousIssueId_ThrowsNotFoundException()
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

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = null;
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);

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
            epicRepositoryMock.Setup(x => x.GetEpicAsync(1))
                .ReturnsAsync(expectedEpic);

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await issueManager.GetIssueAsync(publicProjectId, publicIssueId);
            });
        }

        [Fact]
        public async Task GetIssueAsync_ValidInput_NoExceptionsThrown()
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

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);

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
            epicRepositoryMock.Setup(x => x.GetEpicAsync(1))
                .ReturnsAsync(expectedEpic);

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act
            Issue returnedIssue = await issueManager.GetIssueAsync(publicProjectId, publicIssueId);

            //assert
            Assert.Equal(expectedIssue, returnedIssue);
        }
        #endregion

        #region UpdateIssueAsync Tests
        [Fact]
        public async Task UpdateIssueAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    expectedIssue.IssueType.ToString(),
                    expectedIssue.Name,
                    expectedIssue.Description,
                    expectedIssue.Status.ToString(),
                    expectedIssue.Estimate);
            });
        }

        [Fact]
        public async Task UpdateIssueAsync_FicticiousIssueId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync((Issue)null);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    expectedIssue.IssueType.ToString(),
                    expectedIssue.Name,
                    expectedIssue.Description,
                    expectedIssue.Status.ToString(),
                    expectedIssue.Estimate);
            });
        }

        [Fact]
        public async Task UpdateIssueAsync_FicticiousEpicId_ThrowsValidationException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);

            Guid publicEpicId = Guid.NewGuid();
            Epic expectedEpic = null;
            var epicRepositoryMock = new Mock<IEpicRepository>(MockBehavior.Strict);
            epicRepositoryMock.Setup(x => x.GetEpicAsync(publicEpicId))
                .ReturnsAsync(expectedEpic);

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    expectedIssue.IssueType.ToString(),
                    expectedIssue.Name,
                    expectedIssue.Description,
                    expectedIssue.Status.ToString(),
                    expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "epicId");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be a valid epic ID.");
        }

        [Theory]
        [InlineData("Insect")]
        [InlineData("Feature")]
        [InlineData("Stegosaurus")]
        public async Task UpdateIssueAsync_FicticiousIssueType_ThrowsValidationException(string issueType)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueAsync(expectedIssue.PublicIssueId,
                expectedIssue.EpicId,
                expectedIssue.IssueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status,
                expectedIssue.Estimate))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    issueType,
                    expectedIssue.Name,
                    expectedIssue.Description,
                    expectedIssue.Status.ToString(),
                    expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "issueType");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be a valid issue type.");
        }

        [Fact]
        public async Task UpdateIssueAsync_NameNotSet_ThrowsValidationException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueAsync(expectedIssue.PublicIssueId,
                expectedIssue.EpicId,
                expectedIssue.IssueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status,
                expectedIssue.Estimate))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    expectedIssue.IssueType.ToString(),
                    null,
                    expectedIssue.Description,
                    expectedIssue.Status.ToString(),
                    expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be set.");
        }

        [Theory]
        [MemberData(nameof(GetNameTooLongData))]
        public async Task UpdateIssueAsync_NameTooLong_ThrowsValidationException(string name)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueAsync(expectedIssue.PublicIssueId,
                expectedIssue.EpicId,
                expectedIssue.IssueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status,
                expectedIssue.Estimate))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    expectedIssue.IssueType.ToString(),
                    name,
                    expectedIssue.Description,
                    expectedIssue.Status.ToString(),
                    expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "name");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 64 characters.");
        }

        [Theory]
        [MemberData(nameof(GetDescriptionTooLongData))]
        public async Task UpdateIssueAsync_DescriptionTooLong_ThrowsValidationException(string description)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueAsync(expectedIssue.PublicIssueId,
                expectedIssue.EpicId,
                expectedIssue.IssueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status,
                expectedIssue.Estimate))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    expectedIssue.IssueType.ToString(),
                    expectedIssue.Name,
                    description,
                    expectedIssue.Status.ToString(),
                    expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "description");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be shorter than 512 characters.");
        }

        [Theory]
        [InlineData("ToDone")]
        [InlineData("Running")]
        [InlineData("Completed")]
        public async Task UpdateIssueAsync_FicticiousStatus_ThrowsValidationException(string status)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueAsync(expectedIssue.PublicIssueId,
                expectedIssue.EpicId,
                expectedIssue.IssueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status,
                expectedIssue.Estimate))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.UpdateIssueAsync(publicProjectId,
                    publicIssueId,
                    publicEpicId,
                    expectedIssue.IssueType.ToString(),
                    expectedIssue.Name,
                    expectedIssue.Description,
                    status,
                    expectedIssue.Estimate);
            });

            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "status");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be a valid status.");
        }

        [Fact]
        public async Task UpdateIssueAsync_ValidInput_ReturnsIssue()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = new Project()
            {
                Name = "Project 1",
                Description = "Project 1 description.",
                DateTimeCreated = DateTime.UtcNow.AddDays(-1),
                ProjectId = 1,
                PublicProjectId = publicProjectId
            };
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueAsync(expectedIssue.PublicIssueId,
                expectedIssue.EpicId,
                expectedIssue.IssueType,
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status,
                expectedIssue.Estimate))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act
            Issue updatedIssue = await issueManager.UpdateIssueAsync(publicProjectId,
                publicIssueId,
                publicEpicId,
                expectedIssue.IssueType.ToString(),
                expectedIssue.Name,
                expectedIssue.Description,
                expectedIssue.Status.ToString(),
                expectedIssue.Estimate);

            //assert
            Assert.Equal(expectedIssue, updatedIssue);
        }
        #endregion

        #region UpdateIssueStatusAsync Tests
        [Theory]
        [InlineData("ToDo")]
        [InlineData("InProgress")]
        [InlineData("InTest")]
        [InlineData("Complete")]
        public async Task UpdateIssueStatusAsync_FicticiousProjectId_ThrowsNotFoundException(string status)
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            //Not a fan of Enum.Parse here, but Enum inline data crashes test explorer
            issueRepositoryMock.Setup(x => x.UpdateIssueStatusAsync(publicIssueId, Enum.Parse<Status>(status)))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await issueManager.UpdateIssueStatusAsync(publicProjectId, publicIssueId, expectedIssue.Status.ToString());
            });
        }

        [Theory]
        [InlineData("ToDo")]
        [InlineData("InProgress")]
        [InlineData("InTest")]
        [InlineData("Complete")]
        public async Task UpdateIssueStatusAsync_FicticiousIssueId_ThrowsNotFoundException(string status)
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

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = null;
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueStatusAsync(publicIssueId, Enum.Parse<Status>(status)))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await issueManager.UpdateIssueStatusAsync(publicProjectId, publicIssueId, Status.ToDo.ToString());
            });
        }

        [Theory]
        [InlineData("ToDone")]
        [InlineData("Running")]
        [InlineData("Completed")]
        [InlineData("khjfdjg")]
        public async Task UpdateIssueStatusAsync_FicticiousStatus_ThrowsValidationException(string ficticiousStatus)
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

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                _ = await issueManager.UpdateIssueStatusAsync(publicProjectId, publicIssueId, ficticiousStatus);
            });


            //assert
            Assert.True(exception.ValidationErrors.Count == 1);
            Assert.True(exception.ValidationErrors.Single().Property == "status");
            Assert.True(exception.ValidationErrors.Single().ErrorMessage == "Must be a valid status.");
        }

        [Theory]
        [InlineData("ToDo")]
        [InlineData("InProgress")]
        [InlineData("InTest")]
        [InlineData("Complete")]
        public async Task UpdateIssueStatusAsync_ValidInput_NoExceptionsThrown(string status)
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

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Enum.Parse<Status>(status),
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.UpdateIssueStatusAsync(publicIssueId, Enum.Parse<Status>(status)))
                .ReturnsAsync(expectedIssue);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act
            Issue updatedIssue = await issueManager.UpdateIssueStatusAsync(publicProjectId, publicIssueId, expectedIssue.Status.ToString());

            //assert
            Assert.Equal(expectedIssue, updatedIssue);
        }
        #endregion

        #region DeleteIssueAsync Tests
        [Fact]
        public async Task DeleteIssueAsync_FicticiousProjectId_ThrowsNotFoundException()
        {
            //arrange
            Guid publicProjectId = Guid.NewGuid();
            Project expectedProject = null;
            var projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);
            projectRepositoryMock.Setup(x => x.GetProjectAsync(publicProjectId))
                .ReturnsAsync(expectedProject);

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.DeleteIssueAsync(publicIssueId))
                .Returns(Task.CompletedTask);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await issueManager.DeleteIssueAsync(publicProjectId, publicIssueId);
            });
        }

        [Fact]
        public async Task DeleteIssueAsync_FicticiousIssueId_ThrowsNotFoundException()
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

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = null;
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.DeleteIssueAsync(publicIssueId))
                .Returns(Task.CompletedTask);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act/assert
            _ = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await issueManager.DeleteIssueAsync(publicProjectId, publicIssueId);
            });
        }

        [Fact]
        public async Task DeleteIssueAsync_ValidInput_NoExceptionsThrown()
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

            Guid publicIssueId = Guid.NewGuid();
            Issue expectedIssue = new Issue()
            {
                IssueId = 1,
                PublicIssueId = publicIssueId,
                ProjectId = 1,
                Project = expectedProject,
                EpicId = 1,
                Name = "name",
                Description = null,
                Status = Status.ToDo,
                Estimate = 13,
                DateTimeCreated = DateTime.UtcNow.AddMinutes(-60)
            };
            var issueRepositoryMock = new Mock<IIssueRepository>(MockBehavior.Strict);
            issueRepositoryMock.Setup(x => x.GetIssueAsync(publicIssueId))
                .ReturnsAsync(expectedIssue);
            issueRepositoryMock.Setup(x => x.DeleteIssueAsync(publicIssueId))
                .Returns(Task.CompletedTask);

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

            var issueManager = new IssueManager(issueRepositoryMock.Object,
                projectRepositoryMock.Object,
                epicRepositoryMock.Object);

            //act
            await issueManager.DeleteIssueAsync(publicProjectId, publicIssueId);

            //assert - implicit
        }
        #endregion

        #region Helper Methods
        public static IEnumerable<object[]> GetNameTooLongData()
        {
            return new List<object[]>
            {
                new object[] { Strings.GenerateRandomString(65) },
                new object[] { Strings.GenerateRandomString(200) }
            };
        }
        public static IEnumerable<object[]> GetDescriptionTooLongData()
        {
            return new List<object[]>
            {
                new object[] { Strings.GenerateRandomString(513) },
                new object[] { Strings.GenerateRandomString(800) }
            };
        }
        #endregion
    }
}
