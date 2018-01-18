// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/queues")]
    public class Queues : PagedApiRequest
    {
        public int? id { get; set; }
    }

    [Route("/queues/{id}")]
    public class Queue_Tickets : PagedApiRequest
    {
        public int id { get; set; }
        public string search { get; set; }
    }
}
