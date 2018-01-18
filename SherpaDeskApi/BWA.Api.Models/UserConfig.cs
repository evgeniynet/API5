// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using SherpaDeskApi.ServiceModel;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "User_Config")]
    public class UserConfig
    {
        private ApiUser usr;

        public UserConfig(ApiUser usr)
        {
            LoginId = usr.LoginId;
            UserId = usr.UserId;
            Email = usr.LoginEmail;
            FirstName = usr.FirstName;
            LastName = usr.LastName;
            IsTechOrAdmin = usr.IsTechAdmin;
			IsAdmin = usr.Roles[0] == "admin";
            IsUseWorkDaysTimer = usr.IsUseWorkDaysTimer;
            AccountId = usr.AccountId;
            AccountName = usr.AccountName;
            TimeFormat = usr.TimeFormat;
            DateFormat = usr.DateFormat;
			IsLimitToAssignedTkts = !IsAdmin && bigWebApps.bigWebDesk.Data.GlobalFilters.IsFilterEnabled (usr.OrganizationId, usr.DepartmentId, UserId, bigWebApps.bigWebDesk.Data.GlobalFilters.FilterState.LimitToAssignedTickets);

        }

        public UserConfig(ApiUser usr)
        {
            this.usr = usr;
        }

        [DataMember(Name = "account_name")]
        public string AccountName { get; set; }

        [DataMember(Name = "login_id")]
        public Guid LoginId { get; set; }

        [DataMember(Name = "user_id")]
        public int UserId { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastname")]
        public string LastName { get; set; }

        [DataMember(Name = "is_techoradmin")]
        public bool IsTechOrAdmin { get; set; }

		[DataMember(Name = "is_admin")]
		public bool IsAdmin { get; set; }

		[DataMember(Name = "is_limit_assigned_tkts")]
		public bool IsLimitToAssignedTkts { get; set; }

        [DataMember(Name = "is_useworkdaystimer")]
        public bool IsUseWorkDaysTimer { get; set; }

        [DataMember(Name = "account_id")]
        public int AccountId { get; set; }

        [DataMember(Name = "time_format")]
        public int TimeFormat { get; set; }

        [DataMember(Name = "date_format")]
        public int DateFormat { get; set; }
    }
}
