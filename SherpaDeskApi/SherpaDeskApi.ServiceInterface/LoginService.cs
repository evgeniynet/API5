// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;

namespace SherpaDeskApi.ServiceInterface
{
   //[Authenticate("GoogleOpenId")]
    public class LoginService : Service
    {
        public object Any(Login request)
        {
            var basicAuth = base.Request.GetBasicAuthUserAndPassword();
            string basicAuthEmail = "";
            string basicAuthPassword = "";
            if (basicAuth != null)
            {
                basicAuthEmail = basicAuth.Value.Key;
                basicAuthPassword = basicAuth.Value.Value;
            }
            string userName = request.username ?? basicAuthEmail;
            string userPass = request.password ?? basicAuthPassword;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPass))
            {
               //return null;
                base.Response.AddHeader(HttpHeaders.WwwAuthenticate, "Basic realm=\"/login\"");
                throw new HttpError(HttpStatusCode.Forbidden, "Incorrect login/password.");
            }
           //if not google
           //if (userPass.Length != 70 && !userPass.StartsWith("ya29.1.AADtN_"))
           //{
                if (!ApiUser.ValidateStatic(userName, userPass)) throw new HttpError(HttpStatusCode.Forbidden, "Login or Password is not correct.");
           //}

            string api_token = Micajah.Common.Bll.Providers.LoginTokenProvider.GetApiToken(userName);
            if (string.IsNullOrEmpty(api_token))
                throw new HttpError(HttpStatusCode.Forbidden, "User is not correct or inactive.");
           //var hdUser = new ApiUser(api_token);
            return new LoginResponse { api_token = api_token };
        }
    }
}
