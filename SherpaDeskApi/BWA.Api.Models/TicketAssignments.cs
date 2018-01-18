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
    [DataContract(Name = "Ticket_Assinments")]
    public class TicketAssignments : ModelItemCollectionGeneric<TicketAssignee>
    {
        public TicketAssignments(DataTable AssigneeTable) : base (AssigneeTable){}

        public static List<TicketAssignee> TicketUsers(Guid OrgId, int DeptId, int TktId)
        {
            TicketAssignments _ta = new TicketAssignments(bigWebApps.bigWebDesk.Data.Ticket.SelectTicketAssignees(OrgId, DeptId, TktId, bigWebApps.bigWebDesk.Data.Ticket.TicketAssignmentType.User, false, true));
            return _ta.List;
        }

        public static List<TicketAssignee> TicketTechnicians(Guid OrgId, int DeptId, int TktId)
        {
            TicketAssignments _ta = new TicketAssignments(bigWebApps.bigWebDesk.Data.Ticket.SelectTicketAssignees(OrgId, DeptId, TktId, bigWebApps.bigWebDesk.Data.Ticket.TicketAssignmentType.Technician, false, true));
            return _ta.List;
        }
    }
}
