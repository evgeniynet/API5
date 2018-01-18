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
    [DataContract(Name = "project_time_log")]
    public class ProjectTimeLog : ModelItemBase
    {
        public ProjectTimeLog(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "is_deleted")]
        public bool IsDeleted
        {
            get { return Row.Get<bool>("Deleted"); }
            set { Row["Deleted"] = value; }
        }

        [DataMember(Name = "project_time_id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["projecttimeid"]); }
            set { Row["projecttimeid"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row["user_fullname"].ToString(); }
            set { Row["user_fullname"] = value; }
        }

        [DataMember(Name = "task_type_name")]
        public string TaskTypeName
        {
            get { return Row["TaskTypeName"].ToString(); }
            set { Row["TaskTypeName"] = value; }
        }

        [DataMember(Name = "note")]
        public string Note
        {
            get { return HttpUtility.HtmlDecode(Row["Note"].ToString()); }
            set { Row["Note"] = value; }
        }

        [DataMember(Name = "date")]
        public DateTime? Date
        {
            get
            {
                if (!Row.IsNull("Date")) return (DateTime)Row["Date"];
                else return null;
            }
            set
            {
                Row["Date"] = value;
            }
        }

        [DataMember(Name = "start_time")]
        public DateTime? StartTime
        {
            get
            {
                if (!Row.IsNull("StartTimeUTC")) return (DateTime)Row["StartTimeUTC"];
                else return null;
            }
            set
            {
                Row["StartTimeUTC"] = value;
            }
        }

        [DataMember(Name = "stop_time")]
        public DateTime? StopTime
        {
            get
            {
                if (!Row.IsNull("StopTimeUTC")) return (DateTime)Row["StopTimeUTC"];
                else return null;
            }
            set
            {
                Row["StopTimeUTC"] = value;
            }
        }

        [DataMember(Name = "time_offset")]
        public decimal TimeOffset
        {
            get
            {
                if (Row.IsNull("LocalTimeZoneOffset"))
                {
                    return 0;
                }
                return Convert.ToDecimal(Row["LocalTimeZoneOffset"]);
            }
            set
            {
                Row["LocalTimeZoneOffset"] = value;
            }
        }

        [DataMember(Name = "hours")]
        public decimal? Hours
        {
            get { if (Row.IsNull("Hours")) return null; return Convert.ToDecimal(Row["Hours"]); }
            set { Row["Hours"] = value; }
        }

        [DataMember(Name = "fb_time_entry_id")]
        public int FBTimeEntryID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBTimeEntryID") && !Row.IsNull("FBTimeEntryID"))
                {
                    return Convert.ToInt32(Row["FBTimeEntryID"]);
                }
                return 0;
            }
            set { Row["FBTimeEntryID"] = value; }
        }

        [DataMember(Name = "task_type_id")]
        public int TaskTypeID
        {
            get
            {
                if (Row.Table.Columns.Contains("TaskTypeId") && !Row.IsNull("TaskTypeId"))
                {
                    return Convert.ToInt32(Row["TaskTypeId"]);
                }
                return 0;
            }
            set { Row["TaskTypeId"] = value; }
        }

        [DataMember(Name = "fb_task_type_id")]
        public int FBTaskTypeID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBTaskTypeID") && !Row.IsNull("FBTaskTypeID"))
                {
                    return Convert.ToInt32(Row["FBTaskTypeID"]);
                }
                return 0;
            }
            set { Row["FBTaskTypeID"] = value; }
        }

        [DataMember(Name = "user_id")]
        public int UserID
        {
            get
            {
                if (Row.Table.Columns.Contains("UserId") && !Row.IsNull("UserId"))
                {
                    return Convert.ToInt32(Row["UserId"]);
                }
                return 0;
            }
            set { Row["UserId"] = value; }
        }

        [DataMember(Name = "fb_staff_id")]
        public int FBStaffID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBStaffID") && !Row.IsNull("FBStaffID"))
                {
                    return Convert.ToInt32(Row["FBStaffID"]);
                }
                return 0;
            }
            set { Row["FBStaffID"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int AccountID
        {
            get
            {
                if (Row.Table.Columns.Contains("AccountID") && !Row.IsNull("AccountID"))
                {
                    return Convert.ToInt32(Row["AccountID"]);
                }
                return 0;
            }
            set { Row["AccountID"] = value; }
        }

        [DataMember(Name = "account_name")]
        public string AccountName
        {
            get
            {
                if (Row.Table.Columns.Contains("AccountName")) return HttpUtility.HtmlDecode(Row["AccountName"].ToString());
                return "";
            }
            set { Row["AccountName"] = value; }
        }

        [DataMember(Name = "fb_client_id")]
        public int FBClientId
        {
            get
            {
                if (Row.Table.Columns.Contains("FBClientId") && !Row.IsNull("FBClientId"))
                {
                    return Convert.ToInt32(Row["FBClientId"]);
                }
                return 0;
            }
            set { Row["FBClientId"] = value; }
        }

        [DataMember(Name = "project_id")]
        public int ProjectID
        {
            get
            {
                if (Row.Table.Columns.Contains("ProjectID") && !Row.IsNull("ProjectID"))
                {
                    return Convert.ToInt32(Row["ProjectID"]);
                }
                return 0;
            }
            set { Row["ProjectID"] = value; }
        }

        [DataMember(Name = "project_name")]
        public string ProjectName
        {
            get
            {
                if (Row.Table.Columns.Contains("ProjectName")) return HttpUtility.HtmlDecode(Row["ProjectName"].ToString());
                return "";
            }
            set { Row["ProjectName"] = value; }
        }

        [DataMember(Name = "fb_project_id")]
        public int FBProjectID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBProjectID") && !Row.IsNull("FBProjectID"))
                {
                    return Convert.ToInt32(Row["FBProjectID"]);
                }
                return 0;
            }
            set { Row["FBProjectID"] = value; }
        }

        [DataMember(Name = "fb_default_project_id")]
        public int FBDefaultProjectId
        {
            get
            {
                if (Row.Table.Columns.Contains("FBDefaultProjectId") && !Row.IsNull("FBDefaultProjectId"))
                {
                    return Convert.ToInt32(Row["FBDefaultProjectId"]);
                }
                return 0;
            }
            set { Row["FBDefaultProjectId"] = value; }
        }

        [DataMember(Name = "invoice_id")]
        public int InvoiceID
        {
            get
            {
                if (Row.Table.Columns.Contains("InvoiceId") && !Row.IsNull("InvoiceId"))
                {
                    return Convert.ToInt32(Row["InvoiceId"]);
                }
                return 0;
            }
            set { Row["InvoiceId"] = value; }
        }

        [DataMember(Name = "is_billable")]
        public bool Billable
        {
            get
            {
                if (Row.Table.Columns.Contains("Billable") && !Row.IsNull("Billable"))
                {
                    return Convert.ToBoolean(Row["Billable"]);
                }
                return false;
            }
            set { Row["Billable"] = value; }
        }

        [DataMember(Name = "qb_time_activity_id")]
        public int QBTimeActivityID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBTimeActivityID") && !Row.IsNull("QBTimeActivityID"))
                {
                    return Convert.ToInt32(Row["QBTimeActivityID"]);
                }
                return 0;
            }
            set { Row["QBTimeActivityID"] = value; }
        }

        [DataMember(Name = "qb_service_id")]
        public int QBServiceID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBServiceID") && !Row.IsNull("QBServiceID"))
                {
                    return Convert.ToInt32(Row["QBServiceID"]);
                }
                return 0;
            }
            set { Row["QBServiceID"] = value; }
        }

        [DataMember(Name = "qb_employee_id")]
        public int QBEmployeeID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBEmployeeID") && !Row.IsNull("QBEmployeeID"))
                {
                    return Convert.ToInt32(Row["QBEmployeeID"]);
                }
                return 0;
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
                if (Row.Table.Columns.Contains("QBCustomerId") && !Row.IsNull("QBCustomerId"))
                {
                    return Convert.ToInt32(Row["QBCustomerId"]);
                }
                return 0;
            }
            set { Row["QBCustomerId"] = value; }
        }

        [DataMember(Name = "invoice_hourly_rate")]
        public decimal InvoiceHourlyRate
        {
            get
            {
                if (Row.Table.Columns.Contains("BillRate") && !Row.IsNull("BillRate"))
                {
                    return Convert.ToDecimal(Row["BillRate"]);
                }
                return 0;
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
                if (Row.Table.Columns.Contains("QBSyncToken") && !Row.IsNull("QBSyncToken"))
                {
                    return Convert.ToInt32(Row["QBSyncToken"]);
                }
                return 0;
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
