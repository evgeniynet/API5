// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;

namespace SherpaDeskApi.ServiceInterface
{
    public class ClassesService : Service
    {
        [Secure()]
        public object Get(Classes request)
        {
            ApiUser hdUser = request.ApiUser;
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(2, 0, 0), () =>
                {
                    return Models.Classes.UserClasses(hdUser.OrganizationId, hdUser.DepartmentId, request.user ?? hdUser.UserId, request.class_id ?? -1,
                request.is_global_filters_enabled ?? true, request.get_all_active_for_user);
                });
        }

        [Secure()]
        public object Get(ParentClasses request)
        {
            ApiUser hdUser = request.ApiUser;
            if (!request.class_id.HasValue)
                return new HttpError(HttpStatusCode.NotFound, "class not found");
            return Models.Classes.ParentClasses(hdUser.OrganizationId, hdUser.DepartmentId, request.class_id.Value);
        }
    }
}
