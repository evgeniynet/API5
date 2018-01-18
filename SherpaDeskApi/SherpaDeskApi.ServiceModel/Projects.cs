// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/projects/{id}")]
    public class Projects : ApiRequest
    {
        public int id { get; set; }
        public string search { get; set; }
    }

    [Route("/projects")]
    public class Projects_List : PagedApiRequest
    {
        public int account { get; set; }
        public int tech { get; set; }
		public bool? is_with_statistics { get; set; }
    }
}
