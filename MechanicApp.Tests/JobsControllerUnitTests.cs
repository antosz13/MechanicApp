using AutoFixture;
using MechanicApp.Controllers;
using MechanicApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MechanicApp.Tests;

public class JobsControllerUnitTests
{
    private Mock<IJobService> _jobServiceMock;
    private Fixture _fixture;
    private JobsController _controller;

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

    public JobsControllerUnitTests()
    {
        _jobServiceMock = new Mock<IJobService>();
        _fixture = new Fixture();
        _fixture
            .Customizations.Add(new DateTimeRangeSpecimenBuilder());
    }

    [Fact]
    public async Task ExistingJobs_GetAllJobs_GotAllJobs()
    {
        var joblist = jobList;

        _jobServiceMock.Setup(x => x.GetAll()).ReturnsAsync(joblist);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task ExistingJob_GetJob_GotJob()
    {
        var job = jobList.First();

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync(job);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Get(job.JobId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task MissingJob_GetJob_ErrorMissingJob()
    {
        Job job = new Job();

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync((Job)null);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Get(job.JobId);

        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
    }


    [Fact]
    public async Task NonExistingJob_AddJob_AddedJob()
    {
        var job = jobList.First();

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync((Job)null);
        _jobServiceMock.Setup(x => x.Add(job)).Returns(Task.CompletedTask);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Add(job);

        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ExistingJob_AddJob_ErrorExistingJob()
    {
        var job = jobList.First();

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync(job);
        _jobServiceMock.Setup(x => x.Add(job)).Returns(Task.CompletedTask);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Add(job);

        var conflictResult = Assert.IsType<ConflictResult>(result);
    }

    [Fact]
    public async Task ExistingJob_DeleteJob_DeletedJob()
    {
        var job = jobList.First();

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync(job);
        _jobServiceMock.Setup(x => x.Delete(job.JobId)).Returns(Task.CompletedTask);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Delete(job.JobId);

        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task MissingJob_DeleteJob_ErrorMissingJob()
    {
        var job = jobList.First();

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync((Job)null);
        _jobServiceMock.Setup(x => x.Delete(job.JobId)).Returns(Task.CompletedTask);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Delete(job.JobId);

        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task SameJobs_UpdateJob_JobUpdated()
    {
        var job = jobList.First();
        var newJob = new Job()  
            { JobId = job.JobId, ClientId = job.ClientId, NumberPlate = "ABC-123", YearOfProduction = DateTime.Now, Category = Job.JobCategory.Engine, Description = "Engine repair", Seriousness = 4, Status = Job.JobStatus.Assigned };

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync(job);
        _jobServiceMock.Setup(x => x.Get(newJob.JobId)).ReturnsAsync(newJob);
        _jobServiceMock.Setup(x => x.Update(newJob)).Returns(Task.CompletedTask);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Update(job.JobId, newJob);

        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DifferentJobs_UpdateJob_BadRequestError()
    {
        var job = jobList.First();
        var newJob = jobList.Last();

        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync(job);
        _jobServiceMock.Setup(x => x.Get(newJob.JobId)).ReturnsAsync(newJob);
        _jobServiceMock.Setup(x => x.Update(newJob)).Returns(Task.CompletedTask);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Update(job.JobId, newJob);

        var okResult = Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task NonExistingJob_UpdateJob_NotFoundError()
    {
        var job = jobList.First();
        var newJob = new Job()
            { JobId = job.JobId, ClientId = job.ClientId, NumberPlate = "ABC-123", YearOfProduction = DateTime.Now, Category = Job.JobCategory.Engine, Description = "Engine repair", Seriousness = 4, Status = Job.JobStatus.Assigned };


        _jobServiceMock.Setup(x => x.Get(job.JobId)).ReturnsAsync((Job)null);
        _jobServiceMock.Setup(x => x.Get(newJob.JobId)).ReturnsAsync((Job)null);
        _jobServiceMock.Setup(x => x.Update(newJob)).Returns(Task.CompletedTask);

        _controller = new JobsController(_jobServiceMock.Object);

        var result = await _controller.Update(job.JobId, newJob);

        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }
}