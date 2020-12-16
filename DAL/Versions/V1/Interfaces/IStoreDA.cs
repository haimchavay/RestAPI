using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Versions.V1.Interfaces
{
    public interface IStoreDA
    {
        Task<List<Store>> GetPage(int pageNumber, int pageSize);
        Task<ActionResult<Store>> GetStore(long id);
        Task<int> PutStore(Store store);
        Task<int> PostStore(Store store);
        Task<int> DeleteStore(Store store);
        Task<int> GetRecords();
        bool Exists(long id);
    }
}
