using Auth.Api.Database;
using Auth.Api.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.Api.Services
{
    public class AuthService : BaseService
    {
        private readonly CoreDataContext coreDb;
        private readonly DataContext db;
        private readonly TokenManager tokenManager;

        public string GeneratedToken = null;
        public string GeneratedRefreshToken = null;

        public AuthService(CoreDataContext coreDb, DataContext db, TokenManager tokenManager)
        {
            this.coreDb = coreDb;
            this.db = db;
            this.tokenManager = tokenManager;
        }

        public bool Login(string username, string password, string tokenType, string userAgent)
        {
            if (string.IsNullOrEmpty(tokenType))
            {
                Exception = "Missing some headers for authentication!";
                return false;
            }

            if (string.IsNullOrEmpty(userAgent))
            {
                Exception = "Missing user agent info!";
                return false;
            }

            var tokenTypeRecord = db.TokenTypes.FirstOrDefault(s => s.Value == tokenType);
            if (tokenTypeRecord == null)
            {
                Exception = "This token type is not defined for authentication!";
                return false;
            }

            var user = GetUser(username, password);
            if (user == null)
                return false;

            var accessToken = tokenManager.GenerateToken(new TokenModel()
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                Roles = user.Roles,
                TokenType = tokenType
            });


            var tokenRecord = db.Tokens.FirstOrDefault(s => s.UserId == user.Id && s.TokenTypeId == tokenTypeRecord.Id);
            if (tokenTypeRecord.MultiUsage || tokenRecord == null)
            {
                tokenRecord = new Database.Token()
                {
                    UserId = user.Id,
                    Expire = accessToken.Expire,
                    RefreshExpire = accessToken.RefreshExpire,
                    Value = accessToken.Value,
                    RefreshValue = accessToken.RefreshValue,
                    TokenTypeId = tokenTypeRecord.Id
                };
                db.Tokens.Add(tokenRecord);
            }
            else
            {
                tokenRecord.Expire = accessToken.Expire;
                tokenRecord.Value = accessToken.Value;
                db.Tokens.Update(tokenRecord);
            }

            db.UserAgents.Add(new UserAgent()
            {
                UserId = user.Id,
                Value = userAgent
            });

            try
            {
                DatabaseNumberOfChanges = db.SaveChanges();
                GeneratedToken = accessToken.Value;
                GeneratedRefreshToken = accessToken.RefreshValue;
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
                return false;
            }

            return true;
        }
        public async Task<bool> LoginWithCookieAsync(HttpContext context, string username, string password, string userAgent)
        {
            if (context == null)
            {
                Exception = "Context is null!";
                return false;
            }

            if (string.IsNullOrEmpty(userAgent))
            {
                Exception = "Missing user agent info!";
                return false;
            }

            var user = GetUser(username, password);
            if (user == null)
                return false;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.AuthenticationMethod, CookieAuthenticationDefaults.AuthenticationScheme),
            };
            foreach (string role in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            try
            {
                await context.SignOutAsync();
                await context.SignInAsync(principal);
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
            }

            db.UserAgents.Add(new UserAgent()
            {
                UserId = user.Id,
                Value = userAgent
            });

            try
            {
                DatabaseNumberOfChanges = db.SaveChanges();
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
                return false;
            }

            return true;
        }
        public List<string> GetTokenTypes()
        {
            return db.TokenTypes.Select(s => s.Value).ToList();
        }
        public CoreUserModel GetUser(string username, string password)
        {
            var usernameExist = coreDb.Users.Any(s => s.Username == username);
            var passwordExist = coreDb.Users.Any(s => s.Password == password);

            if (!usernameExist && !passwordExist)
            {
                Exception = "This user not registered!";
                return null;
            }

            var result = (usernameExist && passwordExist);

            if (!result)
            {
                Exception = "Username or password wrong!";
                return null;
            }

            var userModel = coreDb.Users
                .Include(s => s.User_Roles)
                .ThenInclude(s => s.Role)
                .Where(s => s.Username == username && s.Password == password)
                .Select(s => new CoreUserModel()
                {
                    Id = s.Id,
                    Email = s.Email,
                    Username = s.Username,
                    Roles = s.User_Roles.Select(s => s.Role.Value).ToList()
                })
                .FirstOrDefault();

            if (userModel.Roles.Count == 0)
            {
                Exception = "This user don't have any role!";
                return null;
            }

            return userModel;
        }

        public bool UserIsValid(int id, string token)
        {
            var userExist = coreDb.Users.Any(s => s.Id == id);
            if (!userExist)
            {
                Exception = "This user not registered!";
                return false;
            }

            var tokenExist = db.Tokens.Any(s => s.UserId == id && s.Value == token);
            if (!tokenExist)
            {
                Exception = "User token not valid no more!";
                return false;
            }

            return true;
        }
    }

    public class CoreUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
