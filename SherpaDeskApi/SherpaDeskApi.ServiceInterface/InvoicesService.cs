// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Collections.Generic;
using ServiceStack.Common.Web;
using System.Net;
using System;
using System.Data;

namespace SherpaDeskApi.ServiceInterface
{
    public class InvoicesService : Service
    {
        [Secure("tech")]
        public object Get(Invoices request)
        {
            DateTime startDate, endDate;
            endDate = request.end_date ?? DateTime.UtcNow;
            startDate = request.start_date ?? endDate.Date.AddDays(-365);
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            ApiUser hdUser = request.ApiUser;
            List<Models.Invoice> invoices = null;

            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(0, 0, 0), () =>
                {
                    if ("unbilled" == request.status)
                    {
                        invoices = Models.Invoices.GetInvoicesUnbilled(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, startDate, endDate, request.project, request.account);
                    }
                    else
                        invoices = Models.Invoices.GetInvoices(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, startDate, endDate, request.project, request.account);

                    if (invoices.Count == 1)
                    {
                        //invoices[0].AddDetails(hdUser.OrganizationId, hdUser.DepartmentId, startDate, endDate, false, request.time_logs, request.travel_logs, request.expenses, request.retainers, request.adjustments);
                        if (request.account.HasValue && request.account != 0 && "unbilled" == request.status)
                        {
                            bigWebApps.bigWebDesk.Data.Invoice.CalculateStartSopDates(hdUser.OrganizationId, hdUser.DepartmentId, out startDate, out endDate, request.account.Value, request.project ?? 0);
                            invoices[0].BeginDate = startDate;
                            invoices[0].EndDate = endDate;
                        }
                        invoices[0].recipients = AccountUsers.GetAccountUsers(hdUser.OrganizationId, hdUser.DepartmentId, invoices[0].AccountId);
                        return invoices;
                    }
                    return request.FilteredResult<Models.Invoice>(invoices);
                });
        }

        [Secure("tech")]
        public object Get(Invoice request)
        {
            ApiUser hdUser = request.ApiUser;
			return Models.Invoice.GetInvoice(hdUser.OrganizationId, hdUser.DepartmentId, request.id, request.is_detailed);
        }

        [Secure("tech")]
        public object Put(Invoice request)
        {
            ApiUser hdUser = request.ApiUser;
            if (request.action.StartsWith("send"))
            {
                Models.Invoice.SendInvoice(hdUser, request.id, request.recipients, request.is_pdf);
            }
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure("tech")]
        public object Post(Invoices request)
        {
            if (!request.account.HasValue || !request.project.HasValue)
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect account or project.");
            ApiUser hdUser = request.ApiUser;
            var invoice = Models.Invoice.CreateInvoice(hdUser, request.account.Value, request.project.Value, request.start_date, request.end_date, request.time_logs, request.travel_logs, request.expenses, request.retainers, request.adjustments, request.adjustments_note);
            Models.Invoice.SendInvoice(hdUser, invoice.Id.Value.ToString(), request.recipients);
            return invoice;
        }

    }
}
