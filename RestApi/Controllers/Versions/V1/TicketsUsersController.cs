﻿using BLL.Versions.V1.BusinessLogic;
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
        public async Task<ActionResult<TicketUserDTO>> PostTicketUser(TicketUser ticketUser)
        {
            return await ticketUserBL.PostTicketUser(HttpContext.User.Identity, ticketUser);
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetTicketUser(long id)
        {
            return await ticketUserBL.GetTicketUser(id);
        }*/
    }
}
