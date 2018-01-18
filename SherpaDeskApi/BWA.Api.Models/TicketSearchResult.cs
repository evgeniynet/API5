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
    [DataContract(Name = "Ticket_Search_Result")]
    public class TicketSearchResult : ModelItemBase
    {
        public TicketSearchResult(DataRow row) : base(row)
        { }

        [DataMember(Name = "id")]
        public int Id
        {
            get
            {
                return Row.Get<int>("Id");
            }
            set
            {
                Row["Id"] = value;
            }
        }

        [DataMember(Name = "number_subject")]
        public string TicketNumberAndSubject
        {
            get { return Row["TicketNumber"].ToString(); }
            set { Row["TicketNumber"] = value.ToString(); }
        }

        [DataMember(Name = "ticket_key")]
        public string TicketKey
        {
            get { return Row.GetString("PseudoId"); }
            set { Row["PseudoId"] = value; }
        }

    }
}
