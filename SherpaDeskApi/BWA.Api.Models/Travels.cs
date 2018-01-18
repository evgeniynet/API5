// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;

namespace BWA.Api.Models
{
    public class Travels : ModelItemCollectionGeneric<Travel>
    {
        public Travels(DataTable TravelsTable) : base(TravelsTable) { }

        public static List<Travel> GetTravels(Guid organizationId, int departmentId, string type, int accountID, int projectID, int techID)
        {
            int invoiced = -1;
            int billable = -1;
            int hiddenFromInvoice = -1;
            switch (type)
            {
                case "invoiced":
                    invoiced = 1;
                    break;
                case "not_invoiced":
                    invoiced = 0;
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
            }
            Travels _expenses = new Travels(bigWebApps.bigWebDesk.Data.TicketTravelCosts.SelectTop100MostTravels(organizationId, departmentId, invoiced, accountID, projectID, techID, billable, hiddenFromInvoice));
            var expenseTimeList = _expenses.ToList();
            Hashtable htUserProfileImage = new Hashtable();
            foreach (Travel exp in expenseTimeList)
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
