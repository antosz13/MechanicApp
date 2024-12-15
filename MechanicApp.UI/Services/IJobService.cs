using MechanicApp.Shared;

namespace MechanicApp.UI.Services
{
    public interface IJobService
    {
        Task AddJobAsync(Job job);

        Task DeleteJobAsync(Guid id);

        Task<Job> GetJobAsync(Guid id);

        Task<IEnumerable<Job>> GetAllJobAsync();

        Task UpdateJobAsync(Guid id, Job job);
    }
}
