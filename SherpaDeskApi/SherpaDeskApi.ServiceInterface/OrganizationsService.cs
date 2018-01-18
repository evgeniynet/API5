// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com


using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using ServiceStack.Host;
using Micajah.Common.Bll.Providers;

namespace SherpaDeskApi.ServiceInterface
{
    public class OrganizationsService : Service
    {
        [Secure()]
        public object Get(Organizations request)
        {
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(0, 10, 0), () =>
                {
                                return GetOrganizations(request.api_token).organizations;
                });
        }

        [Secure()]
        public object Any(DeleteOrganization request)
        {
            return DeleteOrganization(request.id);
        }

        private object DeleteOrganization(string id)
        {
            Micajah.Common.Bll.Organization org = null;
            Guid orgid = Guid.Empty;
            if (Guid.TryParseExact(id, "N", out orgid))
                org = OrganizationProvider.GetOrganization(orgid);
            else
                org = OrganizationProvider.GetOrganizationByPseudoId(id);
            if (org == null)
                return new HttpResult("Organization not found", HttpStatusCode.NotFound);
            if (org.Deleted)
                return new HttpResult("Organization already inactive", HttpStatusCode.NotModified);
            OrganizationProvider.DeleteOrganization(org.OrganizationId);
            return new HttpResult("Organization " + org.Name + " deleted", HttpStatusCode.OK);
        }

        private NewOrganizationsResponse GetOrganizations(string api_token, string inst_key = "")
        {
            LoginProvider lp = new LoginProvider();
            ApiUser hdUser = new ApiUser(api_token);
            Micajah.Common.Bll.OrganizationCollection orgsMc = lp.GetOrganizationsByLoginId(hdUser.LoginId);
            List<Organization> orgs = new List<Organization>(orgsMc.Count);
            string org_key = "";
            foreach (Micajah.Common.Bll.Organization orgMc in orgsMc)
                try
                {
                    var torg = new Organization(orgMc, lp.GetLoginInstances(hdUser.LoginId, orgMc.OrganizationId));
                    orgs.Add(torg);
                    if (torg.Instances[0].Key == inst_key)
                        org_key = torg.Key;
                }
                catch
                { }
            var org = new NewOrganizationsResponse { organizations = orgs };

            if (!string.IsNullOrEmpty(inst_key))
            {
                org.organization = org_key;
                org.instance = inst_key;
                org.api_token = api_token;
            }

            var o = from element in org.organizations
                    orderby element.is_trial
                    select element;

            o = from element in o
                orderby element.is_expired
                select element;

            org.organizations = o.ToList();

            return org;
        }


        public object Post(Organizations request)
        {
            return CreateOrg(request.name, request.url, request.email, request.firstname, request.lastname, request.password, request.password_confirm, request.how, request.note, request.external_id
                , (request.is_send_hubSpot.HasValue ? request.is_send_hubSpot.Value : true)
                , request.is_force_registration, request.is_force_redirect);
        }


        public object Any(AddOrganization request)
        {
            VerifyOrganization v = new VerifyOrganization();
            v.isNameExists = false;
            v.isUrlExists = v.isEmailExists = false;
            v.ErrorDescription = "";
           //validation
            if (string.IsNullOrWhiteSpace(request.name))
            {
                v.ErrorDescription += "Name is required.\n";
                v.isNameExists = true;
            }

            if (!string.IsNullOrWhiteSpace(request.url))
            {
                if (request.url.Length < 3 || request.url.Length > 20 || !(new Regex("^[0-9a-z][0-9a-z-]{1,18}[0-9a-z]$", RegexOptions.IgnoreCase | RegexOptions.Multiline)).IsMatch(request.url))
                {
                    v.ErrorDescription += "Url should be between 3 and 20 characters and can contains alphanumeric characters and hyphens";
                    v.isUrlExists = true;
                }
                if (!CustomUrlProvider.ValidateCustomUrl(request.url))
                {
                    v.ErrorDescription += "Url: " + request.url + " is already exists.\n";
                    v.isUrlExists = true;
                }
            }
            else
                request.url = null;

            if (!Utils.IsValidEmail(request.email))
            {
                v.ErrorDescription += "Email is required.\n";
                v.isEmailExists = true;
            }
            else
            {
                LoginProvider lp = new LoginProvider();

                if (lp.ValidateLogin(request.email, null))
                {
                    v.isEmailExists = true;
                    v.ErrorDescription += "User already have one registered organization. Please login OR set is_force_registration=true to continue.\n";
                }
            }

            v.isPasswordCorrect = v.isPasswordsMatch = true;
            if (!string.IsNullOrWhiteSpace(request.password))
            {
                if (!Utils.IsValidPassword(request.password))
                {
                    v.isPasswordCorrect = false;
                    v.ErrorDescription += "Password is too weak. It must be at least 5 characters.\n";
                }
                if (!request.password.Equals(request.password_confirm))
                {
                    v.isPasswordsMatch = false;
                    v.ErrorDescription += "Passwords not match.\n";
                }
            }

            return v;
        }

