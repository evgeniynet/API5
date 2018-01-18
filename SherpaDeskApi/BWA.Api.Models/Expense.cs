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
    [DataContract(Name = "Expense")]
    public class Expense : ModelItemBase
    {
        public Expense(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "expense_id")]
        public string ExpenseID
        {
            get
            {
                return Row["Id"].ToString();
            }
            set { Row["Id"] = value; }
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

        [DataMember(Name = "amount")]
        public decimal Amount
        {
            get { return Convert.ToDecimal(Row["Amount"]); }
            set { Row["Amount"] = value; }
        }

        [DataMember(Name = "fb_expense_id")]
        public int FBId
        {
            get { return Convert.ToInt32(Row["FBExpenseID"]); }
            set { Row["FBExpenseID"] = value; }
        }

        [DataMember(Name = "ticket_id")]
        public int TicketID
        {
            get { return Convert.ToInt32(Row["TicketId"]); }
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

        [DataMember(Name = "category_id")]
        public string CategoryID
        {
            get
            {
                if (!Row.IsNull("ExpenseCategoryId")) return Row["ExpenseCategoryId"].ToString();
                else return string.Empty;
            }
            set { Row["ExpenseCategoryId"] = value; }
        }

        [DataMember(Name = "category")]
        public string ExpenseCategoryName
        {
            get { return Row["ExpenseCategoryName"].ToString(); }
            set { Row["ExpenseCategoryName"] = value; }
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
            get { return (Row.Table.Columns.Contains("InvoiceId")) ? Convert.ToInt32(Row["InvoiceId"]) : 0; }
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

        [DataMember(Name = "vendor")]
        public string Vendor
        {
            get { return Row["Vendor"].ToString(); }
            set { Row["Vendor"] = value; }
        }

        [DataMember(Name = "fb_staff_id")]
        public int FBStaffID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBStaffID"))
                {
                    return Convert.ToInt32(Row["FBStaffID"]);
                }
                return 0;
            }
            set { Row["FBStaffID"] = value; }
        }

        [DataMember(Name = "fb_category_id")]
        public int FBCategoryID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBCategoryID"))
                {
                    return Convert.ToInt32(Row["FBCategoryID"]);
                }
                return 0;
            }
            set { Row["FBCategoryID"] = value; }
        }

        [DataMember(Name = "fb_client_id")]
        public int FBClientId
        {
            get
            {
                if (Row.Table.Columns.Contains("FBClientId"))
                {
                    return Convert.ToInt32(Row["FBClientId"]);
                }
                return 0;
            }
            set { Row["FBClientId"] = value; }
        }

        [DataMember(Name = "fb_project_id")]
        public int FBProjectID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBProjectID"))
                {
                    return Convert.ToInt32(Row["FBProjectID"]);
                }
                return 0;
            }
            set { Row["FBProjectID"] = value; }
        }

        [DataMember(Name = "markup")]
        public int Markup
        {
            get 
            { 
                if (Row.Table.Columns.Contains("Markup") && !Row.IsNull("Markup"))
                {
                    return Convert.ToInt32(Row["Markup"]);
                }
                return 0; 
            }
            set { 
                Row["Markup"] = value; 
            }
        }

        [DataMember(Name = "note_internal")]
        public string NoteInternal
        {
            get { return HttpUtility.HtmlDecode(Row["NoteInternal"].ToString()); }
            set { Row["NoteInternal"] = value; }
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
            get { return Row.Get<bool>("IsTechnicianPayment");}
            set { Row["IsTechnicianPayment"] = value; }
        }

        [DataMember(Name = "payment_id")]
        public int PaymentId
        {
            get { return Row.Get<int>("BillId"); }
            set { Row["BillId"] = value; }
        }
    }
}
