using BLL.Versions.V1.BusinessLogic;
using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pagination.Filter;
using System.Threading.Tasks;

namespace RestApi.Controllers.Versions.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBL userBL;
        public IConfiguration configuration;

        public UsersController(IConfiguration config)
        {
            configuration = config;
            userBL = new UserBL(config);
        }

        /*[HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] PaginationFilter filter)
        {
            return await userBL.GetPage(filter, Request);
        }*/

        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            return await userBL.GetUser(id);
        }*/

        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            return await userBL.PutUser(id, user);
        }*/

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            return await userBL.PostUser(user);
        }

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            return await userBL.DeleteUser(id);
        }*/
    }
}
