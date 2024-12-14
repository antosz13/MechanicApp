using MechanicApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Client client)
        {
            var existingClient = await _clientService.Get(client.ClientId);

            if (existingClient is not null)
            {
                return Conflict();
            }

            await _clientService.Add(client);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = await _clientService.Get(id);

            if (client is null)
            {
                return NotFound();
            }

            await _clientService.Delete(id);

            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Client>> Get(Guid id)
        {
            var client = await _clientService.Get(id);

            if (client is null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> GetAll()
        {
            return Ok(await _clientService.GetAll());
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Client newClient)
        {
            if (id != newClient.ClientId)
            {
                return BadRequest();
            }

            var existingClient = await _clientService.Get(id);

            if (existingClient is null)
            {
                return NotFound();
            }

            await _clientService.Update(newClient);

            return Ok();
        }

        [HttpGet("{id:guid}/jobs")]
        public async Task<ActionResult<List<Job>>> GetJobsOfClient(Guid id)
        {
            var client = await _clientService.Get(id);

            if (client is null)
            {
                return NotFound();
            }

            return Ok(await _clientService.GetJobsOfClient(id));
        }
    }
}
