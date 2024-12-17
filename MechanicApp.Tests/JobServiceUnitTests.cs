using System;
using AutoFixture;
using MechanicApp.Shared;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;

namespace MechanicApp.Tests
{
    public class JobServiceUnitTests
    {
        private Mock<ILogger<JobService>> _loggerServiceMock;
        private Mock<AppContext> _appContextMock;
        private Fixture _fixture;
        private JobService _service;

        private List<Job> jobList = new List<Job>()
        {
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = Guid.NewGuid(), NumberPlate = "ABC-123",
                YearOfProduction = DateTime.Now, Category = Job.JobCategory.Engine, Description = "Engine repair",
                Seriousness = 4, Status = Job.JobStatus.Assigned
            },
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = Guid.NewGuid(), NumberPlate = "DEF-456",
                YearOfProduction = DateTime.Today, Category = Job.JobCategory.Brakes, Description = "Brakes repair",
                Seriousness = 2, Status = Job.JobStatus.InProgress
            },
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = Guid.NewGuid(), NumberPlate = "GHI-789",
                YearOfProduction = DateTime.Today, Category = Job.JobCategory.Bodywork, Description = "Bodywork repair",
                Seriousness = 3, Status = Job.JobStatus.Completed
            }
        };


        public JobServiceUnitTests()
        {
            _loggerServiceMock = new Mock<ILogger<JobService>>();
            _appContextMock = new Mock<AppContext>();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExistingJob_GetJob_GotJob()
        {
            var job = jobList.First();

            _appContextMock.Setup(x => x.Jobs.FindAsync(job.JobId)).ReturnsAsync(job);

            _service = new JobService(_loggerServiceMock.Object, _appContextMock.Object);

            var result = await _service.Get(job.JobId);

            Assert.Equal(job, result);
        }

        [Fact]
        public async Task NonExistingJob_AddJob_AddedJob()
        {
            var job = jobList.First();

            _appContextMock.Setup(x => x.Jobs.Add(job)).Returns(It.IsAny<EntityEntry<Job>>());
            _appContextMock.Setup(x => x.SaveChanges()).Returns(1);
            _appContextMock.Setup(x => x.Jobs.Find(job.JobId)).Returns(job);

            _service = new JobService(_loggerServiceMock.Object, _appContextMock.Object);

            await _service.Add(job);

            Assert.Equal(job, _appContextMock.Object.Jobs.Find(job.JobId));
        }

        [Fact]
        public async Task ExistingJob_UpdateJob_UpdatedJob()
        {
            var job = jobList.First();
            var newJob = new Job() {
                JobId = job.JobId,
                ClientId = Guid.NewGuid(),
                NumberPlate = "ABC-123",
                YearOfProduction = DateTime.Now,
                Category = Job.JobCategory.Engine,
                Description = "Engine repair",
                Seriousness = 4,
                Status = Job.JobStatus.Assigned
            };

            _appContextMock.Setup(x => x.Jobs.Find(newJob.JobId)).Returns(job);
            _appContextMock.Setup(x => x.SaveChanges()).Returns(1);
            _service = new JobService(_loggerServiceMock.Object, _appContextMock.Object);
            _appContextMock.Setup(x => x.Jobs.FindAsync(newJob.JobId)).ReturnsAsync(job);
            _appContextMock.Setup(x => x.Jobs.Update(newJob)).Returns(It.IsAny<EntityEntry<Job>>());

            await _service.Update(newJob);

            Assert.Equal(newJob.JobId, _appContextMock.Object.Jobs.Find(job.JobId).JobId);
            Assert.Equal(newJob.ClientId, _appContextMock.Object.Jobs.Find(job.JobId).ClientId);
            Assert.Equal(newJob.NumberPlate, _appContextMock.Object.Jobs.Find(job.JobId).NumberPlate);
            Assert.Equal(newJob.YearOfProduction, _appContextMock.Object.Jobs.Find(job.JobId).YearOfProduction);
            Assert.Equal(newJob.Category, _appContextMock.Object.Jobs.Find(job.JobId).Category);
            Assert.Equal(newJob.Description, _appContextMock.Object.Jobs.Find(job.JobId).Description);
            Assert.Equal(newJob.Seriousness, _appContextMock.Object.Jobs.Find(job.JobId).Seriousness);
            Assert.Equal(newJob.Status, _appContextMock.Object.Jobs.Find(job.JobId).Status);
        }

        [Fact]
        public async Task ExistingJob_DeleteJob_DeletedJob()
        {
            var job = jobList.First();

            _appContextMock.Setup(x => x.Jobs.FindAsync(job.JobId)).ReturnsAsync(job);
            _appContextMock.Setup(x => x.Jobs.Remove(job)).Returns(It.IsAny<EntityEntry<Job>>());
            _appContextMock.Setup(x => x.SaveChanges()).Returns(1);

            _service = new JobService(_loggerServiceMock.Object, _appContextMock.Object);

            await _service.Delete(job.JobId);

            Assert.Null(_appContextMock.Object.Jobs.Find(job.JobId));
        }
    }
}