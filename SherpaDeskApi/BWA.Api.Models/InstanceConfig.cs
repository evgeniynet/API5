// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using bigWebApps.bigWebDesk;
using SherpaDeskApi.ServiceModel;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecord
   /// </summary>
    [DataContract(Name = "Instance_Config")]
    public class Instance_Config : bigWebApps.bigWebDesk.Config
    {
        private int m_TimeZoneOffset;
        private int m_BusinessDayLength;
        private string m_TimeZoneId;
        private Guid m_Org;
        private int m_Dep;

        public Instance_Config(ApiUser usr) : base(usr.OrganizationId, usr.InstanceId, true)
        {
            User=new UserConfig(usr);
            m_Org = usr.OrganizationId;
            m_Dep = usr.DepartmentId;
			if (base.CustomNames)
				Names = new Names(usr.OrganizationId, usr.DepartmentId);
			else
				Names = new Names();
			Logo = string.IsNullOrEmpty (usr.InstanceLogoImageUrl) ? usr.OrganizationLogoImageUrl : usr.InstanceLogoImageUrl;
            Assets = new AssetsConfig(base.Assets);
            m_TimeZoneOffset = usr.TimeZoneOffset;
            m_TimeZoneId = usr.TimeZoneId;
            m_BusinessDayLength = GetBusinessDayLength(base.BusHourStart, base.BusMinStart, base.BusHourStop, base.BusMinStop);
        }

        private int GetBusinessDayLength(int BusHourStart, int BusMinStart, int BusHourStop, int BusMinStop)
        {
            int _result = 1440;

            DateTime current_time = DateTime.UtcNow.Date;
            int hours = current_time.Hour;
            int mins = current_time.Minute;
            int secs = current_time.Second;
            int msecs = current_time.Millisecond;

            current_time = current_time.AddHours(-hours);
            current_time = current_time.AddMinutes(-mins);
            current_time = current_time.AddSeconds(-secs);
            current_time = current_time.AddMilliseconds(-msecs);

            DateTime start_time, end_time;
            start_time = current_time;
            end_time = current_time;

            hours = BusHourStart;
            mins = BusMinStart;

            start_time = start_time.AddHours(hours);
            start_time = start_time.AddMinutes(mins);

            hours = BusHourStop;
            mins = BusMinStop;

            end_time = end_time.AddHours(hours);
            end_time = end_time.AddMinutes(mins);

            if (end_time > start_time)
            {
                TimeSpan _ts = end_time - start_time;
                _result = (int)_ts.TotalMinutes;
            };

            return _result;
        }

        [DataMember(Name = "org_id")]
        public Guid Org
        {
            get { return m_Org; }
        }

        [DataMember(Name = "dep_id")]
        public int Dep
        {
            get { return m_Dep; }
        }

        [DataMember(Name = "mobile_ver")]
        public string MobileVersion {
			get { 

				string mobile_version = string.Empty;
				if (!string.IsNullOrEmpty (System.Configuration.ConfigurationManager.AppSettings ["mobile_version"]))
					mobile_version = System.Configuration.ConfigurationManager.AppSettings ["mobile_version"].ToString ();
				return mobile_version;
			}
		}

        [DataMember(Name = "is_onhold_status")]
        new public bool OnHoldStatus
        {
            get { return base.OnHoldStatus; }
            set { base.OnHoldStatus = value; }
        }

        [DataMember(Name = "is_time_tracking")]
        new public bool TimeTracking
        {
            get { return base.TimeTracking; }
            set { base.TimeTracking = value; }
        }

        [DataMember(Name = "is_freshbooks")]
        public bool FreshBooks
        {
			get { return base.FBIntegration && base.FBURL.Trim() != ""; }
            set { base.FBIntegration = value; }
        }

        [DataMember(Name = "freshbooks_url")]
        public string FreshBooksUrl
        {
            get { return base.FBURL; }
        }
        
        [DataMember(Name = "is_parts_tracking")]
        new public bool PartsTracking
        {
            get { return base.PartsTracking; }
            set { base.PartsTracking = value; }
        }

        [DataMember(Name = "is_project_tracking")]
        new public bool ProjectTracking
        {
            get { return base.ProjectTracking; }
            set { base.ProjectTracking = value; }
        }

		[DataMember(Name = "is_todos")]
		new public bool IsTodos
		{
			get { return base.EnableToDo; }
			set { base.EnableToDo = value; }
		}

        [DataMember(Name = "is_unassigned_queue")]
        new public bool UnassignedQue
        {
            get { return base.UnassignedQue; }
            set { base.UnassignedQue = value; }
        }

        [DataMember(Name = "is_location_tracking")]
        new public bool LocationTracking
        {
            get { return base.LocationTracking; }
            set { base.LocationTracking = value; }
        }

        [DataMember(Name = "is_waiting_on_response")]
        public bool WaitingOnResponse
        {
            get { return base.AllowWaitingOnResponse; }
            set { base.AllowWaitingOnResponse = value; }
        }

		[DataMember(Name = "is_invoice")]
		public bool IsInvoice
		{
			get { return base.EnableBilling; }
			set { base.EnableBilling = value; }
		}

        [DataMember(Name = "is_payments")]
        public bool IsPayments
        {
            get { return base.EnablePayments; }
            set { base.EnablePayments = value; }
        }

		[DataMember(Name = "is_expenses")]
		public bool IsExpenses
		{
			get { return base.MiscCosts; }
			set { base.MiscCosts = value; }
		}

		[DataMember(Name = "is_class_tracking")]
		new public bool ClassTracking
		{
			get { return base.ClassTracking; }
			set { base.ClassTracking = value; }
		}
        
        [DataMember(Name = "is_travel_costs")]
        public bool IsTravelCosts
        {
			get { return base.TravelCosts; }
			set { base.TravelCosts = value; }
        }

        [DataMember(Name = "is_priorities_general")]
        new public bool PrioritiesGeneral
        {
            get { return base.PrioritiesGeneral; }
            set { base.PrioritiesGeneral = value; }
        }

        [DataMember(Name = "is_confirmation_tracking")]
        new public bool ConfirmationTracking
        {
            get { return base.ConfirmationTracking; }
            set { base.ConfirmationTracking = value; }
        }

        [DataMember(Name = "is_resolution_tracking")]
        new public bool ResolutionTracking
        {
            get { return base.ResolutionTracking; }
            set { base.ResolutionTracking = value; }
        }

        [DataMember(Name = "is_ticket_levels")]
        new public bool TktLevels
        {
            get { return base.TktLevels; }
            set { base.TktLevels = value; }
        }

        [DataMember(Name = "is_ticket_levels_for_users")]
        new public bool TktLevelsForUser
        {
            get { return base.TktLevelsForUser; }
            set { base.TktLevelsForUser = value; }
        }

        [DataMember(Name = "is_tech_choose_levels")]
        new public bool TechChooseLevel
        {
            get { return base.TechChooseLevel; }
            set { base.TechChooseLevel = value; }
        }

        [DataMember(Name = "is_restrict_tech_escalate")]
        new public bool AllowTechEscalateDescalateOnly
        {
            get { return base.AllowTechEscalateDescalateOnly; }
            set { base.AllowTechEscalateDescalateOnly = value; }
        }

        [DataMember(Name = "is_account_manager")]
        new public bool AccountManager
        {
            get { return base.AccountManager; }
            set { base.AccountManager = value; }
        }

        [DataMember(Name = "is_require_ticket_initial_post")]
        new public bool RequireTktInitialPost
        {
            get { return base.RequireTktInitialPost; }
            set { base.RequireTktInitialPost = value; }
        }

        [DataMember(Name = "is_ticket_require_closure_note")]
        new public bool TktRequireClosureNote
        {
            get { return base.TktRequireClosureNote; }
            set { base.TktRequireClosureNote = value; }
        }

        [DataMember(Name = "is_asset_tracking")]
        new public bool AssetTracking
        {
            get { return base.AssetTracking; }
            set { base.AssetTracking = value; }
        }

        [DataMember(Name = "assets")]
        new public AssetsConfig Assets
        { get; protected set; }

        [DataMember(Name = "time_hour_increment")]
        new public decimal HourIncrement
        {
            get { return base.HourIncrement; }
            set { base.HourIncrement = value; }
        }

        [DataMember(Name = "time_minimum_time")]
        new public decimal MinimumLoggableTime
        {
            get { return base.MinimumLoggableTime; }
            set { base.MinimumLoggableTime = value; }
        }

        [DataMember(Name = "timezone_offset")]
        public int TimeZoneOffset
        {
            get { return m_TimeZoneOffset; }
        }

        [DataMember(Name = "timezone_name")]
        public string TimeZoneId
        {
            get { return m_TimeZoneId; }
        }

        [DataMember(Name = "currency")]
        public string currency
        {
            get { return base.Currency; }
        }

        [DataMember(Name = "businessday_length")]
        public int BusinessDayLength
        {
            get { return m_BusinessDayLength; }
        }

        public string FBoAuthConsumerKey
        {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["FBoAuthConsumerKey"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["FBoAuthConsumerKey"].ToString();
                }
                return "";
            }
        }

        public string FBoAuthSecret
        {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["FBoAuthSecret"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["FBoAuthSecret"].ToString();
                }
                return "";
            }
        }

        public string QBoAuthConsumerKey
        {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["QBoAuthConsumerKey"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["QBoAuthConsumerKey"].ToString();
                }
                return "";
            }
        }

        public string QBoAuthSecret
        {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["QBoAuthSecret"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["QBoAuthSecret"].ToString();
                }
                return "";
            }
        }

        public string XeroAuthConsumerKey
        {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["XeroAuthConsumerKey"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["XeroAuthConsumerKey"].ToString();
                }
                return "";
            }
        }

        public string XeroRSAPrivateKey
        {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["XeroRSAPrivateKey"]))
                {
                    return HttpUtility.HtmlDecode(System.Configuration.ConfigurationManager.AppSettings["XeroRSAPrivateKey"].ToString());
                }
                return "";
            }
        }

        [DataMember(Name = "logo")]
		public string Logo {
			get;
			set;
		}

        [DataMember(Name = "user")]
        public UserConfig User { get; protected set; }


		[DataMember(Name = "names")]
		public Names Names { get; protected set; }

		[DataMember(Name = "is_customnames")]
		public bool CustomNames { get { return base.CustomNames; } }
	}

	/*names = {
			"ticket": {s: "Ticket", p: "Tickets", a:"Tkt", ap: "Tkts"},
			"account": {s: "Account", p: "Accounts", a:"Acc", ap: "Accs"},
			"tech": {s: "Technician", p: "Technicians", a:"Tech", ap: "Techs"},
			"location": {s: "Location", p: "Locations", a:"Loc", ap: "Locs"},
			"user": {s: "End User", p: "End Users", a:"User", ap: "Users"}
		};*/

	[Serializable]
	public class Names : bigWebApps.bigWebDesk.Data.DBAccess
	{
		//
		// Properties
		//
		public Name account {
			get;set;
		}

		public Name user {
			get;set;
		}

		public Name location {
			get;set;
		}

		public Name tech {
			get;set;
		}

		public Name ticket {
			get;set;
		}

		//
		// Constructors
		//

		public Names (CustomNames customname)
		{
			ticket = new Name (customname.Ticket);
			account = new Name (customname.Account);
			tech = new Name (customname.Technician);
			location = new Name (customname.Location);
			user = new Name (customname.EndUser);
		}

		public Names ()
		{
			ticket = new Name("Ticket", "Tickets", "Tkt", "Tkts");
			account = new Name ("Account", "Accounts", "Acc", "Accs");
			tech = new Name ("Technician", "Technicians", "Tech", "Techs");
			location = new Name ("Location", "Locations", "Loc", "Locs");
			user = new Name ("End User", "End Users", "User", "Users");
		}

		public Names (Guid OrgID, int DeptID) {
			DataTable _dt = SelectRecords("sp_SelectCustomNames", new SqlParameter[] { new SqlParameter("@CompanyId", DeptID) }, OrgID);
			foreach (DataRow _row in _dt.Rows)
			{
				switch ((int)_row["TermId"])
				{
				case 1://Ticket
					ticket = new Name (new CustomName(_row["FullSingular"].ToString(), _row["FullPlural"].ToString(), _row["AbbreviatedSingular"].ToString(), _row["AbbreviatedPlural"].ToString()));
					break;
				case 2://Account
					account = new Name (new CustomName(_row["FullSingular"].ToString(), _row["FullPlural"].ToString(), _row["AbbreviatedSingular"].ToString(), _row["AbbreviatedPlural"].ToString()));
					break;
				case 3://Technician
					tech = new Name (new CustomName(_row["FullSingular"].ToString(), _row["FullPlural"].ToString(), _row["AbbreviatedSingular"].ToString(), _row["AbbreviatedPlural"].ToString()));
					break;
				case 5://Location
					location = new Name (new CustomName(_row["FullSingular"].ToString(), _row["FullPlural"].ToString(), _row["AbbreviatedSingular"].ToString(), _row["AbbreviatedPlural"].ToString()));
					break;
				case 6://End User
					user = new Name (new CustomName(_row["FullSingular"].ToString(), _row["FullPlural"].ToString(), _row["AbbreviatedSingular"].ToString(), _row["AbbreviatedPlural"].ToString()));
					break;
				}
			}
		}
	}

	[Serializable]
	public class Name
	{
		//
		// Properties
		//
        public string ap
        {
            get;set;
        }

		public string a {
			get;set;
		}

		public string p {
			get;set;
		}

		public string s {
			get;set;
		}

		//
		// Constructors
		//
		public Name (CustomName name)
		{
			s = name.FullSingular;
			p = name.FullPlural;
			a = name.AbbreviatedSingular;
			ap = name.AbbreviatedPlural;
		}
		public Name (string s1, string p1, string a1, string ap1)
		{
			s = s1;
			p = p1;
			a = a1;
			ap = ap1;
		}
	}
}
