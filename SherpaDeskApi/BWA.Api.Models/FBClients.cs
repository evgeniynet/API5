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
    [DataContract(Name = "fb_clients")]
    public class FBClients
    {
        public static List<FBClient> GetFBClients(Instance_Config instanceConfig, int page, int limit)
        {
            List<FBClient> arrClients;
            string result = FreshBooks.GetClientList(instanceConfig, out arrClients, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, limit, page);
            if (result == "ok")
            {
                return arrClients;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static FBClient CreateClient(ApiUser User, Instance_Config instanceConfig, int accountID)
        {
            FBClient fbClient;
            string result = FreshBooks.CreateClient(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, accountID, out fbClient);
            if (result == "ok")
            {
                return fbClient;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkFreshBooksExpense(ApiUser hdUser, int accountID)
        {
            FreshBooks.UnlinkFreshBooksClient(hdUser.OrganizationId, hdUser.DepartmentId, accountID);
        }

        public static void DeleteFreshBooksExpense(ApiUser hdUser, Instance_Config instanceConfig, int accountID)
        {
            FreshBooks.DeleteAndUnlinkClient(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, accountID);
        }
    }
}
