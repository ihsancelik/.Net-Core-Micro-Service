using Library.Helpers.Message;
using Library.Responses.Common;
using Microsoft.EntityFrameworkCore;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Models.UserWatch;
using System;
using System.Linq;

namespace Miracle.Core.Api.Services.UserWatch
{
    public class UserWatchService : IUserWatchService
    {
        private readonly MainContext mainContext;

        public UserWatchService(MainContext mainContext)
        {
            this.mainContext = mainContext;
        }
        public void CheckOnlineStates()
        {
            var users = mainContext.UserWatch
                .Where(s => s.Online)
                .Where(s => s.LastPingTime < DateTime.UtcNow.AddMinutes(-1))
                .ToList();

            if (users.Count == 0)
                return;

            users.ForEach(s => s.Offline());

            mainContext.UpdateRange(users);
            mainContext.Save();
        }
        public void SetOnline(int userId)
        {
            var user = mainContext.UserWatch.FirstOrDefault(s => s.UserId == userId);
            if (user == null)
            {
                mainContext.UserWatch.Add(new Database.Models.UserWatch(userId));
                mainContext.Save();
                return;
            }

            user.StillOnline();
            mainContext.UserWatch.Update(user);
            mainContext.Save();
        }
        public void SetOffline(int userId)
        {
            var user = mainContext.UserWatch.FirstOrDefault(s => s.UserId == userId);
            if (user == null)
            {
                mainContext.UserWatch.Add(new Database.Models.UserWatch(userId));
                mainContext.Save();
                return;
            }

            user.Offline();
            mainContext.UserWatch.Update(user);
            mainContext.Save();
        }

        public ListResponse<UserWatchModel> GetOnlineUsers()
        {
            var users = mainContext.UserWatch
                .Where(s => s.Online)
                .Include(s => s.User)
                .ThenInclude(s => s.Company)
                .Select(s => new UserWatchModel()
                {
                    CompanyName = s.User.Company.Name,
                    Username = s.User.Username,
                    Name = s.User.Name,
                    Surname = s.User.Surname,
                    Email = s.User.Email
                });

            var response = new ListResponse<UserWatchModel>();
            response.SetData(users);
            return response;
        }
        public GetResponse<UserWatchModel> GetOnlineUser(int userId)
        {
            var userWatch = mainContext.UserWatch
                .Where(s => s.UserId == userId)
                .Include(s => s.User)
                .ThenInclude(s => s.Company)
                .FirstOrDefault();

            if (userWatch == null)
            {
                var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                return new GetResponse<UserWatchModel>(message);
            }

            var userWatchModel = new UserWatchModel()
            {
                CompanyName = userWatch.User.Company.Name,
                Username = userWatch.User.Username,
                Name = userWatch.User.Name,
                Surname = userWatch.User.Surname,
                Email = userWatch.User.Email
            };

            var response = new GetResponse<UserWatchModel>();
            response.SetData(userWatchModel);
            return response;
        }

        public GetResponse<object> GetCountResponse()
        {
            var response = new GetResponse<object>();
            response.SetData(mainContext.UserWatch.Count());
            return response;
        }
    }
}