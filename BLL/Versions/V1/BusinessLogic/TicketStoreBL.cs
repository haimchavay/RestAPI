using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.DataAccess;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Principal;
using System;
using BLL.Versions.V1.Helpers;
using BLL.Versions.V1.Builders;

namespace BLL.Versions.V1.BusinessLogic
{
    public class TicketStoreBL : ITicketStoreBL
    {
        private readonly ITicketStoreDA ticketStoreDA = new TicketStoreDA();
        private readonly IUserDA userDA = new UserDA();

        public async Task<IActionResult> GetTicketsStores(IIdentity userIdentity)
        {
            string userIdStr = Identity.GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);

            // Get user
            ActionResult<User> userAction = await userDA.GetUser(userId);
            if (userAction == null || userAction.Value == null)
            {
                //return new NotFoundResult();
                JsonMessageResponse jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("UserIdNotFound")
                    .WithMessageInfo("user id : " + userId + " not found")
                    .Build();

                return new NotFoundObjectResult(jmResponse);
            }
            User userData = userAction.Value;

            const int REGULAR_USER = 2;
            if (userData.UserTypeId == null)
            {
                JsonMessageResponse jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("PassUserTypeId")
                    .WithMessageInfo("Please pass userTypeId inside User object")
                    .Build();

                return new ConflictObjectResult(jmResponse);
            }

            // Regular user can't login to admin application
            if (userData.UserTypeId == REGULAR_USER)
            {
                JsonMessageResponse jmResponse = new JsonMessageResponseBuilder()
                    .WithMessage("LoginPermission")
                    .WithMessageInfo("Regular user can't login to admin")
                    .Build();

                return new ConflictObjectResult(jmResponse);
            }

            ActionResult<List<TicketStore>> action = await ticketStoreDA.GetTicketsStoresWithJoin(userId);
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            List<TicketStore> ticketsStoreList = action.Value;
            
            return new OkObjectResult(ticketsStoreList);
        }

        public async Task<IActionResult> GetTicketsStore(long storeId)
        {
            ActionResult<List<TicketStore>> action = await ticketStoreDA.GetTicketsStore(storeId);
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            List<TicketStore> ticketsStoreList = action.Value;
            
            return new OkObjectResult(ticketsStoreList);
        }
        private static TicketStoreDTO ItemToDTO(TicketStore ticketStore) =>
            new TicketStoreDTO
            {
                Id = ticketStore.Id,
                StoreId = ticketStore.StoreId,
                TicketTypeId = ticketStore.TicketTypeId,
                TicketPrice = ticketStore.TicketPrice,
                TotalPunches = ticketStore.TotalPunches
            };
    }
}
