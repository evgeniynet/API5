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
    [DataContract(Name = "common_time_log")]
    public class CommonTimeLog : ModelItemBase
    {
        public CommonTimeLog(DataRow row) : base(row) { 
            if (row == null)
                throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "time_id")]
        public int TimeLogID
        {
            get { return Convert.ToInt32(Row["id"]); }
            set { Row["id"] = value; }
        }

        [DataMember(Name = "project_name")]
        public string ProjectName
        {
            get { return Row["ProjectName"].ToString(); }
            set { Row["ProjectName"] = value; }
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

        [DataMember(Name = "user_id")]
        public int UserId
        {
            get
            {
                return Row.Get<int>("UserId");
            }
            set
            {
                Row["UserId"] = value;
            }
        }

        [DataMember(Name = "note")]
        public string Note
        {
            get { return HttpUtility.HtmlDecode(Utils.NormalizeString(Row["Note"].ToString())); }
            set { Row["Note"] = value; }
        }

        [DataMember(Name = "date")]
        public DateTime? Date
        {
            get
            {
                return Row.Get<DateTime?>("LogDate");
            }
            set
            {
                if (Row.Table.Columns.Contains("LogDate")) Row["LogDate"] = value;
            }
        }

        [DataMember(Name = "stop_time")]
        public DateTime? StopTime
        {
            get
            {
                return Row.Get<DateTime?>("StopTime");
            }
            set
            {
                if (Row.Table.Columns.Contains("StopTime")) Row["StopTime"] = value;
            }
        }

        [DataMember(Name = "start_time")]
        public DateTime? StartTime
        {
            get
            {
                return Row.Get<DateTime?>("StartTime");
            }
            set
            {
                if (Row.Table.Columns.Contains("StartTime")) Row["StartTime"] = value;
            }
        }
        [DataMember(Name = "hours")]
        public decimal? Hours
        {
            get { return Convert.ToDecimal(Row["LogHours"]); }
            set { Row["LogHours"] = value; }
        }

        [DataMember(Name = "fb_id")]
        public int FBId
        {
            get { return Convert.ToInt32(Row["FBID"]); }
            set { Row["FBID"] = value; }
        }

        [DataMember(Name = "is_project_log")]
        public bool IsProjectLog
        {
            get { return Convert.ToBoolean(Row["IsProjectLog"]); }
            set { Row["IsProjectLog"] = value; }
        }

        [DataMember(Name = "ticket_id")]
        public int TicketID
        {
            get { return Convert.ToInt32(Row["TicketID"]); }
            set { Row["TicketID"] = value; }
        }

        [DataMember(Name = "task_type_id")]
        public int TaskTypeId
        {
            get { return Convert.ToInt32(Row["TaskTypeId"]); }
            set { Row["TaskTypeId"] = value; }
        }

        [DataMember(Name = "task_type")]
        public string TaskTypeName
        {
            get { return Row["TaskTypeName"].ToString(); }
            set { Row["TaskTypeName"] = value; }
        } 
        
        [DataMember(Name = "project_id")]
        public int ProjectID
        {
            get { return Convert.ToInt32(Row["ProjectID"]); }
            set { Row["ProjectID"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int AccountID
        {
            get { return Convert.ToInt32(Row["AccountID"]); }
            set { Row["AccountID"] = value; }
        }

        [DataMember(Name = "ticket_number")]
        public int TicketNumber
        {
            get { return Convert.ToInt32(Row["TicketNumber"]); }
            set { Row["TicketNumber"] = value; }
        }

        [DataMember(Name = "account_name")]
        public string AccountName
        {
            get { return Row["AccountName"].ToString(); }
            set { Row["AccountName"] = value; }
        }

        [DataMember(Name = "ticket_subject")]
        public string TicketSubject
        {
            get { return Row["TicketSubject"].ToString(); }
            set { Row["TicketSubject"] = value; }
        }

        [DataMember(Name = "invoice_id")]
        public int InvoiceId
        {
            get { return Row.Get<int>("InvoiceId"); }
            set { Row["InvoiceId"] = value; }
        }

        [DataMember(Name = "billable")]
        public bool Billable
        {
            get { return Row.Get<bool>("Billable"); }
            set { Row["Billable"] = value; }
        }

        [DataMember(Name = "invoice_pseudo_id")]
        public string InvoicePseudoId
        {
            get { return Row.Get<string>("InvoicePseudoId"); }
            set { Row["InvoicePseudoId"] = value; }
        }

        [DataMember(Name = "qb_id")]
        public int QBID
        {
            get { return Row.Get<int>("QBID"); }
            set { Row["QBID"] = value; }
        }

        [DataMember(Name = "payment_id")]
        public int PaymentId
        {
            get { return Row.Get<int>("BillId"); }
            set { Row["BillId"] = value; }
        }

        [DataMember(Name = "prepaid_pack_name")]
        public string ContractName
        {
            get 
            {
                if (Row.Table.Columns.Contains("ContractName"))
                {
                    return Row["ContractName"].ToString(); 
                }
                return ""; 
            }
            set { Row["ContractName"] = value; }
        }

        [DataMember(Name = "user_profile_image")]
        public string UserProfileImage
        { get; set; }

        [DataMember(Name = "hidden_from_invoice")]
        public bool HiddenFromInvoice
        {
            get
            {
                if (Row.Table.Columns.Contains("HiddenFromInvoice"))
                {
                    return Row.Get<bool>("HiddenFromInvoice");
                }
                return false;
            }
            set { Row["HiddenFromInvoice"] = value; }
        }

        [DataMember(Name = "time_offset")]
        public decimal Time_Offset
        {
            get
            {
                return Row.Get<decimal>("UTCOffset");
            }
            set
            {
                Row["UTCOffset"] = value;
            }
        }

        [DataMember(Name = "created_user_name")]
        public string CreatedByUser
        {
            get { return Row["CreatedByUser"].ToString(); }
            set { Row["CreatedByUser"] = value; }
        }

        [DataMember(Name = "created_user_id")]
        public int CreatedBy
        {
            get { return Row.Get<int>("CreatedBy"); }
            set { Row["CreatedBy"] = value; }
        }

        [DataMember(Name = "created_time")]
        public DateTime? CreatedTime
        {
            get { return Row.Get<DateTime?>("CreatedTime"); }
            set { if (value.HasValue) Row["CreatedTime"] = value.Value; else Row["CreatedTime"] = DBNull.Value; }
        }

        [DataMember(Name = "updated_user_name")]
        public string UpdatedByUser
        {
            get { return Row["UpdatedByUser"].ToString(); }
            set { Row["UpdatedByUser"] = value; }
        }

        [DataMember(Name = "updated_user_id")]
        public int UpdatedBy
        {
            get { return Row.Get<int>("UpdatedBy"); }
            set { Row["UpdatedBy"] = value; }
        }

        [DataMember(Name = "updated_time")]
        public DateTime? UpdatedTime
        {
            get { return Row.Get<DateTime?>("UpdatedTime"); }
            set { if (value.HasValue) Row["UpdatedTime"] = value.Value; else Row["UpdatedTime"] = DBNull.Value; }
        }
    }
}
