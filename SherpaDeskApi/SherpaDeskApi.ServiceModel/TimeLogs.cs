// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/time", "GET, OPTIONS")]
    public class GET_Time_Logs : PagedApiRequest
    {
       //v1
        public string key { get; set; }
       //v2
        public string ticket { get; set; }
        public int? project { get; set; }
        public string type { get; set; }
        public int? account { get; set; }
        public int? tech { get; set; }
        public int project_time_id { get; set; }
        public int ticket_time_id { get; set; }
        public int task_type_id { get; set; }
    }

    [Route("/time", "POST, OPTIONS")]
    public class POST_Time_Logs : ApiRequest
    {
        public string ticket_key { get; set; }
        public int task_type_id { get; set; }
        public string note_text { get; set; }
        public decimal hours { get; set; }
        public bool? is_billable { get; set; }
        public int ticket_time_id { get; set; }
        public DateTime? date { get; set; }
        public int tech_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? stop_date { get; set; }
        public decimal? remain_hours { get; set; }
        public int? complete { get; set; }
        public int? prepaid_pack_id { get; set; }
        public bool? is_local_time { get; set; }

       //Project Time Log fields
        public int project_id { get; set; }
        public int account_id { get; set; }
        public int project_time_id { get; set; }
    }

   //v1
    [Route("/time/{billable*}", "PUT, OPTIONS")]
   //v2
    [Route("/time/{key}", "PUT, OPTIONS")]
    [Route("/time", "PUT, OPTIONS")]
    public class Time_Billable : ApiRequest
    {
        public int key { get; set; }
        public string billable { get; set; }
        public bool is_project_log { get; set; }
        public bool? is_billable { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? stop_date { get; set; }
        public decimal? hours { get; set; }
        public int task_type_id { get; set; }
        public string note_text { get; set; }
        public DateTime? date { get; set; }
        public decimal? remain_hours { get; set; }
        public int? complete { get; set; }
        public int account_id { get; set; }
        public int project_id { get; set; }
        public int tech_id { get; set; }
        public int? prepaid_pack_id { get; set; }
        public bool? hidden_from_invoice { get; set; }
    }

    [Route("/time/{key}", "DELETE OPTIONS")]
    public class Time_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_project_log { get; set; }
    }
}
