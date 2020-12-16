using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Versions.V1.Interfaces
{
    public interface ITicketUserDA
    {
        Task<List<TicketUser>> GetTicketsUsers();
        Task<List<TicketUser>> GetTicketsUser(long userId);
        Task<ActionResult<TicketUser>> GetTicketUser(long id);
        Task<int> PostTicketUser(TicketUser ticketUser);
    }
}
