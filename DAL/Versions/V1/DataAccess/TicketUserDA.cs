﻿using DAL.Versions.V1.DataContext;
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

        public async Task<List<TicketUser>> GetTicketsUser(long userId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketsUsers
                                .Where(tu => tu.UserId == userId)
                                .ToListAsync();
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

        public async Task<ActionResult<TicketUser>> GetTicketUser(long id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);
            var ticketUser = await context.TicketsUsers.FindAsync(id);

            return ticketUser;
        }

        public async Task<int> PostTicketUser(TicketUser ticketUser)
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
