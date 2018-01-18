// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{

   //v1
    [Route("/tickets/{key}/todos", "GET, OPTIONS")]
    [Route("/projects/{id}/todos", "GET, OPTIONS")]
   //v2
    [Route("/todos", "GET, OPTIONS")]
    public class Todos : PagedApiRequest
    {
       //v1
        public string key { get; set; }
        public int? id { get; set; }
       //v2
        public string ticket { get; set; }
        public int? project { get; set; }
        public int? assigned_id { get; set; }
        public bool? is_completed { get; set; }
        public bool? all_item_types { get; set; }
		public bool is_sub_view { get; set; }
    }

    [Route("/todos/{id}")]
    public class Todo : ApiRequest
    {
        public string id { get; set; }
        public bool? is_completed { get; set; }
    }

    [Route("/todos/list/{id}")]
    public class TodoList : ApiRequest
    {
        public string id { get; set; }
    }

    [Route("/todos/list", "POST, OPTIONS")]
    public class POST_TodoList : ApiRequest
    {
        public string list_id { get; set; }
        public string name { get; set; }
        public string template_id { get; set; }
        public string ticket_key { get; set; }
        public int? project_id { get; set; }
    }

    [Route("/todos", "POST, OPTIONS")]
    public class POST_TodoItem : ApiRequest
    {
        public string task_id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string list_id { get; set; }
        public int? assigned_id { get; set; }
        public decimal? estimated_remain { get; set; }
        public DateTime? due_date { get; set; }
        public string ticket_key { get; set; }
        public int? project_id { get; set; }
        public bool? notify { get; set; }
        public decimal? time_hours { get; set; }
        public bool? time_is_billable { get; set; }
        public int? time_task_type_id { get; set; }
    }

    [Route("/todos/move", "PUT OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class TodoListMove : ApiRequest
    {
        public string source_list_id { get; set; }
        public string source_task_id { get; set; }
        public string dest_list_id { get; set; }
        public string dest_task_id { get; set; }
    }
}
