using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Hubs;
using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLL.Versions.V1.Interfaces
{
    public interface ITicketUserBL
    {
        Task<IActionResult> getTicketsUsers();
        Task<IActionResult> getTicketsUser(IIdentity userIdentity);
        Task<IActionResult> getTicketsUsersBelongToStore(long storeId);
        //Task<IActionResult> GetTicketUser(long id);
        Task<ActionResult<TicketUserDTO>> CreateTicketUser(IIdentity userIdentity, TicketUser ticketUser);
        Task<ActionResult<TicketUserDTO>> CreatePunch(long ticketStoreId, int tempCode, IHubContext<ChatHub> hub);
        Task<ActionResult<TicketUserDTO>> GenerateTempCode(IIdentity userIdentity, long ticketStoreId);
    }
}
