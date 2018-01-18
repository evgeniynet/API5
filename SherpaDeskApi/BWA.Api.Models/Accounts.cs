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
	[DataContract (Name = "Accounts")]
	public class Accounts : ModelItemCollectionGeneric<Account>
	{
		public Accounts (DataTable AccountsTable) : base (AccountsTable)
		{
		}

		public static List<Account> SearchAccounts (ApiUser user, string search, bool is_with_statistics, bool is_open_tickets, bool is_watch_info, bool is_locations_info, int page = 0, int limit = 25)
		{
			bigWebApps.bigWebDesk.Data.Accounts.ActiveStatus _status = bigWebApps.bigWebDesk.Data.Accounts.ActiveStatus.Active;

			// _status = Data.Accounts.ActiveStatus.Inactive;

			Instance_Config config = new Instance_Config (user);
			bigWebApps.bigWebDesk.Data.Accounts.Filter _filter = new bigWebApps.bigWebDesk.Data.Accounts.Filter (_status, search);
			if (!config.LocationTracking)
				_filter.FilterLocation = !config.LocationTracking;
			/*if (m_TktId != 0)
            {
                if (Request.QueryString["vw"] == "0") _filter.UserId = User.lngUId;
                else if (Request.QueryString["vw"] == "2") _filter.UserId = 0;
                else _filter.UserId = m_Tkt.TechnicianId;
            }
            else
            {
                _filter.UserId = User.lngUId;
                _filter.SupportGroupId = User.lngGSpGrp;
            }
            _filter.SaveToSession();
            */
			var m_ColSet = new bigWebApps.bigWebDesk.Data.Accounts.ColumnsSetting (user.DepartmentId, user.UserId, user.OrganizationId);
			if (!user.IsTechAdmin) {
				m_ColSet.ListViewMode = bigWebApps.bigWebDesk.Data.Accounts.ViewMode.MyAccounts;
				_filter.UserId = user.UserId;
			}
			if (!is_open_tickets && m_ColSet.BrowseColumnsCount > 1)
				m_ColSet.SetBrowseColumn (1, bigWebApps.bigWebDesk.Data.Accounts.BrowseColumn.Updated);

			limit = limit <= 0 ? 25 : limit;
			page = page < 0 ? 0 : page;
			string pager = string.Format (" OFFSET ({0} * {1}) ROWS FETCH NEXT {1} ROWS ONLY ", page, limit);

			Accounts _userAccountsList = new Accounts (bigWebApps.bigWebDesk.Data.Accounts.SelectFiltered (user.OrganizationId, user.DepartmentId, "", user.UserId, m_ColSet, _filter, false, pager));
			var userAccountsList = _userAccountsList.List;
            int account = user.AccountId <= 0 ? -1 : user.AccountId;
			var fix = userAccountsList.FirstOrDefault (acc => acc.Id == account);
			if (fix != null)
				fix.name = user.DepartmentName;

			List<TaskType> task_types = null;

            if (is_watch_info || is_locations_info)
            {
                task_types = Models.TaskTypes.SelectAccountTaskTypes(user.OrganizationId, user.DepartmentId, user.UserId, userAccountsList.First().Id.Value);
            }

			foreach (var userAccount in userAccountsList) {
				if (is_locations_info) {
					if (config.ProjectTracking)
                        userAccount.projects = Projects.GetProjects(user.OrganizationId, user.DepartmentId, userAccount.Id.Value, user.IsTechAdmin ? 0 : user.UserId, config.AccountManager, false);
					if (config.LocationTracking)
						userAccount.locations = Locations.GetAccountLocations(user.OrganizationId, user.DepartmentId, userAccount.Id.Value, 0, "", true);
					userAccount.task_types = task_types;
				}
				else if (is_watch_info) {
					if (config.ProjectTracking)
                        userAccount.projects = Projects.GetProjects(user.OrganizationId, user.DepartmentId, userAccount.Id.Value, user.IsTechAdmin ? 0 : user.UserId, config.AccountManager, false);
					//foreach (var projectAccount in userAccount.projects) {
					//	projectAccount.task_types = Models.TaskTypes.SelectProjectTaskTypes (user.OrganizationId, user.DepartmentId, user.UserId, projectAccount.Id);
					//}
					userAccount.task_types = task_types;
				} 
				else if (is_with_statistics)
					userAccount.account_statistics = AccountStats.GetStatistics (userAccount.Id.Value, user.OrganizationId, user.DepartmentId, user.UserId);
				else if (is_open_tickets) {
					int open = userAccount.Row.Get<int> ("OpenTickets");
					var account_stat = new AccountStats ();
					account_stat.ticket_counts = AccountTicketCount.GetAccountStat (open);
					userAccount.account_statistics = account_stat;
				}
			}
           
			return userAccountsList;
		}
	}
}
