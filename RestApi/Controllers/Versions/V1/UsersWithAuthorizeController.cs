using BLL.Versions.V1.BusinessLogic;
using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RestApi.Controllers.Versions.V1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersWithAuthorizeController : ControllerBase
    {
        private readonly IUserBL userBL;
        public IConfiguration configuration;

        public UsersWithAuthorizeController(IConfiguration config)
        {
            configuration = config;
            userBL = new UserBL(config);
        }

        [HttpGet("generate/")]
        public async Task<ActionResult<UserDTO>> GenerateTempCode()
        {
            //return await ticketUserBL.GenerateTempCode(HttpContext.User.Identity, ticketStoreId);
            return await userBL.GenerateTempCode(HttpContext.User.Identity);
        }
    }
}
