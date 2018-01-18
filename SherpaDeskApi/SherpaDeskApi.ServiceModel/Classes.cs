// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Secure()]
    [Route("/classes")]
    public class Classes : ApiRequest
    {
        public int? user { get; set; }
        public int? class_id { get; set; }
        public bool? is_global_filters_enabled { get; set; }
        public bool? get_all_active_for_user { get; set; }
    }

    [Route("/classes/parent/{class_id}")]
    public class ParentClasses : ApiRequest
    {
        public int? class_id { get; set; }
    }
}
