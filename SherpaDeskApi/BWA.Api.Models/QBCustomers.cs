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
    [DataContract(Name = "qb_customers")]
    public class QBCustomers
    {
        public static QBCustomer CreateCustomer(ApiUser User, Instance_Config instanceConfig, int account_id)
        {
            QBCustomer qbCustomer;
            string result = QuickBooks.CreateCustomer(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey,
                instanceConfig.QBoAuthSecret, account_id, out qbCustomer);
            if (result == "ok")
            {
                return qbCustomer;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkQuickBooksCustomer(ApiUser hdUser, int account_id)
        {
            QuickBooks.UnlinkQuickBooksCustomer(hdUser.OrganizationId, hdUser.DepartmentId, account_id);
        }

        public static void DeleteQuickBooksCustomer(ApiUser hdUser, Instance_Config instanceConfig, int account_id)
        {
            QuickBooks.DeleteAndUnlinkCustomer(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, account_id);
        }
    }
}
