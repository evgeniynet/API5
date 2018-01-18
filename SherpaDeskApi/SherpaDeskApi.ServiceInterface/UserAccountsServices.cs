// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using bigWebApps.bigWebDesk;
using System.Net;
using Micajah.Common.Bll.Providers;

namespace SherpaDeskApi.ServiceInterface
{
    public class TechniciansService : Service
    {

        [Secure()]
        public object Get(Technicians request)
        {
			if (request.c)
				return GetTechnicians(request);
			var query = base.Request.QueryString;
			string search = query["search"];
			if (!string.IsNullOrWhiteSpace (search)) {
				search = search.Trim ();
				if (search == "*"){
                    search = "";
                    query = System.Web.HttpUtility.ParseQueryString(query.ToString());
					query.Remove ("search");
				}
                request.search = search;
			}

			if (string.IsNullOrWhiteSpace (query["search"]) && string.IsNullOrWhiteSpace(query["firstname"]) && string.IsNullOrWhiteSpace(query["lastname"]) && string.IsNullOrWhiteSpace(query["email"]))
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (query.Count > 0 ? ":" + query.ToString() : "")),
                new System.TimeSpan(2, 0, 0), () =>
                {
                    return GetTechnicians(request);
                });
            else
                return GetTechnicians(request);
        }

        private object GetTechnicians(Technicians request)
        {
            ApiUser hdUser = request.ApiUser;
            var _cfg = new bigWebApps.bigWebDesk.Config(hdUser.OrganizationId, hdUser.InstanceId);
            return request.FilteredResult<UserAccount>(UserAccounts.FindUsers(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, _cfg.AccountManager, _cfg.LocationTracking, request.search, request.firstname, request.lastname, request.email, "notuser", _cfg.UnassignedQue));
           //return request.FilteredResult<UserAccount>(UserAccounts.Technicians(hdUser.OrganizationId, hdUser.DepartmentId));
        }
    }

    [Route("/users")]
    [Route("/users/{id}")]
    public class Users : PagedApiRequest
    {
		public bool c { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string search { get; set; }
        public int? account { get; set; }
        public int location { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
        public string role { get; set; }
    }

    public class UsersService : Service
    {
        [Secure("super")]
		public object Get(Users request)
		{
			if (request.c)
				return GetUsers(request);
			var query = base.Request.QueryString;
			string search = query["search"];
			if (!string.IsNullOrWhiteSpace (search)) {
				search = search.Trim ();
				if (search == "*"){
                    search = "";
                    query = System.Web.HttpUtility.ParseQueryString(query.ToString());
					query.Remove ("search");
				}
                request.search = search;
			}

			if (string.IsNullOrWhiteSpace (query["search"]) && string.IsNullOrWhiteSpace(query["firstname"]) && string.IsNullOrWhiteSpace(query["lastname"]) && string.IsNullOrWhiteSpace(query["email"]))
				return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (query.Count > 0 ? ":" + query.ToString() : "")),
					new System.TimeSpan(2, 0, 0), () =>
					{
						return GetUsers(request);
					});
			else
				return GetUsers(request);
		}

		private object GetUsers(Users request)
        {
            ApiUser hdUser = request.ApiUser;
            if (request.id > 0)
                return UserAccounts.GetUser(hdUser.OrganizationId, hdUser.DepartmentId, request.id);
            var _cfg = new bigWebApps.bigWebDesk.Config(hdUser.OrganizationId, hdUser.InstanceId);
           //v3
           //if (!string.IsNullOrEmpty(request.query))
           //{
           //    request.search = request.query;
           //}
            return request.FilteredResult<UserAccount>(UserAccounts.FindUsers(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, _cfg.AccountManager, _cfg.LocationTracking, request.search, request.firstname, request.lastname, request.email, request.role, _cfg.UnassignedQue));
        }

        [Secure("tech")]
        public object Post(Users request)
        {
            if (!Utils.IsValidEmail(request.email))
                return new HttpError("User Not Saved. Correct email");

            string pass = request.password;
            if (!string.IsNullOrWhiteSpace(pass))
            {
                if (string.IsNullOrWhiteSpace(request.password_confirm) || !pass.Equals(request.password_confirm))
                {
                    return new HttpError("Passwords do not match.");
                }
            }
            else
                pass = bigWebApps.bigWebDesk.Data.Logins.GenerateRandomPassword();

            ApiUser hdUser = request.ApiUser;
           //string global = "0";
            int _user_id = 0;
            int _login_id = 0;
            int role = !string.IsNullOrWhiteSpace(request.role) && request.role.ToLower().Equals("tech") ? (int)bigWebApps.bigWebDesk.UserAuth.UserRole.Technician : (int)bigWebApps.bigWebDesk.UserAuth.UserRole.StandardUser;

            bigWebApps.bigWebDesk.Data.Logins.VerifyLogin(hdUser.DepartmentId, request.email, ref _user_id, ref _login_id, hdUser.OrganizationId);

            if (string.IsNullOrWhiteSpace(request.firstname))
                return new HttpError("User Not Saved. Correct Firstname");
            if (string.IsNullOrWhiteSpace(request.lastname))
                return new HttpError("User Not Saved. Correct Lastname");

            if (_login_id == 0)
            {
                _login_id = bigWebApps.bigWebDesk.Data.Logins.UpdateLogin(hdUser.OrganizationId, hdUser.InstanceId, request.email, request.firstname, request.lastname, pass, "", role);
                _user_id = bigWebApps.bigWebDesk.Data.Logins.InsertLoginCompanyJunc(hdUser.OrganizationId, hdUser.DepartmentId, _login_id, role);
            }
            else
            {
                if (_user_id == -1)
                {
                    _user_id = bigWebApps.bigWebDesk.Data.Logins.InsertLoginCompanyJunc(hdUser.OrganizationId, hdUser.DepartmentId, _login_id, role);
                }
                if (_user_id > 0)
                {
                    return new HttpError("This login/email is already in use for this department/organization.");
                }
            }
            if (_login_id > 0 &&_user_id > 0)
            {
                    return UserAccounts.GetUser(hdUser.OrganizationId, hdUser.DepartmentId, _user_id);
            }
            return new HttpResult(HttpStatusCode.NotFound, "Can't find selected login. Maybe this login was deleted by another user.");
        }
    }
}
