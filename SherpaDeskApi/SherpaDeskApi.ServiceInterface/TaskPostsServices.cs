// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;

namespace SherpaDeskApi.ServiceInterface
{
    public class TaskPostsService : Service
    {
        [Secure()]
        public object Get(Posts request)
        {
            ApiUser hdUser = request.ApiUser;
           //v1
            string ticket_key = request.key;
           //v2 
            if (!string.IsNullOrEmpty(request.ticket))
                ticket_key = request.ticket;
            int ticket_id = Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, ticket_key);
            int test_ticket_id = 0;
            if (int.TryParse(ticket_key, out test_ticket_id))
                ticket_key = Ticket.GetPseudoId(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id);

            return request.FilteredResult<TicketLogRecord>(TicketLogRecords.TicketLog(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, ticket_key));
        }

        [Secure()]
        public object Post(Posts request)
        {
            ApiUser hdUser = request.ApiUser;
           //v1
            string ticket_key = request.key;
           //v2 
            if (!string.IsNullOrEmpty(request.ticket))
                ticket_key = request.ticket;
            request.note_text = request.note_text ?? "";
            int ticketId = Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, ticket_key);
            bigWebApps.bigWebDesk.Data.Ticket _tktNew = null;
            Ticket.Response(hdUser, ticketId, request.note_text, request.is_waiting_on_response, ref _tktNew, request.files, request.user_id);
            if (_tktNew != null)
                return TicketLogRecords.TicketLog(hdUser.OrganizationId, hdUser.DepartmentId, _tktNew.ID, _tktNew.PseudoID);
            return new HttpResult("", HttpStatusCode.OK);
        }
    }
}
