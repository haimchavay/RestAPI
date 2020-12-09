using BLL.Interfaces;
using DAL.DataAccess;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BusinessLogic
{
    public class UserInfoBL : IUserInfoBL
    {
        private IUserInfoDA userInfoDA = new UserInfoDA();
        private IConfiguration configuration;

        public UserInfoBL(IConfiguration config)
        {
            this.configuration = config;
        }

        public async Task<IActionResult> GetToken(UserInfo userData)
        {
            if (userData != null && userData.Email != null && userData.Password != null)
            {
                var user = await userInfoDA.GetUserInfo(userData.Email, userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        configuration["Jwt:Issuer"],
                        configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);

                    return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return new BadRequestObjectResult("Invalid credentials");
                }
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
