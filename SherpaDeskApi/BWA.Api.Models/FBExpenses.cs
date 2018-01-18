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
    [DataContract(Name = "fb_expense")]
    public class FBExpenses
    {
        public static string CreateExpense(ApiUser hdUser, Instance_Config instanceConfig, int staffID, int projectID, int categoryID, int clientID, decimal amount, 
            string vendor, string notes, DateTime date, string expenseID, int fbExpenseID)
        {
            return FreshBooks.CreateExpense(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret,
                staffID, projectID, categoryID, clientID, amount, vendor, notes, date, expenseID, fbExpenseID);
        }

        public static void UnlinkFreshBooksExpense(ApiUser hdUser, string expenseID)
        {
            FreshBooks.UnlinkFreshBooksExpense(hdUser.OrganizationId, hdUser.DepartmentId, expenseID);
        }

        public static void DeleteFreshBooksExpense(ApiUser hdUser, Instance_Config instanceConfig, string expenseID)
        {
            FreshBooks.DeleteAndUnlinkExpense(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.FBoAuthConsumerKey, instanceConfig.FBoAuthSecret, expenseID);
        }
    }
}
