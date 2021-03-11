using DAL.Versions.V1.Entities;
using System.Threading.Tasks;

namespace DAL.Versions.V1.Interfaces
{
    public interface IPunchHistoryDA
    {
        Task<int> CreatePunchHistory(PunchHistory punchHistory);
    }
}
