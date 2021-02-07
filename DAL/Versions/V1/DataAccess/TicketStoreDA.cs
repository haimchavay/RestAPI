using DAL.Versions.V1.DataContext;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Versions.V1.DataAccess
{
    public class TicketStoreDA : ITicketStoreDA
    {
        public async Task<List<TicketStore>> GetTicketsStoresWithJoin(long userId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            var data = (from s in context.Stores
                        join ts in context.TicketsStores
                        on s.Id equals ts.StoreId
                        where s.UserId == userId
                        select new TicketStore
                        {
                            Id = ts.Id,
                            StoreId = ts.StoreId,
                            TicketTypeId = ts.TicketTypeId,
                            TicketPrice = ts.TicketPrice,
                            TotalPunches = ts.TotalPunches
                        }).ToListAsync();

            return await data;
        }
 
        public async Task<List<TicketStore>> GetTicketsStore(long storeId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketsStores
                                .Where(ts => ts.StoreId == storeId)
                                .ToListAsync();
        }

        public async Task<ActionResult<TicketStore>> GetTicketStore(long id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);
            var ticketStore = await context.TicketsStores.FindAsync(id);

            return ticketStore;
        }
    }
}
