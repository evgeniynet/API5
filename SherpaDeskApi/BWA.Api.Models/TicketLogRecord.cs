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
    [DataContract(Name = "Ticket_Log_Record")]
    public class TicketLogRecord : ModelItemBase
    {
        public TicketLogRecord(DataRow row) : base(row)
        {
        }

        [DataMember(Name = "id")]
        public int id
        {
            get { return (int)Row["Id"]; }
            set { Row["Id"] = value; }
        }

       //[DataMember(Name = "ticketid")]
        public int TicketId
        {
            get { return (int)Row["TId"]; }
            set { Row["TId"] = value; }
        }

        string _TktPseudoId;

        [DataMember(Name = "ticket_key")]
        public string TktPseudoId
        {
            get { return _TktPseudoId; }
            set { _TktPseudoId = value; }
        }

        [DataMember(Name = "user_id")]
        public int UserId
        {
            get { return !Row.IsNull("UId") ? (int)Row["UId"] : 0; }
            set { Row["UId"] = value; }
        }

        [DataMember(Name = "user_email")]
        public string UserEmail
        {
            get { return Row["Email"].ToString(); }
            set { Row["Email"] = value; }
        }

        [DataMember(Name = "user_firstname")]
        public string UserFirstName
        {
            get { return Row["FirstName"].ToString(); }
            set { Row["FirstName"] = value; }
        }

        [DataMember(Name = "user_lastname")]
        public string UserLastName
        {
            get { return Row["LastName"].ToString(); }
            set { Row["LastName"] = value; }
        }

        [DataMember(Name = "record_date")]
        public DateTime RecordDate
        {
            get { return (DateTime)Row["dtDate"]; }
            set { Row["dtDate"] = value; }
        }

        [DataMember(Name = "log_type")]
        public string LogType
        {
            get { return Row["vchType"].ToString(); }
            set { Row["vchType"] = value; }
        }

        [DataMember(Name = "note")]
        public string Note
        {
            get { return Row["vchNote"].ToString(); }
            set { Row["vchNote"] = value; }
        }

        [DataMember(Name = "ticket_time_id")]
        public int TicketTimeId
        {
            get { return Row.IsNull("TicketTimeId") ? 0 : (int)Row["TicketTimeId"]; }
            set { Row["TicketTimeId"] = value; }
        }

        [DataMember(Name = "sent_to")]
        public string SentTo
        {
            get { return Row["To"].ToString(); }
            set { Row["To"] = value; }
        }

        [DataMember(Name = "is_waiting")]
        public bool? is_waiting
        {
            get { return Row.Get<bool?>("Waiting"); }
            set { Row["Waiting"] = value; }
        }

        [DataMember(Name = "sla_used")]
        public int? sla_used
        {
            get { return Row.Get<int?>("SLAUsed"); }
            set { Row["SLAUsed"] = value; }
        }
    }
}
