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
    [DataContract(Name = "activity")]
    public class ActivityLog : ModelItemBase
    {
        public ActivityLog(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["ObjectId"]); }
            set { Row["ObjectId"] = value; }
        }

        [DataMember(Name = "ticket_id")]
        public int TicketId
        {
            get { return Convert.ToInt32(Row["TicketId"]); }
            set { Row["TicketId"] = value; }
        }

        [DataMember(Name = "user_id")]
        public int? UserID
        {
            get { return !Row.IsNull("UserID") ? ((int?)Convert.ToInt32(Row["UserID"])) : null; }
            set { Row["UserID"] = value; }
        }

        [DataMember(Name = "friendly_id")]
        public string ObjectFriendlyId
        {
            get { return Row["ObjectFriendlyId"].ToString(); }
            set { Row["ObjectFriendlyId"] = value; }
        }

        [DataMember(Name = "user_name")]
        public string UserName
        {
            get { return Row["UserName"].ToString(); }
            set { Row["UserName"] = value; }
        }

        [DataMember(Name = "user_email")]
        public string UserEmail
        {
            get { return Row["UserEmail"].ToString(); }
            set { Row["UserEmail"] = value; }
        }

        [DataMember(Name = "date")]
        public DateTime? Date
        {
            get
            {
                if (Row.Table.Columns.Contains("Date") && !Row.IsNull("Date")) return (DateTime)Row["Date"];
                else return null;
            }
            set
            {
                if (Row.Table.Columns.Contains("Date")) Row["Date"] = value;
            }
        }

        [DataMember(Name = "object")]
        public string Object
        {
            get { return Row["Object"].ToString(); }
            set { Row["Object"] = value; }
        }

        [DataMember(Name = "title")]
        public string Title
        {
            get { return Row["Title"].ToString(); }
            set { Row["Title"] = value; }
        }

        [DataMember(Name = "note")]
        public string Note
        {
            get { return Row["Note"].ToString(); }
            set { Row["Note"] = value; }
        }
    }
}
