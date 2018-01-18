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
    [DataContract(Name = "Unassigned_Queue")]
    public class UnassignedQueue : ModelItemBase
    {
        public UnassignedQueue(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }
        
        [DataMember(Name = "id")]
        public string Id
        {
            get { return Row["Id"].ToString(); }
            set { Row["Id"] = value; }
        }

       //[DataMember(Name = "email")]
        public string Email
        {
            get { return Row["QueEmailAddress"].ToString(); }
            set { Row["QueEmailAddress"] = value; }
        }

        [DataMember(Name = "fullname")]
        public string FullName
        {
            get { return Row["FullName"].ToString().Replace(" Queue", ""); }
            set { Row["FullName"] = value; }
        }

        [DataMember(Name = "tickets_count")]
        public int TicketsCount
        {
            get { return Row.Table.Columns.Contains("TicketsCount") ? (int)Row["TicketsCount"] : 0; }
            set { if (Row.Table.Columns.Contains("TicketsCount")) Row["TicketsCount"] = value; }
        }
    }
}
