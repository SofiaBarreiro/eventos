using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventosCeremonial.Helpers
{
    //public class JWTHelper : IJwtAuthenticationService
    //{
    //    //private readonly string secret;

    //    //public JWTHelper(string secretKey)
    //    //{
    //    //    this.secret = secretKey;
    //    //}
    //    //public string CreateToken(string @userName, string password)
    //    //{

    //    //    var claims = new ClaimsIdentity();
    //    //    claims.AddClaim(new Claim(ClaimTypes.Name, userName));


    //    //    var tokenHandler = new JwtSecurityTokenHandler();


    //    //    var tokenKey = Encoding.ASCII.GetBytes(secret);



    //    //    var tokenDescription = new SecurityTokenDescriptor(){

    //    //        Subject = claims,
    //    //        Expires = DateTime.UtcNow.AddHours(7),
    //    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)};


    //    //    var createdToken = tokenHandler.CreateToken(tokenDescription);


    //    //    return tokenHandler.WriteToken(createdToken);
    //    //}


    //}
}
