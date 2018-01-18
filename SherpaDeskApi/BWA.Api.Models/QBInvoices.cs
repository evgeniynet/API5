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
    [DataContract(Name = "qb_invoices")]
    public class QBInvoices
    {
        public static QBInvoice CreateInvoice(ApiUser User, Instance_Config instanceConfig, int invoice_id)
        {
            QBInvoice qbInvoice;
            string result = QuickBooks.CreateInvoice(User.OrganizationId, User.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey,
                instanceConfig.QBoAuthSecret, invoice_id, out qbInvoice);
            if (result != "ok")
            {
                throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
            }
            return qbInvoice;
        }

        public static void UnlinkQuickBooksInvoice(ApiUser hdUser, int invoice_id)
        {
            QuickBooks.UnlinkQuickBooksInvoice(hdUser.OrganizationId, hdUser.DepartmentId, invoice_id);
        }

        public static void DeleteQuickBooksInvoice(ApiUser hdUser, Instance_Config instanceConfig, int invoice_id)
        {
            QuickBooks.DeleteAndUnlinkInvoice(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.QBoAuthConsumerKey, instanceConfig.QBoAuthSecret, invoice_id);
        }
    }
}
