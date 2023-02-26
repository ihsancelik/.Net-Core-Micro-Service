using Library.Helpers.ExceptionManager;
using Library.Helpers.Message;
using Library.Helpers.Security;
using Library.Responses.Common;
using Library.Responses.Database;
using Miracle.Core.Api.Database;
using Miracle.Core.Api.Database.Models;
using System;
using System.Linq;
using System.Net.Mail;

namespace Miracle.Core.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly MainContext db;
        private readonly IMailService mailService;
        private DatabaseResponse dbResponse;
        public ExceptionManager ExceptionManager { get; set; }
        public AccountService(MainContext db, IMailService mailService)
        {
            this.db = db;
            this.mailService = mailService;
            dbResponse = new DatabaseResponse();
            ExceptionManager = new ExceptionManager();
        }
        public EmptyResponse ResetPassword(int id, string password)
        {
            var data = db.Users.FirstOrDefault(s => s.Id == id);
            if (data == null)
            {
                var message = MessageGenerator.Generate("User", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            data.Password = SHA512Encryptor.Encrypt(password);
            db.Users.Update(data);
            var dbResult = db.Save();
            return new EmptyResponse(dbResult);
        }

        public EmptyResponse ForgotPasswordRequest(string email)
        {
            var data = db.Users.FirstOrDefault(u => u.Email == email);
            if (data == null)
            {
                var message = MessageGenerator.Generate("Email", MessageGeneratorActions.NotFound);
                return new EmptyResponse(message);
            }

            var code = Guid.NewGuid().ToString();
            var resetPassword = new ResetPassword()
            {
                UserId = data.Id,
                ExpireDate = DateTime.Now.AddMinutes(30),
                Code = code,
                IsUsed = false
            };

            var smtp = db.SMTPSettings.FirstOrDefault();
            mailService.Initialize(smtp.Host, smtp.Port, smtp.Email, smtp.Password, smtp.EnableSSL);
            db.ResetPasswords.Add(resetPassword);
            var dbResult = db.Save();
            var result = false;
            if (dbResult.Success)
            {
                var mailMessage = new MailMessage(smtp.Email, email, "Password Reset Code", "http://admin.miracle-software.com/account/forgot-password-res/" + code);
                result = mailService.Send(mailMessage);
            }

            return new EmptyResponse(result);

        }
        public EmptyResponse ForgotPasswordResponse(string code, string password)
        {
            var data = db.ResetPasswords.FirstOrDefault(s => s.Code == code);
            if (data == null)
                return new EmptyResponse(MessageGenerator.Generate("ResetPassword", MessageGeneratorActions.NotFound));

            if (data.IsUsed)
                return new EmptyResponse(MessageGenerator.Generate("Code", MessageGeneratorActions.AlreadyUsed));

            if (data.ExpireDate < DateTime.Now)
                return new EmptyResponse(MessageGenerator.Generate("Code", MessageGeneratorActions.Expired));

            var result = ResetPassword(data.User.Id, password);

            data.IsUsed = true;
            db.ResetPasswords.Update(data);
            db.Save();

            return new EmptyResponse(result.Success);
        }

        public DatabaseResponse Create(User model)
        {
            model.Username = model.Username.ToLower();
            model.Password = SHA512Encryptor.Encrypt(model.Password);

            var exist = db.Users.Any(s => s.Email == model.Email);
            if (exist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("User Email", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            exist = db.Users.Any(s => s.Username == model.Username);
            if (exist)
            {
                ExceptionManager.AddException(MessageGenerator.Generate("Username", MessageGeneratorActions.Exist));
                return dbResponse;
            }

            db.Users.Add(model);
            dbResponse = db.Save();
            if (!dbResponse.Success)
                ExceptionManager.AddException(dbResponse.Exception);

            return dbResponse;
        }
        public EmptyResponse CreateResponse(User model)
        {
            Create(model);

            if (ExceptionManager.HaveException)
                return new EmptyResponse(ExceptionManager.Exceptions);

            return new EmptyResponse(dbResponse);
        }
    }
}