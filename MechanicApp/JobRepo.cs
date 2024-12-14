using Microsoft.EntityFrameworkCore;
using MechanicApp.Shared;

namespace MechanicApp
{
    public class JobRepo : IJobRepo
    {
        private readonly AppContext _context;
        private readonly ILogger<JobService> _logger;

        public JobRepo(ILogger<JobService> logger, AppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(Job job)
        {
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Job added");
        }

        public async Task Delete(Guid id)
        {
            var job = await Get(id);

            _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();
        }

        public async Task<Job> Get(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _logger.LogInformation("Job retrieved: {@Job}", job);
            return job;
        }

        public async Task<List<Job>> GetAll()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task Update(Job newJob)
        {
            var job = await Get(newJob.JobId);
            job.ClientId = newJob.ClientId;
            job.NumberPlate = newJob.NumberPlate;
            job.YearOfProduction = newJob.YearOfProduction;
            job.Category = newJob.Category;
            job.Description = newJob.Description;
            job.Seriousness = newJob.Seriousness;
            job.Status = newJob.Status;
            await _context.SaveChangesAsync();
        }
    }
}
