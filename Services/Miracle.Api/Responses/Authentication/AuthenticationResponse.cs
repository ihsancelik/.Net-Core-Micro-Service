using Miracle.Api.Database;
using Miracle.Api.Responses.Base;
using System;
using System.Collections.Generic;

namespace Miracle.Api.Responses.Authentication
{
    public class AuthenticationResponse : BaseResponse
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string WebToken { get; set; }
        public string WebRefreshToken { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public AuthenticationResponse() : base()
        {

        }
        public AuthenticationResponse(bool success)
            : base(success)
        {
        }
        public AuthenticationResponse(Exception exception, bool success = false)
            : base(exception, success)
        {
        }
        public AuthenticationResponse(string exceptionMessage, bool success = false)
            : base(exceptionMessage, success)
        {
        }
        public AuthenticationResponse(IEnumerable<Exception> exceptionMessages, bool success = false)
            : base(exceptionMessages, success)
        {
        }
        public AuthenticationResponse(IEnumerable<string> exceptionMessages, bool success = false)
            : base(exceptionMessages, success)
        {
        }
        public AuthenticationResponse(DBResult dbResult) : base(dbResult)
        {
        }
        public void SetData(string username, string token, string refreshToken, string webToken, string webRefreshToken)
        {
            Username = username;
            Token = token;
            RefreshToken = refreshToken;
            WebToken = webToken;
            WebRefreshToken = webRefreshToken;
        }
    }
}
