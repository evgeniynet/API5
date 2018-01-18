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
    [DataContract(Name = "qb_vendors")]
    public class QBVendors
    {
        public static QBVendor CreateVendor(ApiUser User, Instance_Config instanceConfig, int user_id)
        {
            QBVendor qbVendor;
            string result = QuickBooks.CreateVendor(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey,
                instanceConfig.QBoAuthSecret, user_id, out qbVendor);
            if (result == "ok")
            {
                return qbVendor;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkQuickBooksVendor(ApiUser hdUser, int userID)
        {
            QuickBooks.UnlinkQuickBooksVendor(hdUser.OrganizationId, hdUser.DepartmentId, userID);
        }

        public static void DeleteQuickBooksVendor(ApiUser hdUser, Instance_Config instanceConfig, int userID)
        {
            QuickBooks.DeleteAndUnlinkVendor(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, userID);
        }
    }
}
