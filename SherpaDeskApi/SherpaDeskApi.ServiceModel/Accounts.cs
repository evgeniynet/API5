// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/accounts")]
    public class Accounts : PagedApiRequest
    {
		public bool c { get; set; }
        public int? user { get; set; }
		public bool? is_with_statistics { get; set; }
        public string search { get; set; }
        public bool? is_open_tickets { get; set; }
		public bool? is_watch_info { get; set; }
		public bool? is_locations_info { get; set; }
    }

    [Route("/accounts/{account_id}")]
    public class Account : ApiRequest
    {
        public int account_id { get; set; }
		public bool? is_with_statistics { get; set; }
		public string note { get; set; }
    }
}
