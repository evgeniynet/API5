// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk.Data;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for Ticket
   /// </summary>
    [DataContract(Name = "Worklist_Tickets")]
    public class WorklistTickets : ModelItemCollectionGeneric<WorklistTicket>
    {
        public WorklistTickets(DataTable TicketsTable) : base(TicketsTable) { }

        public string OrgKey { get; set; }
        public string InstKey { get; set; }

        public static List<WorklistTicket> GetTickets(ApiUser hdUser, Guid OrgId, int DeptId, int UserId, string status, string role, string Class, string account, string location, string project, bool IsTechAdmin, bool IsUseWorkDaysTimer, string sort_order, string sort_by, string search = "", int page = 0, int limit = 25, DateTime? start_date = null, DateTime? end_date = null)
        {
            string tkts = "";
            if (!string.IsNullOrWhiteSpace(search) && Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.Integration.Azure.Search.Enabled)
            {
                tkts = bigWebApps.bigWebDesk.Data.Tickets.SearchTicketsInAzure(hdUser.OrganizationId, hdUser.DepartmentId, false, search);
                /*if (string.IsNullOrEmpty(tkts))
                {
                    return new List<WorklistTicket>();
                   //tkts = "0";
                }
                */
            }
            Worklist.QueryFilter _qFilter = new Worklist.QueryFilter();
            if (string.IsNullOrEmpty(role) || role.ToLower() == "all")
                role = "notset";
            if (!string.IsNullOrEmpty(status))
            {
                string originalstatus = status;
                status = status.Replace(",", "").Replace("_", "").ToLowerInvariant();
                Worklist.TicketStatusMode ticketStatusMode;
                if (Enum.TryParse<Worklist.TicketStatusMode>(status, true, out ticketStatusMode))
                {
                    _qFilter.TicketStatus = ticketStatusMode;
                }
                else
                {
                    if (originalstatus.Contains(","))
                    {
                        string statuses = originalstatus.Replace("_", "").ToLowerInvariant();
                        string status_query = string.Empty;
                        foreach (string _status in statuses.Split(','))
                        {
                            switch (_status.ToLower())
                            {
                                case "closed":
                                    status_query += " OR tkt.status='Closed' ";
                                    break;
                                case "open":
                                    status_query += " OR tkt.status='Open' ";
                                    break;
                                case "onhold":
                                    status_query += " OR tkt.status='On Hold' ";
                                    break;
                                case "partsonorder":
                                    status_query += " OR tkt.status='Parts On Order' ";
                                    break;
                                case "waiting":
                                    status_query += " OR (tkt.status='Open' AND tkt.WaitingDate is not null) ";
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(status_query))
                            _qFilter.SQLWhere = " AND (" + status_query.Substring(3) + ")";
                    }
                    else
                    {
                        switch (status)
                        {
                            case "waiting": _qFilter.TicketStatus = Worklist.TicketStatusMode.OpenWaitingOnResponse;
                                break;
                            case "newmessages":
                                switch (role)
                                {
                                    case "tech": _qFilter.ShowNewMessages = Worklist.NewMessagesMode.User; break;
                                    case "user": _qFilter.ShowNewMessages = Worklist.NewMessagesMode.Technician; break;
                                    default: _qFilter.ShowNewMessages = Worklist.NewMessagesMode.UserAndTech; break;
                                };
                                break;
                            case "closed": _qFilter.TicketStatus = Worklist.TicketStatusMode.Close;
                                break;
                        }
                    }
                }
            }

            Worklist.SortMode ticketSortMode;
            if (Enum.TryParse<Worklist.SortMode>(role, true, out ticketSortMode))
            {
                _qFilter.Sort = ticketSortMode;
            }
            else
                switch (role)
                {
                    case "tech": _qFilter.Sort = Worklist.SortMode.MyTickets; break;
                    case "user": _qFilter.Sort = Worklist.SortMode.MyTicketsAsUser; break;
                    case "alt_tech": _qFilter.Sort = Worklist.SortMode.MyTicketsAsAlternateTech; break;
                }
            int queueId;
            if (int.TryParse(Class, out queueId))
            {
                _qFilter.TechnicianId = queueId;
                Class = "";
            }
            if (!string.IsNullOrEmpty(Class))
            {
                int class_id = 0;
                if (int.TryParse(Class, out class_id))
                {
                    if (class_id > 0)
                        _qFilter.SQLWhere += " AND tkt.class_id = " + class_id;
                }
                else
                    _qFilter.SQLWhere += " AND dbo.fxGetFullClassName(" + DeptId.ToString() + ", tkt.class_id) like '%" + Class + "%'";
            }
            if (!string.IsNullOrEmpty(account))
            {
                int account_id = 0;
                if (int.TryParse(account, out account_id))
                {
                    if (account_id != 0)
                    {
                        _qFilter.AccountId = account_id;
                    }
                }
                else
                    _qFilter.SQLWhere += " AND (ISNULL(acct.vchName, CASE WHEN ISNULL(tkt.btNoAccount, 0) = 0 THEN co.company_name ELSE '' END) like '%" + account + "%')";
            }
            if (!string.IsNullOrEmpty(project))
            {
                int project_id = 0;
                if (int.TryParse(project, out project_id))
                {
                    if (project_id != 0)
                    {
                        _qFilter.ProjectID = project_id;
                    }
                }
                else
                    _qFilter.SQLWhere += " AND dbo.fxGetFullProjectName(" + DeptId.ToString() + ", tkt.ProjectID) like '%" + project + "%'";
            }
            if (!string.IsNullOrEmpty(location))
            {
                int location_id = 0;
                if (int.TryParse(location, out location_id))
                {
                    if (location_id != 0)
                    {
                        _qFilter.AccountLocationId = location_id;
                    }
                }
                else
                    _qFilter.SQLWhere += " AND dbo.fxGetUserLocationName(" + DeptId.ToString() + ", tkt.LocationId) like '%" + location + "%'";
            }

            if (start_date.HasValue)
            {
                if (start_date.Value.TimeOfDay.TotalSeconds > 0)
                    start_date = start_date.Value.Date;
                _qFilter.SQLWhere += string.Format(" AND tkt.CreateTime >= '{0}'", start_date.Value.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            if (end_date.HasValue)
            {
                if (end_date.Value.TimeOfDay.TotalSeconds == 0)
                    end_date = end_date.Value.Date.AddDays(1).AddSeconds(-1);
                _qFilter.SQLWhere += string.Format(" AND tkt.CreateTime <= '{0}'", end_date.Value.ToString("yyyy-MM-dd hh:mm:ss"));
            }

            Instance_Config config = new Instance_Config(hdUser);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string _query = "";
                if (!string.IsNullOrEmpty(tkts))
                {
                    _query += " AND (tkt.Id IN (" + tkts + ")";
                }
                else
                {
                    _query += " AND (tkt.subject LIKE '%" + search + "%' OR ISNULL(tlip2.CountFoundInitPost, 0) > 0 OR tkt.note LIKE '%" + search + "%' OR ISNULL(tlcn2.CountFoundClosureNotes, 0) > 0";
                    _qFilter.SQLJoin += " LEFT OUTER JOIN (SELECT TId, COUNT(*) AS CountFoundInitPost FROM TicketLogs WHERE DId=" + hdUser.DepartmentId + " AND vchType = 'Initial Post' AND vchNote LIKE '%" + search + "%' GROUP BY TId) tlip2 ON tlip2.TId = tkt.Id "
           + "LEFT OUTER JOIN (SELECT TId, COUNT(*) AS CountFoundClosureNotes FROM TicketLogs WHERE DId=" + hdUser.DepartmentId + " AND vchType = 'Closed' AND vchNote LIKE '%" + search + "%' GROUP BY TId) tlcn2 ON tlcn2.TId = tkt.Id ";
                }

                if (config.SerialNumber)
                {
                    _query += " OR tkt.SerialNumber LIKE '%" + search + "%'";
                }
                _query += ")";
                _qFilter.SQLWhere += _query;
            }

            limit = limit <= 0 ? 25 : limit;
            page = page < 0 ? 0 : page;
            string pager = string.Format(" OFFSET ({0} * {1}) ROWS FETCH NEXT {1} ROWS ONLY ", page, limit);
            _qFilter.PageIndex = page;

            if (sort_by != "days_old")
            {
                _qFilter.SortColumnIndex = 0;
                _qFilter.IsSortColumnDesc = "desc" == sort_order;
                _qFilter.SortColumnSQLAlias = sort_by ?? "CreateTime";
            }


            bigWebApps.bigWebDesk.UserAuth userAuth = new bigWebApps.bigWebDesk.UserAuth
            {
                Role = hdUser.Role,
                OrgID = hdUser.OrganizationId,
                lngDId = hdUser.DepartmentId,
                tintTicketTimer = hdUser.tintTicketTimer,
                lngUId = hdUser.UserId,
                InstanceID = hdUser.InstanceId
               //ee.strGSUserRootLocationId
               //ee.sintGSUserType
            };
            //Use global filters
            bool useGlobalFilters = hdUser.Role != bigWebApps.bigWebDesk.UserAuth.UserRole.Administrator && (role == "notset" || role == "tech");
            bool limitToAssignedTickets = (useGlobalFilters) ? GlobalFilters.IsFilterEnabled(hdUser.OrganizationId, hdUser.DepartmentId, UserId, GlobalFilters.FilterState.LimitToAssignedTickets) : false;

            if (limitToAssignedTickets)
            {
                _qFilter.SQLWhere += " AND tkt.technician_id=" + UserId.ToString();
                _qFilter.SQLJoin += " LEFT OUTER JOIN TicketAssignment AS TA2 ON TA2.DepartmentId=" + hdUser.DepartmentId + " AND TA2.TicketId = tkt.Id AND TA2.UserId = " + UserId + " AND TA2.AssignmentType = " + ((int)Ticket.TicketAssignmentType.Technician).ToString() + " AND TA2.IsPrimary = 0 AND TA2.StopDate IS NULL";
            }

            //Use global filters
            if (useGlobalFilters)
                _qFilter.SQLWhere += GlobalFilters.GlobalFiltersSqlWhere(userAuth, config, "tkt.", "tlj2.", "SupGroupID", null);
            //(limitToAssignedTickets ? "TA2." : null

            //bigWebApps.bigWebDesk.Data.Worklist.Filter m_Filter = new bigWebApps.bigWebDesk.Data.Worklist.Filter(tt.DepartmentId, tt.UserId, true, tt.OrganizationId);
            //bool shortMode = true;
            //Worklist.ColumnsSetting m_ColSet = shortMode ? new Worklist.ColumnsSetting("0,2", tt.DepartmentId, tt.UserId, config.AccountManager && config.LocationTracking, tt.OrganizationId) : 
            //    new Worklist.ColumnsSetting(tt.DepartmentId, tt.UserId, config.AccountManager && config.LocationTracking, tt.OrganizationId);
            //if sherpadesk
            //DataTable dt = Worklist.SelectTicketsByFilter(ee, config, m_ColSet, m_Filter, _qFilter, false, FilesSources.AzureFileService, limit);

            DataTable dt = Worklist.SelectTicketsByFilter(OrgId, DeptId, UserId, _qFilter, IsTechAdmin, pager);
            if (dt != null)
            {
               //var config = new Instance_Config()
                dt.Columns.Add(new DataColumn("DaysOldSort", typeof(long)));
                dt.Columns.Add(new DataColumn("DaysOlds", typeof(string)));
                foreach (DataRow dr in dt.Rows)
                {
                    int min = Utils.GetDaysOldInMinutes(OrgId, DeptId, IsUseWorkDaysTimer, dr.GetString("Status"), dr.Get<DateTime>("CreateTime"), dr.Get<DateTime?>("ClosedTime") ?? DateTime.UtcNow);
                    dr["DaysOldSort"] = min;
                    dr["DaysOlds"] = bigWebApps.bigWebDesk.Functions.DisplayDateDuration(min, config.BusinessDayLength, true);
                }
            }
            IEnumerable<WorklistTicket> wrklisttickets = new WorklistTickets(dt);
            return wrklisttickets.ToList();
        }
    }
}
