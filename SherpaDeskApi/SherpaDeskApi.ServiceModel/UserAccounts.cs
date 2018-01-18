// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/technicians")]
    public class Technicians : PagedApiRequest {
		public bool c { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string search { get; set; }
    }

    [Route("/users")]
    [Route("/users/{id}")]
    public class Users : PagedApiRequest
    {
		public bool c { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string search { get; set; }
        public int? account { get; set; }
        public int location { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
        public string role { get; set; }
    }
}
