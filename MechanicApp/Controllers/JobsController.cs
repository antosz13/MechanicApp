using MechanicApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Job job)
        {
            var existingJob = await _jobService.Get(job.JobId);

            if (existingJob is not null)
            {
                return Conflict();
            }

            await _jobService.Add(job);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var job = await _jobService.Get(id);

            if (job is null)
            {
                return NotFound();
            }

            await _jobService.Delete(id);

            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Job>> Get(Guid id)
        {
            var job = await _jobService.Get(id);

            if (job is null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [HttpGet]
        public async Task<ActionResult<List<Job>>> GetAll()
        {
            return Ok(await _jobService.GetAll());
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Job newJob)
        {
            if (id != newJob.JobId)
            {
                return BadRequest();
            }

            var existingJob = await _jobService.Get(id);

            if (existingJob is null)
            {
                return NotFound();
            }

            await _jobService.Update(newJob);

            return Ok();
        }
    }
}
