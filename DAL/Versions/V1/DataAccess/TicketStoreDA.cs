using DAL.Versions.V1.DataContext;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Versions.V1.DataAccess
{
    public class TicketStoreDA : ITicketStoreDA
    {
        public async Task<List<TicketStore>> GetTicketsStore(long storeId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketsStores
                                .Where(ts => ts.StoreId == storeId)
                                .ToListAsync();
        }
    }
}
