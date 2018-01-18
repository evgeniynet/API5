// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/prepaid_packs", "GET, OPTIONS")]
    public class GET_Contracts : PagedApiRequest
    {
        public int? account { get; set; }
        public DateTime? date { get; set; }
        public int? project { get; set; }
    }

    [Route("/prepaid_packs/{prepaid_pack_id}", "GET, OPTIONS")]
    public class GET_Contract : ApiRequest
    {
        public int prepaid_pack_id { get; set; }
    }
}
