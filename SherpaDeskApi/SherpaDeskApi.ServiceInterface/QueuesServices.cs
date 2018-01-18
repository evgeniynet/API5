// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
namespace SherpaDeskApi.ServiceInterface
{
    public class QueuesService : Service
    {
        [Secure("tech")]
        public object Get(Queues request)
        {
            ApiUser hdUser = request.ApiUser;

            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(0, 5, 0), () =>
                {
                    return request.FilteredResult<UnassignedQueue>(UnassignedQueues.TechQueues(hdUser));
                });
            }

        [Secure("tech")]
        public object Get(Queue_Tickets request)
        {
            ApiUser hdUser = request.ApiUser;
            var tickets = WorklistTickets.GetTickets(hdUser, hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, "allopen", "TechTickets", request.id.ToString(), "", "", "", hdUser.IsTechAdmin, hdUser.IsUseWorkDaysTimer, request.sort_order, request.sort_by, request.search, request.page, request.limit, request.start_date, request.end_date);
            return request.QueryResult<WorklistTicket>(tickets);
        }
    }
}
