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
    [DataContract(Name = "fb_category")]
    public class FBCategories
    {
        public static FBCategory CreateCategory(ApiUser User, Instance_Config instanceConfig, string categoryID)
        {
            FBCategory fbCategory;
            string result = FreshBooks.CreateCategory(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey,
                instanceConfig.FBoAuthSecret, categoryID, out fbCategory);
            if (result == "ok")
            {
                return fbCategory;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }

        public static void UnlinkFreshBooksCategory(ApiUser hdUser, string categoryID)
        {
            FreshBooks.UnlinkFreshBooksCategory(hdUser.OrganizationId, hdUser.DepartmentId, categoryID);
        }

        public static void DeleteFreshBooksCategory(ApiUser hdUser, Instance_Config instanceConfig, string categoryID)
        {
            FreshBooks.DeleteAndUnlinkCategory(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, categoryID);
        }

        public static List<FBCategory> GetFBCategories(Instance_Config instanceConfig)
        {
            List<FBCategory> arrCategories;
            string result = FreshBooks.GetCategoryList(instanceConfig, out arrCategories, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret);
            if (result == "ok")
            {
                return arrCategories;
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
        }
    }
}
