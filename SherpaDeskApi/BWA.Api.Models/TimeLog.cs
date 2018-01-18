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
    [DataContract(Name = "ticket_time_log")]
    public class Ticket_Time_Log : ModelItemBase
    {
        public Ticket_Time_Log(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "is_deleted")]
        public bool IsDeleted
        {
            get { return Row.Get<bool>("Deleted"); }
            set { Row["Deleted"] = value; }
        }

        [DataMember(Name = "ticket_time_id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["Id"]); }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "ticket_key")]
        public string Ticket_Key
        {
            get
            {
                return Row.GetString("TktPseudoId");
            }
            set { Row["TktPseudoId"] = value; }
        }

        public int TicketID
        {
            get { return Row.Get<int>("TicketID"); }
            set { Row["TicketID"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row["vchFullName"].ToString(); }
            set { Row["vchFullName"] = value; }
        }

        [DataMember(Name = "task_type_name")]
        public string Task_Type_Name
        {
            get { return Row["TaskTypeName"].ToString(); }
            set { Row["TaskTypeName"] = value; }
        }

        [DataMember(Name = "ticket_subject")]
        public string Ticket_Subject
        {
            get
            {
                return Row.GetString("TicketSubject");
            }
            set { Row["TicketSubject"] = value; }
        }

        [DataMember(Name = "note")]
        public string Note
        {
            get { return Utils.FormatTicketNote(Row["Note"].ToString()); }
            set { Row["Note"] = value; }
        }

        [DataMember(Name = "date")]
        public DateTime? Date
        {
            get
            {
                return Row.Get<DateTime?>("Date");
            }
            set
            {
                Row["Date"] = value;
            }
        }

        [DataMember(Name = "start_time")]
        public DateTime? Start_Time
        {
            get
            {
                return Row.Get<DateTime?>("StartTime");
            }
            set
            {
                Row["StartTime"] = value;
            }
        }

        [DataMember(Name = "stop_time")]
        public DateTime? Stop_Time
        {
            get
            {
                return Row.Get<DateTime?>("StopTime");
            }
            set
            {
                Row["StopTime"] = value;
            }
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

        [DataMember(Name = "hours_remaining")]
        public decimal? Hours_Remaining
        {
            get { return Row.Get<decimal?>("HoursRemaining"); }
            set { Row["HoursRemaining"] = value; }
        }

        [DataMember(Name = "hours")]
        public decimal? Hours
        {
            get { return Row.Get<decimal?>("Hours"); }
            set { Row["Hours"] = value; }
        }

        [DataMember(Name = "fb_time_entry_id")]
        public int FB_Time_Entry_ID
        {
            get
            {
                return Row.Get<int>("FBTimeEntryID");
            }
            set { Row["FBTimeEntryID"] = value; }
        }

        [DataMember(Name = "is_billable")]
        public bool Is_Billable
        {
            get
            {
                return Row.Get<bool>("Billable", false);
            }
            set { Row["Billable"] = value; }
        }

        [DataMember(Name = "task_type_id")]
        public int Task_Type_ID
        {
            get
            {
                return Row.Get<int>("TaskTypeId");
            }
            set { Row["TaskTypeId"] = value; }
        }

        [DataMember(Name = "fb_task_type_id")]
        public int FB_Task_Type_ID
        {
            get
            {
                return Row.Get<int>("FBTaskTypeID");
            }
            set { Row["FBTaskTypeID"] = value; }
        }

        [DataMember(Name = "user_id")]
        public int User_ID
        {
            get
            {
                return Row.Get<int>("UserId");
            }
            set { Row["UserId"] = value; }
        }

        [DataMember(Name = "fb_staff_id")]
        public int FB_Staff_ID
        {
            get
            {
                return Row.Get<int>("FBStaffID");
            }
            set { Row["FBStaffID"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int Account_ID
        {
            get
            {
                return Row.Get<int>("AccountID");
            }
            set { Row["AccountID"] = value; }
        }

        [DataMember(Name = "account_name")]
        public string Account_Name
        {
            get
            {
                return HttpUtility.HtmlDecode(Row.GetString("AccountName"));
            }
            set { Row["AccountName"] = value; }
        }

        [DataMember(Name = "fb_client_id")]
        public int FB_Client_Id
        {
            get
            {
                return Row.Get<int>("FBClientId");
            }
            set { Row["FBClientId"] = value; }
        }

        [DataMember(Name = "complete")]
        public int Complete
        {
            get
            {
                return Convert.ToInt32(Row.Get<byte>("Complete"));
            }
            set { Row["Complete"] = value; }
        }

        [DataMember(Name = "project_id")]
        public int Project_ID
        {
            get
            {
                return Row.Get<int>("ProjectID");
            }
            set { Row["ProjectID"] = value; }
        }

        [DataMember(Name = "project_name")]
        public string Project_Name
        {
            get
            {
                return HttpUtility.HtmlDecode(Row.GetString("ProjectName"));
            }
            set { Row["ProjectName"] = value; }
        }

        [DataMember(Name = "fb_project_id")]
        public int FB_Project_ID
        {
            get
            {
                return Row.Get<int>("FBProjectID");
            }
            set { Row["FBProjectID"] = value; }
        }

        [DataMember(Name = "fb_default_project_id")]
        public int FB_Default_Project_Id
        {
            get
            {
                return Row.Get<int>("FBDefaultProjectId");
            }
            set { Row["FBDefaultProjectId"] = value; }
        }

        [DataMember(Name = "ticket_number")]
        public string Ticket_Number
        {
            get
            {
                return Row.GetString("TicketNumber");
            }
            set { Row["TicketNumber"] = value; }
        }

        [DataMember(Name = "invoice_id")]
        public int Invoice_ID
        {
            get
            {
                return Row.Get<int>("InvoiceId");
            }
            set { Row["InvoiceId"] = value; }
        }

        [DataMember(Name = "qb_time_activity_id")]
        public int QBTimeActivityID
        {
            get
            {
                return Row.Get<int>("QBTimeActivityID");
            }
            set { Row["QBTimeActivityID"] = value; }
        }

        [DataMember(Name = "qb_service_id")]
        public int QBServiceID
        {
            get
            {
                return Row.Get<int>("QBServiceID");
            }
            set { Row["QBServiceID"] = value; }
        }

        [DataMember(Name = "qb_employee_id")]
        public int QBEmployeeID
        {
            get
            {
                return Row.Get<int>("QBEmployeeID");
            }
            set { Row["QBEmployeeID"] = value; }
        }

        [DataMember(Name = "qb_vendor_id")]
        public int QBVendorID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBVendorID") && !Row.IsNull("QBVendorID"))
                {
                    return Convert.ToInt32(Row["QBVendorID"]);
                }
                return 0;
            }
            set { Row["QBVendorID"] = value; }
        }

        [DataMember(Name = "qb_customer_id")]
        public int QBCustomerId
        {
            get
            {
                return Row.Get<int>("QBCustomerId");
            }
            set { Row["QBCustomerId"] = value; }
        }

        [DataMember(Name = "invoice_hourly_rate")]
        public decimal InvoiceHourlyRate
        {
            get
            {
                return Row.Get<decimal>("BillRate");
            }
            set
            {
                Row["BillRate"] = value;
            }
        }

        [DataMember(Name = "qb_sync_token")]
        public int QBSyncToken
        {
            get
            {
                return Row.Get<int>("QBSyncToken");
            }
            set { Row["QBSyncToken"] = value; }
        }

        [DataMember(Name = "payment_id")]
        public int PaymentId
        {
            get
            {
                if (Row.Table.Columns.Contains("BillId") && !Row.IsNull("BillId"))
                {
                    return Convert.ToInt32(Row["BillId"]);
                }
                return 0;
            }
            set { Row["BillId"] = value; }
        }

        [DataMember(Name = "is_invoice_rate_fixed")]
        public bool IsInvoiceFixed
        {
            get
            {
                if (Row.Table.Columns.Contains("IsInvoiceFixed") && !Row.IsNull("IsInvoiceFixed"))
                {
                    return Row.Get<bool>("IsInvoiceFixed", false);
                }
                return false;
            }
            set { Row["IsInvoiceFixed"] = value; }
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

        [DataMember(Name = "prepaid_pack_id")]
        public int ContractID
        {
            get
            {
                if (Row.Table.Columns.Contains("ContractID") && !Row.IsNull("ContractID"))
                {
                    return Convert.ToInt32(Row["ContractID"]);
                }
                return 0;
            }
            set { Row["ContractID"] = value; }
        }
    }
}
