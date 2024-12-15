using MechanicApp.Shared;
using System.Net.Http.Json;

namespace MechanicApp.UI.Services
{
    public class JobService : IJobService
    {
        private readonly HttpClient _httpClient;

        public JobService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddJobAsync(Job job)
        {
            await _httpClient.PostAsJsonAsync("/Jobs", job);
        }

        public async Task DeleteJobAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"/Jobs/{id}");
        }

        public async Task<Job> GetJobAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Job>($"Jobs/{id}");
        }

        public async Task<IEnumerable<Job>> GetAllJobAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Job>>("/Jobs");
        }

        public async Task UpdateJobAsync(Guid id, Job job)
        {
            await _httpClient.PutAsJsonAsync($"/Jobs/{id}", job);
        }
    }
}
