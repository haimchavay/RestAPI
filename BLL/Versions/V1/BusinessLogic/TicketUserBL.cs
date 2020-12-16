using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.DataAccess;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLL.Versions.V1.BusinessLogic
{
    public class TicketUserBL : ITicketUserBL
    {
        private readonly ITicketUserDA ticketUserDA = new TicketUserDA();
        public async Task<IActionResult> getTicketsUsers()
        {
            ActionResult<List<TicketUser>> action = await ticketUserDA.GetTicketsUsers();
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            List<TicketUser> ticketsUserList = action.Value;
            List<TicketUserDTO> ticketsUserDTOList = new List<TicketUserDTO>();

            foreach (TicketUser ticket in ticketsUserList)
            {
                ticketsUserDTOList.Add(ItemToDTO(ticket));
            }

            return new OkObjectResult(ticketsUserDTOList);
        }
        public async Task<IActionResult> getTicketsUser(IIdentity userIdentity)
        {
            string userIdStr = GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);

            ActionResult<List<TicketUser>> action = await ticketUserDA.GetTicketsUser(userId);
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            List<TicketUser> ticketsUserList = action.Value;
            List<TicketUserDTO> ticketsUserDTOList = new List<TicketUserDTO>();

            foreach(TicketUser ticket in ticketsUserList)
            {
                ticketsUserDTOList.Add(ItemToDTO(ticket));
            }

            return new OkObjectResult(ticketsUserDTOList);
        }
        public async Task<IActionResult> GetTicketUser(long id)
        {
            ActionResult<TicketUser> action = await ticketUserDA.GetTicketUser(id);

            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            TicketUser ticketUser = action.Value;
            TicketUserDTO ticketUserDTO = ItemToDTO(ticketUser);

            return new OkObjectResult(ticketUserDTO);
        }
        public async Task<ActionResult<TicketUserDTO>> PostTicketUser(IIdentity userIdentity, TicketUser ticketUser)
        {
            string userIdStr = GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);
            ticketUser.UserId = userId;

            await ticketUserDA.PostTicketUser(ticketUser);

            return new CreatedAtRouteResult(new { Id = ticketUser.Id }, ItemToDTO(ticketUser));
        }
        private static TicketUserDTO ItemToDTO(TicketUser ticketUser) =>
            new TicketUserDTO
            {
                Id = ticketUser.Id,
                UserId = ticketUser.UserId,
                TicketStoreId = ticketUser.TicketStoreId,
                Punch = ticketUser.Punch,
                Status = ticketUser.Status,
                CreatedDate = ticketUser.CreatedDate,
                LastPunching = ticketUser.LastPunching
            };
        private static string GetValueFromClaim(IIdentity userIdentity, string key)
        {
            string value = null;
            if (userIdentity is ClaimsIdentity identity)
            {
                //IEnumerable<Claim> claims = identity.Claims;
                value = identity.FindFirst(key).Value;
            }

            return value;
        }
    }
}
