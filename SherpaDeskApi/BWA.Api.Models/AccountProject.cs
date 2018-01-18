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
    [DataContract(Name = "Account_Project")]
    public class Account_Project : ModelItemBase
    {
        public Account_Project(DataRow row) : base(row)  {  }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["ProjectID"]); }
            set { Row["ProjectID"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return HttpUtility.HtmlDecode(Row["Name"].ToString()); }
            set { Row["Name"] = value; }
        }

        [DataMember(Name = "billing_info")]
        public string Billing_Info
        {
            get { return Row["InfoField"].ToString(); }
            set { Row["InfoField"] = value; }
        }

        [DataMember(Name = "billing_method_id")]
        public string Billing_Method_ID
        {
            get { return Row["BillingMethodID"].ToString(); }
            set { Row["BillingMethodID"] = value; }
        }

        [DataMember(Name = "time_and_billing")]
        public string Time_And_Billing
        {
            get
            {
                string overrideRate = Row["OverrideRate"].ToString();
                string BillingMethodID = Row["BillingMethodID"].ToString();
                if (overrideRate == "Billing Disabled") overrideRate = "Tracking Time Only";
                if (overrideRate == "No") overrideRate = "Not Defined";
                return overrideRate.Replace("Hourly ", "");
            }
            set { Row["OverrideRate"] = value; }
        }        
        
        [DataMember(Name = "open_tickets")]
        public int Open_Tickets
        {
            get { return Convert.ToInt32(Row["OpenTickets"]); }
            set { Row["OpenTickets"] = value; }
        }

        [DataMember(Name = "closed_tickets")]
        public int Closed_Tickets
        {
            get { return Convert.ToInt32(Row["ClosedTickets"]); }
            set { Row["ClosedTickets"] = value; }
        }        
        
        [DataMember(Name = "logged_hours")]
        public decimal? Logged_Hours
        {
            get { if (Row.IsNull("TotalHours")) return null; return Convert.ToDecimal(Row["TotalHours"]); }
            set { Row["TotalHours"] = value; }
        }

        [DataMember(Name = "remaining_hours")]
        public decimal? Remaining_Hours
        {
            get { if (Row.IsNull("RemainingHours")) return null; return Convert.ToDecimal(Row["RemainingHours"]); }
            set { Row["RemainingHours"] = value; }
        }

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

        [DataMember(Name = "internal_client_manager")]
        public string Internal_Client_Manager
        {
            get { return Row["InternalPMFullName"].ToString() + (Row.IsNull("ClientPMLFullName") ? "" : " / " + Row["ClientPMLFullName"].ToString()); }
            set { Row["InternalPMFullName"] = value; }
        }
    }
}
