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
    [DataContract(Name = "Travel")]
    public class Travel : ModelItemBase
    {
        public Travel(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }
        
        [DataMember(Name = "travel_id")]
        public int TravelId
        {
            get
            {
                if (Row.Table.Columns.Contains("Id") && !Row.IsNull("Id"))
                {
                    return Convert.ToInt32(Row["Id"]);
                }
                return 0;
            }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "start_location")]
        public string StartLocation
        {
            get { return HttpUtility.HtmlDecode(Row["StartLocation"].ToString()); }
            set { Row["StartLocation"] = value; }
        }

        [DataMember(Name = "end_location")]
        public string EndLocation
        {
            get { return HttpUtility.HtmlDecode(Row["EndLocation"].ToString()); }
            set { Row["EndLocation"] = value; }
        }

        [DataMember(Name = "distance")]
        public int Distance
        {
            get
            {
                if (Row.Table.Columns.Contains("Distance") && !Row.IsNull("Distance"))
                {
                    return Convert.ToInt32(Row["Distance"]);
                }
                return 0;
            }
            set { Row["Distance"] = value; }
        }


        [DataMember(Name = "distance_rate")]
        public decimal DistanceRate
        {
            get
            {
                if (Row.Table.Columns.Contains("DistanceRate") && !Row.IsNull("DistanceRate"))
                {
                    return Convert.ToDecimal(Row["DistanceRate"]);
                }
                return 0;
            }
            set { Row["DistanceRate"] = value; }
        }

        [DataMember(Name = "total")]
        public decimal Total
        {
            get
            {
                if (Row.Table.Columns.Contains("Total") && !Row.IsNull("Total"))
                {
                    return Convert.ToDecimal(Row["Total"]);
                }
                return 0;
            }
            set { Row["Total"] = value; }
        }


        [DataMember(Name = "project_name")]
        public string ProjectName
        {
            get { return HttpUtility.HtmlDecode(Row["ProjectName"].ToString()); }
            set { Row["ProjectName"] = value; }
        }

        [DataMember(Name = "user_id")]
        public string UserID
        {
            get
            {
                return Row["UserId"].ToString();
            }
            set { Row["UserId"] = value; }
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

        [DataMember(Name = "ticket_id")]
        public int TicketID
        {
            get
            {
                if (Row.Table.Columns.Contains("TicketId") && !Row.IsNull("TicketId"))
                {
                    return Convert.ToInt32(Row["TicketId"]);
                }
                return 0;
            }
                    
            set { Row["TicketId"] = value; }
        }

        [DataMember(Name = "ticket_key")]
        public string TicketKey
        {
            get
            {
                if (!Row.IsNull("TktPseudoId"))
                {
                    return Row["TktPseudoId"].ToString();
                }
                return "";
            }
            set { Row["TktPseudoId"] = value; }
        }       

        [DataMember(Name = "project_id")]
        public int ProjectID
        {
            get { return (Row.Table.Columns.Contains("ProjectID") && !Row.IsNull("ProjectID")) ? Convert.ToInt32(Row["ProjectID"]) : 0; }
            set { Row["ProjectID"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int AccountID
        {
            get { return (Row.Table.Columns.Contains("Account_ID") && !Row.IsNull("Account_ID")) ? Convert.ToInt32(Row["Account_ID"]) : 0;}
            set { Row["Account_ID"] = value; }
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
            get { return HttpUtility.HtmlDecode(Row["AccountName"].ToString()); }
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
            get { return (Row.Table.Columns.Contains("InvoiceId") && !Row.IsNull("InvoiceId")) ? Convert.ToInt32(Row["InvoiceId"]) : 0; }
            set { Row["InvoiceId"] = value; }
        }

        [DataMember(Name = "billable")]
        public bool Billable
        {
            get { return Convert.ToBoolean(Row["Billable"]); }
            set { Row["Billable"] = value; }
        }

        [DataMember(Name = "invoice_pseudo_id")]
        public string InvoicePseudoId
        {
            get { return Row["InvoicePseudoId"].ToString(); }
            set { Row["InvoicePseudoId"] = value; }
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

        [DataMember(Name = "is_technician_payment")]
        public bool IsTechnicianPayment
        {
            get { return Convert.ToBoolean(Row["IsTechnicianPayment"]); }
            set { Row["IsTechnicianPayment"] = value; }
        }

        [DataMember(Name = "payment_id")]
        public int PaymentId
        {
            get { return Row.Get<int>("BillId"); }
            set { Row["BillId"] = value; }
        }

        [DataMember(Name = "qb_expense_id")]
        public int QBExpenseID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBExpenseID") && !Row.IsNull("QBExpenseID"))
                {
                    return Convert.ToInt32(Row["QBExpenseID"]);
                }
                return 0;
            }
            set { Row["QBExpenseID"] = value; }
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

        [DataMember(Name = "qb_is_employee")]
        public bool QBIsEmployee
        {
            get
            {
                if (Row.Table.Columns.Contains("QBIsEmployee") && !Row.IsNull("QBIsEmployee"))
                {
                    return Convert.ToBoolean(Row["QBIsEmployee"]);
                }
                return false;
            }
            set { Row["QBIsEmployee"] = value; }
        }

        [DataMember(Name = "qb_account_id")]
        public int QBAccountID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBAccountID") && !Row.IsNull("QBAccountID"))
                {
                    return Convert.ToInt32(Row["QBAccountID"]);
                }
                return 0;
            }
            set { Row["QBAccountID"] = value; }
        }
    }
} 
