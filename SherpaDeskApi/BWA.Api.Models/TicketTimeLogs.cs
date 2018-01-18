// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Runtime.Serialization;
using System.Collections;

namespace BWA.Api.Models
{
    [DataContract(Name = "project_ticket_time_logs")]
    public class ProjectTicketTimeLogs : ModelItemCollectionGeneric<Ticket_Time_Log>
    {
        public ProjectTicketTimeLogs(DataTable TicketTimeLogsTable) : base(TicketTimeLogsTable) { }

        public static List<Ticket_Time_Log> GetProjectTicketTimeLogs(Guid organizationId, int departmentId, int projectId, int accountId, int techId, int taskTypeId, int offset)
        {
            ProjectTicketTimeLogs _projectTicketTimeLogs = new ProjectTicketTimeLogs(bigWebApps.bigWebDesk.Data.Tickets.SelectProjectTimeLogs(organizationId, departmentId, DateTime.MinValue, projectId, techId, accountId, taskTypeId, offset));

            var projectTicketTimeLogs = _projectTicketTimeLogs.List;

            foreach (Ticket_Time_Log tl in projectTicketTimeLogs)
            {
                tl.Ticket_Key = Models.Ticket.GetPseudoId(organizationId, departmentId, tl.TicketID);
            }

            return projectTicketTimeLogs;
        }
    }

    [DataContract(Name = "ticket_time_logs")]
    public class TicketTimeLogs : ModelItemCollectionGeneric<Ticket_Time_Log>
    {
        public TicketTimeLogs(DataTable TicketTimeLogsTable) : base(TicketTimeLogsTable) { }

        public static List<Ticket_Time_Log> GetTicketTimeLogs(Guid organizationId, int departmentId, int ticketId)
        {
            List<Ticket_Time_Log> _ticketTimeLogs = new TicketTimeLogs(bigWebApps.bigWebDesk.Data.Tickets.SelectTimes(organizationId, departmentId, ticketId)).ToList();

            return _ticketTimeLogs; 
        }
    }


    [DataContract(Name = "project_time_logs")]
    public class ProjectTimeLogs : ModelItemCollectionGeneric<ProjectTimeLog>
    {
        public ProjectTimeLogs(DataTable ProjectTimeLogsTable) : base(ProjectTimeLogsTable) { }

        public static List<ProjectTimeLog> GetProjectTimeLog(Guid organizationId, int departmentId, int projectId, int accountId, int techId, int taskTypeId)
        {
            IEnumerable<ProjectTimeLog> _projectTimeLog = new ProjectTimeLogs(bigWebApps.bigWebDesk.Data.Project.SelectProjectTimeList(organizationId, departmentId, projectId, DateTime.MinValue, techId,
                accountId, taskTypeId));
 
            return _projectTimeLog.ToList();
        }
    }

    [DataContract(Name = "common_time_logs")]
    public class CommonTimeLogs : ModelItemCollectionGeneric<CommonTimeLog>
    {
        public CommonTimeLogs(DataTable CommonTimeLogsTable) : base(CommonTimeLogsTable) { }

        public static List<CommonTimeLog> GetCommonTimeLog(Guid organizationId, int departmentId, string type, int accountID,
            int projectID, int techID, DateTime? start_date, DateTime? end_date, string timeZoneId)
        {
            int linkedFB = -1;
            int invoiced = -1;
            int billable = -1;
            int linkedQB = -1;
            int hiddenFromInvoice = -1;
            int onlyProjectTime = -1;
            switch (type)
            {
                case "linked_fb":
                    linkedFB = 1;
                    break;
                case "unlinked_fb":
                    linkedFB = 0;
                    break;
                case "invoiced":
                    invoiced = 1;
                    break;
                case "not_invoiced":
                    invoiced = 0;
                    break;
                case "unlinked_fb_billable":
                    linkedFB = 0;
                    billable = 1;
                    break;
                case "not_invoiced_billable":
                    invoiced = 0;
                    billable = 1;
                    break;
                case "not_invoiced_nonbillable":
                    invoiced = 0;
                    billable = 0;
                    break;
                case "hidden_from_invoice":
                    hiddenFromInvoice = 1;
                    break;
                case "linked_qb":
                    linkedQB = 1;
                    break;
                case "unlinked_qb":
                    linkedQB = 0;
                    break;
                case "unlinked_qb_billable":
                    linkedQB = 0;
                    billable = 1;
                    break;
                case "no_ticket_assigned":
                    onlyProjectTime = 1;
                    break;
            }
            TimeZoneInfo userTimeZone = null;
            if (!string.IsNullOrEmpty(timeZoneId))
            {
                try
                {
                    userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                }
                catch { }
            }
            DataTable dtTimeLogs = bigWebApps.bigWebDesk.Data.Tickets.SelectTop100MostTimeLogs(organizationId, departmentId, linkedFB, invoiced, accountID, projectID,
                techID, billable, linkedQB, hiddenFromInvoice, start_date, end_date, onlyProjectTime);
            if (userTimeZone != null)
            {
                foreach(DataRow row in dtTimeLogs.Rows)
                {
                    if (!row.IsNull("CreatedTime"))
                    {
                        row["CreatedTime"] = TimeZoneInfo.ConvertTimeFromUtc((DateTime)row["CreatedTime"], userTimeZone);
                    }
                    if (!row.IsNull("UpdatedTime"))
                    {
                        row["UpdatedTime"] = TimeZoneInfo.ConvertTimeFromUtc((DateTime)row["UpdatedTime"], userTimeZone);
                    }
                }
            }
            IEnumerable<CommonTimeLog> _commonTimeLog = new CommonTimeLogs(dtTimeLogs);
            var commonTimeList = _commonTimeLog.ToList();
            Hashtable htUserProfileImage = new Hashtable();
            foreach (CommonTimeLog ctl in commonTimeList)
            {
                if(!htUserProfileImage.Contains(ctl.UserEmail))
                {
                    htUserProfileImage.Add(ctl.UserEmail, ProfileImageProvider.GetImageUrl(ctl.UserEmail));
                }
                ctl.UserProfileImage = htUserProfileImage[ctl.UserEmail].ToString();
            }
            return commonTimeList;
        }
    }
}
