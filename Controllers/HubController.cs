using DotNetCoreSignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DotNetCoreSignalR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HubController : ControllerBase
    {
        private IHubContext<DataHub> _hub;

        public HubController(IHubContext<DataHub> hub)
        {
            _hub = hub;
        }

        [HttpGet("hub1")]
        public IActionResult GetTest()
        {
            //var result = _hub.Clients.All.SendAsync("hub1", "from hub 1");
            _hub.Clients.All.SendAsync("hub1", "admin", "Hi it from me admin");
            return Ok();
        }

        [HttpGet("hub2")]
        public IActionResult GetTest2()
        {
            var result = _hub.Clients.All.SendAsync("hub2", "from hub 2");
            return Ok();
        }

    }
}
