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
    [DataContract(Name = "qb_expenses")]
    public class QBExpenses
    {
        public static string CreateExpense(ApiUser hdUser, Instance_Config instanceConfig, int qb_employee_id, int qb_customer_id, int qb_service_id, int qb_account_id, bool qb_is_employee,
            decimal amount, string notes, string note_internal, DateTime date, bool is_billable, int markup, string expense_id, int qb_expense_id, int qb_sync_token, bool overwrite_changes, int qb_vendor_id,
            int travel_id)
        {
            return QuickBooks.CreateExpense(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, qb_employee_id, qb_customer_id, 
                qb_service_id, qb_account_id, qb_is_employee, amount, notes, note_internal, date, is_billable, markup, expense_id, qb_expense_id, qb_sync_token, overwrite_changes, qb_vendor_id, travel_id);
        }

        public static void UnlinkQuickBooksExpense(ApiUser hdUser, string expenseID)
        {
            QuickBooks.UnlinkQuickBooksExpense(hdUser.OrganizationId, hdUser.DepartmentId, expenseID);
        }

        public static void DeleteQuickBooksExpense(ApiUser hdUser, Instance_Config instanceConfig, string expenseID)
        {
            QuickBooks.DeleteAndUnlinkExpense(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, expenseID);
        }

        public static void UnlinkQuickBooksTravel(ApiUser hdUser, int travel_id)
        {
            QuickBooks.UnlinkQuickBooksTravel(hdUser.OrganizationId, hdUser.DepartmentId, travel_id);
        }

        public static void DeleteQuickBooksTravel(ApiUser hdUser, Instance_Config instanceConfig, int travel_id)
        {
            QuickBooks.DeleteAndUnlinkTravel(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, travel_id);
        }
    }
}
