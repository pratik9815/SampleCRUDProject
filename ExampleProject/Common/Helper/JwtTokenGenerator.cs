﻿using DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExampleProject.Common.Helper
{
    public class JwtTokenGenerator
    {
        public static string GenerateToken(ApplicationUser user, IConfiguration configuration)
        {
            var claim = GetClaims(user);
            return GetToken(claim, configuration);

        }
        private static List<Claim> GetClaims(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Username", user.UserName),
                new Claim("Email", user.Email),
            };
            return claims;

        }
        private static string GetToken(List<Claim> userClaims, IConfiguration _configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenDefination:JwtKey"]));

            var securityToken = new JwtSecurityToken(issuer: _configuration["TokenDefination:JwtIssuer"],
                                                           audience: _configuration["TokenDefination:JwtAudience"],
                                                           claims: userClaims,
                                                           expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["TokenDefination:JwtValidMinutes"])),
                                                           signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
           
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}