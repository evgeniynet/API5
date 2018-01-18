// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{

    [Route("/travel_logs")]
    public class Travel_Logs : ApiRequest
    {
        public string ticket_key { get; set; }
        public int account { get; set; }
        public string start_location { get; set; }
        public string end_location { get; set; }
        public int distance { get; set; }
        public decimal rate { get; set; }
        public DateTime date { get; set; }
        public string note { get; set; }
    }

    [Route("/travel_logs/{id}", "DELETE OPTIONS")]
    public class Travel_Log : ApiRequest
    {
        public int id { get; set; }
    }

    [Route("/travel_logs/{id}", "PUT, OPTIONS")]
    public class Travel_Put : ApiRequest
    {
        public int id { get; set; }
        public bool hidden_from_invoice { get; set; }
    }
}
