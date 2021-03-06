﻿using BLL.Versions.V1.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pagination.Filter;
using System.Threading.Tasks;

namespace BLL.Versions.V1.Interfaces
{
    public interface IStoreBL
    {
        Task<IActionResult> GetPage(PaginationFilter filter, HttpRequest request);
        Task<IActionResult> GetStore(long id);
        Task<IActionResult> PutStore(long id, StoreDTO storeDTO);
        Task<ActionResult<StoreDTO>> PostStore(StoreDTO storeDTO);
        Task<IActionResult> DeleteStore(long id);
    }
}
