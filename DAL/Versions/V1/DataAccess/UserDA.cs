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
    public class UserDA : IUserDA
    {
        public async Task<List<User>> GetPage(int pageNumber, int pageSize)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<User> GetUser(string email, string password)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<ActionResult<User>> GetUser(long id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Users.FindAsync(id);
        }

        public async Task<int> PutUser(User user)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.Entry(user).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> CreateUser(User user)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.Users.Add(user);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteUser(User user)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.Users.Remove(user);
            return await context.SaveChangesAsync();
        }

        public async Task<int> GetRecords()
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.Users.CountAsync();
        }
        public bool Exists(long id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return context.Users.Any(e => e.Id == id);
        }

        public bool IsExists(string phone, string email)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return context.Users.Any(e => e.Email == email || e.Phone == phone);
        }
        public bool IsPhoneExists(string phone)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return context.Users.Any(e => e.Phone == phone);
        }
        public bool IsEmailExists(string email)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return context.Users.Any(e => e.Email == email);
        }
    }
}
