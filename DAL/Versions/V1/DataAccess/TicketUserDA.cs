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
    public class TicketUserDA : ITicketUserDA
    {
        public async Task<List<TicketUser>> GetTicketsUsers()
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketsUsers
                                .ToListAsync();
        }

        public async Task<List<TicketUserJoinTicketStoreJoinStore>> GetTicketsUserWithJoin(long userId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            var data = (from tu in context.TicketsUsers
                        join ts in context.TicketsStores
                        on tu.TicketStoreId equals ts.Id
                        join s in context.Stores
                        on ts.StoreId equals s.Id
                        where tu.UserId == userId
                        select new TicketUserJoinTicketStoreJoinStore
                        {
                            Id = tu.Id,
                            UserId = tu.UserId,
                            TicketStoreId = tu.TicketStoreId,
                            Punch = tu.Punch,
                            Status = tu.Status,
                            CreatedDate = tu.CreatedDate,
                            LastPunching = tu.LastPunching,
                            TempCode = tu.TempCode,
                            CreatedTempCode = tu.CreatedTempCode,
                            TotalPunches = ts.TotalPunches,
                            StoreName = s.Name,
                            TicketTypeId = ts.TicketTypeId
                        }).ToListAsync();

            return await data;
        }

        /*public async Task<List<TicketUser>> GetTicketsUser(long userId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketsUsers
                                .Where(tu => tu.UserId == userId)
                                .ToListAsync();
        }*/

        public async Task<List<TicketUserJoinTicketStoreJoinStore>> GetTicketUserWithJoin(long userId, long ticketStoreId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            var data = (from tu in context.TicketsUsers
                        join ts in context.TicketsStores
                        on tu.TicketStoreId equals ts.Id
                        join s in context.Stores
                        on ts.StoreId equals s.Id
                        where tu.UserId == userId && ts.Id == ticketStoreId
                        select new TicketUserJoinTicketStoreJoinStore
                        {
                            Id = tu.Id,
                            UserId = tu.UserId,
                            TicketStoreId = tu.TicketStoreId,
                            Punch = tu.Punch,
                            Status = tu.Status,
                            CreatedDate = tu.CreatedDate,
                            LastPunching = tu.LastPunching,
                            TempCode = tu.TempCode,
                            CreatedTempCode = tu.CreatedTempCode,
                            TotalPunches = ts.TotalPunches,
                            StoreName = s.Name,
                            TicketTypeId = ts.TicketTypeId
                        }).ToListAsync();

            return await data;
        }
        public async Task<List<TicketUserJoinTicketStoreJoinStore>> getTicketsUsersBelongToStore(long storeId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            var data = (from tu in context.TicketsUsers
                        join ts in context.TicketsStores
                        on tu.TicketStoreId equals ts.Id
                        join s in context.Stores
                        on ts.StoreId equals s.Id
                        where s.Id == storeId
                        select new TicketUserJoinTicketStoreJoinStore
                        {
                            Id = tu.Id,
                            UserId = tu.UserId,
                            TicketStoreId = tu.TicketStoreId,
                            Punch = tu.Punch,
                            Status = tu.Status,
                            CreatedDate = tu.CreatedDate,
                            LastPunching = tu.LastPunching,
                            TempCode = tu.TempCode,
                            CreatedTempCode = tu.CreatedTempCode,
                            TotalPunches = ts.TotalPunches,
                            StoreName = s.Name,
                            TicketTypeId = ts.TicketTypeId
                        }).ToListAsync();

            return await data;
        }

        public async Task<ActionResult<TicketUser>> GetTicketUser(long userId, long ticketStoreId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketsUsers.FirstOrDefaultAsync(tu => tu.UserId == userId &&
                                                                        tu.TicketStoreId == ticketStoreId);
        }

        public async Task<ActionResult<TicketUser>> GetTicketUser(int tempCode, long ticketStoreId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketsUsers.FirstOrDefaultAsync(tu => tu.TempCode == tempCode &&
                                                                        tu.TicketStoreId == ticketStoreId);
        }

        /*public async Task<ActionResult<TicketUser>> GetTicketUser(long id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);
            var ticketUser = await context.TicketsUsers.FindAsync(id);

            return ticketUser;
        }*/

        public async Task<int> CreateTicketUser(TicketUser ticketUser)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.TicketsUsers.Add(ticketUser);
            return await context.SaveChangesAsync();
        }

        public async Task<int> PutTicketUser(TicketUser ticketUser)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.Entry(ticketUser).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public bool Exists(long id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return context.TicketsUsers.Any(e => e.Id == id);
        }
    }
}
