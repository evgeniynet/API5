// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/payments/{id}")]
    public class Payment : ApiRequest
    {
        public int id { get; set; }
    }
}