// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
   //v1
    [Route("/tickets/{key}/posts")]
   //v2
    [Route("/posts")]
    public class Posts : PagedApiRequest
    {
        public string key { get; set; }
        public string note_text { get; set; }
       //v2
        public string ticket { get; set; }
        public bool? is_waiting_on_response { get; set; }
        public string[] files { get; set; }
        public int user_id { get; set; }
    }
}
