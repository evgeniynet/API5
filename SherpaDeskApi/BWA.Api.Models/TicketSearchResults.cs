// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;

namespace BWA.Api.Models
{
    public class TicketSearchResults : ModelItemCollectionGeneric<TicketSearchResult>
    {
        public TicketSearchResults(DataTable TicketSearchResultsTable) : base(TicketSearchResultsTable) { }

        public static List<TicketSearchResult> GetTickets(Guid orgId, int deptId, string account, string project)
        {
            int account_id = -1;
            if (!string.IsNullOrEmpty(account))
            {
                int.TryParse(account, out account_id);
            }
            int project_id = -1;
            if (!string.IsNullOrEmpty(project))
            {
                int.TryParse(project, out project_id);
            }
            TicketSearchResults _tickets = new TicketSearchResults(bigWebApps.bigWebDesk.Data.Tickets.SelectTicketsSearch(orgId, deptId, 0, account_id, project_id));
            return _tickets.ToList();
        }
    }
}
