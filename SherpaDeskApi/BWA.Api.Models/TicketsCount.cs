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
    [DataContract(Name = "Tickets_Count")]
    public class TicketCount : ModelItemBase
    {
        public TicketCount(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "new_messages")]
        public int New
        {
            get { return Row.IsNull("NewMessagesCount") ? 0 : (int)Row["NewMessagesCount"]; }
            set { Row["NewMessagesCount"] = value; }
        }

        [DataMember(Name = "open_all")]
        public int Open
        {
            get { return Row.IsNull("OpenAllTickets") ? 0 : (int)Row["OpenAllTickets"]; }
            set { Row["OpenAllTickets"] = value; }
        }
        
        [DataMember(Name = "open_as_tech")]
        public int OpenAsTech
        {
            get { return !Row.Table.Columns.Contains("OpenTickets") ? 0 : (int)Row["OpenTickets"]; }
            set { Row["OpenTickets"] = value; }
        }

        [DataMember(Name = "open_as_alttech")]
        public int OpenAsAltTech
        {
            get { return !Row.Table.Columns.Contains("OpenAsAltTechTickets") ? 0 : (int)Row["OpenAsAltTechTickets"]; }
            set { Row["OpenAsAltTechTickets"] = value; }
        }

        [DataMember(Name = "open_as_user")]
        public int OpenAsUser
        {
            get { return Row.IsNull("userTickets") ? 0 : (int)Row["userTickets"]; }
            set { Row["userTickets"] = value; }
        }

        [DataMember(Name = "onhold")]
        public int OnHold
        {
            get { return Row.IsNull("OnHoldTickets") ? 0 : (int)Row["OnHoldTickets"]; }
            set { Row["OnHoldTickets"] = value; }
        }

        [DataMember(Name = "reminder")]
        public int Reminder
        {
            get { return Row.IsNull("reminderTicket") ? 0 : (int)Row["reminderTicket"]; }
            set { Row["reminderTicket"] = value; }
        }

        [DataMember(Name = "parts_on_order")]
        public int PartsOnOrder
        {
            get { return Row.IsNull("PartsOnOrderTickets") ? 0 : (int)Row["PartsOnOrderTickets"]; }
            set { Row["PartsOnOrderTickets"] = value; }
        }

        [DataMember(Name = "unconfirmed")]
        public int Unconfirmed
        {
            get { return Row.IsNull("UnconfirmedUserTickets") ? 0 : (int)Row["UnconfirmedUserTickets"]; }
            set { Row["UnconfirmedUserTickets"] = value; }
        }

        [DataMember(Name = "waiting")]
        public object Waiting
        {
            get { return Row.IsNull("WaitingTickets") ? 0 : (int)Row["WaitingTickets"]; }
            set { Row["WaitingTickets"] = value; }
        }
    }
}
