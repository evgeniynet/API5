// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/travels", "GET, OPTIONS")]
    public class GET_Travels : PagedApiRequest
    {
        public int? project { get; set; }
        public string type { get; set; }
        public int? account { get; set; }
        public int? tech { get; set; }
    }

    [Route("/travels", "POST, OPTIONS")]
    public class POST_Travel : ApiRequest
    {
        public int travel_id { get; set; }
        public string ticket_key { get; set; }
        public int tech_id { get; set; }
        public int account_id { get; set; }
        public int project_id { get; set; }
        public bool? is_billable { get; set; }
        public string note { get; set; }
        public string start_location { get; set; }
        public string end_location { get; set; }
        public int distance { get; set; }
        public decimal distance_rate { get; set; }
        public int qb_account_id { get; set; }
        public DateTime? date { get; set; }
        public bool? is_technician_payment { get; set; }
    }

    [Route("/travels/{travel_id}", "GET, OPTIONS")]
    public class GET_Travel : ApiRequest
    {
        public int travel_id { get; set; }
    }

    [Route("/travels/{travel_id}", "PUT, OPTIONS")]
    public class Travel_Billable : ApiRequest
    {
        public int travel_id { get; set; }
        public bool is_billable { get; set; }        
    }

    [Route("/travels/{travel_id}", "DELETE OPTIONS")]
    public class Travel_Delete : ApiRequest
    {
        public int travel_id { get; set; }
    }   
}
