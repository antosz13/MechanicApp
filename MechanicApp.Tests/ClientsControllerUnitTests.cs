using AutoFixture;
using MechanicApp.Controllers;
using MechanicApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MechanicApp.Tests;

public class ClientsControllerUnitTests
{
    private Mock<IClientService> _clientServiceMock;
    private Fixture _fixture;
    private ClientsController _controller;


    public ClientsControllerUnitTests()
    {
        _clientServiceMock = new Mock<IClientService>();
        _fixture = new Fixture();
        _fixture
            .Customizations.Add(new DateTimeRangeSpecimenBuilder());
    }

    [Fact]
    public async Task ExistingClients_GetAllClients_GotAllClients()
    {
       var clientList = _fixture.CreateMany<Client>(3).ToList();

       _clientServiceMock.Setup(x => x.GetAll()).ReturnsAsync(clientList);

       _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task ExistingClient_GetClient_GotClient()
    {
        var client = _fixture.Create<Client>();

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync(client);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Get(client.ClientId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task MisingClient_GetClient_ErrorMissingClient()
    {
        Client client = new Client();

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync((Client)null);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Get(client.ClientId);

        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
    }


    [Fact]
    public async Task NonExistingClient_AddClient_AddedClient()
    {
        var client = _fixture.Create<Client>();

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync((Client)null);
        _clientServiceMock.Setup(x => x.Add(client)).Returns(Task.CompletedTask);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Add(client);

        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ExistingClient_AddClient_ErrorExistingClient()
    {
        var client = _fixture.Create<Client>();

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync(client);
        _clientServiceMock.Setup(x => x.Add(client)).Returns(Task.CompletedTask);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Add(client);

        var conflictResult = Assert.IsType<ConflictResult>(result);
    }

    [Fact]
    public async Task ExistingClient_DeleteClient_DeletedClient()
    {
        var client = _fixture.Create<Client>();

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync(client);
        _clientServiceMock.Setup(x => x.Delete(client.ClientId)).Returns(Task.CompletedTask);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Delete(client.ClientId);

        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task MissingClient_DeleteClient_ErrorMissingClient()
    {
        var client = _fixture.Create<Client>();

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync((Client)null);
        _clientServiceMock.Setup(x => x.Delete(client.ClientId)).Returns(Task.CompletedTask);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Delete(client.ClientId);

        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task SameClient_UpdateClient_ClientUpdated()
    {
        var client = _fixture.Create<Client>();
        var newClient = new Client()
            { ClientId = client.ClientId, Address = client.Address, Name = "NewName", Email = client.Email };

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync(client);
        _clientServiceMock.Setup(x => x.Get(newClient.ClientId)).ReturnsAsync(newClient);
        _clientServiceMock.Setup(x => x.Update(newClient)).Returns(Task.CompletedTask);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Update(client.ClientId, newClient);

        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DifferentClient_UpdateClient_BadRequestError()
    {
        var client = _fixture.Create<Client>();

        var newClient = _fixture.Create<Client>();

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync(client);
        _clientServiceMock.Setup(x => x.Get(newClient.ClientId)).ReturnsAsync(newClient);
        _clientServiceMock.Setup(x => x.Update(newClient)).Returns(Task.CompletedTask);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Update(client.ClientId, newClient);

        var okResult = Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task NonExistingClient_UpdateClient_NotFoundError()
    {
        var client = _fixture.Create<Client>();

        var newClient = new Client()
            { ClientId = client.ClientId, Address = client.Address, Name = "NewName", Email = client.Email };

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync((Client)null);
        _clientServiceMock.Setup(x => x.Get(newClient.ClientId)).ReturnsAsync((Client)null);
        _clientServiceMock.Setup(x => x.Update(newClient)).Returns(Task.CompletedTask);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.Update(client.ClientId, newClient);

        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task ExistingClient_GetJobsOfClient_ClientJobsReturned()
    {
        var client = _fixture.Create<Client>();

        var fixture = new Fixture();
        var jobList = new List<Job>()
        {
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = client.ClientId, NumberPlate = "ABC-123",
                YearOfProduction = DateTime.Now, Category = Job.JobCategory.Engine, Description = "Engine repair",
                Seriousness = 4, Status = Job.JobStatus.Assigned
            },
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = client.ClientId, NumberPlate = "DEF-456",
                YearOfProduction = DateTime.Today, Category = Job.JobCategory.Brakes, Description = "Brakes repair",
                Seriousness = 2, Status = Job.JobStatus.InProgress
            },
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = client.ClientId, NumberPlate = "GHI-789",
                YearOfProduction = DateTime.Today, Category = Job.JobCategory.Bodywork, Description = "Bodywork repair",
                Seriousness = 3, Status = Job.JobStatus.Completed
            }
        };

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync(client);
        _clientServiceMock.Setup(x => x.GetJobsOfClient(client.ClientId)).ReturnsAsync(jobList);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.GetJobsOfClient(client.ClientId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task NonExistingClient_GetJobsOfClient_ClientNotFoundError()
    {
        var client = _fixture.Create<Client>();

        var jobList = new List<Job>()
        {
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = client.ClientId, NumberPlate = "ABC-123",
                YearOfProduction = DateTime.Now, Category = Job.JobCategory.Engine, Description = "Engine repair",
                Seriousness = 4, Status = Job.JobStatus.Assigned
            },
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = client.ClientId, NumberPlate = "DEF-456",
                YearOfProduction = DateTime.Today, Category = Job.JobCategory.Brakes, Description = "Brakes repair",
                Seriousness = 2, Status = Job.JobStatus.InProgress
            },
            new Job()
            {
                JobId = Guid.NewGuid(), ClientId = client.ClientId, NumberPlate = "GHI-789",
                YearOfProduction = DateTime.Today, Category = Job.JobCategory.Bodywork, Description = "Bodywork repair",
                Seriousness = 3, Status = Job.JobStatus.Completed
            }
        };

        _clientServiceMock.Setup(x => x.Get(client.ClientId)).ReturnsAsync((Client)null);
        _clientServiceMock.Setup(x => x.GetJobsOfClient(client.ClientId)).ReturnsAsync(jobList);

        _controller = new ClientsController(_clientServiceMock.Object);

        var result = await _controller.GetJobsOfClient(client.ClientId);

        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
    }
}