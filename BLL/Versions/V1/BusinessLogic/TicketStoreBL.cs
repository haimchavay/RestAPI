using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.DataAccess;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Principal;
using System;
using System.Security.Claims;

namespace BLL.Versions.V1.BusinessLogic
{
    public class TicketStoreBL : ITicketStoreBL
    {
        private readonly ITicketStoreDA ticketStoreDA = new TicketStoreDA();

        public async Task<IActionResult> GetTicketsStores(IIdentity userIdentity)
        {
            string userIdStr = GetValueFromClaim(userIdentity, "Id");
            long userId = Convert.ToInt64(userIdStr);

            ActionResult<List<TicketStore>> action = await ticketStoreDA.GetTicketsStoresWithJoin(userId);
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            List<TicketStore> ticketsStoreList = action.Value;
            /*List<TicketStoreDTO> ticketsStoreDTOList = new List<TicketStoreDTO>();

            foreach (TicketStore ticket in ticketsStoreList)
            {
                ticketsStoreDTOList.Add(ItemToDTO(ticket));
            }*/

            /*return new OkObjectResult(ticketsStoreDTOList);*/
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
            /*List<TicketStoreDTO> ticketsStoreDTOList = new List<TicketStoreDTO>();

            foreach (TicketStore ticket in ticketsStoreList)
            {
                ticketsStoreDTOList.Add(ItemToDTO(ticket));
            }

            return new OkObjectResult(ticketsStoreDTOList);*/
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
