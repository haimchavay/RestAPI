using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;
using System.Threading.Tasks;

namespace SignalRChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BroadcastController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hub;

        public BroadcastController(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        [HttpGet]
        public async Task Get(string user, string message)
        {
            await _hub.Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
