using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.DataAccess;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Versions.V1.BusinessLogic
{
    public class TicketStoreBL : ITicketStoreBL
    {
        private readonly ITicketStoreDA ticketStoreDA = new TicketStoreDA();

        public async Task<IActionResult> GetTicketsStore(long storeId)
        {
            ActionResult<List<TicketStore>> action = await ticketStoreDA.GetTicketsStore(storeId);
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            List<TicketStore> ticketsStoreList = action.Value;
            List<TicketStoreDTO> ticketsStoreDTOList = new List<TicketStoreDTO>();

            foreach (TicketStore ticket in ticketsStoreList)
            {
                ticketsStoreDTOList.Add(ItemToDTO(ticket));
            }

            return new OkObjectResult(ticketsStoreDTOList); 
        }
        private static TicketStoreDTO ItemToDTO(TicketStore ticketStore) =>
            new TicketStoreDTO
            {
                Id = ticketStore.Id,
                StoreId = ticketStore.StoreId,
                TicketTypeId = ticketStore.TicketTypeId,
                TicketPrice = ticketStore.TicketPrice,
                TotalPunches = ticketStore.TotalPunches,
                TempCode = ticketStore.TempCode,
                CreatedTempDateCode = ticketStore.CreatedTempDateCode
            };
    }
}
