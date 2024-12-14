using MechanicApp.Shared;

namespace MechanicApp
{
    public interface IClientRepo
    {
        Task Add(Client client);

        Task Delete(Guid id);

        Task<Client> Get(Guid id);

        Task<List<Client>> GetAll();

        Task Update(Client client);

        Task<List<Job>> GetJobsOfClient(Guid clientId);
    }
}
