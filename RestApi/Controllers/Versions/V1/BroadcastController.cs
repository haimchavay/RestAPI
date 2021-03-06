﻿using System.Threading.Tasks;
using BLL.Versions.V1.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace RestApi.Controllers.Versions.V1
{
    [Route("api/v1/[controller]")]
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
