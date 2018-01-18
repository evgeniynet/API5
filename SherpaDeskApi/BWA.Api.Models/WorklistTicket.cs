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
   /// Summary description for TicketLogRecord
   /// </summary>
    [DataContract(Name = "Worklist_Ticket")]
    public class WorklistTicket : ModelItemBase
    {
        public WorklistTicket(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get
            {
                return Row.Get<int>("Id");
            }
            set
            {
                Row["Id"] = value;
            }
        }

        [DataMember(Name = "key")]
        public string PseudoID
        {
            get { return Row["PseudoId"].ToString(); }
            set { Row["PseudoId"] = value; }
        }

        [DataMember(Name = "sla_complete_date")]
        public DateTime? SLAComplete
        {
            get { return Row.Get<DateTime?>("dtSlaComplete"); }
            set { if (value.HasValue) Row["dtSlaComplete"] = value.Value; else Row["dtSlaComplete"] = DBNull.Value; }
        }

        [DataMember(Name = "created_time")]
        public DateTime? CreateTime
        {
            get { return Row.Get<DateTime?>("CreateTime"); }
            set { if (value.HasValue) Row["CreateTime"] = value.Value; else Row["CreateTime"] = DBNull.Value; }
        }

        [DataMember(Name = "updated_time")]
        public DateTime? UpdatedTime
        {
            get { return Row.Get<DateTime?>("UpdatedTime"); }
            set { if (value.HasValue) Row["UpdatedTime"] = value.Value; else Row["UpdatedTime"] = DBNull.Value; }
        }

        [DataMember(Name = "closed_time")]
        public DateTime? ClosedTime
        {
            get { return Row.Get<DateTime?>("ClosedTime"); }
            set { if (value.HasValue) Row["ClosedTime"] = value.Value; else Row["ClosedTime"] = DBNull.Value; }
        }

        [DataMember(Name = "number")]
        public int TicketNumber
        {
            get { return (int)Row["TicketNumber"]; }
            set { Row["TicketNumber"] = value.ToString(); }
        }

        [DataMember(Name = "is_new_user_post")]
        public bool NewUserPost
        {
            get { return (bool)Row["NewUserPost"]; }
            set { Row["NewUserPost"] = value; }
        }

        [DataMember(Name = "is_new_tech_post")]
        public bool NewTechPost
        {
            get { return (bool)Row["NewTechPost"]; }
            set { Row["NewTechPost"] = value; }
        }  
       
        [DataMember(Name = "prefix")]
        public string TicketNumberPrefix
        {
            get { return Row["TicketNumberPrefix"].ToString(); }
            set { Row["TicketNumberPrefix"] = value; }
        }

        [DataMember(Name = "subject")]
        public string Subject
        {
            get { return Row["Subject"].ToString(); }
            set { Row["Subject"] = value; }
        }

        [DataMember(Name = "support_group_name")]
        public string support_group_name
        {
            get { return Row["SupportGroupName"].ToString(); }
            set { Row["SupportGroupName"] = value; }
        }

        [DataMember(Name = "next_step")]
        public string next_step
        {
            get { return Row["NextStep"].ToString(); }
            set { Row["NextStep"] = value; }
        }


        [DataMember(Name = "resolution_category_id")]
        public int? resolution_category_id
        {
            get { return Row.Get<int?>("ResolutionCatsId"); }
            set { Row["ResolutionCatsId"] = value; }
        }

        [DataMember(Name = "resolution_category_name")]
        public string resolution_category_name
        {
            get { return Row["ResolutionName"].ToString(); }
            set { Row["ResolutionName"] = value; }
        }


        [DataMember(Name = "support_group_id")]
        public int? support_group_id
        {
            get { return Row.Get<int?>("SupportGroupID"); }
            set { Row["SupportGroupID"] = value; }
        }

        [DataMember(Name = "initial_post")]
        public string initial_post
        {
            get { return Row["InitPost"].ToString(); }
            set { Row["InitPost"] = value; }
        }

        [DataMember(Name = "user_id")]
        public int? user_id
        {
            get { return Row.Get<int?>("user_id"); }
            set { Row["user_id"] = value; }
        }

        [DataMember(Name = "user_firstname")]
        public string UserFirstName
        {
            get { return Row["user_firstname"].ToString(); }
            set { Row["user_firstname"] = value; }
        }

        [DataMember(Name = "user_lastname")]
        public string UserLastName
        {
            get { return Row["user_lastname"].ToString(); }
            set { Row["user_lastname"] = value; }
        }

        [DataMember(Name = "user_email")]
        public string UserEmail
        {
            get { return Row["user_email"].ToString(); }
            set { Row["user_email"] = value; }
        }

        [DataMember(Name = "tech_id")]
        public int? tech_id
        {
            get { return Row.Get<int?>("Technician_id"); }
            set { Row["Technician_id"] = value; }
        }

        [DataMember(Name = "technician_firstname")]
        public string TechnicianFirstName
        {
            get { return Row["technician_firstname"].ToString(); }
            set { Row["technician_firstname"] = value; }
        }

        [DataMember(Name = "technician_lastname")]
        public string TechnicianLastName
        {
            get { return Row["technician_lastname"].ToString(); }
            set { Row["technician_lastname"] = value; }
        }

        [DataMember(Name = "technician_email")]
        public string TechnicianEmail
        {
            get { return Row["technician_email"].ToString(); }
            set { Row["technician_email"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int? AccountId
        {
            get { return Row.Get<int?>("intAcctId"); }
            set { Row["intAcctId"] = value; }
        }

        [DataMember(Name = "account_name")]
        public string AccountName
        {
            get { return Row["vchAcctName"].ToString(); }
            set { Row["vchAcctName"] = value; }
        }

        [DataMember(Name = "location_id")]
        public int? LocationId
        {
            get { return Row.Get<int?>("LocationId"); }
            set { Row["LocationId"] = value; }
        }

        [DataMember(Name = "location_name")]
        public string LocationName
        {
            get { return Row["LocationName"].ToString(); }
            set { Row["LocationName"] = value; }
        }

        [DataMember(Name = "account_location_id")]
        public int? AccountLocationId
        {
            get { return Row.Get<int?>("AccountLocationId"); }
            set { Row["AccountLocationId"] = value; }
        }

        [DataMember(Name = "account_location_name")]
        public string AccountLocationName
        {
            get { return Row["AccountLocationName"].ToString(); }
            set { Row["AccountLocationName"] = value; }
        }

        [DataMember(Name = "priority_id")]
        public int? priority_id
        {
            get { return Row.Get<int?>("PriorityID"); }
            set { Row["PriorityID"] = value; }
        }

        [DataMember(Name = "priority_name")]
        public string PriorityName
        {
            get { return Row["PriName"].ToString(); }
            set { Row["PriName"] = value; }
        }

        [DataMember(Name = "priority")]
        public byte priority
        {
            get { return Row.Get<byte>("tintPriority"); }
            set { Row["tintPriority"] = value; }
        }

        [DataMember(Name = "level")]
        public byte level
        {
            get { return Row.Get<byte>("tintLevel"); }
            set { Row["tintLevel"] = value; }
        }

        [DataMember(Name = "level_name")]
        public string LevelName
        {
            get { return Row["LevelName"].ToString(); }
            set { Row["LevelName"] = value; }
        }

        [DataMember(Name = "status")]
        public string TicketStatus
        {
            get
            {
                return Row["Status"].ToString();
            }
            set
            {
                    Row["Status"] = value;
            }
        }

        [DataMember(Name = "creation_category_id")]
        public int? CreationCategoryId
        {
            get { return Row.Get<int?>("CreationCatsId"); }
            set { Row["CreationCatsId"] = value; }
        }

        [DataMember(Name = "creation_category_name")]
        public string CreationCategoryName
        {
            get { return Row["CreationCategory"].ToString(); }
            set { Row["CreationCategory"] = value; }
        }

        [DataMember(Name = "days_old_in_minutes")]
        public long days_old_in_minutes
        {
            get { return Row.Get<long>("DaysOldSort"); }
            set { Row["DaysOldSort"] = value; }
        }

        [DataMember(Name = "days_old")]
        public string days_old
        {
            get { return Row.GetString("DaysOlds"); }
            set { Row["DaysOlds"] = value; }
        }

        [DataMember(Name = "class_id")]
        public int? ClassId
        {
            get { return Row.Get<int?>("class_id"); }
            set { Row["class_id"] = value; }
        }

        [DataMember(Name = "class_name")]
        public string ClassName
        {
            get { return Row["class_name"].ToString(); }
            set { Row["class_name"] = value; }
        }

        [DataMember(Name = "total_hours")]
        new public decimal TotalHours
        {
            get { return Row.Get<decimal>("TotalHours"); }
            set { Row["TotalHours"] = value; }
        }
    }
}
