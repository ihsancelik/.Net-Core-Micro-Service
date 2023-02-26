using Library.Helpers.Constraints;
using Library.Helpers.Message;
using Library.Helpers.Security;
using Library.Responses.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Miracle.Core.Api.Services
{
    /// <summary>
    /// JWT Bearer Token Authentication yardımıyla kullanıcı yetkilendirmesi görevini üstlenir.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly MainContext db;
        private IConfiguration configuration;
        private string authType;

        public AuthenticationService(MainContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }

        /// <summary>
        /// Parametre olarak gönderilen
        /// kullanıcı için JWT Bearer tokenı oluşturur.
        /// İlk defa mı token oluşturalacak yoksa 
        /// refresh token ile mi token oluşturulacak
        /// kontrolünü kendi içinde sağlar.
        /// </summary>
        /// <param name="user">Kullanıcı referansı</param>
        /// <returns></returns>
        public AuthenticationResponse Authenticate(string username, string password, string authType)
        {
            this.authType = authType;

            var isUserExist = db.Users.Any(s => s.Username == username);

            var user = db.Users
                .Where(s => s.Username == username && s.Password == SHA512Encryptor.Encrypt(password))
                .Include(s => s.User_Roles)
                .ThenInclude(s => s.Role)
                .FirstOrDefault();

            if (user == null)
            {
                var message = isUserExist ?
                    MessageGenerator.Generate("Username or password", MessageGeneratorActions.Wrong) :
                    MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                return new AuthenticationResponse(message);
            }

            if (!user.IsActive)
            {
                var message = MessageGenerator.Generate("User", MessageGeneratorActions.IsNotActive);
                return new AuthenticationResponse(message);
            }

            BuildToken(ref user);
            BuildRefreshToken(ref user);

            var response = new AuthenticationResponse();
            var roles = user.User_Roles.Select(s => s.Role.Value);
            response.SetData(user.Username, user.Token, user.RefreshToken, user.WebToken, user.WebRefreshToken, roles);
            return response;
        }

        /// <summary>
        /// Parametre olarak gönderilen
        /// kullanıcı için JWT Bearer tokenı oluşturur.
        /// İlk defa mı token oluşturalacak yoksa 
        /// refresh token ile mi token oluşturulacak
        /// kontrolünü kendi içinde sağlar.
        /// </summary>
        /// <param name="user">Kullanıcı referansı</param>
        /// <returns></returns>
        public AuthenticationResponse AuthenticateByRefreshToken(string refreshToken, string authType)
        {
            this.authType = authType;

            if (refreshToken == null)
            {
                return new AuthenticationResponse(false);
            }

            User user = null;

            if (authType == AuthTypeConstraints.Application)
            {
                user = db.Users.FirstOrDefault(s => s.RefreshToken == refreshToken);

                if (user == null)
                {
                    var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                    return new AuthenticationResponse(message);
                }
                if (user.RefreshTokenExpire < DateTime.Now)
                {
                    var message = MessageGenerator.Generate("Refresh Token", MessageGeneratorActions.Expired);
                    return new AuthenticationResponse(message);
                }
            }
            else
            {
                user = db.Users.FirstOrDefault(s => s.WebRefreshToken == refreshToken);

                if (user == null)
                {
                    var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                    return new AuthenticationResponse(message);
                }
                if (user.WebRefreshTokenExpire < DateTime.Now)
                {
                    var message = MessageGenerator.Generate("Refresh Token", MessageGeneratorActions.Expired);
                    return new AuthenticationResponse(message);
                }
            }

            BuildToken(ref user);

            var response = new AuthenticationResponse();
            var roles = user.User_Roles.Select(s => s.Role.Value);
            response.SetData(user.Username, user.Token, user.RefreshToken, user.WebToken, user.WebRefreshToken, roles);
            return response;
        }

        /// <summary>
        /// Parametre olarak gönderilen kullanıcının
        /// tokenını yok eder. Bu kullanıcı artık yetkisizdir.
        /// </summary>
        /// <param name="id">Kullanıcı referansı</param>
        /// <returns></returns>
        public AuthenticationResponse RevokeAuthenticate(int userId, string authType)
        {
            this.authType = authType;

            var user = db.Users.FirstOrDefault(s => s.Id == userId);

            if (user == null)
            {
                var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                return new AuthenticationResponse(message);
            }

            if (authType == AuthTypeConstraints.Application)
            {
                user.Token = null;
                user.TokenExpire = null;
                user.RefreshToken = null;
                user.RefreshTokenExpire = null;
            }
            else
            {
                user.WebToken = null;
                user.WebTokenExpire = null;
                user.WebRefreshToken = null;
                user.WebRefreshTokenExpire = null;
            }

            db.Users.Update(user);
            var dbResult = db.Save();
            return new AuthenticationResponse(dbResult);
        }


        private void BuildToken(ref User user)
        {
            var tokenExpiration = GetTokenExpiration();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = GetSigningCredentials(),
                NotBefore = DateTime.UtcNow,
                Expires = tokenExpiration,
                Subject = new ClaimsIdentity(GetClaims(user)),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            if (authType == AuthTypeConstraints.Application)
            {
                user.Token = tokenHandler.WriteToken(token);
                user.TokenExpire = tokenExpiration;
            }
            else
            {
                user.WebToken = tokenHandler.WriteToken(token);
                user.WebTokenExpire = tokenExpiration;
            }

            db.Users.Update(user);
            db.Save();
        }
        private string BuildRefreshToken(ref User user)
        {
            var numberByte = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(numberByte);

            var refreshToken = Convert.ToBase64String(numberByte);

            if (authType == AuthTypeConstraints.Application)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpire = GetRefreshTokenExpiration();
            }
            else
            {
                user.WebRefreshToken = refreshToken;
                user.WebRefreshTokenExpire = GetRefreshTokenExpiration();
            }

            db.Users.Update(user);
            db.Save();
            return refreshToken;
        }
        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.AuthenticationMethod, JwtBearerDefaults.AuthenticationScheme),
                new Claim("Authentication-Type", authType)
            };

            var roles = db.User_Roles
                .Where(s => s.UserId == user.Id)
                .Include(s => s.Role)
                .Select(s => s.Role)
                .ToList();

            foreach (var item in roles)
                claims.Add(new Claim(ClaimTypes.Role, item.Value));

            return claims;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
        private DateTime GetTokenExpiration()
        {
            return DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["Jwt:TokenExpirationDays"]));
        }
        private DateTime GetRefreshTokenExpiration()
        {
            return DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["Jwt:RefreshTokenExpirationDays"]));
        }
    }
}
