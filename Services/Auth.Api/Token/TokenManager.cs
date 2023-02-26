using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Api.Token
{
    public class TokenModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string TokenType { get; set; }
    }
    public class AccessToken
    {
        public string Value { get; set; }
        public string RefreshValue { get; set; }
        public DateTime Expire { get; set; }
        public DateTime RefreshExpire { get; set; }
        public List<Claim> Claims { get; set; }
    }
    public class TokenManager
    {
        private readonly IConfiguration configuration;
        private TokenModel model;
        public TokenManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public AccessToken GenerateToken(TokenModel model)
        {
            this.model = model;

            var now = DateTime.Now;
            var expire = now.AddDays(Convert.ToDouble(configuration["Jwt:ExpirationDays"]));
            var expireRefresh = now.AddDays(Convert.ToDouble(configuration["Jwt:ExpirationRefreshDays"]));
            var claims = GetClaims();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = GetSigningCredentials(),
                NotBefore = now,
                Expires = expire,
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            var numberByte = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(numberByte);

            var refreshToken = Convert.ToBase64String(numberByte);

            return new AccessToken()
            {
                Value = token,
                Expire = expire,
                RefreshValue = refreshToken,
                RefreshExpire = expireRefresh,
                Claims = claims
            };
        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.AuthenticationMethod, JwtBearerDefaults.AuthenticationScheme),
                new Claim("token_type", model.TokenType)
            };

            foreach (var role in model.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