        public object CreateOrg(string name, string url, string email, string firstname, string lastname, string password, string password_confirm, string how, string note
            , string external_id, bool sendHubSpot
            , bool is_force_registration, bool is_force_redirect)
        {
            bool isSalesforceProviderRequest = false;
            if (!string.IsNullOrEmpty(how))
            {
                isSalesforceProviderRequest = (how.IndexOf("Salesforce", StringComparison.OrdinalIgnoreCase) > -1);
            }

           //validation
            if (string.IsNullOrWhiteSpace(name))
                return new HttpResult("Name is required.", HttpStatusCode.Forbidden);

            if (!Utils.IsValidEmail(email))
                return new HttpResult("Email is required.", HttpStatusCode.Forbidden);

            /*if (OrganizationProvider.GetOrganizationIdByName(name) != Guid.Empty)
            {
                return new HttpResult("Name is already exists.", HttpStatusCode.Forbidden);
            }
            */

            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.Length < 3 || url.Length > 20)
                    return new HttpResult("Url should be between 3 and 20 characters and can contains alphanumeric characters and hyphens", HttpStatusCode.Forbidden);

                if (!CustomUrlProvider.ValidateCustomUrl(url))
                    return new HttpResult("Url: " + url + " is already exists.", HttpStatusCode.Forbidden);
            }
            else
                url = null;


            bool isAlreadyRegistered = false;
            LoginProvider lp = new LoginProvider();

            if (isSalesforceProviderRequest)
            {
                if (!string.IsNullOrEmpty(external_id))
                {
                    if (SalesforceSettingProvider.ExternalOrganizationIdExists(external_id))
                    {
                        return new HttpResult("The organization is already registered.", HttpStatusCode.Conflict);
                    }
                }
            }
            else
            {
                isAlreadyRegistered = lp.ValidateLogin(email, null);

                if (isAlreadyRegistered && !is_force_registration)
                    return new HttpResult("User already have one registered organization. Please login OR set is_force_registration=true to continue.", HttpStatusCode.Conflict);
            }

            if (string.IsNullOrWhiteSpace(firstname))
                firstname = "Organization";

            if (string.IsNullOrWhiteSpace(lastname))
                lastname = "Administrator";

           //if (string.IsNullOrWhiteSpace(lastname))
           //    return new HttpResult("LastName is required.", HttpStatusCode.Forbidden);

           //if (string.IsNullOrWhiteSpace(password))
           //    return new HttpResult("Password is required.", HttpStatusCode.Forbidden);

            if (!string.IsNullOrWhiteSpace(password))
            {
                if (!Utils.IsValidPassword(password))
                    return new HttpResult("Password is too weak. It must be at least 5 characters.", HttpStatusCode.Forbidden);

                if (!password.Equals(password_confirm))
                    return new HttpResult("Passwords not match.", HttpStatusCode.Forbidden);
            }
            else
                password = password_confirm = null;

            /*
             * Future
             * 
            string ip = base.RequestContext.Get<IHttpRequest>().UserHostAddress;
            if (!Micajah.Common.Bll.Support.ValidateInviteToken(invite_code, ip))
            {
                throw new HttpError(HttpStatusCode.Forbidden, "Invite code is not correct or expired. Please get new one.");
            }
            */
            Guid organizationId = Guid.Empty;
            Micajah.Common.Bll.Instance inst = null;
            if (!isAlreadyRegistered || is_force_registration)
            {
                Micajah.Common.Bll.Instance templateInstance = null;
                Micajah.Common.Bll.InstanceCollection insts = InstanceProvider.GetTemplateInstances();

                if (insts.Count == 0)
                {
                    return new HttpResult("No Active Template Instances", HttpStatusCode.NotFound);
                }
                else
                {
                    templateInstance = insts[0];
                }

                string howYouHearAboutUs = how;

                NameValueCollection parameters = new NameValueCollection();

                if (isSalesforceProviderRequest)
                {
                    if (!string.IsNullOrEmpty(external_id))
                    {
                        SalesforceSettingProvider.AddExternalOrganizationId(external_id, parameters);
                    }
                }

                organizationId = OrganizationProvider.InsertOrganization(name, null, null
                    , null, null, null, null, null, null, string.Empty, howYouHearAboutUs, note
                    , templateInstance.TimeZoneId, templateInstance.InstanceId
                    , email, password, firstname, lastname, null, null, null
                    , url, parameters
                    , true, true, sendHubSpot);

                inst = InstanceProvider.GetFirstInstance(organizationId);
            }

            if (!is_force_redirect)
            {
                string api_token = LoginTokenProvider.GetApiToken(email);
                ApiUser hdUser = new ApiUser(api_token);
                if (string.IsNullOrWhiteSpace(api_token))
                    return new HttpResult("User is not correct or inactive.", HttpStatusCode.Forbidden);
                return new HttpResult(GetOrganizations(api_token, inst.PseudoId), organizationId != Guid.Empty ? HttpStatusCode.Created : HttpStatusCode.Found);
            }
           //return new HttpResult(HttpStatusCode.OK, "Already registered");
            url = lp.GetLoginUrl(email, true, organizationId, inst.InstanceId, null);

           //added redirect
            url = url.Replace("mc/login.aspx?", "login/?ReturnUrl=%2Fhome%2Fdefault.aspx%3Ffx%3Demlstp%26org%3D" + organizationId.ToString("N") + "&");

           //Headers ["Location"] = url;
            return "{\"url\" : \"" + url + "\"}";
        }
    }
}
