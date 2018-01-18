// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "ticket_log_records")]
    public class TicketLogRecords : ModelItemCollectionGeneric<TicketLogRecord>
    {
        public TicketLogRecords(DataTable RecordsTable) : base(RecordsTable) { }

        public static List<TicketLogRecord> TicketLog(Guid OrgId, int DeptId, int TktId, string TktPseudoId)
        {
            TicketLogRecords _recs = new TicketLogRecords(bigWebApps.bigWebDesk.Data.Tickets.SelectTicketLog(OrgId, DeptId, TktId));
            var recs = _recs.List;
            foreach (TicketLogRecord _rec in recs)
            {
                _rec.TktPseudoId = TktPseudoId;
            }
            return recs;
        }
    }
}
