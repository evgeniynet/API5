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
    [DataContract(Name = "Account_Tickets_Count")]
    public class AccountTicketCount : ModelItemBase
    {
        public AccountTicketCount(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "open")]
        public int Open
        {
			get { return Row.Get<int>("OpenTkt"); }
            set { Row["OpenTkt"] = value; }
        }

        [DataMember(Name = "closed")]
        public int Closed
        {
			get { return Row.Get<int>("ClosedTkt"); }
            set { Row["ClosedTkt"] = value; }
        }


		[DataMember(Name = "hours")]
		public decimal Hours
		{
			get { return Row.Get<decimal>("TotalHours"); }
			set { Row["TotalHours"] = value; }
		}


		[DataMember(Name = "total_invoiced_amount")]
		public decimal TotalInvoicedAmount
		{
			get { return Row.Get<decimal>("TotalInvoicedAmount"); }
			set { Row["TotalInvoicedAmount"] = value; }
		}


		[DataMember(Name = "total_non_invoiced_amount")]
		public decimal TotalNonInvoicedAmount
		{
			get { return Row.Get<decimal>("TotalNonInvoicedAmount"); }
			set { Row["TotalNonInvoicedAmount"] = value; }
		}

		[DataMember(Name = "total_billed_amount")]
		public decimal TotalBillAmount
		{
			get { return Row.Get<decimal>("TotalBillAmount"); }
			set { Row["TotalBillAmount"] = value; }
		}


		[DataMember(Name = "total_unbilled_amount")]
		public decimal TotalUnBilledAmount
		{
			get { return Row.Get<decimal>("TotalUnBilledAmount"); }
			set { Row["TotalUnBilledAmount"] = value; }
		}

        [DataMember(Name = "scheduled")]
        public int Scheduled
        {
			get { return Row.Get<int>("SchedTktCount"); }
            set { Row["SchedTktCount"] = value; }
        }

        [DataMember(Name = "followups")]
        public int FollowUps
        {
			get { return Row.Get<int>("FollowUpTkt"); }
            set { Row["FollowUpTkt"] = value; }
        }

        public static AccountTicketCount GetAccountStat(int open)
        {
            Models.AccountTicketCount account = null;
                DataTable table = new DataTable();
                table.Columns.Add("OpenTkt", typeof(int));
                account = new Models.AccountTicketCount (table.NewRow());
                account.Open = open;
            return account;
        }
    }
}
