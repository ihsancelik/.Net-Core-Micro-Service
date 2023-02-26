using Auth.Api.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Auth.Api.Services
{
    public class TokenTypeService : BaseService
    {
        private readonly DataContext db;

        public TokenTypeService(DataContext db)
        {
            this.db = db;
        }

        public bool Create(TokenTypeCreateModel model)
        {
            bool exist = db.TokenTypes.Any(s => s.Value == model.Value);

            if (exist)
            {
                Exception = "Token Type already exist!";
                return false;
            }

            db.TokenTypes.Add(new TokenType()
            {
                Value = model.Value,
                MultiUsage = model.MultiUsage
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

        public bool Delete(int tokenTypeId)
        {
            var tokenType = db.TokenTypes
                .Include(s => s.Tokens)
                .FirstOrDefault(s => s.Id == tokenTypeId);

            if (tokenType == null)
            {
                Exception = "Token Type not found!";
                return false;
            }

            db.Tokens.RemoveRange(tokenType.Tokens);
            db.TokenTypes.Remove(tokenType);

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
    }

    public class TokenTypeCreateModel
    {
        public string Value { get; set; }
        public bool MultiUsage { get; set; }
    }
}
