using BLL.Versions.V1.Builders;
using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Helpers;
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
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLL.Versions.V1.BusinessLogic
{
    public class TicketUserBL : ITicketUserBL
    {
        private const bool TICKET_UNACTIVE = false;
        private const bool TICKET_ACTIVE = true;
        private const int CASH_TICKET = 2;
        private const int EMPTY_CODE = 0;

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

                // Get user
                ActionResult<User> userAction = await userDA.GetUser(ticket.UserId);
                if (userAction == null || userAction.Value == null)
                {
                    //return new NotFoundResult();
                    JsonMessageResponse jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("UserIdNotFound")
                    .WithMessageInfo("user id : " + ticket.UserId + " not found")
                    .Build();

                    return new NotFoundObjectResult(jmResponse);
                }
                User userData = userAction.Value;

                // Get ticket type
                // TODO - remove this ticketTypeAction and use with ticketStore.TicketTypeId
                ActionResult<TicketType> ticketTypeAction = await ticketTypeDA.GetTicketType(ticketStore.TicketTypeId);
                if (ticketTypeAction == null || ticketTypeAction.Value == null)
                {
                    return new NotFoundResult();
                }
                TicketType ticketType = ticketTypeAction.Value;

                ticketsUserDTOList.Add(ItemToDTO(ticket, ticketStore.TotalPunches, store.Name, ticketType.Id,
                    ticketStore.PunchValue, ticketStore.GiftDescription, userData.Email, ticketStore.TicketPrice));
            }

            return new OkObjectResult(ticketsUserDTOList);
        }
        public async Task<IActionResult> getTicketsUser(IIdentity userIdentity)
        {
            //string userIdStr = GetValueFromClaim(userIdentity, "Id");
            string userIdStr = Identity.GetValueFromClaim(userIdentity, "Id");
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
        public async Task<ActionResult<TicketUserJoinTicketStoreJoinStore>> CreateTicketUser(
            IIdentity userIdentity, TicketUser ticketUser,
            int userTempCode, string userEmail, IHubContext<ChatHub> hub)
        {
            string userIdStr = null;
            long userId = 0;
            User user = null;
            JsonMessageResponse jmResponse;

            // Check if Cash ticket
            ActionResult<TicketStore> action = await ticketStoreDA.GetTicketStore(ticketUser.TicketStoreId);
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }
            TicketStore ticketStore = action.Value;

            // Cash ticket
            if(ticketStore.TicketTypeId == CASH_TICKET)
            {
                if (string.IsNullOrEmpty(userEmail))
                {
                    jmResponse = new JsonMessageResponseBuilder()
                        .WithMessage("Email")
                        .WithMessageInfo("Please pass email")
                        .Build();

                    return new ConflictObjectResult(jmResponse);
                }
                

                ActionResult<User> actionUser = await userDA.GetUser(userEmail);
                if (actionUser == null || actionUser.Value == null)
                {
                    jmResponse = new JsonMessageResponseBuilder()
                        .WithMessage("UserEmailNotFound")
                        .WithMessageInfo("user email : " + userEmail + " not found")
                        .Build();

                    return new NotFoundObjectResult(jmResponse);
                }
                user = actionUser.Value;

                userId = user.Id;
            }
            // Regular ticket
            else
            {
                userIdStr = Identity.GetValueFromClaim(userIdentity, "Id");
                userId = Convert.ToInt64(userIdStr);
                //ticketUser.UserId = userId;
            }

            ticketUser.UserId = userId;

            #region// Get ticket user
            //TicketUser existTicketUser = null;
            ActionResult<TicketUser> actionTicketUser = await ticketUserDA.GetTicketUser(userId,
                ticketUser.TicketStoreId, TICKET_ACTIVE);
            if (actionTicketUser != null && actionTicketUser.Value != null)
            {
                jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("TicketExist")
                    .WithMessageInfo("The ticket already exists and active")
                    .Build();

                return new ConflictObjectResult(jmResponse);
            }
            #endregion

            if(ticketStore.TicketTypeId == CASH_TICKET)
            {
                if(userTempCode == EMPTY_CODE)
                {
                    jmResponse = new JsonMessageResponseBuilder()
                        .WithMessage("GenerateCode")
                        .WithMessageInfo("Please generate code and pass it")
                        .Build();

                    return new ConflictObjectResult(jmResponse);
                }

                if(user.TempCode != userTempCode)
                {
                    jmResponse = new JsonMessageResponseBuilder()
                        .WithMessage("WrongTempCode")
                        .WithMessageInfo("Wrong temp code")
                        .Build();

                    return new ConflictObjectResult(jmResponse);
                }
                if(user.CreatedTempCode.Value.AddMinutes(5) < DateTime.Now)
                {
                    jmResponse = new JsonMessageResponseBuilder()
                        .WithMessage("FiveMinutesPassed")
                        .WithMessageInfo("More than five minutes have passed, please try again")
                        .Build();

                    return new NotFoundObjectResult(jmResponse);
                }

                // Remove tempCode and createdTempCode from database
                user.TempCode = null;
                user.CreatedTempCode = null;

                #region// Insert to database
                try
                {
                    await userDA.PutUser(user);
                }
                catch (DbUpdateConcurrencyException) when (!userDA.Exists(user.Id))
                {
                    return new NotFoundResult();
                }
                #endregion
            }

            await ticketUserDA.CreateTicketUser(ticketUser);

            ActionResult<List<TicketUserJoinTicketStoreJoinStore>> actionJoin =
                await ticketUserDA.GetTicketUserWithJoin(userId, ticketUser.TicketStoreId);
            if (actionJoin == null || actionJoin.Value == null)
            {
                return new NotFoundResult();
            }
            List<TicketUserJoinTicketStoreJoinStore> ticketJoin = actionJoin.Value;

            // Caller chat notification
            jmResponse = new JsonMessageResponseBuilder()
                .WithEvent("CreateTicketUser")
                .WithIsSuccess(true)
                .Build();

            await hub.Clients.All.SendAsync(ticketUser.UserId.ToString(), jmResponse, ticketJoin[0]);

            return new CreatedAtRouteResult(new { Id = ticketUser.Id }, ticketJoin[0]);
        }
        public async Task<ActionResult<TicketUserDTO>> CreatePunch(long ticketStoreId, int tempCode,
            IHubContext<ChatHub> hub)
        {
            JsonMessageResponse jmResponse;

            #region// Get ticket user with temp code
            ActionResult<TicketUser> actionTicketUser = await ticketUserDA.GetTicketUser(tempCode, ticketStoreId);
            if (actionTicketUser == null || actionTicketUser.Value == null)
            {
                jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("WrongTempCode")
                    .WithMessageInfo("Wrong temp code")
                    .Build();

                return new NotFoundObjectResult(jmResponse);
            }
            TicketUser ticketUser = actionTicketUser.Value;

            if (ticketUser.CreatedTempCode.Value.AddMinutes(5) < DateTime.Now)
            {
                jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("FiveMinutesPassed")
                    .WithMessageInfo("More than five minutes have passed, please try again")
                    .Build();

                return new NotFoundObjectResult(jmResponse);
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
                jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("TicketIdFinish")
                    .WithMessageInfo("Ticket id '" + ticketUser.Id + "' is finish")
                    .Build();

                return new BadRequestObjectResult(jmResponse);
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

            // Get user
            ActionResult<User> userAction = await userDA.GetUser(ticketUser.UserId);
            if (userAction == null || userAction.Value == null)
            {
                jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("UserIdNotFound")
                    .WithMessageInfo("user id : " + ticketUser.UserId + " not found")
                    .Build();

                return new NotFoundObjectResult(jmResponse);
            }
            User userData = userAction.Value;

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

            TicketUserDTO ticketUserDTO = ItemToDTO(ticketUser, ticketStore.TotalPunches, store.Name,
                ticketStore.TicketTypeId, ticketStore.PunchValue, ticketStore.GiftDescription, userData.Email,
                ticketStore.TicketPrice);

            // Caller chat notification
            jmResponse = new JsonMessageResponseBuilder()
                .WithEvent("CreatePunch")
                .WithIsSuccess(true)
                .Build();

            await hub.Clients.All.SendAsync(ticketUser.UserId.ToString(), jmResponse, ticketUserDTO);

            return new OkObjectResult(ticketUserDTO);
        }
        public async Task<ActionResult<TicketUserDTO>> GenerateTempCode(IIdentity userIdentity, long ticketStoreId)
        {
            int tempCode = 0;
            #region// Get user id
            string userIdStr = Identity.GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);
            #endregion

            #region// Get ticket user
            ActionResult<TicketUser> actionTicketUser = await ticketUserDA.GetTicketUser(userId,
                ticketStoreId, TICKET_ACTIVE);
            if (actionTicketUser == null || actionTicketUser.Value == null)
            {
                JsonMessageResponse jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("TicketNotExistOrUnactive")
                    .WithMessageInfo("The ticket not exist or unactive")
                    .Build();

                return new NotFoundObjectResult(jmResponse);
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
                JsonMessageResponse jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("TryAgain")
                    .WithMessageInfo("Please try again or talk with customer service")
                    .Build();

                return new NotFoundObjectResult(jmResponse);
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

            // Get user
            ActionResult<User> userAction = await userDA.GetUser(userId);
            if (userAction == null || userAction.Value == null)
            {
                JsonMessageResponse jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("UserIdNotFound")
                    .WithMessageInfo("user id : " + userId + " not found")
                    .Build();

                return new NotFoundObjectResult(jmResponse);
            }
            User userData = userAction.Value;

            return new OkObjectResult(
                ItemToDTO(ticketUser, ticketStore.TotalPunches, store.Name, ticketStore.TicketTypeId,
                ticketStore.PunchValue, ticketStore.GiftDescription, userData.Email, ticketStore.TicketPrice));
        }

        private static TicketUserDTO ItemToDTO(TicketUser ticketUser,
            int totalPunches, string storeName, int ticketTypeId, long punchValue, string giftDescription,
            string email, int ticketPrice) =>
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
                TicketTypeId = ticketTypeId,
                PunchValue = punchValue,
                GiftDescription = giftDescription,
                Email = email,
                TicketPrice = ticketPrice
            };
    }
}
