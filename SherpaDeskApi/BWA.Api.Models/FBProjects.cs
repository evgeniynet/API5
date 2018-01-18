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
    [DataContract(Name = "fb_projects")]
    public class FBProjects
    {
        public static List<FBProject> GetFBProjects(Instance_Config instanceConfig, int page, int limit, int clientID, int staffID)
        {
            List<FBProject> arrProjects;
            string result = FreshBooks.GetProjectsList(instanceConfig, out arrProjects, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, limit, page, clientID, staffID);
            if (result == "ok")
            {
                return arrProjects;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static FBProject CreateProject(ApiUser User, Instance_Config instanceConfig, int projectID, int fbStaffID, int fbClientID, int accountID)
        {
            FBProject fbProject;
            string result = FreshBooks.CreateProject(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey,
                instanceConfig.FBoAuthSecret, projectID, fbStaffID, fbClientID, accountID, out fbProject);
            if (result == "ok")
            {
                return fbProject;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkFreshBooksProject(ApiUser hdUser, int projectID)
        {
            FreshBooks.UnlinkFreshBooksProject(hdUser.OrganizationId, hdUser.DepartmentId, projectID);
        }

        public static void DeleteFreshBooksProject(ApiUser hdUser, Instance_Config instanceConfig, int projectID)
        {
            FreshBooks.DeleteAndUnlinkProject(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, projectID);
        }
    }
}
