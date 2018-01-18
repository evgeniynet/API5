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
    [DataContract(Name = "qb_employees")]
    public class QBEmployees
    {
        public static QBEmployee CreateEmployee(ApiUser User, Instance_Config instanceConfig, int user_id)
        {
            QBEmployee qbEmployee;
            string result = QuickBooks.CreateEmployee(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey,
                instanceConfig.QBoAuthSecret, user_id, out qbEmployee);
            if (result == "ok")
            {
                return qbEmployee;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkQuickBooksEmployee(ApiUser hdUser, int userID)
        {
            QuickBooks.UnlinkQuickBooksEmployee(hdUser.OrganizationId, hdUser.DepartmentId, userID);
        }

        public static void DeleteQuickBooksEmployee(ApiUser hdUser, Instance_Config instanceConfig, int userID)
        {
            QuickBooks.DeleteAndUnlinkEmployee(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, userID);
        }
    }
}
