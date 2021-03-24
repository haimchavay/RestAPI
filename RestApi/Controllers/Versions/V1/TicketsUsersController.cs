using BLL.Versions.V1.BusinessLogic;
using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Hubs;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RestApi.Controllers.Versions.V1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TicketsUsersController : ControllerBase
    {
        private readonly ITicketUserBL ticketUserBL = new TicketUserBL();
        private readonly IHubContext<ChatHub> hub;

        public TicketsUsersController(IHubContext<ChatHub> hub)
        {
            this.hub = hub;
        }

        /*[HttpGet]
        public async Task<IActionResult> GetTicketsUsers()
        {
            return await ticketUserBL.getTicketsUsers();
        }*/

        [HttpGet]
        public async Task<IActionResult> GetTicketsUser()
        {
            return await ticketUserBL.getTicketsUser(HttpContext.User.Identity);
        }
        [HttpGet("store/{storeId}")]
        public async Task<IActionResult> GetTicketsUsers(long storeId)
        {
            return await ticketUserBL.getTicketsUsersBelongToStore(storeId);
        }

        [HttpPost]
        public async Task<ActionResult<TicketUserDTO>> CreateTicketUser(TicketUser ticketUser)
        {
            return await ticketUserBL.CreateTicketUser(HttpContext.User.Identity, ticketUser, 0, "", hub);
        }
        [HttpPost("code/{userTempCode}/email/{userEmail}")]
        public async Task<ActionResult<TicketUserDTO>> CreateTicketUser(TicketUser ticketUser,
            int userTempCode, string userEmail)
        {
            return await ticketUserBL.CreateTicketUser(HttpContext.User.Identity, ticketUser,
                userTempCode, userEmail, hub);
        }

        [HttpGet("punch/")]
        public async Task<ActionResult<TicketUserDTO>> CreatePunch([FromQuery] TicketUserDTO ticketUserDTO)
        {
            return await ticketUserBL.CreatePunch(ticketUserDTO.TicketStoreId, (int)ticketUserDTO.TempCode, hub);
        }

        [HttpGet("generate/")]
        public async Task<ActionResult<TicketUserDTO>> GenerateTempCode(long ticketStoreId)
        {
            return await ticketUserBL.GenerateTempCode(HttpContext.User.Identity, ticketStoreId);
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetTicketUser(long id)
        {
            return await ticketUserBL.GetTicketUser(id);
        }*/
    }
}
