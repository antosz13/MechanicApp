using MechanicApp.Shared;

namespace MechanicApp.UI.Services
{
    public interface IClientService
    {
        Task AddClientAsync(Client client);

        Task DeleteClientAsync(Guid id);

        Task<Client> GetClientAsync(Guid id);

        Task<IEnumerable<Client>> GetAllClientAsync();

        Task UpdateClientAsync(Guid id, Client person);

        Task <IEnumerable<Job>> GetJobsOfClientAsync(Guid clientId);
    }
}
