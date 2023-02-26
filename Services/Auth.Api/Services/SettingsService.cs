using Auth.Api.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Auth.Api.Services
{
    public class SettingsService : BaseService
    {
        private readonly DataContext db;

        public SettingsService(DataContext db)
        {
            this.db = db;
        }

        public bool ChangeTokenTypeState(string tokenType, bool multiUsage)
        {
            var tokenTypeRecord = db.TokenTypes
                .Include(s => s.Tokens)
                .FirstOrDefault(s => s.Value == tokenType);

            if (tokenTypeRecord == null)
            {
                Exception = $"Token type not found! '{tokenType}'";
                return false;
            }

            if (tokenTypeRecord.MultiUsage == multiUsage)
            {
                Message = $"Token type multi usage is already {multiUsage}";
                return true;
            }

            if (tokenTypeRecord.MultiUsage)
            {
                var tokens = tokenTypeRecord.Tokens;
                db.Tokens.RemoveRange(tokens.Take(tokens.Count - 1));
            }

            tokenTypeRecord.MultiUsage = multiUsage;
            db.TokenTypes.Update(tokenTypeRecord);

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
}
