// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "Account")]
    public class Account : ModelItemBase
    {
        public Account(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int? Id
        {
            get { return Row.Get<int?>("AccountId") ?? Row.Get<int?>("Id"); }
            set
            {
                if (Row.Table.Columns.Contains("AccountId"))
                    Row["AccountId"] = value;
                else
                    Row["Id"] = value;
            }
        }

        [DataMember(Name = "name")]
        public string name
        {
            get { return HttpUtility.HtmlDecode(Row.GetString("AcctName") ?? Row.GetString("Name") ?? Row.GetString("AccName")); }
            set
            {
                if (Row.Table.Columns.Contains("AcctName"))
                    Row["AcctName"] = value;
                else if (Row.Table.Columns.Contains("Name"))
                    Row["Name"] = value;
                else if (Row.Table.Columns.Contains("AccName"))
                    Row["AccName"] = value;
            }
        }

		[DataMember(Name = "task_types")]
		public List<TaskType> task_types { get; set; }

		[DataMember(Name = "projects")]
		public List<Project> projects { get; set; }

		[DataMember(Name = "locations")]
		public List<Location> locations { get; set; }

        [DataMember(Name = "account_statistics")]
        public AccountStats account_statistics { get; set; }

        public static Account GetAccount(Guid organizationId, int departmentId, int accountId, string department_name, int userId)
        {
            Models.Account account = null;
            if (accountId > 0)
                account = new Account(bigWebApps.bigWebDesk.Data.Accounts.SelectOne(departmentId, accountId, organizationId));
            else if (accountId == -1)
            {
                DataTable table = new DataTable();
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Id", typeof(int));

                account = new Models.Account(table.NewRow());
                account.name = department_name;
                account.Id = -1;
            }
            return account;
        }

		public static void SaveNote(Guid organizationId, int departmentId, int accountId, string note)
		{
			bigWebApps.bigWebDesk.Data.Accounts.SetAccountNote(organizationId, departmentId, accountId, note);
		}
    }
}
