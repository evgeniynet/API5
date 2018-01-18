// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using BWA.Api.Models;
using ServiceStack;
using SherpaDeskApi.ServiceModel;

namespace SherpaDeskApi.ServiceInterface
{
    public class ConfigService : Service
    {
        [Secure()]
        public object Get(Config request)
        {
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(0, 10, 0), () =>
                {
                                return new Instance_Config(request.ApiUser);
                });
        }

        public object Any(LoadTest request)
        {
            return "loaderio-7be5c8746b7afb8d497942075f59155e";
        }
    }
}
