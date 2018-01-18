// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Micajah.Common.Bll;
using System.Threading.Tasks;
using System.Net;
using Skype4Sharp;
using Skype4Sharp.Events;
using Skype4Sharp.Auth;
using Skype4Sharp.Helpers;
using Skype4Sharp.Enums;
using ServiceStack.Common.Web;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using bigWebApps.bigWebDesk.Data;
using Newtonsoft.Json;
using Microsoft.Bot.Connector;
using System.Configuration;
using Micajah.Common.Extensions;
using ServiceStack;
using SherpaDeskApi.ServiceModel;

namespace SherpaDeskApi.ServiceInterface
{
    [Route("/ping")]
    [Route("/favicon.ico")]
    [Route("/index.htm")]
    [Route("/index.php")]
    public class Ping {
        public string name { get; set; }
    }

    public class PingService : Service
    {
        public object Any(Ping ping)
        {
            if (string.IsNullOrWhiteSpace(ping.name))
                return "All OK!";
            string email = ping.name;
            Micajah.Common.Bll.Providers.LoginProvider lp = new Micajah.Common.Bll.Providers.LoginProvider();
            string paid = "false";
            var login_id = lp.GetLoginId(email);
            if (login_id != Guid.Empty)
            {
                var orgs = lp.GetOrganizationsByLoginId(login_id);
                paid = "lead";
                if (orgs.Count > 1)
                    paid = "opportunity";
                foreach (var o in orgs)
                {
                    if (lp.LoginIsOrganizationAdministrator(login_id, o.OrganizationId))
                    {
                        var insts = lp.GetLoginInstances(login_id, o.OrganizationId).FirstOrDefault(i => i.BillingPlan == BillingPlan.Paid && i.CreditCardStatus == CreditCardStatus.Registered);
                        if (insts != null)
                        {
                            paid = "customer";
                            break;
                        }
                        insts = lp.GetLoginInstances(login_id, o.OrganizationId).FirstOrDefault(i => i.BillingPlan == BillingPlan.Paid && i.CreditCardStatus == CreditCardStatus.Expired);
                        if (insts != null)
                        {
                            paid = "ex-customer";
                            break;
                        }
                        if (o.Expired || o.ExpirationTime.HasValue && (o.ExpirationTime.Value - DateTime.UtcNow).Days < 0)
                            paid = "expired";
                    }
                }
            }
            /*
            var org = Micajah.Common.Bll.Providers.OrganizationProvider.GetOrganizationByPseudoId("ncg1in");
            var inst = Micajah.Common.Bll.Providers.InstanceProvider.GetInstanceByPseudoId("8d1rag", org.OrganizationId);
            string url = lp.GetLoginUrl("patrick.clements@bigwebapps.com", true, org.OrganizationId, inst.InstanceId, "");
            */
            return paid;
            return "All OK!";
        }
    }

    [Route("/hubspot/get")]
    public class HubspotGet
    {
        public string index { get; set; }
    }

    [Route("/hubspot/post")]
    public class HubspotPost
    {
        public string email { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string old_value { get; set; }
    }

    public class HubspotService : Service
    {
        public object Any(HubspotGet hubspot)
        {
            string hubspot_url = "";
            if (hubspot.index.IndexOf("@") > 0)
                hubspot_url = string.Format("https://api.hubapi.com/contacts/v1/search/query?q={0}&hapikey=2544c445-c609-4712-9088-f240c582799a&property=lifecyclestage&propertyMode=value_only&count=1", hubspot.index);
            else
                hubspot_url = string.Format("https://api.hubapi.com/contacts/v1/lists/all/contacts/all?hapikey=2544c445-c609-4712-9088-f240c582799a&property=lifecyclestage&propertyMode=value_only&count=1&vidOffset={0}", hubspot.index);
            var hubspot_Json = WebR.GetApiJson(hubspot_url, "");
            //var token = JsonObject.Parse(ghost_Json).Object("data").Object("extractions").Child("apikey");
            return hubspot_Json;
        }

