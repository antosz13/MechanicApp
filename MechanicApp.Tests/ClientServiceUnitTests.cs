
using AutoFixture;
using MechanicApp.Shared;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;

namespace MechanicApp.Tests
{
    public class ClientServiceUnitTests
    {
        private Mock<ILogger<ClientService>> _loggerServiceMock;
        private Mock<AppContext> _appContextMock;
        private Fixture _fixture;
        private ClientService _service;

        public ClientServiceUnitTests()
        {
            _loggerServiceMock = new Mock<ILogger<ClientService>>();
            _appContextMock = new Mock<AppContext>();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExistingClient_GetClient_GotClient()
        {
            var client = _fixture.Create<Client>();

            _appContextMock.Setup(x => x.Clients.FindAsync(client.ClientId)).ReturnsAsync(client);

            _service = new ClientService(_loggerServiceMock.Object, _appContextMock.Object);

            var result = await _service.Get(client.ClientId);

            Assert.Equal(client, result);
        }

        [Fact]
        public async Task NonExistingClient_AddClient_AddedClient()
        {
            var client = _fixture.Create<Client>();

            _appContextMock.Setup(x => x.Clients.Add(client)).Returns(It.IsAny<EntityEntry<Client>>());
            _appContextMock.Setup(x => x.SaveChanges()).Returns(1);
            _appContextMock.Setup(x => x.Clients.Find(client.ClientId)).Returns(client);

            _service = new ClientService(_loggerServiceMock.Object, _appContextMock.Object);
            
            await _service.Add(client);

            Assert.Equal(client, _appContextMock.Object.Clients.Find(client.ClientId));
        }
        
        [Fact]
        public async Task ExistingClient_UpdateClient_UpdatedClient()
        {
            var client = _fixture.Create<Client>();
            var newClient = new Client() { ClientId = client.ClientId, Name = "NewName", Address = "newAddress", Email = "newemail@neweamil.com"};
            
            _appContextMock.Setup(x => x.Clients.Find(newClient.ClientId)).Returns(client);
            _appContextMock.Setup(x => x.SaveChanges()).Returns(1);
            _service = new ClientService(_loggerServiceMock.Object, _appContextMock.Object);
            _appContextMock.Setup(x => x.Clients.FindAsync(client.ClientId)).ReturnsAsync(client);
            _appContextMock.Setup(x => x.Clients.Update(newClient)).Returns(It.IsAny<EntityEntry<Client>>());

            await _service.Update(newClient);

            Assert.Equal(newClient.Name, _appContextMock.Object.Clients.Find(client.ClientId).Name);
            Assert.Equal(newClient.Address, _appContextMock.Object.Clients.Find(client.ClientId).Address);
            Assert.Equal(newClient.Email, _appContextMock.Object.Clients.Find(client.ClientId).Email);
            Assert.Equal(client.ClientId, _appContextMock.Object.Clients.Find(client.ClientId).ClientId);
        }
        
        [Fact]
        public async Task ExistingClient_DeleteClient_DeletedClient()
        {
            var client = _fixture.Create<Client>();

            _appContextMock.Setup(x => x.Clients.FindAsync(client.ClientId)).ReturnsAsync(client);
            _appContextMock.Setup(x => x.Clients.Remove(client)).Returns(It.IsAny<EntityEntry<Client>>());
            _appContextMock.Setup(x => x.SaveChanges()).Returns(1);

            _service = new ClientService(_loggerServiceMock.Object, _appContextMock.Object);

            await _service.Delete(client.ClientId);

            Assert.Null(_appContextMock.Object.Clients.Find(client.ClientId));
        }
    }
}