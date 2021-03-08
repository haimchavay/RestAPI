using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Helpers;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.DataAccess;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pagination.DO;
using Pagination.Filter;
using Pagination.Helpers;
using Pagination.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Versions.V1.BusinessLogic
{
    public class UserBL : IUserBL
    {
        private readonly IUserDA userDA;
        private readonly IConfiguration configuration;

        public UserBL(IConfiguration config)
        {
            userDA = new UserDA();
            this.configuration = config;
        }
        public async Task<IActionResult> GetPage(PaginationFilter filter, HttpRequest request)
        {
            var route = request.Path.Value;
            var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            IUriService uriService = new UriService(uri);
            PaginationFilter validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<User> userList = await userDA.GetPage(validFilter.PageNumber, validFilter.PageSize);
            List<UserDTO> userDtoList = new List<UserDTO>();

            foreach (User user in userList)
            {
                userDtoList.Add(ItemToDTO(user));
            }

            int totalRecords = await userDA.GetRecords();
            Information info = InformationHelper.FillInformation(totalRecords, validFilter.PageNumber, validFilter.PageSize);

            var pagedResponse = PaginationHelper.CreatePagedReponse<UserDTO>(
                userDtoList, info, validFilter, totalRecords, uriService, route
                );

            return new OkObjectResult(pagedResponse);
        }
        public async Task<IActionResult> GetUser(long id)
        {
            ActionResult<User> action = await userDA.GetUser(id);

            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            User user = action.Value;
            UserDTO userDTO = ItemToDTO(user);

            return new OkObjectResult(userDTO);
        }
        /*
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return new BadRequestResult();
            }

            ActionResult<User> action = await userDA.GetUser(user.Id);
            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            User userPost = action.Value;
            userPost.FirstName = user.FirstName;
            userPost.LastName = user.LastName;
            userPost.Phone = user.Phone;
            userPost.Area = user.Area;
            userPost.City = user.City;
            userPost.Street = user.Street;
            userPost.Email = user.Email;
            userPost.Password = user.Password;
            userPost.UserName = user.UserName;
            userPost.CreatedDate = user.CreatedDate;
            userPost.LastVisited = user.LastVisited;
            userPost.UserTypeId = user.UserTypeId;
            userPost.FacebookSocialId = user.FacebookSocialId;
            userPost.GmailSocialId = user.GmailSocialId;

            try
            {
                await userDA.PutUser(user);
            }
            catch (DbUpdateConcurrencyException) when (!userDA.Exists(id))
            {
                return new NotFoundResult();
            }

            return new NoContentResult();
        }
        */
        public async Task<ActionResult<UserDTO>> CreateUser(User user)
        {
            // Email exist in database
            if (userDA.IsEmailExists(user.Email))
            {
                return new ConflictObjectResult("The email already exists");
            }
            // Phone exist in database
            if (userDA.IsPhoneExists(user.Phone))
            {
                return new ConflictObjectResult("The phone already exists");
            }

            /*// User exist in database
            if (IsUserExist(user))
            {
                return new ConflictObjectResult("The user already exists");
            }*/

            // Hashing password with BCrypt
            user.Password = Hashing.HashPassword(user.Password);

            await userDA.CreateUser(user);

            return new CreatedAtRouteResult(new { Id = user.Id }, ItemToDTO(user));
        }
        public async Task<IActionResult> DeleteUser(long id)
        {
            ActionResult<User> action = await userDA.GetUser(id);

            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }
            await userDA.DeleteUser(action.Value);

            return new NoContentResult();
        }
        public async Task<IActionResult> GetToken(User userData)
        {
            // Verifies request credentials
            if ( !(userData != null && userData.Email != null && userData.Password != null) )
            {
                return new BadRequestResult();
            }

            // Verifies credentials
            /*var user = await userDA.GetUser(userData.Email, userData.Password);
            if ( !(user != null) )
            {
                return new BadRequestObjectResult("Invalid credentials");
            }*/

            var user = await userDA.GetUser(userData.Email);
            if (!(user != null))
            {
                return new BadRequestObjectResult("Invalid credentials");
            }
            // Validate userPassword with hashPassword with BCrypt
            if( !Hashing.ValidatePassword(userData.Password, user.Password) )
            {
                return new BadRequestObjectResult("Invalid credentials");
            }

            //Create claims details based on the user information
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("Phone", user.Phone),
                    new Claim("Email", user.Email)
                   };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddYears(1),
                signingCredentials: signIn);

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return new OkObjectResult(TokenToTokenDTO(user, tokenStr));
        }

        private static UserDTO ItemToDTO(User user) =>
            new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Area = user.Area,
                City = user.City,
                Street = user.Street,
                Email = user.Email,
                UserName = user.UserName,
                LastVisited = user.LastVisited,
                UserTypeId = user.UserTypeId,
                PhotoPath = user.PhotoPath
            };
        private static TokenDTO TokenToTokenDTO(User user, string token) =>
            new TokenDTO
            {
                Token = token,
                FirstName = user.FirstName,
                Phone = user.Phone,
                Email = user.Email
            };
        private bool IsUserExist(User user)
        {
            return userDA.IsExists(user.Phone, user.Email);
        }
    }
}
