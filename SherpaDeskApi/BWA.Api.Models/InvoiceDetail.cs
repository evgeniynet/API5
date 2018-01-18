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
    [DataContract(Name = "InvoiceDetail")]
    public class InvoiceDetail : ModelItemBase
    {
        public InvoiceDetail(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "date")]
        public DateTime? date
        {
            get { return Row.Get<DateTime?>("TechStart") ?? Row.Get<DateTime?>("Date"); }
            set { Row["Level"] = value; }
        }

        [DataMember(Name = "name")]
        public string name
        {
            get { return Row.GetString("TechName") ?? Row.GetString("FullName"); }
        }

        [DataMember(Name = "total")]
        public decimal? total
        {
            get
            {
                decimal? total = Row.Get<decimal?>("Total") ?? Row.Get<decimal?>("Amount");
                if (total.HasValue)
                    return total;
                decimal? billrate = Row.Get<decimal>("BillRate");
                decimal? hours = Row.Get<decimal>("Hours");
                return billrate * hours;
            }
        }

        [DataMember(Name = "hours")]
        public decimal? hours
        {
            get
            {
                return Row.Get<decimal?>("Hours");
            }
        }

        [DataMember(Name = "id")]
        public string id
        {
            get { return Row.GetString("Id"); }
        }

        [DataMember(Name = "ticket_id")]
        public string ticket_id
        {
            get { return Row.GetString("TicketId"); }
        }
    }
}
