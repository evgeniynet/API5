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
    [DataContract(Name = "qb_bills")]
    public class QBBills
    {
        public static string CreateBill(ApiUser User, Instance_Config instanceConfig, int bill_id)
        {
            string result = QuickBooks.CreateBill(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey,
                instanceConfig.QBoAuthSecret, bill_id);
            if (result != "ok")
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
            return result;
        }

        public static void UnlinkQuickBooksBill(ApiUser hdUser, int bill_id)
        {
            QuickBooks.UnlinkQuickBooksBill(hdUser.OrganizationId, hdUser.DepartmentId, bill_id, true);
        }

        public static void DeleteQuickBooksBill(ApiUser hdUser, Instance_Config instanceConfig, int bill_id)
        {
            QuickBooks.DeleteAndUnlinkBill(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, bill_id);
        }
    }
}
