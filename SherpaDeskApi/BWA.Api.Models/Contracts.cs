// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;
using ServiceStack.Common.Web;

namespace BWA.Api.Models
{
    [DataContract(Name = "Contracts")]
    public class Contracts : ModelItemCollectionGeneric<Contract>
    {
        public Contracts(DataTable ContractsTable) : base(ContractsTable) { }

        public static List<Contract> GetContracts(Guid organizationId, int departmentId, int accountId, DateTime date, int projectId)
        {
            Contracts _Contracts = new Contracts(bigWebApps.bigWebDesk.Data.Accounts.SelectAccountContracts(organizationId, departmentId, accountId, true, date, projectId));
            return _Contracts.List;
        }

        public static string CheckAccountContract(ApiUser hdUser, int accountId, int contractId, DateTime date, int ticketTimeID, int projectTimeID, int projectId)
        {
            if (contractId > 0)
            {
                DataTable acTbl = bigWebApps.bigWebDesk.Data.Accounts.SelectAccountContracts(hdUser.OrganizationId, hdUser.DepartmentId, accountId, true, date, projectId, contractId);
                if (acTbl != null && acTbl.Rows.Count == 1)
                {
                    return acTbl.Rows[0]["Name"].ToString();
                }
                if (ticketTimeID > 0)
                {
                    DataRow row = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketTimeByID(hdUser.OrganizationId, hdUser.DepartmentId, ticketTimeID);
                    if (row != null)
                    {
                        Ticket_Time_Log timeEntry = new Ticket_Time_Log(row);
                        if (timeEntry.ContractID == contractId)
                        {
                            return row["ContractName"].ToString();
                        }
                    }
                }
                if (projectTimeID > 0)
                {
                    DataRow row = bigWebApps.bigWebDesk.Data.Project.SelectProjectTimeByID(hdUser.OrganizationId, hdUser.DepartmentId, projectTimeID);
                    if (row != null)
                    {
                        ProjectTimeLog timeEntry = new ProjectTimeLog(row);
                        if (timeEntry.ContractID == contractId)
                        {
                            return row["ContractName"].ToString();
                        }
                    }
                }
                throw new HttpError("Incorrect PrePaid Pack Id.");
            }
            return string.Empty;
        }
    }
}
