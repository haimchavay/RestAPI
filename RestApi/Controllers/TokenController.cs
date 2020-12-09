using BLL.BusinessLogic;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IUserInfoBL userInfoBL;
        public IConfiguration configuration;

        public TokenController(IConfiguration config)
        {
            configuration = config;
            userInfoBL = new UserInfoBL(config);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userData)
        {
            return await userInfoBL.GetToken(userData);
        }
    }
}
