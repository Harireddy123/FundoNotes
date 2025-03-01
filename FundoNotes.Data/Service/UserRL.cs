using System;
using FundoNotes.Data.Models;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RepositoryLayer.Helpers;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundoContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserRL(FundoContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        public bool EmailExists(string email) =>

            _dbContext.Users.Any(e => e.Email == email);

        public User RegisterUser(RegisterModel model)
        {
            try
            {
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = UserHelpers.EncodePassword(model.Password),
                    DOB = model.DOB,
                    Gender = model.Gender
                };

                _dbContext.Add(user);
                _dbContext.SaveChanges();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user", ex);
            }
        }

        public string Login(LoginModel model)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                    return null;

                string decodedPassword = UserHelpers.Decode(user.Password);
                return decodedPassword == model.Password ? GenerateToken(user.Email, user.Id) : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during login", ex);
            }
        }

        private string GenerateToken(string Email, int userId)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim("custom_email", Email),
                    new Claim("id", userId.ToString())
                };
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(50),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating JWT token", ex);
            }
        }


        
    }
}
