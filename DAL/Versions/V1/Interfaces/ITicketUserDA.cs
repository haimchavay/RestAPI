using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Versions.V1.Interfaces
{
    public interface ITicketUserDA
    {
        Task<List<TicketUser>> GetTicketsUsers();
        Task<List<TicketUserJoinTicketStoreJoinStore>> GetTicketsUserWithJoin(long userId);
        //Task<List<TicketUser>> GetTicketsUser(long userId);
        //Task<ActionResult<TicketUser>> GetTicketUser(long id);
        Task<ActionResult<TicketUser>> GetTicketUser(long userId, long ticketStoreId, bool status);
        Task<List<TicketUserJoinTicketStoreJoinStore>> GetTicketUserWithJoin(long userId, long ticketStoreId);
        Task<List<TicketUserJoinTicketStoreJoinStore>> getTicketsUsersBelongToStore(long storeId);
        Task<ActionResult<TicketUser>> GetTicketUser(int tempCode, long ticketStoreId);
        Task<int> CreateTicketUser(TicketUser ticketUser);
        Task<int> PutTicketUser(TicketUser ticketUser);
        bool Exists(long id);
    }
}
