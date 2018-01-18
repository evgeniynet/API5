// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;
using ServiceStack.Common.Web;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "Projects")]
    public class Projects : ModelItemCollectionGeneric<Project>
    {
        public Projects(DataTable ProjectsTable) : base(ProjectsTable) { }

		public static List<Project> GetProjects(Guid organizationId, int departmentId, int accountId, int userId, bool isAccountManager, bool is_with_statistics)
        {
			var projectList = is_with_statistics ? 
				bigWebApps.bigWebDesk.Data.Project.SelectListWithHours (organizationId, departmentId, accountId, 1, userId, false, isAccountManager)
				: bigWebApps.bigWebDesk.Data.Project.SelectList(organizationId, departmentId, accountId, 1, userId, false, isAccountManager);
				 
			Projects _Projects = new Projects(projectList);
            return _Projects.List;
        }

        public static ProjectDetail GetProjectDetails(Guid OrgId, int DeptId, int projectId)
        {
            DataRow rowProjectDetail = bigWebApps.bigWebDesk.Data.Project.SelectDetail(OrgId, DeptId, projectId);
            if (rowProjectDetail == null)
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Wrong Project Id");
            return new ProjectDetail(rowProjectDetail, projectId);
        }

        public static void EditProjectTime(ApiUser User, int? AccountId, int? ProjectId, int? TechId, int TaskTypeId, DateTime? start_date, DateTime? stop_date,
            decimal? Hours, int HoursOffset, string NoteText, bool? is_billable, DateTime? date, int projectTimeID, int? contract_id)
        {
            if (projectTimeID <= 0)
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Incorrect Project Time Id");

            ProjectTimeLog timeEntry = null;

            DataRow row = bigWebApps.bigWebDesk.Data.Project.SelectProjectTimeByID(User.OrganizationId, User.DepartmentId, projectTimeID);
            if (row == null)
            {
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Project Time Id Not Found");
            }
            else
            {
                timeEntry = new ProjectTimeLog(row);
                if (timeEntry.InvoiceID > 0)
                {
                    throw new HttpError("Time log has been invoiced and can not be edited.");
                }
                if (timeEntry.PaymentId > 0)
                {
                    throw new HttpError("Time log has been paid and can not be edited.");
                }
            }

            ProjectId = ProjectId ?? timeEntry.ProjectID;

            if (ProjectId.Value > 0 && bigWebApps.bigWebDesk.Data.Project.SelectDetail(User.OrganizationId, User.DepartmentId, ProjectId.Value) == null)
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Project Id Not Found");

            if (TaskTypeId > 0)
            {
                DataRow _rowTaskType = bigWebApps.bigWebDesk.Data.TaskType.SelectTaskType(User.OrganizationId, User.DepartmentId, TaskTypeId);
                if (_rowTaskType == null)
                    throw new HttpError(System.Net.HttpStatusCode.NotFound, "No Task Types found for TaskTypeId=" + TaskTypeId.ToString() + ".");
            }
            else
                TaskTypeId = timeEntry.TaskTypeID;

            if (Hours > 999)
            {
                throw new HttpError("Hours value should be less 1000.");
            }
            if (Hours == 0)
                Hours = timeEntry.Hours.Value;

            DateTime startDate = DateTime.MinValue;
            DateTime stopDate = DateTime.MinValue;
            if (start_date.HasValue && start_date > DateTime.MinValue)
                startDate = start_date.Value;//.AddHours(HoursOffset);
            else if (timeEntry.StartTime.HasValue && timeEntry.StartTime > DateTime.MinValue)
            {
                startDate = timeEntry.StartTime.Value;
            }

            if (stop_date.HasValue && stop_date > DateTime.MinValue)
            {
                stopDate = stop_date.Value;//.AddHours(HoursOffset);
            }
            else if (startDate != DateTime.MinValue)
            {
                stopDate = startDate.AddHours((double)Hours);
            }
            else if (timeEntry.StopTime.HasValue && timeEntry.StopTime > DateTime.MinValue)
            {
                stopDate = timeEntry.StopTime.Value;
            }
            if (Hours == 0 && startDate > DateTime.MinValue && stopDate > DateTime.MinValue)
                Hours = Convert.ToDecimal((stopDate - startDate).TotalHours);
            DateTime dateTimeLog = timeEntry.Date.Value;
            if (date.HasValue && date > DateTime.MinValue)
            {
                dateTimeLog = (DateTime)date;
            }

            is_billable = is_billable ?? timeEntry.Billable;
            TechId = TechId ?? timeEntry.UserID;
            AccountId = AccountId ?? timeEntry.AccountID;
            NoteText = !string.IsNullOrWhiteSpace(NoteText) ? NoteText : timeEntry.Note;

            bigWebApps.bigWebDesk.Data.Project.UpdateTimeRecord(User.OrganizationId, projectTimeID, User.DepartmentId, ProjectId.Value, TechId.Value, TaskTypeId, dateTimeLog,
                Hours.Value, NoteText, startDate, stopDate, DateTime.UtcNow, User.UserId, HoursOffset, bigWebApps.bigWebDesk.Data.Logins.SelectTechHourlyRate(User.OrganizationId, User.DepartmentId, User.UserId, TaskTypeId),
                ProjectId.Value == -1 ? AccountId.Value : -1, is_billable.Value, contract_id ?? 0);
        }

        public static string PostProjectTime(ApiUser User, int AccountId, int ProjectId, int TechId, int TaskTypeId, DateTime? start_date, DateTime? stop_date,
            decimal Hours, int HoursOffset, string NoteText, bool is_billable, DateTime? date, int projectTimeID, int? contract_id, string toDoItemId, string ticket_key)
        {
            string response = "";
            if (ProjectId > 0 && bigWebApps.bigWebDesk.Data.Project.SelectDetail(User.OrganizationId, User.DepartmentId, ProjectId) == null)
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Wrong Project Id");
            DateTime dateTimeLog = DateTime.UtcNow;
            if (date.HasValue && date > DateTime.MinValue)
            {
                dateTimeLog = (DateTime)date;
            }
            string contractName = Contracts.CheckAccountContract(User, AccountId, contract_id ?? 0, dateTimeLog, 0, projectTimeID, ProjectId);
            string _taskTypeName = string.Empty;
            DataRow _rowTaskType = bigWebApps.bigWebDesk.Data.TaskType.SelectTaskType(User.OrganizationId, User.DepartmentId, TaskTypeId);
            if (_rowTaskType == null)
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "No Task Types found for TaskTypeId=" + TaskTypeId.ToString() + ".");
            _taskTypeName = _rowTaskType["TaskTypeName"].ToString();
            if ((Hours > 999) || (Hours < -999))
            {
                throw new HttpError("Hours value should be between -999 and 999.");
            }
            DateTime startDate = DateTime.MinValue;
            DateTime stopDate = DateTime.MinValue;
            if (start_date.HasValue && start_date > DateTime.MinValue)
                startDate = start_date.Value;//.AddHours(HoursOffset);
            if (stop_date.HasValue && stop_date > DateTime.MinValue)
            {
                stopDate = stop_date.Value;//.AddHours(HoursOffset);
            }
            else if (startDate != DateTime.MinValue)
            {
                stopDate = startDate.AddHours((double)Hours);
            }
            if (Hours == 0 && startDate > DateTime.MinValue && stopDate > DateTime.MinValue)
                Hours = Convert.ToDecimal((stopDate - startDate).TotalHours);
            if (projectTimeID > 0)
            {
                EditProjectTime(User, AccountId, ProjectId, TechId, TaskTypeId, start_date, stop_date, Hours, HoursOffset, NoteText + "", is_billable, date, projectTimeID, contract_id);
                if (!string.IsNullOrEmpty(ticket_key) && ticket_key != "0")
                {
                    int tktId = Ticket.GetId(User.OrganizationId, User.DepartmentId, ticket_key);
                    if(tktId > 0)
                    {
                        int _timeLogId = bigWebApps.bigWebDesk.Data.Project.MoveProjectTimeToTicket(User.OrganizationId, User.DepartmentId, projectTimeID, tktId);
                        response = _timeLogId.ToString();
                        string _hoursFull = "";
                        if (Hours >= 1)
                        {
                            _hoursFull = ((int)Hours).ToString();
                            if ((int)Hours == 1) _hoursFull += " hour ";
                            else _hoursFull += " hours ";
                        }

                        string _minutes = string.Format("{0:00}", Hours * 60 % 60).TrimStart('0');

                        if (!string.IsNullOrEmpty(_minutes))
                        {
                            _hoursFull += _minutes;
                            if (_minutes == "1") _hoursFull += " minute";
                            else _hoursFull += " minutes";
                        }

                        if (!string.IsNullOrEmpty(_hoursFull)) _hoursFull = "(" + _hoursFull.Trim() + ")";
                        string sysGeneratedText = User.FullName + " logged " + Hours.ToString("0.00") + " hours " + _hoursFull + " as " + _taskTypeName + " task type";
                        if (contract_id > 0)
                        {
                            sysGeneratedText += " on " + contractName + " contract";
                        }
                        sysGeneratedText += ".";
                        CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);
                        bigWebApps.bigWebDesk.Data.Ticket _tkt = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, tktId, true);
                        bigWebApps.bigWebDesk.Data.Ticket.InsertLogMessage(User.OrganizationId, User.DepartmentId, tktId, TechId, "Log Entry", NoteText, sysGeneratedText, string.Empty, string.Empty, string.Empty, _timeLogId);
                        if (NoteText.Length > 0) sysGeneratedText = NoteText + "<br><br>" + sysGeneratedText;
                        _tkt.TicketLogs.Insert(0, new bigWebApps.bigWebDesk.Data.Ticket.LogEntry(TechId, User.LoginEmail, User.FirstName, User.LastName, DateTime.UtcNow, _cNames.Technician.FullSingular + " Costs", sysGeneratedText));
                        foreach (bigWebApps.bigWebDesk.Data.Ticket.TicketAssignee ta in _tkt.Technicians)
                            ta.SendResponse = true;
                        foreach (bigWebApps.bigWebDesk.Data.Ticket.TicketAssignee ta in _tkt.Users)
                            ta.SendResponse = true;
                        bigWebApps.bigWebDesk.Data.NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, TechId,
                            bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.EnterLaborCosts, _tkt, null, null);
                    }
                }
                return response;
            }
            bigWebApps.bigWebDesk.Data.Project.InsertTimeRecord(User.OrganizationId, User.DepartmentId, ProjectId, TechId,
                    TaskTypeId, dateTimeLog, Hours, NoteText + "",
                    startDate,
                    stopDate,
                    DateTime.UtcNow,
                    TechId, HoursOffset, bigWebApps.bigWebDesk.Data.Logins.SelectTechHourlyRate(User.OrganizationId, User.DepartmentId, TechId, TaskTypeId),
                    ProjectId == -1 ? AccountId : -1, is_billable, contract_id ?? 0, toDoItemId);
            return response;
        }

        public static void DeleteProjectTime(ApiUser user, int projectTimeID)
        {
            DataRow row = bigWebApps.bigWebDesk.Data.Project.SelectProjectTimeByID(user.OrganizationId, user.DepartmentId, projectTimeID);
            if (row == null)
            {
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Wrong Time Id");
            }
            else
            {
                if (int.Parse(row["InvoiceId"].ToString()) > 0)
                {
                    throw new HttpError("Time log has been invoiced and can not be removed.");
                }
            }
            bigWebApps.bigWebDesk.Data.Project.DeleteProjectTime(projectTimeID, user.DepartmentId, user.OrganizationId);
        }
    }
}