        public object Any(HubspotPost hubspot)
        {
            string email = hubspot.email;
            string name = hubspot.name;
            string value = hubspot.value;
            if (string.IsNullOrEmpty(email))
            {
                return "";
            }

            if (!string.IsNullOrWhiteSpace(hubspot.old_value) && ((hubspot.old_value == "customer" && hubspot.value != "customer") || (hubspot.old_value == "lead" && hubspot.value != "lead")))
            {
                value = "";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("{ \"properties\": [");
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, " {{ \"property\": \"{0}\", \"value\": \"{1}\" }}", name, value);
            sb.Append(" ] }");


            string data = sb.ToString();

            string hubspot_url = string.Format("https://api.hubapi.com/contacts/v1/contact/email/{0}/profile?hapikey=2544c445-c609-4712-9088-f240c582799a", email);
            var hubspot_Json = WebR.PostApiJson(hubspot_url, data);

            if (!string.IsNullOrWhiteSpace(hubspot.old_value) && ((hubspot.old_value == "customer" && hubspot.value != "customer") || (hubspot.old_value == "lead" && hubspot.value != "lead")))
            {
                value = hubspot.value;
                sb = new StringBuilder();

                sb.Append("{ \"properties\": [");
                sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, " {{ \"property\": \"{0}\", \"value\": \"{1}\" }}", name, value);
                sb.Append(" ] }");

                data = sb.ToString();

                hubspot_Json = WebR.PostApiJson(hubspot_url, data);
            }
            //var token = JsonObject.Parse(ghost_Json).Object("data").Object("extractions").Child("apikey");
            return "true";
        }
    }

    /*
     if (m_UserContext != null && m_UserContext.InstanceId != Guid.Empty)
                {
                    Instance inst = m_UserContext.Instance;
                    DateTime? expDate = m_UserContext.Organization.ExpirationTime;

                    if (inst.BillingPlan == BillingPlan.Paid && inst.CreditCardStatus != CreditCardStatus.Registered && expDate.HasValue && (expDate.Value - DateTime.UtcNow).Days < 0)
                    {
                        HeaderMessage = "We see your account is over the free usage limit. Please update your credit card. You have 15 days before your account expires.";
                        HeaderMessageType = Micajah.Common.WebControls.NoticeMessageType.Warning;
                        if (!IsPostBack && m_UserContext.IsOrganizationAdministrator)
                        {
                            Random rnd = new Random(DateTime.UtcNow.Millisecond);
                            if (rnd.Next(1, 5) == 3) ShowCreditCardRegistrationWindow = true;
                        }
                    }
                }
     */

    [Route("/robots.txt")]
    public class Robots { }

    public class RobotsService : Service
    {
        public object Any(Robots ping)
        {
            return "User-agent: * \nDisallow: /";
        }
    }

    [Route("/getid")]
    public class get_id
    {
        public string name { get; set; }
        public string org { get; set; }
        public string inst { get; set; }
        public string s { get; set; }
    }

