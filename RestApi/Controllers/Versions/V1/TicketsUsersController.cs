using BLL.Versions.V1.BusinessLogic;
using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RestApi.Controllers.Versions.V1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TicketsUsersController : ControllerBase
    {
        private readonly ITicketUserBL ticketUserBL = new TicketUserBL();

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

        [HttpPost]
        public async Task<ActionResult<TicketUserDTO>> CreateTicketUser(TicketUser ticketUser)
        {
            return await ticketUserBL.CreateTicketUser(HttpContext.User.Identity, ticketUser);
        }

        // Needs to pass ticketStoreId and tempCode
        [HttpPatch("punch/")]
        public async Task<ActionResult<TicketUserDTO>> CreatePunch([FromQuery] TicketUserDTO ticketUserDTO)
        {
            return await ticketUserBL.CreatePunch(ticketUserDTO.TicketStoreId, (int)ticketUserDTO.TempCode);
        }

        [HttpPatch("generate/")]
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
