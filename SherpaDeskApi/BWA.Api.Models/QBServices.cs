// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk.Data;
using System.Net;
using ServiceStack.Common.Web;

namespace BWA.Api.Models
{
    [DataContract(Name = "qb_services")]
    public class QBServices
    {
        public static QBService CreateService(ApiUser User, Instance_Config instanceConfig, int task_type_id)
        {
            QBService qbService;
            string result = QuickBooks.CreateService(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey,
                instanceConfig.QBoAuthSecret, task_type_id, out qbService);
            if (result == "ok")
            {
                return qbService;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkQuickBooksService(ApiUser hdUser, int task_type_id)
        {
            QuickBooks.UnlinkQuickBooksService(hdUser.OrganizationId, hdUser.DepartmentId, task_type_id);
        }

        public static void DeleteQuickBooksService(ApiUser hdUser, Instance_Config instanceConfig, int task_type_id)
        {
            QuickBooks.DeleteAndUnlinkService(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, task_type_id);
        }
    }
}
