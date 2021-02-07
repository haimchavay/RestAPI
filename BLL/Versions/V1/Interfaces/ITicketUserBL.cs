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
        //Task<IActionResult> GetTicketUser(long id);
        Task<ActionResult<TicketUserDTO>> CreateTicketUser(IIdentity userIdentity, TicketUser ticketUser);
        Task<ActionResult<TicketUserDTO>> CreatePunch(long ticketStoreId, int tempCode);
        Task<ActionResult<TicketUserDTO>> GenerateTempCode(IIdentity userIdentity, long ticketStoreId);
    }
}
