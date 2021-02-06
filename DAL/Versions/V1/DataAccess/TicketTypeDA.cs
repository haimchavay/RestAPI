using DAL.Versions.V1.DataContext;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DAL.Versions.V1.DataAccess
{
    public class TicketTypeDA : ITicketTypeDA
    {
        public async Task<ActionResult<TicketType>> GetTicketType(int id)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            return await context.TicketTypes.FindAsync(id);
        }
    }
}
