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
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "TaskTypes")]
    public class TaskTypes : ModelItemCollectionGeneric<TaskType>
    {
        public TaskTypes(DataTable TaskTypesTable) : base(TaskTypesTable) { }

        public static List<TaskType> TicketAssignedTaskTypes(Guid OrgId, int DeptId, int UserId, int TktId)
        {
            var _taskTypes = new TaskTypes(bigWebApps.bigWebDesk.Data.TaskType.SelectTicketAssignedTaskTypes(OrgId, DeptId, UserId, TktId));
            return _taskTypes.List;
        }

        public static List<TaskType> SelectAllTaskTypes(Guid OrgId, int DeptId)
        {
            DataTable dt = bigWebApps.bigWebDesk.Data.TaskType.SelectAll(OrgId, DeptId);
            dt.Columns[0].ColumnName = "ttID";
            var _taskTypes = new TaskTypes(dt);
            return _taskTypes.List;
        }

        public static List<TaskType> SelectProjectTaskTypes(Guid OrgId, int DeptId, int UserId, int ProjectId)
        {
            DataTable dt = bigWebApps.bigWebDesk.Data.TaskType.SelectProjectAssignedTaskTypes(OrgId, DeptId, UserId, ProjectId);
            var _taskTypes = new TaskTypes(dt);
            return _taskTypes.List;
        }

        public static List<TaskType> SelectAccountTaskTypes(Guid OrgId, int DeptId, int UserId, int accountID)
        {
            DataTable dt = bigWebApps.bigWebDesk.Data.TaskType.SelectAccountAssignedTaskTypes(OrgId, DeptId, UserId, accountID);
            var _taskTypes = new TaskTypes(dt);
            return _taskTypes.List;
        }
    }
}
