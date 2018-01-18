// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace SherpaDeskApi.ServiceInterface
{
    public class TodosService : Service
    {
        [Secure()]
        public object Get(Todos request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
           //v1
            if (!string.IsNullOrEmpty(request.key))
                request.ticket = request.key;
            if (request.id.HasValue)
                request.project = request.id;
           //v2
			var todos = request.FilteredResult<Models.Todo>(Models.Todos.GetTicketTodos(hdUser.OrganizationId, hdUser.DepartmentId, string.IsNullOrEmpty (request.ticket) ?
				0 : Models.Ticket.GetId (hdUser.OrganizationId, hdUser.DepartmentId, request.ticket),
				request.project ?? 0, request.all_item_types ?? false, request.assigned_id ?? 0, request.is_completed));
			if (todos.Count > 0 && request.is_sub_view) {
				return MakeTreeFromFlatList (todos);
			}
			return todos;
        }

		private static List<Models.Todo> MakeTreeFromFlatList(List<Models.Todo> flatList)
		{
			var rootTodo = new List<Models.Todo>();
			if (flatList.Count == 1 && flatList.First ().ToDoItemId == null && flatList.First ().ToDoListId == null)
				return rootTodo;
			var dic = flatList.Where(n => n.ToDoItemId == null && n.ItemType != 3).ToDictionary(n => n.ToDoListId, n => n);
            if (dic.Count == 0)
                return flatList.FindAll(n => n.ItemType == 2);
			foreach (var todo in flatList)
			{
				if (todo.ItemType == 3)
					continue;
				if (todo.ToDoItemId.HasValue)
				{
					Models.Todo parent = dic[todo.ToDoListId];
					if ( parent.Sub == null)
						parent.Sub = new List<Models.Todo>();
					parent.Sub.Add(todo);
				}
				else
				{
					rootTodo.Add(todo);
				}
			}
			return rootTodo;
		}

        [Secure()]
        public object Get(Todo request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
            Guid todoId;
            if (Guid.TryParse(request.id, out todoId))
            {
                return new Models.Todo(bigWebApps.bigWebDesk.Data.ToDo.SelectToDoItem(hdUser.OrganizationId, hdUser.DepartmentId, todoId.ToString()));
            }
            return new HttpResult("", HttpStatusCode.NotFound);
        }

        [Secure()]
        public object Delete(Todo request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
            Guid todoId;
            if (Guid.TryParse(request.id, out todoId))
            {
                bigWebApps.bigWebDesk.Data.ToDo.DeleteToDoItem(hdUser.OrganizationId, hdUser.DepartmentId, todoId.ToString(), true);
                return new HttpResult("", HttpStatusCode.OK);
            }
            return new HttpResult("", HttpStatusCode.NotFound);
        }

        [Secure()]
        public object Put(Todo request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
            Guid todoId;
            if (Guid.TryParse(request.id, out todoId))
            {
                bigWebApps.bigWebDesk.Data.ToDo.CompleteToDoItem(hdUser.OrganizationId, hdUser.DepartmentId, todoId.ToString(), request.is_completed ?? false, hdUser.UserId, true);
                return new HttpResult("", HttpStatusCode.OK);
            }
            return new HttpResult("", HttpStatusCode.NotFound);
        }

        [Secure()]
        public object Delete(TodoList request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
            Guid todoId;
            if (Guid.TryParse(request.id, out todoId))
            {
                bigWebApps.bigWebDesk.Data.ToDo.DeleteToDoList(hdUser.OrganizationId, hdUser.DepartmentId, todoId.ToString(), true);
                return new HttpResult("", HttpStatusCode.OK);
            }
            return new HttpResult("", HttpStatusCode.NotFound);
        }

        [Secure()]
        public object Post(POST_TodoList request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
            Guid todoListId = Guid.Empty;
            if (!string.IsNullOrEmpty(request.list_id) && !Guid.TryParse(request.list_id, out todoListId))
            {
                return new HttpResult("", HttpStatusCode.NotFound);
            }
            if (!string.IsNullOrEmpty(request.list_id))
            {
                CheckToDoList(hdUser, todoListId);
            }
            if (!string.IsNullOrEmpty(request.list_id))
            {
                bigWebApps.bigWebDesk.Data.ToDo.UpdateToDoList(hdUser.OrganizationId, todoListId.ToString(), hdUser.DepartmentId, request.name);
            }
            else
            {
                int ticketID = CheckAddToDoListConditions(hdUser, request.ticket_key, request.project_id);
                bigWebApps.bigWebDesk.Data.ToDo.InsertToDoList(hdUser.OrganizationId, hdUser.DepartmentId, request.name, request.template_id, ticketID, request.project_id ?? 0);
            }
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure()]
        public object Post(POST_TodoItem request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
            Guid todoItemId = Guid.Empty;
            Guid todoListId = Guid.Empty;
            if (!string.IsNullOrEmpty(request.task_id) && !Guid.TryParse(request.task_id, out todoItemId))
            {
                return new HttpResult("", HttpStatusCode.NotFound);
            }
            DataRow plRow = null;
            if (!string.IsNullOrEmpty(request.task_id))
            {
                plRow = bigWebApps.bigWebDesk.Data.ToDo.SelectToDoItem(hdUser.OrganizationId, hdUser.DepartmentId, todoItemId.ToString());
                if (plRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong ToDo Task Id");
                }
                todoListId = Guid.Parse(plRow["ToDoListId"].ToString());
            }
            string text = (request.text != null ? request.text : "");
            DateTime due_date = request.due_date ?? DateTime.MinValue;
            decimal estimated_remain = request.estimated_remain ?? 0;
            int assigned_id = request.assigned_id ?? 0;
            if (!string.IsNullOrEmpty(request.task_id))
            {
                bigWebApps.bigWebDesk.Data.ToDo.UpdateToDoItem(hdUser.OrganizationId, todoItemId.ToString(), hdUser.DepartmentId, text, hdUser.UserId, assigned_id,
                    estimated_remain, due_date, true, request.title, request.notify ?? true);
                if (request.time_hours.HasValue)
                {
                    Instance_Config instanceConfig = new Instance_Config(hdUser);
                    if (!instanceConfig.TimeTracking)
                    {
                        throw new HttpError("Time Tracking is not enabled for this instance.");
                    }
                    if (!plRow.IsNull("TimeInvoiceId"))
                    {
                        throw new HttpError("Time log associated with this ToDo Item has been invoiced and can not be edited.");
                    }
                    if (!plRow.IsNull("TimeBillId"))
                    {
                        throw new HttpError("Time log associated with this ToDo Item has been paid and can not be edited.");
                    }
                    decimal time_hours = request.time_hours.Value;
                    bool time_is_billable = request.time_is_billable ?? true;
                    int time_task_type_id = request.time_task_type_id ?? 0;
                    if (!plRow.IsNull("TimeId"))
                    {
                        if (time_hours < 0)
                        {
                            bigWebApps.bigWebDesk.Data.ToDo.UnlinkToDoTime(hdUser.OrganizationId, hdUser.DepartmentId, int.Parse(plRow["TimeId"].ToString()), !plRow.IsNull("ProjectId"));
                        }
                        else if (time_hours == 0)
                        {
                            if (!plRow.IsNull("ProjectId"))
                            {
                                Models.Projects.DeleteProjectTime(hdUser, int.Parse(plRow["TimeId"].ToString()));
                            }
                            else
                            {
                                Models.Ticket.DeleteTicketTime(hdUser, int.Parse(plRow["TimeId"].ToString()));
                            }
                        }
                        else
                        {
                            if (!plRow.IsNull("ProjectId"))
                            {
                                DataRow rowProjectTime = bigWebApps.bigWebDesk.Data.Project.SelectProjectTimeByID(hdUser.OrganizationId, hdUser.DepartmentId, int.Parse(plRow["TimeId"].ToString()));
                                if (rowProjectTime == null)
                                {
                                    throw new HttpError(System.Net.HttpStatusCode.NotFound, "Project Time Id Not Found");
                                }
                                int techID = 0;
                                if (!rowProjectTime.IsNull("UserId"))
                                {
                                    techID = int.Parse(rowProjectTime["UserId"].ToString());
                                }
                                DateTime? start_date = null;
                                if (!rowProjectTime.IsNull("StartTimeUTC"))
                                {
                                    start_date = (DateTime)rowProjectTime["StartTimeUTC"];
                                }
                                DateTime? stop_date = null;
                                if (!rowProjectTime.IsNull("StopTimeUTC"))
                                {
                                    stop_date = (DateTime)rowProjectTime["StopTimeUTC"];
                                }
                                Models.Projects.EditProjectTime(hdUser, -1, int.Parse(plRow["ProjectId"].ToString()), techID, time_task_type_id, start_date, stop_date,
                                    time_hours, hdUser.TimeZoneOffset, rowProjectTime["Note"].ToString(), time_is_billable, (DateTime)rowProjectTime["Date"], int.Parse(plRow["TimeId"].ToString()),
                                    int.Parse(rowProjectTime["ContractID"].ToString()));
                            }
                            else
                            {
                                DataRow tlRow = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketTimeByID(hdUser.OrganizationId, hdUser.DepartmentId, int.Parse(plRow["TimeId"].ToString()));
                                if (tlRow == null)
                                {
                                    throw new HttpError(HttpStatusCode.NotFound, "No data found");
                                }
                                DateTime? start_date = null;
                                if (!tlRow.IsNull("StartTime"))
                                {
                                    start_date = (DateTime)tlRow["StartTime"];
                                }
                                DateTime? stop_date = null;
                                if (!tlRow.IsNull("StopTime"))
                                {
                                    stop_date = (DateTime)tlRow["StopTime"];
                                }
                                decimal? remainHours = null;
                                if (!tlRow.IsNull("HoursRemaining"))
                                {
                                    remainHours = decimal.Parse(tlRow["HoursRemaining"].ToString());
                                }
                                int contract_id = 0;
                                if (!tlRow.IsNull("ContractID"))
                                {
                                    contract_id = int.Parse(tlRow["ContractID"].ToString());
                                }
                                Ticket.EditTime(hdUser, int.Parse(plRow["TimeId"].ToString()), start_date, stop_date, time_is_billable, (DateTime)tlRow["Date"], time_hours, time_task_type_id,
                                    tlRow["Note"].ToString(), remainHours, int.Parse(tlRow["Complete"].ToString()), contract_id, tlRow["ContractName"].ToString());
                            }
                        }
                    }
                    else
                    {
                        int techID = (request.assigned_id.HasValue && request.assigned_id.Value > 0 && hdUser.IsAdmin) ? request.assigned_id.Value : hdUser.UserId;
                        if (!plRow.IsNull("ProjectId"))
                        {
                            Models.Projects.PostProjectTime(hdUser, -1, int.Parse(plRow["ProjectId"].ToString()), techID, time_task_type_id, null, null, time_hours, hdUser.TimeZoneOffset,
                                string.Empty, time_is_billable, DateTime.UtcNow, 0, 0, todoItemId.ToString(), string.Empty);
                        }
                        else
                        {
                            bigWebApps.bigWebDesk.Data.Ticket _tktNew = null;
                            Ticket.InputTime(hdUser, int.Parse(plRow["ListTicketId"].ToString()), time_task_type_id, null, null, time_hours, hdUser.TimeZoneOffset, string.Empty,
                                time_is_billable, DateTime.UtcNow, 0, techID, ref _tktNew, 0, 0, 0, todoItemId.ToString());
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(request.list_id) && !Guid.TryParse(request.list_id, out todoListId))
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong ToDo List Id");
                }
                if (!string.IsNullOrEmpty(request.list_id))
                {
                    CheckToDoList(hdUser, todoListId);
                }
                else
                {
                    int ticketID = CheckAddToDoListConditions(hdUser, request.ticket_key, request.project_id);
                    int projectID = (request.project_id ?? 0);

                    string name = "Main";
                    string filter = string.Empty;

                    if (ticketID > 0)
                    {
                        filter = "TicketId = " + ticketID.ToString();
                    }
                    else if (projectID > 0)
                    {
                        filter = "ProjectId = " + projectID.ToString();
                    }
                    else
                    {
                        name = "My ToDo's";

                        filter = "ProjectId IS NULL AND TicketId IS NULL";
                    }

                    DataTable table = bigWebApps.bigWebDesk.Data.ToDo.SelectToDoListsByUser(hdUser.OrganizationId, hdUser.DepartmentId, assigned_id);

                    foreach (DataRow row in table.Select(filter))
                    {
                        string toDoListName = (string)row["ToDoListName"];
                        if (string.Compare(name, toDoListName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            todoListId = (Guid)row["ToDoListId"];
                            break;
                        }
                    }

                    if (todoListId == Guid.Empty)
                    {
                        todoListId = Guid.NewGuid();

                        bigWebApps.bigWebDesk.Data.ToDo.InsertToDoList(hdUser.OrganizationId, todoListId.ToString(), hdUser.DepartmentId, name, string.Empty, ticketID, projectID);
                    }
                }
                bigWebApps.bigWebDesk.Data.ToDo.InsertToDoItem(hdUser.OrganizationId, hdUser.DepartmentId, text, todoListId.ToString(), hdUser.UserId, assigned_id,
                    estimated_remain, due_date, true, request.title, request.notify ?? true);
            }
            if (request.notify ?? false)
            {
                string userName = string.Empty;
                if (!string.IsNullOrEmpty(hdUser.FirstName) && !string.IsNullOrEmpty(hdUser.LastName))
                {
                    userName = hdUser.FirstName + " " + hdUser.LastName;
                }
                bigWebApps.bigWebDesk.Data.ToDo.SendToDoEmail(hdUser.OrganizationId, hdUser.InstanceId, hdUser.UserId, userName, hdUser.LoginEmail, todoListId.ToString(), text,
                    due_date, estimated_remain, assigned_id, request.title);
            }
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure()]
        public object Put(TodoListMove request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckToDos(hdUser);
            CheckToDoListAndTask(hdUser, request.source_list_id, request.source_task_id, "Source", true);
            CheckToDoListAndTask(hdUser, request.dest_list_id, request.dest_task_id, "Destination", false);
            bigWebApps.bigWebDesk.Data.ToDo.MoveToDoItem(hdUser.OrganizationId, hdUser.DepartmentId, Guid.Parse(request.source_list_id).ToString(), Guid.Parse(request.source_task_id).ToString(),
                Guid.Parse(request.dest_list_id).ToString(), (string.IsNullOrEmpty(request.dest_task_id) ? "" : Guid.Parse(request.dest_task_id).ToString()));
            return new HttpResult("", HttpStatusCode.OK);
        }

        private void CheckToDoListAndTask(ApiUser hdUser, string listID, string taskID, string type, bool checkTask)
        {
            Guid guidListID = Guid.Empty;
            Guid guidTaskID = Guid.Empty;
            if (!string.IsNullOrEmpty(listID) && !Guid.TryParse(listID, out guidListID))
            {
                throw new HttpError(HttpStatusCode.NotFound, "Wrong " + type + " ToDo List Id");
            }
            if (!string.IsNullOrEmpty(taskID) && !Guid.TryParse(taskID, out guidTaskID))
            {
                throw new HttpError(HttpStatusCode.NotFound, "Wrong " + type + " ToDo Task Id");
            }
            CheckToDoList(hdUser, guidListID);
            if (checkTask || !string.IsNullOrEmpty(taskID))
            {
                DataRow plRow = bigWebApps.bigWebDesk.Data.ToDo.SelectToDoItem(hdUser.OrganizationId, hdUser.DepartmentId, guidTaskID.ToString());
                if (plRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong " + type + " ToDo Task Id");
                }
                if (plRow["ToDoListId"].ToString() != guidListID.ToString())
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong " + type + " ToDo List Id");
                }
            }
            DataRow listRow = bigWebApps.bigWebDesk.Data.ToDo.SelectToDoList(hdUser.OrganizationId, hdUser.DepartmentId, guidListID.ToString());
            if (listRow == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, "Wrong " + type + " ToDo List Id");
            }
        }

        private void CheckToDos(ApiUser hdUser)
        {
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (!instanceConfig.EnableToDo)
            {
                throw new HttpError("ToDos is not enabled for this instance.");
            }
        }

        private void CheckToDoList(ApiUser hdUser, Guid todoListId)
        {
            DataRow plRow = bigWebApps.bigWebDesk.Data.ToDo.SelectToDoList(hdUser.OrganizationId, hdUser.DepartmentId, todoListId.ToString());
            if (plRow == null)
            {
                throw new HttpError(HttpStatusCode.NotFound, "Wrong ToDo List Id");
            }
        }

        private int CheckAddToDoListConditions(ApiUser hdUser, string ticket_key, int? project_id)
        {
            int ticketID = 0;
            if (!string.IsNullOrEmpty(ticket_key))
            {
                ticketID = Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, ticket_key);
                if (ticketID == 0)
                {
                    bigWebApps.bigWebDesk.CustomNames _cNames = bigWebApps.bigWebDesk.CustomNames.GetCustomNames(hdUser.OrganizationId, hdUser.DepartmentId);
                    throw new HttpError("Incorrect " + _cNames.Ticket.abbreviatedSingular + " key.");
                }
            }
            if (project_id.HasValue && project_id > 0)
            {
                DataRow rowProjectDetail = bigWebApps.bigWebDesk.Data.Project.SelectDetail(hdUser.OrganizationId, hdUser.DepartmentId, project_id.Value);
                if (rowProjectDetail == null)
                    throw new HttpError(System.Net.HttpStatusCode.NotFound, "Wrong Project Id");
            }
            if (ticketID > 0 && (project_id ?? 0) > 0)
            {
                throw new HttpError("Please set ticket_key or project_id for adding To Do List.");
            }
            return ticketID;
        }
    }
}