    [Secure]
    [Route("/skype/login")]
    public class skype_login : ApiRequest
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string skype { get; set; }
        public string conversation_id { get; set; }
    }

    [Route("/skype/search")]
    [Route("/skype_search")]
    public class skype_search
    {
        public string name { get; set; }
        public string token { get; set; }
    }

    [Route("/skype/save_token")]
    public class skype_token
    {
        public string token { get; set; }
        public bool force { get; set; }
    }

    [Route("/skype/group")]
    public class skype_group
    {
        public string ticket { get; set; }
        public string subject { get; set; }
        //public string note { get; set; }
        public string skype { get; set; }
        //public string emails { get; set; }
        public string data { get; set; }
    }

    [Secure]
    [Route("/skype/invite")]
    public class skype_invite : ApiRequest
    {
        public string ticket { get; set; }
        public string subject { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public string emails { get; set; }
    }

    public class skype_searchService : Service
    {

        public object Any(get_id request)
        {
            if (string.IsNullOrWhiteSpace(request.name) || string.IsNullOrWhiteSpace(request.org) || string.IsNullOrWhiteSpace(request.inst) || "BigWebApps88" != request.s)
            {
                return "";
            }
            string token = Micajah.Common.Bll.Providers.LoginTokenProvider.GetApiToken(request.name);
            if (string.IsNullOrWhiteSpace(token))
            {
                return "";
            }
            ApiUser hdUser = null;
            string error = "";
            try
            {
                hdUser = ApiUser.getUser(token, request.org, request.inst);
            }
            catch (Exception ex)
            {
                error = "ERROR: " + ex.Message;
            }
            if (hdUser == null || !string.IsNullOrWhiteSpace(error))
                return error;
            //UserProfile user = UserProfile.GetProfile(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId);
            return $"{{\"id\":\"{hdUser.UserId}\",\"skype\":\"{hdUser.Skype}\",\"name\":\"{hdUser.FullName}\"}}"; ;
        }

        public object Any(skype_login request)
        {
            if (!string.IsNullOrWhiteSpace(request.user_id))
            {
                ApiUser hdUser = request.ApiUser;
                // Store access token to bot state
                ///// Here we store the only access token.
                ///// Please store refresh token, too.
                var botCred = new MicrosoftAppCredentials(
                    ConfigurationManager.AppSettings["MicrosoftAppId"],
                    ConfigurationManager.AppSettings["MicrosoftAppPassword"]);
                var stateClient = new StateClient(botCred);
                BotState botState = new BotState(stateClient);
                BotData botData = new BotData(eTag: "*");

                string token = base.Request.GetBasicAuth();
                botData.SetProperty<string>("Token", token);
                string name = string.IsNullOrWhiteSpace(request.user_name) ? hdUser.FullName : request.user_name;
                string skype = string.IsNullOrWhiteSpace(request.skype) ? hdUser.Skype : request.skype;

                string data = $"{{\"t\":\"{token}\",\"users\":[{{\"id\":\"{hdUser.UserId}\",\"skype\":\"{skype}\",\"name\":\"{name}\"}}]}}";
                botData.SetProperty("UserInfo", data);
                List<UserData> users = new List<UserData>();
                var user = new UserData(hdUser.UserId.ToString(), skype, name);
                users.Add(user);
                botData.SetProperty("UserData", users);
                stateClient.BotState.SetUserData("skype", request.user_id, botData);
                stateClient.BotState.SetConversationData("skype", request.conversation_id, botData);
            }
            return "{}";
        }

        //const string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IjEyIn0.eyJpYXQiOjE0OTg5Mjc3MjgsImV4cCI6MTQ5OTAxNDEyMSwic2t5cGVpZCI6ImxpdmU6ZXVnZW5lXzc1NiIsInNjcCI6OTU4LCJjc2kiOiIxIiwiY2lkIjoiOWZhNjI3MDNlYzdhNzExZSIsImFhdCI6MTQ5ODgxMTM3MX0.vwr4rufX53b3i7aDcKbXyhRp_gZAD5ThP65azrv1Em-VPZMD4kTX7ux_BQXeAU_3tQzqcxl3fB-QWkDfleEyMeWTc00BgMIVno7oEoqGMfpXEo1UxzWGI-DgSe9NQze_jgi6OMbMSf3HqqHkriW6GJvjuVMFNaJYdu7d8AASJ-fqxkwtoQHzIwCruWPbt3vHqY42jsTZk4r55imP";
        const string Skypesearchurl = "https://skypegraph.skype.com/search/v1.1/namesearch/swx/?requestid=skype&searchstring=";
        const string Ghosturl = "https://api.ghostinspector.com/v1/tests/{0}/execute/?apiKey=63e4e972d8538a7764efcbf2b19e218ad697a14f";

        public object Any(skype_token request)
        {
            if (!string.IsNullOrWhiteSpace(request.token) && !request.force)
            {
                base.Cache.CacheSet<string>("skype_token", request.token, new System.TimeSpan(24, 0, 0));
                return request.token;
            }
            return GetSkypeToken(request.force);
        }

        public object Any(skype_search search)
        {
            string token = GetSkypeToken(false);
            var skypeJson = WebR.GetApiJson(Skypesearchurl + search.name + "&locale=en-us", string.IsNullOrEmpty(search.token) ? token : search.token);
            if (!skypeJson.StartsWith("{"))
                skypeJson = WebR.GetApiJson(Skypesearchurl + search.name, GetSkypeToken(true));
            return skypeJson;
        }

        public object Any(skype_group request)
        {
            string topic = string.IsNullOrWhiteSpace(request.ticket) ? "New Ticket Discussion" : $"Ticket #{request.ticket}: {request.subject} - Discussion";
            string joinUrl = CreateSkypeGroup(topic, request.skype, request.ticket, request.data);
            //send emails
            return joinUrl;
        }

        private string CreateSkypeGroup(string topic, string skypenames, string ticket, string data)
        {
            Skype4Sharp.Skype4Sharp mainSkype;
            SkypeCredentials authCreds = new SkypeCredentials("bigWebApps@gmail.com", "BIG4web1");
            //if (string.IsNullOrEmpty(skypenames))
            //    throw new HttpError(HttpStatusCode.NotFound, "Skype names are empty");
            mainSkype = new Skype4Sharp.Skype4Sharp(authCreds);
            //mainSkype.authTokens.SkypeToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IjEyIn0.eyJpYXQiOjE1MDIzNjY3NTUsImV4cCI6MTUwMjQ1MzE0Nywic2t5cGVpZCI6ImxpdmU6Ymlnd2ViYXBwc18xIiwic2NwIjo5NTgsImNzaSI6IjEiLCJjaWQiOiJkNTRiODlhNjYzY2JkYmM2IiwiYWF0IjoxNTAyMjAxMTA0fQ.BFXZgoiS7rhMLgOGgj3D71PBnUn3R4kRTK6t5NhoEwweYVHEzunI3KpoDK2ap66pX_8H2SW9GTUtswb-FYoNJ2gh_-2RnBlu2a8xDhQwTAkAxyppzwVWC_Zs1s2FBWCbPkaq5iuw9v7H8XWE_JabKW6rCjFbs7CYISGKiziXjY0WXizguYJeVVZxoEM7PZkamqGgA-eLfKGLf8Vw";
            mainSkype.authTokens.SkypeToken = GetSkypeToken(false);
            mainSkype.Login();
            Chat newChat = new Chat(mainSkype);
            //string chatId = newChat.CreateNew(skypenames);
            var chatId = CreateNew(mainSkype, "");//, skypenames);
            newChat.ID = chatId;
            if (string.IsNullOrEmpty(chatId))
                throw new HttpError(HttpStatusCode.NotFound, "Cannot create group chat");
            if (!chatId.StartsWith("19:"))
                return chatId;
            //newChat.ID = "19:fa3bc105a3fc432bb5a70725158e4d2f@thread.skype";
            newChat.Type = ChatType.Group;
            newChat.JoiningEnabled = true;
            newChat.Topic = topic;
            var joinUrl = newChat.JoinUrl;
            joinUrl += "?" + chatId.Substring(3, chatId.IndexOf("@thread.skype") - 3);
            string token = "";
            string new_data = "[]";
            List<UserData> users = new List<UserData>();

            if (!string.IsNullOrWhiteSpace(data) && data.StartsWith("{"))
            {
                dynamic userdata = JsonConvert.DeserializeObject(data);
                token = (string)userdata.t;
                for (int i = 0; i < userdata.users.Count; i++)
                {
                    var user = new UserData((string)userdata.users[i].id, (string)userdata.users[i].skype, (string)userdata.users[i].name);
                    users.Add(user);
                    var chat_user = mainSkype.GetUser(user.skype);
                    user.name = chat_user.DisplayName;
                }

            }

            //sent invite(s) to following user(s) by email: Jon ()
            if (users.Count > 0)
            {
                new_data = JsonConvert.SerializeObject(users.ToArray());
            }
            data = $"{{\"t\":\"{token}\",\"users\":{new_data}}}";
            var message = newChat.SendMessage("setuser " + data, "28:8b270cdf-8d2a-41e7-bdb1-9108f3c220bd");
            newChat.DeleteMessage(message.ID);
            message = newChat.SendMessage(joinUrl, "28:8b270cdf-8d2a-41e7-bdb1-9108f3c220bd");
            newChat.DeleteMessage(message.ID);
            if (!string.IsNullOrWhiteSpace(ticket) && !string.IsNullOrWhiteSpace(token))
            {
                message = newChat.SendMessage(ticket, "28:8b270cdf-8d2a-41e7-bdb1-9108f3c220bd");
                newChat.DeleteMessage(message.ID);
            }

            if (!string.IsNullOrWhiteSpace(skypenames))
            {
                string[] usernames = skypenames.Split(',');
                foreach (var name in usernames)
                {
                    newChat.Add(name);
                }
            }
            else if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    newChat.Add(user.skype);
                }
            }
            //newChat.HistoryEnabled = true;
            return joinUrl;
        }


        public string CreateNew(Skype4Sharp.Skype4Sharp parentSkype, string usernamesToAdd)
        {
            string usernamesJson = "";
            if (!string.IsNullOrWhiteSpace(usernamesToAdd))
            {
                string[] usernames = usernamesToAdd.Split(',');
                foreach (var name in usernames)
                {
                    usernamesJson += $",{{\"id\":\"8:{name}\",\"role\":\"User\"}}";
                }
            }
            usernamesToAdd = @"{""members"":[{""id"":""28:8b270cdf-8d2a-41e7-bdb1-9108f3c220bd"",""role"":""User""},{""id"":""8:live:bigwebapps_1"",""role"":""Admin""}" + usernamesJson + "]}";
            string chatId = "";
            HttpWebRequest createNewRequest = parentSkype.mainFactory.createWebRequest_POST($"https://{parentSkype.authTokens.Endpoint}/v1/threads", new string[][] { new string[] { "RegistrationToken", parentSkype.authTokens.RegistrationToken } }, Encoding.ASCII.GetBytes(usernamesToAdd), "application/json");
            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)createNewRequest.GetResponse())
                {
                    chatId = webResponse.GetResponseHeader("Location");
                    //"https://db5-client-s.gateway.messenger.live.com/v1/threads/19:fa3bc105a3fc432bb5a70725158e4d2f@thread.skype"
                    chatId = chatId.Substring(chatId.IndexOf("threads/") + 8);
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    chatId = reader.ReadToEnd();
                }
            }
            return chatId;
        }

        public object Any(skype_invite request)
        {
            if (string.IsNullOrWhiteSpace(request.emails))
                return "";
            string topic = System.Web.HttpUtility.UrlDecode("Invitation to " + (string.IsNullOrWhiteSpace(request.ticket) ? "New Ticket Discussion" : $"Ticket #{request.ticket}: {request.subject} - Discussion"));
            return SendInvitations(request.ApiUser, request.emails, topic, request.url);
        }

        private string SendInvitations(ApiUser hdUser, string recipients, string topic, string url)
        {
            Guid organizationId = hdUser.OrganizationId;
            int departmentId = hdUser.DepartmentId;
            int userId = hdUser.UserId;
            string Email = hdUser.LoginEmail;
            string userName = hdUser.FullName;
            string department = hdUser.DepartmentName;
            List<int> intUserIds = new List<int>();
            string[] emails = recipients.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
            recipients = "";
            foreach (string email in emails)
            {
                if (!Utils.IsValidEmail(email))
                {
                    continue;
                }
                recipients += email + ";";
            }
            if (string.IsNullOrWhiteSpace(recipients))
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect Email(s)");
            string subject = topic;
            string from = "\"" + userName + " - " + department + "\"<" + Email + ">";

            if (!string.IsNullOrWhiteSpace(recipients))
            {
                try
                {
                    string body = "Hi! You got " + topic + ".\n Please open this url to start chat: " + url + $". Or click <a href=\"{url}\" target=\"_blank\">{url}</a>";
                    MailNotification _mail_notification = new MailNotification(organizationId, departmentId, userId, from, recipients, subject, body);
                    string _return_string = _mail_notification.Commit(true);
                }
                catch
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Email error.");
                }
            }
            else
                throw new HttpError(HttpStatusCode.NotFound, "No recepients selected.");
            return "OK";
        }



        private string GetSkypeToken(bool force)
        {
            string token = base.Cache.Get<string>("skype_token");
            if (force || string.IsNullOrWhiteSpace(token))
            {
                token = RenewSkypeToken();
            }
            return token;
        }

        private string RenewSkypeToken()
        {
            string ghost_skypetoken_url = string.Format(Ghosturl, "5981fc78b546396a3f107d43");
            var ghost_Json = WebR.GetApiJson(ghost_skypetoken_url, "");
            var token = JsonObject.Parse(ghost_Json).Object("data").Object("extractions").Child("apikey");
            base.Cache.CacheSet<string>("skype_token", token, new System.TimeSpan(24, 0, 0));
            return token;
        }
    }

    public static class WebR
    {
        public static string GetApiJson(string url, string token = "")
    {
        string result = "";
        using (var webClient = new WebClient())
        {
            webClient.Headers["Access-Control-Allow-Origin"] = "*";
            webClient.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization, Accept, Origin";
            webClient.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
            webClient.Headers["User-Agent"] =
"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36)";
            webClient.Headers["Content-Type"] = "text/plain";
            webClient.Headers["Accept"] = "application/json";
            webClient.Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4,de;q=0.2";
            webClient.Headers["cache-control"] = "no-cache";
            webClient.Encoding = Encoding.UTF8;
            if (!string.IsNullOrEmpty(token))
            {
                webClient.Headers["x-skypetoken"] = token;
            }
            try
            {
                result = webClient.DownloadString(url);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = (HttpWebResponse)e.Response;

                    if ((int)response.StatusCode == 500)
                    {
                        ;
                    }
                }
            }
            return result;
        }
    }

        public static string PostApiJson(string url, string data, string token = "")
        {
            string result = "";
            using (var webClient = new WebClient())
            {
                webClient.Headers["Access-Control-Allow-Origin"] = "*";
                webClient.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization, Accept, Origin";
                webClient.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
                webClient.Headers["User-Agent"] =
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36)";
                webClient.Headers["Content-Type"] = "text/plain";
                webClient.Headers["Accept"] = "application/json";
                webClient.Headers["Accept-Language"] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4,de;q=0.2";
                webClient.Headers["cache-control"] = "no-cache";
                webClient.Encoding = Encoding.UTF8;
                if (!string.IsNullOrEmpty(token))
                {
                    webClient.Headers["x-skypetoken"] = token;
                }
                try
                {
                    WebRequest request = WebRequest.Create(url);
                    request.Post(data, "application/json");
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = (HttpWebResponse)e.Response;

                        if ((int)response.StatusCode == 500)
                        {
                            ;
                        }
                    }
                }
                return result;
            }
        }
    }

    public class UserData
    {
        public UserData()
        {
        }

        public UserData(string _id, string _skype, string _name)
        {
            id = _id;
            skype = _skype;
            name = _name;
        }
        public string id { get; set; }
        public string name { get; set; }
        public string skype { get; set; }
    }
    /*
     * Future
     *    
        [Route("/invite")]
        public class Invite { }

        public class InviteService : Service
        {
            public object Any(Invite request)
            {
               //string IP = base.RequestContext.Get<IHttpRequest>().RemoteIp;
               //string ipreal = base.RequestContext.Get<IHttpRequest>().XRealIp;
                string ip = base.RequestContext.Get<IHttpRequest>().UserHostAddress;
                string token = Support.GetInviteToken(ip);
                return token;
            }
        }
    */
}
