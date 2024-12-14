using MechanicApp.Shared;

namespace MechanicApp
{
    public interface IJobRepo
    {
        Task Add(Job job);

        Task Delete(Guid id);

        Task<Job> Get(Guid id);

        Task<List<Job>> GetAll();

        Task Update(Job job);
    }
}
