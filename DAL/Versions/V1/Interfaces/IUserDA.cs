using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Versions.V1.Interfaces
{
    public interface IUserDA
    {
        Task<List<User>> GetPage(int pageNumber, int pageSize);
        Task<User> GetUser(string email, string password);
        Task<ActionResult<User>> GetUser(long id);
        Task<int> PutUser(User user);
        Task<int> PostUser(User user);
        Task<int> DeleteUser(User user);
        Task<int> GetRecords();
        bool Exists(long id);
        bool IsExists(string phone, string email);
    }
}
