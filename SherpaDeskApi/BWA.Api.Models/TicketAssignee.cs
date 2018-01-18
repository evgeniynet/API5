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
    [DataContract(Name = "Ticket_Assignee")]
    public class TicketAssignee : ModelItemBase
    {
        public TicketAssignee(DataRow row) : base(row){}

        [DataMember(Name = "user_id")]
        public int UserId
        {
            get { return (int)Row["UserId"]; }
            set { Row["UserId"] = value; }
        }

        [DataMember(Name = "user_fullname")]
        public string UserFullName
        {
            get { return Row["Name"].ToString(); }
            set { Row["Name"] = value; }
        }

        [DataMember(Name = "is_primary")]
        public bool IsPrimary
        {
            get 
            {
                if (Row.Table.Columns.Contains("IsPrimary")) return (bool)Row["IsPrimary"];
                else return false;
            }
            set 
            {
                if (Row.Table.Columns.Contains("IsPrimary")) Row["IsPrimary"]=value;
            }
        }

        [DataMember(Name = "start_date")]
        public DateTime? StartDate
        {
            get 
            {
                if (Row.Table.Columns.Contains("StartDate") && !Row.IsNull("StartDate")) return (DateTime)Row["StartDate"];
                else return null;
            }
            set 
            {
                if (Row.Table.Columns.Contains("StartDate")) Row["StartDate"]=value;
            }
        }

        [DataMember(Name = "stop_date")]
        public DateTime? StopDate
        {
            get
            {
                if (Row.Table.Columns.Contains("StopDate") && !Row.IsNull("StopDate")) return (DateTime)Row["StopDate"];
                else return null;
            }
            set
            {
                if (Row.Table.Columns.Contains("StopDate")) Row["StopDate"] = value;
            }
        }

        [DataMember(Name = "user_fullname2")]
        public string UserFullName2
        {
            get { return Row.GetString("Name2"); }
            set { Row["Name2"] = value; }
        }
    }
}
