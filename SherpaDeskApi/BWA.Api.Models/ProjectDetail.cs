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
    [DataContract(Name = "Project_Detail")]
    public class ProjectDetail : ModelItemBase
    {
        protected DataRow m_Row;
        protected int _id;

        public ProjectDetail(DataRow row)
            : base(row)
        {
            m_Row = row;
        }

        public ProjectDetail(DataRow row, int id)
            : base(row)
        {

            m_Row = row;
            _id = id;
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return _id; }
        }

        [DataMember(Name = "account_id")]
        public int AccountId
        {
            get { return Convert.ToInt32(Row["AccountID"]); }
            set { Row["AccountID"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return HttpUtility.HtmlDecode(Row["Name"].ToString()); }
            set { Row["Name"] = value; }
        }

        [DataMember(Name = "full_name")]
        public string ProjectFullName
        {
            get { return HttpUtility.HtmlDecode(Row["ProjectFullName"].ToString()); }
            set { Row["ProjectFullName"] = value; }
        }
        
        [DataMember(Name = "account_name")]
        public string AccountName
        {
            get { return HttpUtility.HtmlDecode(Row["AccountName"].ToString()); }
            set { Row["AccountName"] = value; }
        }

         [DataMember(Name = "internal_project_manager")]
        public string InternalPM
        {
            get { return Row["InternalPM"].ToString(); }
            set { Row["InternalPM"] = value; }
        }

         [DataMember(Name = "client_project_manager")]
         public string ClientPM
        {
            get { return Row["ClientPM"].ToString(); }
            set { Row["ClientPM"] = value; }
        }

       [DataMember(Name = "priority")]
        public int? Priority
        {
            get
            {
                if (!Row.IsNull("tintPriority"))
                    return Convert.ToInt32(Row["tintPriority"]);
                return null;
            }
            set { Row["tintPriority"] = value; }
        }

        [DataMember(Name = "priority_name")]
        public string PriorityName
        {
            get { return Row["PriorityName"].ToString(); }
            set { Row["PriorityName"] = value; }
        }

        [DataMember(Name = "priority_description")]
        public string PriorityDescription
        {
            get { return Row["PriorityDescription"].ToString(); }
            set { Row["PriorityDescription"] = value; }
        }

        [DataMember(Name = "support_group_name")]
        public string SupportGroupName
        {
            get { return Row["SupportGroupName"].ToString(); }
            set { Row["SupportGroupName"] = value; }
        } 
        
        /*
        [DataMember(Name = "complete")]
        public int Complete
        {
            get
            {
                if (!Row.IsNull("TotalHours") && !Row.IsNull("RemainingHours"))
                {
                    decimal remainingHours = Convert.ToDecimal(Row["RemainingHours"]);
                    decimal loggedHours = Convert.ToDecimal(Row["TotalHours"]);
                    decimal totalHours = remainingHours + loggedHours;
                    if (totalHours > 0)
                    {
                        return ((int)Math.Round(loggedHours / totalHours * 100));
                    }
                }
                return 0;
            }
        }
        */

        [DataMember(Name = "estimated_hours")]
        public decimal? EstimatedHours
        {
            get { if (Row.IsNull("EstimatedHours")) return null; return Convert.ToDecimal(Row["EstimatedHours"]); }
            set { Row["EstimatedHours"] = value; }
        }

        [DataMember(Name = "project_hours")]
        public decimal? ProjectHours
        {
            get { if (Row.IsNull("ProjectHours")) return null; return Convert.ToDecimal(Row["ProjectHours"]); }
            set { Row["ProjectHours"] = value; }
        }

        [DataMember(Name = "ticket_hours")]
        public decimal? TicketHours
        {
            get { if (Row.IsNull("TicketHours")) return null; return Convert.ToDecimal(Row["TicketHours"]); }
            set { Row["TicketHours"] = value; }
        }

        [DataMember(Name = "ticket_remaining_hours")]
        public decimal? TicketRemainingHours
        {
            get { if (Row.IsNull("TicketRemainingHours")) return null; return Convert.ToDecimal(Row["TicketRemainingHours"]); }
            set { Row["TicketRemainingHours"] = value; }
        }

        [DataMember(Name = "todo_project_estimated")]
        public decimal? ToDoProjectEstimated
        {
            get { if (Row.IsNull("ToDoProjectEstimated")) return null; return Convert.ToDecimal(Row["ToDoProjectEstimated"]); }
            set { Row["ToDoProjectEstimated"] = value; }
        }

        [DataMember(Name = "estimated_cost")]
        public decimal? EstimatedCost
        {
            get { if (Row.IsNull("EstimatedCost")) return null; return Convert.ToDecimal(Row["EstimatedCost"]); }
            set { Row["EstimatedCost"] = value; }
        }

        [DataMember(Name = "ticket_invoice_amount")]
        public decimal? TicketInvoiceAmount
        {
            get { if (Row.IsNull("TicketInvoiceAmount")) return null; return Convert.ToDecimal(Row["TicketInvoiceAmount"]); }
            set { Row["TicketInvoiceAmount"] = value; }
        }

        [DataMember(Name = "project_invoice_amount")]
        public decimal? ProjectInvoiceAmount
        {
            get { if (Row.IsNull("ProjectInvoiceAmount")) return null; return Convert.ToDecimal(Row["ProjectInvoiceAmount"]); }
            set { Row["ProjectInvoiceAmount"] = value; }
        }

        [DataMember(Name = "total_invoice_amount")]
        public decimal? TotalInvoiceAmount
        {
            get { if (Row.IsNull("TotalInvoiceAmount")) return null; return Convert.ToDecimal(Row["TotalInvoiceAmount"]); }
            set { Row["TotalInvoiceAmount"] = value; }
        }

        [DataMember(Name = "ticket_noninvoiced_amount")]
        public decimal? TicketNonInvoicedAmount
        {
            get { if (Row.IsNull("TicketNonInvoicedAmount")) return null; return Convert.ToDecimal(Row["TicketNonInvoicedAmount"]); }
            set { Row["TicketNonInvoicedAmount"] = value; }
        }

        [DataMember(Name = "project_noninvoiced_amount")]
        public decimal? ProjectNonInvoicedAmount
        {
            get { if (Row.IsNull("ProjectNonInvoicedAmount")) return null; return Convert.ToDecimal(Row["ProjectNonInvoicedAmount"]); }
            set { Row["ProjectNonInvoicedAmount"] = value; }
        }

        [DataMember(Name = "estimated_invoiced_amount")]
        public decimal? EstimatedInvoicedAmount
        {
            get { if (Row.IsNull("EstimatedInvoicedAmount")) return null; return Convert.ToDecimal(Row["EstimatedInvoicedAmount"]); }
            set { Row["EstimatedInvoicedAmount"] = value; }
        }

        [DataMember(Name = "ticket_bill_amount")]
        public decimal? TicketBillAmount
        {
            get { if (Row.IsNull("TicketBillAmount")) return null; return Convert.ToDecimal(Row["TicketBillAmount"]); }
            set { Row["TicketBillAmount"] = value; }
        }

        [DataMember(Name = "project_bill_amount")]
        public decimal? ProjectBillAmount
        {
            get { if (Row.IsNull("ProjectBillAmount")) return null; return Convert.ToDecimal(Row["ProjectBillAmount"]); }
            set { Row["ProjectBillAmount"] = value; }
        }

        [DataMember(Name = "ticket_unbilled_amount")]
        public decimal? TicketUnBilledAmount
        {
            get { if (Row.IsNull("TicketUnBilledAmount")) return null; return Convert.ToDecimal(Row["TicketUnBilledAmount"]); }
            set { Row["TicketUnBilledAmount"] = value; }
        }

        [DataMember(Name = "project_unbilled_amount")]
        public decimal? ProjectUnBilledAmount
        {
            get { if (Row.IsNull("ProjectUnBilledAmount")) return null; return Convert.ToDecimal(Row["ProjectUnBilledAmount"]); }
            set { Row["ProjectUnBilledAmount"] = value; }
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

        [DataMember(Name = "fb_client_id")]
        public int FBClientID
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
    }
}
