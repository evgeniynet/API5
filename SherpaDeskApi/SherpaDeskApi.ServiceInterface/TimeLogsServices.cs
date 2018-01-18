// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace SherpaDeskApi.ServiceInterface
{
    public class TimeLogsService : Service
    {
        [Secure()]
        public object Get(GET_Time_Logs request)
        {
            var query = base.Request.QueryString;
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (query.Count > 0 ? ":" + query.ToString() : "")),
                new System.TimeSpan(0, 1, 0), () =>
                {
                    return GetTimeLogs(request);
                });
        }

        private object GetTimeLogs(GET_Time_Logs request)
        {

            ApiUser hdUser = request.ApiUser;
           //v2
            if (request.project_time_id > 0)
            {
                DataRow plRow = bigWebApps.bigWebDesk.Data.Project.SelectProjectTimeByID(hdUser.OrganizationId, hdUser.DepartmentId, request.project_time_id);
                if (plRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "No data found");
                }
                return new ProjectTimeLog(plRow);
            }
            else if (request.ticket_time_id > 0)
            {
                DataRow tlRow = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketTimeByID(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket_time_id);
                if (tlRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "No data found");
                }
                return new Ticket_Time_Log(tlRow);
            }

            if (!string.IsNullOrEmpty(request.ticket))
                return request.FilteredResult<Ticket_Time_Log>(TicketTimeLogs.GetTicketTimeLogs(hdUser.OrganizationId, hdUser.DepartmentId, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket)));

            int accountID = request.account ?? 0;
            int projectID = request.project ?? 0;
            int techID = request.tech ?? 0;
            string type = string.IsNullOrEmpty(request.type) ? "recent" : request.type.ToLower();

            if ((string.IsNullOrEmpty(request.type) || type == "tickets") && request.project.HasValue)
            {
                if (projectID == 0)
                    projectID = -2;

                if (!request.account.HasValue)
                    accountID = -2;

                if (projectID > 0 && accountID < 0)
                {
                    ProjectDetail projectDetail = Models.Projects.GetProjectDetails(hdUser.OrganizationId, hdUser.DepartmentId, projectID);
                    accountID = projectDetail.AccountId;
                }
                if (type == "tickets")
                    return request.FilteredResult<Ticket_Time_Log>(Models.ProjectTicketTimeLogs.GetProjectTicketTimeLogs(hdUser.OrganizationId, hdUser.DepartmentId, projectID, accountID, techID, request.task_type_id, hdUser.TimeZoneOffset));
                return request.FilteredResult<ProjectTimeLog>(Models.ProjectTimeLogs.GetProjectTimeLog(hdUser.OrganizationId, hdUser.DepartmentId, projectID, accountID, techID, request.task_type_id));
            }

            if (type == "recent"  || type == "linked_fb" || type == "unlinked_fb"
                || type == "invoiced" || type == "not_invoiced" || type == "unlinked_fb_billable"
                || type == "not_invoiced_billable" || type == "not_invoiced_nonbillable" || type == "linked_qb"
                || type == "unlinked_qb" || type == "unlinked_qb_billable" || type == "hidden_from_invoice" || type == "no_ticket_assigned")
            {
                return request.SortedResult<CommonTimeLog>(Models.CommonTimeLogs.GetCommonTimeLog(hdUser.OrganizationId, hdUser.DepartmentId, type, accountID, projectID, techID, request.start_date, request.end_date, hdUser.TimeZoneId));
            }
            throw new HttpError(HttpStatusCode.NotFound, "Incorrect time type");
        }

        [Secure()]
        public object Post(POST_Time_Logs request)
        {
            ApiUser hdUser = request.ApiUser;
            bool is_billable = request.is_billable ?? true;
            bool is_local_time = request.is_local_time ?? false;
            int techID = (request.tech_id > 0 && hdUser.IsTechAdmin) ? request.tech_id : hdUser.UserId;
            bigWebApps.bigWebDesk.Data.Ticket _tktNew = null;
            DateTime? start_date = request.start_date;
            DateTime? stop_date = request.stop_date;
            if (is_local_time)
            {
                if(request.start_date.HasValue && request.start_date > DateTime.MinValue)
                {
                    start_date = request.start_date.Value.AddHours(-1 * hdUser.TimeZoneOffset);
                }
                if (request.stop_date.HasValue && request.stop_date > DateTime.MinValue)
                {
                    stop_date = request.stop_date.Value.AddHours(-1 * hdUser.TimeZoneOffset);
                }
            }
            string key = request.ticket_key;
            string response = "";
            if (!string.IsNullOrEmpty(key) && key != "0" && request.project_time_id <= 0)
            {
                Ticket.InputTime(hdUser, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, key), request.task_type_id, start_date,
                    stop_date, request.hours, hdUser.TimeZoneOffset, HttpUtility.HtmlDecode(request.note_text), is_billable, request.date, request.ticket_time_id,
                    techID, ref _tktNew, request.complete, request.remain_hours, request.prepaid_pack_id, string.Empty);
            }
            else
            {
                int accountID = request.account_id != 0 ? request.account_id : -1;
                int projectID = request.project_id > 0 ? request.project_id : -1;
                response = Models.Projects.PostProjectTime(hdUser, accountID, projectID, techID, request.task_type_id, start_date, stop_date, request.hours, hdUser.TimeZoneOffset,
                    HttpUtility.HtmlDecode(request.note_text), is_billable, request.date, request.project_time_id, request.prepaid_pack_id, string.Empty, key);
            }
            if (_tktNew != null)
                return TicketLogRecords.TicketLog(hdUser.OrganizationId, hdUser.DepartmentId, _tktNew.ID, _tktNew.PseudoID);
            return new HttpResult(response, HttpStatusCode.OK);
        }
        
        [Secure()]
        public object Put(Time_Billable request)
        {
            ApiUser hdUser = request.ApiUser;
           //v1
            if (request.key == 0 && request.billable != null && request.billable.Contains("/"))
            {
                string[] keys = request.billable.Split('/');
                request.is_billable = keys[0].StartsWith("billable", StringComparison.InvariantCultureIgnoreCase);
                request.key = Convert.ToInt32(keys[1]);
            }
           //v2
            if (request.key == 0)
                throw new HttpError(HttpStatusCode.NotFound, "Time Id is missing");
            try
            {
                if (request.is_billable.HasValue && !request.hours.HasValue)
                {
                    bigWebApps.bigWebDesk.Data.Tickets.SetBillableTimeEntry(hdUser.OrganizationId, hdUser.DepartmentId, request.key, request.is_project_log, request.is_billable.Value);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                if (request.hidden_from_invoice.HasValue && !request.hours.HasValue)
                {
                    if(request.is_project_log)
                    {
                        bigWebApps.bigWebDesk.Data.Project.HideFromInvoiceProjectTime(hdUser.OrganizationId, hdUser.DepartmentId, request.key, request.hidden_from_invoice.Value);
                    }
                    else
                    {
                        bigWebApps.bigWebDesk.Data.Tickets.HideFromInvoiceTicketTime(hdUser.OrganizationId, hdUser.DepartmentId, request.key, request.hidden_from_invoice.Value);
                    }
                    return new HttpResult("", HttpStatusCode.OK);
                }
                int _contract_id = request.prepaid_pack_id.HasValue ? request.prepaid_pack_id.Value : 0;
                string contractName = string.Empty;
                if (!request.is_project_log)
                {
                    if (_contract_id > 0 && request.key > 0)
                    {
                        DataRow row = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketTimeByID(hdUser.OrganizationId, hdUser.DepartmentId, request.key);
                        if (row != null)
                        {
                            DateTime date = DateTime.UtcNow;
                            if (request.date.HasValue && request.date > DateTime.MinValue)
                            {
                                date = (DateTime)request.date;
                            }
                            contractName = Contracts.CheckAccountContract(hdUser, (int)row["AccountID"], _contract_id, date, request.key, 0, (int)row["ProjectID"]);
                        }
                    }
                    Ticket.EditTime(hdUser, request.key, request.start_date, request.stop_date, request.is_billable, request.date, request.hours, request.task_type_id, request.note_text,
                        request.remain_hours, request.complete, request.prepaid_pack_id, contractName);
                }
                else
                {
                    int techID = (request.tech_id > 0 && hdUser.IsAdmin) ? request.tech_id : hdUser.UserId;
                    int accountID = request.account_id != 0 ? request.account_id : -1;
                    int projectID = request.project_id > 0 ? request.project_id : -1;
                    DateTime date = DateTime.UtcNow;
                    if (request.date.HasValue && request.date > DateTime.MinValue)
                    {
                        date = (DateTime)request.date;
                    }
                    Contracts.CheckAccountContract(hdUser, accountID, request.prepaid_pack_id ?? 0, date, 0, request.key, projectID);
                    Models.Projects.EditProjectTime(hdUser, accountID, projectID, techID, request.task_type_id, request.start_date, request.stop_date, request.hours, hdUser.TimeZoneOffset,
                    request.note_text, request.is_billable, request.date, request.key, request.prepaid_pack_id);
                }

                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(Time_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (!instanceConfig.TimeTracking) throw new HttpError("Time Tracking is not enabled for this instance.");
            try
            {
                if (request.is_project_log)
                {
                    Models.Projects.DeleteProjectTime(hdUser, request.key);
                }
                else
                {
                    Models.Ticket.DeleteTicketTime(hdUser, request.key);
                }
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }
    }
}
