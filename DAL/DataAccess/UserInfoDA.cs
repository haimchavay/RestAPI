using DAL.DataContext;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
    public class UserInfoDA : IUserInfoDA
    {
        public async Task<UserInfo> GetUserInfo(string email, string password)
        {
            using var context = new CarticiaDbContext(CarticiaDbContext.ops.dbOptions);

            return await context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
