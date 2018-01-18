// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk.Data;
using System.Net;
using ServiceStack.Common.Web;

namespace BWA.Api.Models
{
    [DataContract(Name = "qb_time_activity")]
    public class QBTimeActivities
    {
        public static string CreateTimeActivity(ApiUser hdUser, Instance_Config instanceConfig, int qb_employee_id, int qb_customer_id, int qb_service_id, decimal hours, decimal hourly_rate, string notes,
            DateTime date, int timeLogID, bool isProjectLog, bool is_billable, DateTime? start_date, DateTime? stop_date, decimal hoursOffset, int timeActivityID, int qb_sync_token, bool overwrite_changes, int qb_vendor_id, bool is_rate_fixed)
        {
            return QuickBooks.CreateTimeActivity(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret,
                qb_employee_id, qb_customer_id, qb_service_id, hours, hourly_rate, notes, date, timeLogID, isProjectLog, is_billable, start_date, stop_date, (double)hoursOffset, timeActivityID,
                qb_sync_token, overwrite_changes, qb_vendor_id, is_rate_fixed);
        }

        public static void UnlinkQuickBooksTimeActivity(ApiUser hdUser, int timeLogID, bool isProjectLog)
        {
            QuickBooks.UnlinkQuickBooksTimeActivity(hdUser.OrganizationId, hdUser.DepartmentId, timeLogID, isProjectLog);
        }

        public static void DeleteQuickBooksTimeActivity(ApiUser hdUser, Instance_Config instanceConfig, int timeLogID, bool isProjectLog)
        {
            QuickBooks.DeleteAndUnlinkTimeActivity(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, timeLogID, isProjectLog);
        }
    }
}
