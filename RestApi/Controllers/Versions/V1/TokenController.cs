using BLL.Versions.V1.BusinessLogic;
using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RestApi.Controllers.Versions.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserBL userBL;
        public IConfiguration configuration;

        public TokenController(IConfiguration config)
        {
            configuration = config;
            userBL = new UserBL(config);
        }

        [HttpPost]
        public async Task<IActionResult> Login(User userData)
        {
            return await userBL.GetToken(userData);
        }
    }
}
