using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Hubs;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.DataAccess;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLL.Versions.V1.BusinessLogic
{
    public class TicketUserBL : ITicketUserBL
    {
        private const bool TICKET_UNACTIVE = false;

        private readonly ITicketUserDA ticketUserDA = new TicketUserDA();
        private readonly ITicketStoreDA ticketStoreDA = new TicketStoreDA();
        private readonly IStoreDA storeDA = new StoreDA();
        private readonly ITicketTypeDA ticketTypeDA = new TicketTypeDA();
        private readonly IUserDA userDA = new UserDA();
        private readonly IPunchHistoryDA punchHistoryDA = new PunchHistoryDA();

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
                // Get total punches
                ActionResult<TicketStore> ticketStoreAction = await ticketStoreDA.GetTicketStore(ticket.TicketStoreId);
                if (ticketStoreAction == null || ticketStoreAction.Value == null)
                {
                    return new NotFoundResult();
                }
                TicketStore ticketStore = ticketStoreAction.Value;

                // Get store name
                ActionResult<Store> storeAction = await storeDA.GetStore(ticketStore.StoreId);
                if (storeAction == null || storeAction.Value == null)
                {
                    return new NotFoundResult();
                }
                Store store = storeAction.Value;

                // Get ticket type
                ActionResult<TicketType> ticketTypeAction = await ticketTypeDA.GetTicketType(ticketStore.TicketTypeId);
                if (ticketTypeAction == null || ticketTypeAction.Value == null)
                {
                    return new NotFoundResult();
                }
                TicketType ticketType = ticketTypeAction.Value;

                ticketsUserDTOList.Add(ItemToDTO(ticket, ticketStore.TotalPunches, store.Name, ticketType.Id));
            }

            return new OkObjectResult(ticketsUserDTOList);
        }
        public async Task<IActionResult> getTicketsUser(IIdentity userIdentity)
        {
            string userIdStr = GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);

            ActionResult<List<TicketUserJoinTicketStoreJoinStore>> actionJoin =
                await ticketUserDA.GetTicketsUserWithJoin(userId);
            if (actionJoin == null || actionJoin.Value == null)
            {
                return new NotFoundResult();
            }
            List<TicketUserJoinTicketStoreJoinStore> ticketJoin = actionJoin.Value;

            return new OkObjectResult(ticketJoin);
        }

        public async Task<IActionResult> getTicketsUsersBelongToStore(long storeId)
        {
            ActionResult<List<TicketUserJoinTicketStoreJoinStore>> actionJoin =
                await ticketUserDA.getTicketsUsersBelongToStore(storeId);
            if (actionJoin == null || actionJoin.Value == null)
            {
                return new NotFoundResult();
            }
            List<TicketUserJoinTicketStoreJoinStore> ticketJoin = actionJoin.Value;

            return new OkObjectResult(ticketJoin);
        }

        /*public async Task<IActionResult> GetTicketUser(long id)
        {
            ActionResult<TicketUser> action = await ticketUserDA.GetTicketUser(id);

            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            TicketUser ticketUser = action.Value;

            // Get total punches
            ActionResult<TicketStore> ticketStoreAction = await ticketStoreDA.GetTicketStore(ticketUser.TicketStoreId);
            if (ticketStoreAction == null || ticketStoreAction.Value == null)
            {
                return new NotFoundResult();
            }
            TicketStore ticketStore = ticketStoreAction.Value;

            // Get store name
            ActionResult<Store> storeAction = await storeDA.GetStore(ticketStore.StoreId);
            if (storeAction == null || storeAction.Value == null)
            {
                return new NotFoundResult();
            }
            Store store = storeAction.Value;

            // Get ticket type
            ActionResult<TicketType> ticketTypeAction = await ticketTypeDA.GetTicketType(ticketStore.TicketTypeId);
            if (ticketTypeAction == null || ticketTypeAction.Value == null)
            {
                return new NotFoundResult();
            }
            TicketType ticketType = ticketTypeAction.Value;

            TicketUserDTO ticketUserDTO = ItemToDTO(ticketUser, ticketStore.TotalPunches, store.Name, ticketType.Id);

            return new OkObjectResult(ticketUserDTO);
        }*/
        public async Task<ActionResult<TicketUserDTO>> CreateTicketUser(IIdentity userIdentity, TicketUser ticketUser)
        {
            string userIdStr = GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);
            ticketUser.UserId = userId;

            #region// Get ticket user
            ActionResult<TicketUser> actionTicketUser = await ticketUserDA.GetTicketUser(userId, ticketUser.TicketStoreId);
            if (actionTicketUser != null && actionTicketUser.Value != null)
            {
                return new ConflictObjectResult("The ticket already exists");
            }
            #endregion

            await ticketUserDA.CreateTicketUser(ticketUser);

            ActionResult<List<TicketUserJoinTicketStoreJoinStore>> actionJoin =
                await ticketUserDA.GetTicketUserWithJoin(userId, ticketUser.TicketStoreId);
            if (actionJoin == null || actionJoin.Value == null)
            {
                return new NotFoundResult();
            }
            List<TicketUserJoinTicketStoreJoinStore> ticketJoin = actionJoin.Value;

            return new CreatedAtRouteResult(new { Id = ticketUser.Id }, ticketJoin[0]);
        }
        public async Task<ActionResult<TicketUserDTO>> CreatePunch(long ticketStoreId, int tempCode,
            IHubContext<ChatHub> hub)
        {
            #region// Get ticket user with temp code
            ActionResult<TicketUser> actionTicketUser = await ticketUserDA.GetTicketUser(tempCode, ticketStoreId);
            if (actionTicketUser == null || actionTicketUser.Value == null)
            {
                return new NotFoundObjectResult("Wrong temp code ");
            }
            TicketUser ticketUser = actionTicketUser.Value;

            if (ticketUser.CreatedTempCode.Value.AddMinutes(5) < DateTime.Now)
            {
                return new NotFoundObjectResult("More than five minutes have passed, please try again");
            }
            #endregion

            #region// Get ticket store
            ActionResult<TicketStore> actionStore = await ticketStoreDA.GetTicketStore(ticketStoreId);
            if (actionStore == null || actionStore.Value == null)
            {
                return new NotFoundResult();
            }
            TicketStore ticketStore = actionStore.Value;
            #endregion

            if (ticketUser.Punch >= ticketStore.TotalPunches || ticketUser.Status == TICKET_UNACTIVE)
            {
                return new BadRequestObjectResult("Ticket id '" + ticketUser.Id + "' is finish");
            }

            // Make punch in the ticket
            ticketUser.Punch++;
            ticketUser.LastPunching = DateTime.Now;
            // Remove tempCode and createdTempCode from database
            ticketUser.TempCode = null;
            ticketUser.CreatedTempCode = null;

            if(ticketUser.Punch >= ticketStore.TotalPunches)
            {
                ticketUser.Status = TICKET_UNACTIVE;
            }

            #region// Insert to database
            try
            {
                await ticketUserDA.PutTicketUser(ticketUser);
            }
            catch (DbUpdateConcurrencyException) when (!ticketUserDA.Exists(ticketUser.Id))
            {
                return new NotFoundResult();
            }
            #endregion

            // Get store name
            ActionResult<Store> storeAction = await storeDA.GetStore(ticketStore.StoreId);
            if (storeAction == null || storeAction.Value == null)
            {
                return new NotFoundResult();
            }
            Store store = storeAction.Value;

            // Caller chat notification
            await hub.Clients.All.SendAsync(ticketUser.UserId.ToString(), "success", ticketUser);
       
            // Create punch history
            PunchHistory punchHistory = new PunchHistory
            {
                Id = Guid.NewGuid().ToString(),
                UserId = ticketUser.UserId,
                TicketStoreId = ticketUser.TicketStoreId,
                Punch = ticketUser.Punch,
                CreatedDate = (DateTime)ticketUser.LastPunching
            };
            await punchHistoryDA.CreatePunchHistory(punchHistory);

            return new OkObjectResult(
                ItemToDTO(ticketUser, ticketStore.TotalPunches, store.Name, ticketStore.TicketTypeId));
        }
        public async Task<ActionResult<TicketUserDTO>> GenerateTempCode(IIdentity userIdentity, long ticketStoreId)
        {
            int tempCode = 0;
            #region// Get user id
            string userIdStr = GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);
            #endregion

            #region// Get ticket user
            ActionResult<TicketUser> actionTicketUser = await ticketUserDA.GetTicketUser(userId, ticketStoreId);
            if (actionTicketUser == null || actionTicketUser.Value == null)
            {
                return new NotFoundResult();
            }
            TicketUser ticketUser = actionTicketUser.Value;
            #endregion

            #region// Get ticket store
            ActionResult<TicketStore> actionStore = await ticketStoreDA.GetTicketStore(ticketStoreId);
            if (actionStore == null || actionStore.Value == null)
            {
                return new NotFoundResult();
            }
            TicketStore ticketStore = actionStore.Value;
            #endregion

            if (ticketUser.Punch >= ticketStore.TotalPunches || ticketUser.Status == TICKET_UNACTIVE)
            {
                return new BadRequestObjectResult("Ticket id '" + ticketUser.Id + "' is finish");
            }

            // Generate temp code(1 - 99999) and insert to ticket user(database)
            tempCode = new Random().Next(10001, 100000);

            int i = 0;
            for(i = 0; i < 10; i++)
            {
                ActionResult<TicketUser> actionTicketUserWithSameTempCode = await ticketUserDA.GetTicketUser(tempCode, ticketStoreId);
                if (actionTicketUserWithSameTempCode != null &&
                    actionTicketUserWithSameTempCode.Value != null)
                {
                    tempCode = new Random().Next(10001, 100000);
                    continue;
                }
                break;
            }
            // 10 time generate same code with same store id
            if(i == 10)
            {
                return new NotFoundObjectResult("Please try again or talk with customer service");
            }

            // Insert temp code to ticket user
            ticketUser.TempCode = tempCode;
            ticketUser.CreatedTempCode = DateTime.Now;

            #region// Insert to database
            try
            {
                await ticketUserDA.PutTicketUser(ticketUser);
            }
            catch (DbUpdateConcurrencyException) when (!ticketUserDA.Exists(ticketUser.Id))
            {
                return new NotFoundResult();
            }
            #endregion

            // Get store name
            ActionResult<Store> storeAction = await storeDA.GetStore(ticketStore.StoreId);
            if (storeAction == null || storeAction.Value == null)
            {
                return new NotFoundResult();
            }
            Store store = storeAction.Value;

            return new OkObjectResult(
                ItemToDTO(ticketUser, ticketStore.TotalPunches, store.Name, ticketStore.TicketTypeId));
        }

        /*private static TicketUserDTO ItemToDTO(TicketUser ticketUser) =>
            new TicketUserDTO
            {
                Id = ticketUser.Id,
                UserId = ticketUser.UserId,
                TicketStoreId = ticketUser.TicketStoreId,
                Punch = ticketUser.Punch,
                Status = ticketUser.Status,
                CreatedDate = ticketUser.CreatedDate,
                LastPunching = ticketUser.LastPunching,
                TempCode = ticketUser.TempCode,
                CreatedTempCode = ticketUser.CreatedTempCode
            };*/

        private static TicketUserDTO ItemToDTO(TicketUser ticketUser,
            int totalPunches, string storeName, int ticketTypeId) =>
            new TicketUserDTO
            {
                Id = ticketUser.Id,
                UserId = ticketUser.UserId,
                TicketStoreId = ticketUser.TicketStoreId,
                Punch = ticketUser.Punch,
                Status = ticketUser.Status,
                CreatedDate = ticketUser.CreatedDate,
                LastPunching = ticketUser.LastPunching,
                TempCode = ticketUser.TempCode,
                CreatedTempCode = ticketUser.CreatedTempCode,
                TotalPunches = totalPunches,
                StoreName = storeName,
                TicketTypeId = ticketTypeId
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
