// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;

namespace BWA.Api.Models
{
    public class Expenses : ModelItemCollectionGeneric<Expense>
    {
        public Expenses(DataTable ExpensesTable) : base(ExpensesTable) { }

        public static List<Expense> GetExpenses(Guid organizationId, int departmentId, string type, int accountID,
            int projectID, int techID)
        {
            int linkedFB = -1;
            int invoiced = -1;
            int billable = -1;
            int linkedQB = -1;
            int hiddenFromInvoice = -1;
            switch (type)
            {
                case "linked_fb":
                    linkedFB = 1;
                    break;
                case "unlinked_fb":
                    linkedFB = 0;
                    break;
                case "invoiced":
                    invoiced = 1;
                    break;
                case "not_invoiced":
                    invoiced = 0;
                    break;
                case "unlinked_fb_billable":
                    linkedFB = 0;
                    billable = 1;
                    break;
                case "not_invoiced_billable":
                    invoiced = 0;
                    billable = 1;
                    break;
                case "not_invoiced_nonbillable":
                    invoiced = 0;
                    billable = 0;
                    break;
                case "hidden_from_invoice":
                    hiddenFromInvoice = 1;
                    break;
                case "linked_qb":
                    linkedQB = 1;
                    break;
                case "unlinked_qb":
                    linkedQB = 0;
                    break;
                case "unlinked_qb_billable":
                    linkedQB = 0;
                    billable = 1;
                    break;
            }
            Expenses _expenses = new Expenses(bigWebApps.bigWebDesk.Data.Expense.SelectTop100MostExpenses(organizationId, departmentId, linkedFB, invoiced, accountID, projectID, 
                techID, billable, linkedQB, hiddenFromInvoice));
            var expenseTimeList = _expenses.ToList();
            Hashtable htUserProfileImage = new Hashtable();
            foreach (Expense exp in expenseTimeList)
            {
                if (!htUserProfileImage.Contains(exp.UserEmail))
                {
                    htUserProfileImage.Add(exp.UserEmail, ProfileImageProvider.GetImageUrl(exp.UserEmail));
                }
                exp.UserProfileImage = htUserProfileImage[exp.UserEmail].ToString();
            }
            return expenseTimeList;
        }
    }
}
