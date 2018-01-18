// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{   
   //v1
    [Route("/tickets/{key}/task_types")]
   //v2
    [Route("/task_types")]
    public class Task_Types : ApiRequest {
       //v1
        public string key { get; set; }
       //v2
        public string ticket { get; set; }
        public int project { get; set; }
        public int account { get; set; }
        public int tech { get; set; }
    }

    [Route("/task_types/{task_type_id}", "GET, OPTIONS")]
    public class GET_TaskType : ApiRequest
    {
        public int task_type_id { get; set; }
    }
}
