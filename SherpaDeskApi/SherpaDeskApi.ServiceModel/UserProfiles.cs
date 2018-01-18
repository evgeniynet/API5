// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/profile/{id}")]
    public class Profile : ApiRequest {
        public string skype { get; set; }
        public string phone { get; set; }
        public string mobile_phone { get; set; }
        //public string email { get; set; }
        public int id { get; set; }
    }
}
