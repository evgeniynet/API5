// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk.Data;
using System.Net;
using ServiceStack.Common.Web;

namespace BWA.Api.Models
{
    [DataContract(Name = "fb_tasks")]
    public class FBTasks
    {
        public static List<FBTask> GetFBTasks(Instance_Config instanceConfig, int page, int limit, int projectID)
        {
            List<FBTask> arrTasks;
            string result = bigWebApps.bigWebDesk.Data.FreshBooks.GetTasksList(instanceConfig, out arrTasks, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, 
                limit, page, projectID);
            if (result == "ok")
            {
                return arrTasks;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static FBTask CreateTask(ApiUser User, Instance_Config instanceConfig, int taskTypeID, int fbProjectID)
        {
            FBTask fbTask;
            string result = FreshBooks.CreateTask(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey,
                instanceConfig.FBoAuthSecret, taskTypeID, fbProjectID, out fbTask);
            if (result == "ok")
            {
                return fbTask;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static FBTask FindFBTask(Instance_Config instanceConfig, string name)
        {
            FBTask arrTask;
            string result = FreshBooks.FindTask(instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret,
                name, out arrTask);
            if (result == "ok")
            {
                if (arrTask == null)
                {
                    arrTask = new FBTask(0, "");
                }
                return arrTask;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkFreshBooksTask(ApiUser hdUser, int taskTypeID)
        {
            FreshBooks.UnlinkFreshBooksTask(hdUser.OrganizationId, hdUser.DepartmentId, taskTypeID);
        }

        public static void DeleteFreshBooksTask(ApiUser hdUser, Instance_Config instanceConfig, int taskTypeID)
        {
            FreshBooks.DeleteAndUnlinkTask(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, taskTypeID);
        }
    }
}
