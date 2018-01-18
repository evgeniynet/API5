// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/invoices")]
    public class Invoices : PagedApiRequest
    {
        public int? project { get; set; }
        public int? account { get; set; }
        public string status { get; set; }
        public string recipients { get; set; }
        public string time_logs { get; set; }
        public string travel_logs { get; set; }
        public string expenses { get; set; }
        public string retainers { get; set; }
        public decimal adjustments { get; set; }
        public string adjustments_note { get; set; }
    }

    [Route("/invoices/{id}")]
    public class Invoice : ApiRequest
    {
        public string id { get; set; }
        public string action { get; set; }
        public string recipients { get; set; }
        public bool is_pdf { get; set; }
		public bool is_detailed { get; set; }
    }

}
