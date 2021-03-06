﻿using BLL.Versions.V1.BusinessLogic;
using BLL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RestApi.Controllers.Versions.V1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TicketsStoresController : ControllerBase
    {
        private readonly ITicketStoreBL ticketStoreBL = new TicketStoreBL();

        [HttpGet("admin/")]
        public async Task<IActionResult> GetTicketsStores()
        {
            return await ticketStoreBL.GetTicketsStores(HttpContext.User.Identity);
        }

        [HttpGet]
        public async Task<IActionResult> GetTicketsStore(long storeId)
        {
            return await ticketStoreBL.GetTicketsStore(storeId);
        }
    }
}
