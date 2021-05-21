using DAL.Versions.V1.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Versions.V1.Interfaces
{
    public interface IPunchHistoryDA
    {
        Task<int> CreatePunchHistory(PunchHistory punchHistory);
        Task<List<PunchHistory>> GetpunchesHistories(long ticketUserId);
    }
}
