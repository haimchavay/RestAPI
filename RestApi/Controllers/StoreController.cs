using BLL.BusinessLogic;
using BLL.DataTransferObjects;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination.Filter;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IStoreBL storeBL = new StoreBL();

        /// <summary> This function gets page number, page size and returns the desired page data with stores
        ///     <example> For example:
        ///         <code>GET: api/store?pageNumber=2&pageSize=3</code>
        ///         Results in this case: page 2 with 3 stores (4 - 6)
        ///     </example>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] PaginationFilter filter)
        {
            return await storeBL.GetPage(filter, Request);
        }

        /// <summary> This function gets store id and returns 'Store' object
        ///     <example> For example:
        ///         <code>GET: api/store/5</code>
        ///         Results in this case: Object of store id number 5 with status code
        ///     </example>
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(long id)
        {
            return await storeBL.GetStore(id);
        }

        /// <summary> This function gets store id, new Store object and updates the existing store object by the new one
        ///     <example> For example:
        ///         <code>PUT: api/store/5
        ///                    pass new Store object in the body of the request
        ///         </code>
        ///         Results in this case: Only status code
        ///     </example>
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(long id, StoreDTO storeDTO)
        {
            return await storeBL.PutStore(id, storeDTO);
        }


        /// <summary> This function gets new store object, store id and create new store object by the new one
        ///     <example> For example:
        ///         <code>POST: api/store</code>
        ///         Results in this case: status code with a Location header
        ///     </example>
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<StoreDTO>> PostStore(StoreDTO storeDTO)
        {
            return await storeBL.PostStore(storeDTO);
        }

        /// <summary> This function gets store id and delete the exist store object
        ///     <example> For example:
        ///         <code>DELETE: api/DummyUser/5</code>
        ///         Results in this case: status code
        ///     </example>
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            return await storeBL.DeleteStore(id);
        }

    }
}
