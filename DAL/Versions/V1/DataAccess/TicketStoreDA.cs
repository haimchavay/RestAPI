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
        public async Task<List<TicketStore>> GetTicketsStores(long[] storeIdArr)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            string whereCondition = string.Format("WHERE StoreId = {0}", storeIdArr[0]);
            for(int i = 1; i < storeIdArr.Length; i++)
            {
                string condition = string.Format(" or StoreId = {0}", storeIdArr[i]);
                whereCondition += condition;
            }
            string cmd = string.Format("SELECT * FROM [DevTicket].[dbo].[TicketsStores]" +
                " {0};", whereCondition);

            /*string cmd = "SELECT * FROM [DevTicket].[dbo].[TicketsStores]" +
                " WHERE StoreId = 1 or StoreId = 3;";*/

            return await context.TicketsStores.FromSqlRaw(cmd).ToListAsync();
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
