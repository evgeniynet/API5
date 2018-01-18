// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
   //v1
    [Route("/activity/{user_id}")]
   //v2
    [Route("/activity")]
    public class Activity : PagedApiRequest
    {
       //v1
        public int user_id { get; set; }
       //v2
        public int user { get; set; }
    }
}
