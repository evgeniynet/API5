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
   /// Summary description for TicketLogRecord
   /// </summary>
    [DataContract(Name = "TaskType")]
    public class TaskType : ModelItemBase
    {
        public TaskType(DataRow row) : base(row)
        { }

        [DataMember(Name = "id")]
        public int Id
        {
            get {
                if (Row.Table.Columns.Contains("ttID"))
                {
                    return Convert.ToInt32(Row["ttID"]);
                }
                return Convert.ToInt32(Row["TaskTypeId"]);
            }
            set {
                if (Row.Table.Columns.Contains("ttID"))
                {
                    Row["ttID"] = value;
                }
                else
                {
                    Row["TaskTypeId"] = value;
                }
            }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row["TaskTypeName"].ToString(); }
            set { Row["TaskTypeName"] = value; }
        }

        [DataMember(Name = "fb_task_type_id")]
        public int FBTaskTypeID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBTaskTypeID") && !Row.IsNull("FBTaskTypeID"))
                {
                    return Convert.ToInt32(Row["FBTaskTypeID"]);
                }
                return 0;
            }
            set { Row["FBTaskTypeID"] = value; }
        }

        [DataMember(Name = "qb_service_id")]
        public int QBServiceID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBServiceID") && !Row.IsNull("QBServiceID"))
                {
                    return Convert.ToInt32(Row["QBServiceID"]);
                }
                return 0;
            }
            set { Row["QBServiceID"] = value; }
        }

        [DataMember(Name = "billable")]
        public bool Billable
        {
            get
            {
                return Row.Get<bool>("Billable");
            }
            set { Row["Billable"] = value; }
        }
    }
}
