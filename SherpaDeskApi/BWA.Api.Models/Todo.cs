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
    [DataContract(Name = "Todo")]
    public class Todo : ModelItemBase
    {
        public Todo(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "list_id")]
        public Guid? ToDoListId
        {
            get
            {
                if (!Row.IsNull("ToDoListId"))
                {
                    return Guid.Parse(Row["ToDoListId"].ToString());
                }
                return null;
            }
            set { Row["ToDoListId"] = value; }
        }

        [DataMember(Name = "id")]
        public Guid? ToDoItemId
        {
            get
            {
                if (Row.Table.Columns.Contains("ToDoItemId") && !Row.IsNull("ToDoItemId")) return (Guid)Row["ToDoItemId"];
                if (Row.Table.Columns.Contains("Id") && !Row.IsNull("Id")) return (Guid)Row["Id"];
                else return null;
            }
            set { Row["ToDoItemId"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get
            {
                if (Row.Table.Columns.Contains("Name"))
                    return Row["Name"].ToString();
                return string.Empty;
            }
            set { Row["Name"] = value; }
        }

        [DataMember(Name = "text")]
        public string Text
        {
            get { return Row["Text"].ToString(); }
            set { Row["Text"] = value; }
        }

        [DataMember(Name = "title")]
        public string Title
        {
            get { return Row["Title"].ToString(); }
            set { Row["Title"] = value; }
        }

        [DataMember(Name = "order_list")]
        public int OrderList
        {
            get
            {
                if (Row.Table.Columns.Contains("OrderList")) return Convert.ToInt32(Row["OrderList"]);
                return 0;
            }
            set { Row["OrderList"] = value; }
        }

        [DataMember(Name = "order_item")]
        public int OrderItem
        {
            get
            {
                if (Row.Table.Columns.Contains("OrderItem")) return Convert.ToInt32(Row["OrderItem"]);
                return 0; 
            }
            set { Row["OrderItem"] = value; }
        }

        [DataMember(Name = "assigned_id")]
        public int? AssignedId
        {
            get
            {
                if (!Row.IsNull("AssignedId")) return Convert.ToInt32(Row["AssignedId"]);
                else return null; 
            }
            set { Row["AssignedId"] = value; }
        }

        [DataMember(Name = "assigned_name")]
        public string AssignedName
        {
            get
            {
                if (Row.Table.Columns.Contains("AssignedName")) return Row["AssignedName"].ToString();
                return "";
            }
            set { Row["AssignedName"] = value; }
        }

        [DataMember(Name = "type")]
        public int ItemType
        {
            get
            {
                if (Row.Table.Columns.Contains("ItemType")) return Convert.ToInt32(Row["ItemType"]);
                return 0; 
            }
            set { Row["ItemType"] = value; }
        }

        [DataMember(Name = "ticket_id")]
        public int TicketId
        {
            get
            {
                if (Row.Table.Columns.Contains("TicketId") && !Row.IsNull("TicketId")) return Convert.ToInt32(Row["TicketId"]);
                return 0;
            }
            set { Row["TicketId"] = value; }
        }

        [DataMember(Name = "project_id")]
        public int ProjectId
        {
            get
            {
                if (Row.Table.Columns.Contains("ProjectId") && !Row.IsNull("ProjectId")) return Convert.ToInt32(Row["ProjectId"]);
                return 0;
            }
            set { Row["ProjectId"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int AccountId
        {
            get
            {
                if (Row.Table.Columns.Contains("AccountId") && !Row.IsNull("AccountId")) return Convert.ToInt32(Row["AccountId"]);
                return 0;
            }
            set { Row["AccountId"] = value; }
        }
        
        [DataMember(Name = "due_date")]
        public DateTime? DueDate
        {
            get
            {
                if (!Row.IsNull("Due")) return (DateTime)Row["Due"];
                else return null;
            }
            set
            {
                Row["Due"] = value;
            }
        }

        [DataMember(Name = "updated_at")]
        public DateTime? UpdatedAt
        {
            get
            {
                return Row.Get<DateTime?> ("UpdatedAt");
            }
            set
            {
                Row["UpdatedAt"] = value;
            }
        }

        [DataMember(Name = "is_completed")]
        public bool Completed
        {
            get { return Convert.ToBoolean(Row["Completed"]); }
            set { Row["Completed"] = value; }
        }

        [DataMember(Name = "estimated_remain")]
        public decimal? HoursEstimatedRemaining
        {
            get
            {
                if (Row.Table.Columns.Contains("HoursEstimatedRemaining") && !Row.IsNull("HoursEstimatedRemaining"))
                {
                    return Convert.ToDecimal(Row["HoursEstimatedRemaining"]);
                }
                else return null;
            }
            set { Row["HoursEstimatedRemaining"] = value; }
        }

        [DataMember(Name = "ticket_number")]
        public int? TicketNumber
        {
            get
            {
                if (Row.Table.Columns.Contains("TicketNumber") && !Row.IsNull("TicketNumber"))
                {
                    return Convert.ToInt32(Row["TicketNumber"]);
                }
                else return null;
            }
            set { Row["TicketNumber"] = value; }
        }

        [DataMember(Name = "time_hours")]
        public decimal? TimeHours
        {
            get { return Row.Get<decimal?>("TimeHours"); }
            set { Row["TimeHours"] = value; }
        }

        [DataMember(Name = "time_is_billable")]
        public bool TimeIsBillable
        {
            get { return Row.Get<bool>("TimeBillable"); }
            set { Row["TimeBillable"] = value; }
        }

        [DataMember(Name = "time_task_type_id")]
        public int? TaskTypeId
        {
            get { return Row.Get<int?>("TimeTaskTypeId"); }
            set { Row["TimeTaskTypeId"] = value; }
        }

        [DataMember(Name = "time_task_type_name")]
        public string TimeTaskTypeName
        {
            get
            {
                if (Row.Table.Columns.Contains("TimeTaskTypeName")) return Row["TimeTaskTypeName"].ToString();
                return "";
            }
            set { Row["TimeTaskTypeName"] = value; }
        }
        
        [DataMember(Name = "time_payment_id")]
        public int? TimePaymentId
        {
            get { return Row.Get<int?>("TimeBillId"); }
            set { Row["TimeBillId"] = value; }
        }

        [DataMember(Name = "time_invoice_id")]
        public int? TimeInvoiceId
        {
            get { return Row.Get<int?>("TimeInvoiceId"); }
            set { Row["TimeInvoiceId"] = value; }
        }

        [DataMember(Name = "time_start_time")]
        public DateTime? TimeStartTime
        {
            get { return Row.Get<DateTime?>("TimeStartTime"); }
            set { Row["TimeStartTime"] = value; }
        }

        [DataMember(Name = "time_stop_time")]
        public DateTime? TimeStopTime
        {
            get { return Row.Get<DateTime?>("TimeStopTime"); }
            set { Row["TimeStopTime"] = value; }
        }

        [DataMember(Name = "time_user_id")]
        public int? TimeUserId
        {
            get { return Row.Get<int?>("TimeUserId"); }
            set { Row["TimeUserId"] = value; }
        }

        [DataMember(Name = "list_ticket_id")]
        public int ListTicketId
        {
            get { return Row.Get<int>("ListTicketId", 0); }
            set { Row["ListTicketId"] = value; }
        }

        [DataMember(Name = "list_ticket_number")]
        public int? ListTicketNumber
        {
            get { return Row.Get<int?>("ListTicketNumber", null);}
            set { Row["ListTicketNumber"] = value; }
        }

        [DataMember(Name = "list_ticket_subject")]
        public string ListTicketSubject
        {
            get
            {
                if (Row.Table.Columns.Contains("ListTicketSubject")) return Row["ListTicketSubject"].ToString();
                return "";
            }
            set { Row["ListTicketSubject"] = value; }
        }

        [DataMember(Name = "project_name")]
        public string ProjectName
        {
            get
            {
                if (Row.Table.Columns.Contains("ProjectName")) return Row["ProjectName"].ToString();
                return "";
            }
            set { Row["ProjectName"] = value; }
        }

        [DataMember(Name = "notify")]
        public bool Notify
        {
            get { return Convert.ToBoolean(Row["Notify"]); }
            set { Row["Notify"] = value; }
        }

        [DataMember(Name = "created_by")]
        public int CreatedBy
        {
            get
            {
                if (Row.Table.Columns.Contains("CreatedBy")) return Convert.ToInt32(Row["CreatedBy"]);
                return 0;
            }
            set { Row["CreatedBy"] = value; }
        }

        private List<Models.Todo> _subTodos = null;

		[DataMember(Name = "sub")]
		public List<Models.Todo> Sub {
			get { return _subTodos; }
			set
			{
				_subTodos = value;
			}
		}
    }
}
