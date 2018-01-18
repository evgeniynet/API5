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
    [DataContract(Name = "fb_time_entry")]
    public class FBTimeEntries
    {
        public static string CreateTimeEntry(ApiUser hdUser, Instance_Config instanceConfig, int staffID, int projectID, int taskID,
            decimal hours, string notes, DateTime date, int timeLogID, bool isProjectLog, int timeEntryID)
        {
            return FreshBooks.CreateTimeEntry(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret,
                staffID, projectID, taskID, hours, notes, date, timeLogID, isProjectLog, timeEntryID);
        }

        public static void UnlinkFreshBooksTimeEntry(ApiUser hdUser, int timeLogID, bool isProjectLog)
        {
            FreshBooks.UnlinkFreshBooksTimeEntry(hdUser.OrganizationId, hdUser.DepartmentId, timeLogID, isProjectLog);
        }

        public static void DeleteFreshBooksTimeEntry(ApiUser hdUser, Instance_Config instanceConfig, int timeLogID, bool isProjectLog)
        {
            FreshBooks.DeleteAndUnlinkTimeEntry(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, timeLogID, isProjectLog);
        }
    }
}
