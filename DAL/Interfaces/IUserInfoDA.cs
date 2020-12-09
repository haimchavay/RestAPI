using DAL.Entities;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserInfoDA
    {
        Task<UserInfo> GetUserInfo(string email, string password);
    }
}
