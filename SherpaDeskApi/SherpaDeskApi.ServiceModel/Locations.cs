// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/locations", "GET, OPTIONS")]
    public class Locations : PagedApiRequest
    {
        public int? account { get; set; }
        public bool? is_active { get; set; }
        public int parent_id { get; set; }
        public string name { get; set; }
        public string search { get; set; }
        public bool? is_tree { get; set; }
    }

    [Route("/locations", "POST, OPTIONS")]
    public class LocationsPost : PagedApiRequest
    {
        public bool? is_active { get; set; }
        public int parent_location_id { get; set; }
        public int type_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? auditor_id { get; set; }
        public int? audit_days { get; set; }
    }

    [Route("/locations/{location_id}", "GET, OPTIONS")]
    public class Location : ApiRequest
    {
        public int location_id { get; set; }
    }

    [Route("/locations/{location_id}", "PUT, OPTIONS")]
    public class LocationEdit : ApiRequest
    {
        public int location_id { get; set; }
        public bool? is_active { get; set; }
        public int? parent_location_id { get; set; }
        public int? type_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? auditor_id { get; set; }
        public int? audit_days { get; set; }
    }

    [Route("/location_types")]
    public class LocationTypes : ApiRequest
    {
        public int location { get; set; }
    }
}
