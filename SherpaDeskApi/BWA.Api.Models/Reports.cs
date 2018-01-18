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
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "project_time_log")]
    public class Reports : ModelItemBase
    {
        public Reports(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "project_time_id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["projecttimeid"]); }
            set { Row["projecttimeid"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row["user_fullname"].ToString(); }
            set { Row["user_fullname"] = value; }
        }

        [DataMember(Name = "task_type_name")]
        public string TaskTypeName
        {
            get { return Row["TaskTypeName"].ToString(); }
            set { Row["TaskTypeName"] = value; }
        }

        [DataMember(Name = "note")]
        public string Note
        {
            get { return Row["Note"].ToString(); }
            set { Row["Note"] = value; }
        }

        [DataMember(Name = "date")]
        public DateTime? Date
        {
            get
            {
                if (!Row.IsNull("Date")) return (DateTime)Row["Date"];
                else return null;
            }
            set
            {
                Row["Date"] = value;
            }
        }

        [DataMember(Name = "start_time")]
        public DateTime? StartTime
        {
            get
            {
                if (!Row.IsNull("StartTimeUTC")) return (DateTime)Row["StartTimeUTC"];
                else return null;
            }
            set
            {
                Row["StartTimeUTC"] = value;
            }
        }

        [DataMember(Name = "stop_time")]
        public DateTime? StopTime
        {
            get
            {
                if (!Row.IsNull("StopTimeUTC")) return (DateTime)Row["StopTimeUTC"];
                else return null;
            }
            set
            {
                Row["StopTimeUTC"] = value;
            }
        }

        [DataMember(Name = "hours")]
        public decimal? Hours
        {
            get { if (Row.IsNull("Hours")) return null; return Convert.ToDecimal(Row["Hours"]); }
            set { Row["Hours"] = value; }
        }

        [DataMember(Name = "fb_time_entry_id")]
        public int FBTimeEntryID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBTimeEntryID"))
                {
                    return Convert.ToInt32(Row["FBTimeEntryID"]);
                }
                return 0;
            }
            set { Row["FBTimeEntryID"] = value; }
        }

        [DataMember(Name = "fb_task_type_id")]
        public int FBTaskTypeID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBTaskTypeID"))
                {
                    return Convert.ToInt32(Row["FBTaskTypeID"]);
                }
                return 0;
            }
            set { Row["FBTaskTypeID"] = value; }
        }

        [DataMember(Name = "fb_staff_id")]
        public int FBStaffID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBStaffID"))
                {
                    return Convert.ToInt32(Row["FBStaffID"]);
                }
                return 0;
            }
            set { Row["FBStaffID"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int AccountID
        {
            get
            {
                if (Row.Table.Columns.Contains("AccountID"))
                {
                    return Convert.ToInt32(Row["AccountID"]);
                }
                return 0;
            }
            set { Row["AccountID"] = value; }
        }

        [DataMember(Name = "account_name")]
        public string AccountName
        {
            get
            {
                if (Row.Table.Columns.Contains("AccountName")) return Row["AccountName"].ToString();
                return "";
            }
            set { Row["AccountName"] = value; }
        }

        [DataMember(Name = "fb_client_id")]
        public int FBClientId
        {
            get
            {
                if (Row.Table.Columns.Contains("FBClientId"))
                {
                    return Convert.ToInt32(Row["FBClientId"]);
                }
                return 0;
            }
            set { Row["FBClientId"] = value; }
        }

        [DataMember(Name = "project_id")]
        public int ProjectID
        {
            get
            {
                if (Row.Table.Columns.Contains("ProjectID"))
                {
                    return Convert.ToInt32(Row["ProjectID"]);
                }
                return 0;
            }
            set { Row["ProjectID"] = value; }
        }

        [DataMember(Name = "project_name")]
        public string ProjectName
        {
            get
            {
                if (Row.Table.Columns.Contains("ProjectName")) return Row["ProjectName"].ToString();
                return "";
            }
            set { Row["ProjectName"] = value; }
        }

        [DataMember(Name = "fb_project_id")]
        public int FBProjectID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBProjectID"))
                {
                    return Convert.ToInt32(Row["FBProjectID"]);
                }
                return 0;
            }
            set { Row["FBProjectID"] = value; }
        }

        [DataMember(Name = "fb_default_project_id")]
        public int FBDefaultProjectId
        {
            get
            {
                if (Row.Table.Columns.Contains("FBDefaultProjectId"))
                {
                    return Convert.ToInt32(Row["FBDefaultProjectId"]);
                }
                return 0;
            }
            set { Row["FBDefaultProjectId"] = value; }
        }
    }
}
