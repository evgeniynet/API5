// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "Invoices")]
    public class Invoices : ModelItemCollectionGeneric<Invoice>
    {
        public Invoices(DataTable InvoicesTable) : base(InvoicesTable) { }

        public static List<Invoice> GetInvoices(Guid organizationId, int departmentId, int userId, DateTime beginDate, DateTime endDate, int? project, int? account)
        {
            Invoices _invoices = new Invoices(bigWebApps.bigWebDesk.Data.Invoice.SelectInvoices(organizationId, departmentId, beginDate, endDate, false));
            IEnumerable<Invoice> invoices = _invoices;
            if (project.HasValue)
                invoices = invoices.Where(i => i.ProjectId == project.Value);
            if (account.HasValue)
                invoices = invoices.Where(i => i.AccountId == account.Value);
            return invoices.ToList();
        }

        public static List<Invoice> GetInvoicesUnbilled(Guid organizationId, int departmentId, int userId, DateTime beginDate, DateTime endDate, int? project, int? account)
        {
            Invoices _invoices = new Invoices(bigWebApps.bigWebDesk.Data.Invoice.SelectUnbilledProjects(organizationId, departmentId, beginDate, endDate, true));
            IEnumerable<Invoice> invoices = _invoices;
            if (project.HasValue)
                invoices = invoices.Where(i => i.ProjectId == project.Value);
            if (account.HasValue)
                invoices = invoices.Where(i => i.AccountId == account.Value);
            return invoices.ToList();
        }
    }
}
