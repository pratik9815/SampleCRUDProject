using DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExampleProject.Common.Helper
{
    //The jwt token consist of mainly 3 parts header, payload and the signature
    public class JwtTokenGenerator
    {
        //The IConfiguration parameter is passed in order to access the appsetting.json of the application
        public static string GenerateToken(ApplicationUser user, IConfiguration configuration)
        {
            var claim = GetClaims(user);
            return GetToken(claim, configuration);

        }
        //The claims are for generating second part of the jwt token called the payload which contains the 
        //information about the user 
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
            //The securityKey is for the header of the security key 
            //which contains the encoding type
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenDefination:JwtKey"]));

            //Here we define a method to encode data which is the third part of the jwt token
            //
            var securityToken = new JwtSecurityToken(issuer: _configuration["TokenDefination:JwtIssuer"],
                                                           audience: _configuration["TokenDefination:JwtAudience"],
                                                           claims: userClaims,
                                                           expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["TokenDefination:JwtValidMinutes"])),
                                                           signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
            //This code uses the writetoken method of the jwtsecuritytokenhandler class to create a string representation of the jwttoken    
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
