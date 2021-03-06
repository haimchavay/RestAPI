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
    public class StoreDA : IStoreDA
    {
        public async Task<List<Store>> GetPage(int pageNumber, int pageSize)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Stores
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ActionResult<Store>> GetStore(long id)
        
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Stores.FindAsync(id);
        }

        public async Task<List<Store>> GetStores(long userId)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Stores
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> PutStore(Store store)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.Entry(store).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> PostStore(Store store)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.Stores.Add(store);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteStore(Store store)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.Stores.Remove(store);
            return await context.SaveChangesAsync();
        }
        public async Task<int> GetRecords()
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Stores.CountAsync();
        }
        public bool Exists(long id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return context.Stores.Any(e => e.Id == id);
        }
    }
}
