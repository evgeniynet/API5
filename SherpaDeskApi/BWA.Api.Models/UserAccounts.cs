// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "User_Accounts")]
    public class UserAccounts : ModelItemCollectionGeneric<UserAccount>
    {
        public UserAccounts(DataTable UserAccountsTable) : base(UserAccountsTable) { }

        public static List<UserAccount> Technicians(Guid OrgId, int DeptId)
        {
            UserAccounts _users = new UserAccounts(bigWebApps.bigWebDesk.Data.Logins.SelectTechnicians(OrgId, DeptId));
            return _users.List;
        }

        public static UserAccount GetUser(Guid OrgId, int DeptId, int UserId)
        {
            UserAccount _user = new UserAccount(bigWebApps.bigWebDesk.Data.Logins.SelectUserDetails(OrgId, DeptId, UserId));
            return _user;
        }

        public static List<UserAccount> FindUsers(Guid OrgId, int DeptId, int UserId, bool AccountManager, bool LocationTracking, string SearchText, string Firstname, string Lastname, string Email, string Type = "user", bool UnassignedQueues = false)
        {
            var _filter = new bigWebApps.bigWebDesk.Data.Logins.Filter("");
            switch (Type)
            {
                case "notuser":
                    _filter.Type = bigWebApps.bigWebDesk.Data.Logins.UserType.NotUser;
                    _filter.ConfigUnassignedQueuesEnabled = UnassignedQueues;
                    break;
                case "admin":
                    _filter.Type = bigWebApps.bigWebDesk.Data.Logins.UserType.Administrator;
                    break;
                case "tech":
                    _filter.Type = bigWebApps.bigWebDesk.Data.Logins.UserType.Technician;
                    break;
                case "user":
                    _filter.Type = bigWebApps.bigWebDesk.Data.Logins.UserType.StandardUser;
                    break;
                case "superuser":
                    _filter.Type = bigWebApps.bigWebDesk.Data.Logins.UserType.SuperUser;
                    break;
                case "queue":
                    _filter.Type = bigWebApps.bigWebDesk.Data.Logins.UserType.Queue;
                    break;
                default:
                    _filter.Type = bigWebApps.bigWebDesk.Data.Logins.UserType.NotSet;
                    break;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                _filter.SearchString = SearchText;
            }
            if (!string.IsNullOrEmpty(Firstname))
            {
                _filter.FirstName = Firstname;
            }
            if (!string.IsNullOrEmpty(Lastname))
            {
                _filter.LastName = Lastname;
            }
            if (!string.IsNullOrEmpty(Email))
            {
                _filter.EMail = Email;
            }
            _filter.UseGlobalFilters = true;
            _filter.SearchAccountsToo = false;
            _filter.ConfigAccountsEnabled = AccountManager;
            _filter.ConfigLocationsEnabled = LocationTracking;
            UserAccounts _users = new UserAccounts(bigWebApps.bigWebDesk.Data.Logins.SelectUsersByFilter(OrgId, DeptId, UserId, -1, _filter));
            return _users.List;
        }
    }
}
