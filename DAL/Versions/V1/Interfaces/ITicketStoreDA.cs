using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Versions.V1.Interfaces
{
    public interface ITicketStoreDA
    {
        Task<List<TicketStore>> GetTicketsStoresWithJoin(long userId);
        //Task<List<TicketStore>> GetTicketsStores(long[] storeIdArr);
        Task<List<TicketStore>> GetTicketsStore(long storeId);
        Task<ActionResult<TicketStore>> GetTicketStore(long id);
    }
}
