using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserInfoBL
    {
        Task<IActionResult> GetToken(UserInfo userData);
    }
}
