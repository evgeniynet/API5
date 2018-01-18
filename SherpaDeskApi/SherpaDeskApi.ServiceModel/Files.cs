// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
   //v2
    [Route("/files")]
    public class Files : ApiRequest
    {
        public string ticket { get; set; }
        public string account { get; set; }
        public string asset { get; set; }
        public int post_id { get; set; }
    }

    [Route("/files/{file_id*}")]
    public class File : ApiRequest
    {
        public string ticket { get; set; }
        public string file_id { get; set; }
        public bool is_link_only { get; set; }
    }
}
