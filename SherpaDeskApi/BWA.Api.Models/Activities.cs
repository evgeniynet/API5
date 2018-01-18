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
    [DataContract(Name = "activities")]
    public class ActivityLogs : ModelItemCollectionGeneric<ActivityLog>
    {
        public ActivityLogs(DataTable ActivitiesTable) : base(ActivitiesTable) { }

        public static List<ActivityLog> GetAllActivity(Guid organizationId, int departmentId, int UserId, string sort_order, string sort_by)
        {
            ActivityLogs _activity = new ActivityLogs(bigWebApps.bigWebDesk.Data.Tickets.Select25MostActivities(organizationId, departmentId, UserId, sort_order, sort_by));           
            return _activity.List;
        }
    }
}
