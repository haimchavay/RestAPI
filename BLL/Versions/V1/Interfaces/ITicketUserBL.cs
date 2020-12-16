using BLL.Versions.V1.DataTransferObjects;
using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLL.Versions.V1.Interfaces
{
    public interface ITicketUserBL
    {
        Task<IActionResult> getTicketsUsers();
        Task<IActionResult> getTicketsUser(IIdentity userIdentity);
        Task<IActionResult> GetTicketUser(long id);
        Task<ActionResult<TicketUserDTO>> PostTicketUser(IIdentity userIdentity, TicketUser ticketUser);
    }
}
