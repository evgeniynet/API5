// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System;
using System.Net;

namespace SherpaDeskApi.ServiceInterface
{
    public class Travel_LogsService : Service
    {
        [Secure()]
        public object Post(Travel_Logs request)
        {
            ApiUser hdUser = request.ApiUser;
            int ticketId = string.IsNullOrWhiteSpace(request.ticket_key) ? 0 : Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket_key);
            bigWebApps.bigWebDesk.Data.TicketTravelCosts.Insert(hdUser.OrganizationId, hdUser.DepartmentId, ticketId, request.account, request.start_location, request.end_location, request.distance, request.rate, request.date, true, request.note);
            return new HttpResult(HttpStatusCode.OK);
        }

        [Secure()]
        public object Delete(Travel_Log request)
        {
            ApiUser hdUser = request.ApiUser;
            bigWebApps.bigWebDesk.Data.TicketTravelCosts.Delete(hdUser.OrganizationId, hdUser.DepartmentId, request.id);
            return new HttpResult(HttpStatusCode.OK);
        }

        [Secure()]
        public object Put(Travel_Put request)
        {
            ApiUser hdUser = request.ApiUser;
            try {
                bigWebApps.bigWebDesk.Data.TicketTravelCosts.HideFromInvoiceTicketTravelCost(hdUser.OrganizationId, hdUser.DepartmentId, request.id, request.hidden_from_invoice);
                return new HttpResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }
    }
}
