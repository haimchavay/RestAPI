using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IStoreDA
    {
        Task<List<Store>> GetPage(int pageNumber, int pageSize);
        Task<ActionResult<Store>> GetStore(long id);
        Task<int> PutStore(Store store);
        Task<int> PostStore(Store store);
        Task<int> DeleteStore(long id, Store store);
        Task<int> GetRecords();
        bool Exists(long id);
    }
}
