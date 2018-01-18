// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;
using System.Data;

namespace SherpaDeskApi.ServiceInterface
{   
    public class TaskTypesService : Service
    {
        [Secure()]
        public object Any(Task_Types request)
        {
            ApiUser hdUser = request.ApiUser;
            int techID = hdUser.UserId;
            if(request.tech > 0)
            {
                techID = request.tech;
            }
           //v1
            if (!string.IsNullOrEmpty(request.key))
                request.ticket = request.key;

            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(2, 0, 0), () =>
                {
                    if (!string.IsNullOrEmpty(request.ticket))
                        return Models.TaskTypes.TicketAssignedTaskTypes(hdUser.OrganizationId, hdUser.DepartmentId, techID, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket));
                    if (request.project > 0)
                        return Models.TaskTypes.SelectProjectTaskTypes(hdUser.OrganizationId, hdUser.DepartmentId, techID, request.project);
                    if (request.account != 0)
                        return Models.TaskTypes.SelectAccountTaskTypes(hdUser.OrganizationId, hdUser.DepartmentId, techID, request.account);
                    return Models.TaskTypes.SelectAllTaskTypes(hdUser.OrganizationId, hdUser.DepartmentId);
                });
        }

        [Secure()]
        public object Get(GET_TaskType request)
        {
            ApiUser hdUser = request.ApiUser;
            if (request.task_type_id >0)
            {
                DataRow row = bigWebApps.bigWebDesk.Data.TaskType.SelectTaskType(hdUser.OrganizationId, hdUser.DepartmentId, request.task_type_id);
                if (row == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "No data found");
                }
                return new TaskType(row);
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect request");
            }
        }
    }
}
