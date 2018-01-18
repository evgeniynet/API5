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
    [DataContract(Name = "Project")]
    public class Project : ModelItemBase
    {
        public Project(DataRow row) : base(row)  {  }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["ProjectID"]); }
            set { Row["ProjectID"] = value; }
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
        
        [DataMember(Name = "account_name")]
        public string AccountName
        {
			get { return HttpUtility.HtmlDecode(Row.GetString ("AccountName")); }
            set { Row["AccountName"] = value; }
        }

        [DataMember(Name = "open_tickets")]
        public int OpenTickets
        {
			get { return Row.Get<int>("OpenTickets"); }
            set { Row["OpenTickets"] = value; }
        }

        [DataMember(Name = "closed_tickets")]
        public int ClosedTickets
        {
			get { return Row.Get<int>("ClosedTickets"); }
            set { Row["ClosedTickets"] = value; }
        }

        [DataMember(Name = "priority")]
        public byte? Priority
        {
            get
            {
				return Row.Get<byte?> ("tintPriority");
            }
            set { Row["tintPriority"] = value; }
        }

        [DataMember(Name = "priority_name")]
        public string PriorityName
        {
			get { return Row.GetString ("PriorityName"); }
            set { Row["PriorityName"] = value; }
        }
        
        
        [DataMember(Name = "logged_hours")]
        public decimal? TotalHours
        {
			get { return Row.Get<decimal?> ("TotalHours"); }
            set { Row["TotalHours"] = value; }
        }

        [DataMember(Name = "remaining_hours")]
        public decimal? RemainingHours
        {
			get { return Row.Get<decimal?>("RemainingHours"); }
            set { Row["RemainingHours"] = value; }
        }

        [DataMember(Name = "complete")]
        public int Complete
        {
            get
            {
				decimal? remainingHours = Row.Get<decimal?> ("RemainingHours");
				decimal? loggedHours = Row.Get<decimal?> ("TotalHours");

				if (remainingHours.HasValue && loggedHours.HasValue)
                {
                    
					decimal totalHours = remainingHours.Value + loggedHours.Value;
                    if (totalHours > 0)
                    {
						return ((int)Math.Round(loggedHours.Value / totalHours * 100));
                    }
                }
                return 0;
            }
        }

        [DataMember(Name = "client_manager")]
        public string ClientManager
        {
			get { return Row.GetString ("PMFullName"); }
            set { Row["PMFullName"] = value; }
        }
    }
}
