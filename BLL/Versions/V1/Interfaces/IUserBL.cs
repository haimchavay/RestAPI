using BLL.Versions.V1.DataTransferObjects;
using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pagination.Filter;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLL.Versions.V1.Interfaces
{
    public interface IUserBL
    {
        Task<IActionResult> GetToken(User userData, bool isAdminPath);
        //Task<ActionResult<TicketUserDTO>> GenerateTempCode(IIdentity userIdentity);
        Task<ActionResult<UserDTO>> GenerateTempCode(IIdentity userIdentity);
        Task<ActionResult> GenerateEmailTempCode(string email);
        Task<IActionResult> GetPage(PaginationFilter filter, HttpRequest request);
        Task<IActionResult> GetUser(long id);
        Task<IActionResult> PutUser(PasswordRecovery passwordRecovery);
        Task<ActionResult<UserDTO>> CreateUser(User user);
        Task<IActionResult> DeleteUser(long id);
    }
}
