// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/tickets/", "GET, OPTIONS")]
    public class Tickets_List : PagedApiRequest
    {
        public string status { get; set; }
        public string role { get; set; }
        public string Class { get; set; }
        public string account { get; set; }
        public string location { get; set; }
		public string project { get; set; }
        public int user { get; set; }
        public string search { get; set; }
    }

    [Route("/tickets/{key}", "GET, DELETE, OPTIONS")]
    public class Tickets : ApiRequest
    {
        public string key { get; set; }
        public string search { get; set; }
    }

   //v1
    [Route("/tickets/counts/{status}")]
   //v2
    [Route("/tickets/counts")]
    public class Tickets_Counts : ApiRequest
    {
        public string status { get; set; }
    }

   //v1
    [Route("/tickets/{key}/technicians/{tech_id}", "PUT, POST, OPTIONS")]
    [Route("/tickets/{key}/users/{user_id}", "POST, OPTIONS")]
    [Route("/tickets/{key}/{action}", "PUT, OPTIONS")]
   //v2
    [Route("/tickets/{key}", "PUT, POST, OPTIONS")]
    public class Ticket_Actions : ApiRequest
    {
        public string key { get; set; }
        public string status { get; set; }
        public string action { get; set; }
        public string note_text { get; set; }
		public string workpad { get; set; }
        public string note { get; set; }
        public int tech_id { get; set; }
        public int user_id { get; set; }
		public int project_id { get; set; }
        public int class_id { get; set; }
        public int level_id { get; set; }
        public int priority_id { get; set; }
        public int location_id { get; set; }
        public int account_id { get; set; }
        public int account_location_id { get; set; }
        public bool is_transfer_user_to_account { get; set; }
        public bool keep_attached { get; set; }
        public bool is_send_notifications { get; set; }
        public bool? is_waiting_on_response { get; set; }
        public bool resolved { get; set; }
        public int resolution_id { get; set; }
        public bool confirmed { get; set; }
        public string confirm_note { get; set; }
        public string[] files { get; set; }
    }
}
