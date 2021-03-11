using DAL.Versions.V1.DataContext;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using System.Threading.Tasks;

namespace DAL.Versions.V1.DataAccess
{
    public class PunchHistoryDA : IPunchHistoryDA
    {
        public async Task<int> CreatePunchHistory(PunchHistory punchHistory)
        {
            using var context = new DevTicketDatabaseContext(DevTicketDatabaseContext.ops.dbOptions);

            context.PunchingHistory.Add(punchHistory);
            return await context.SaveChangesAsync();
        }
    }
}
