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
    [DataContract(Name = "Unassigned_Queues")]
    public class UnassignedQueues : ModelItemCollectionGeneric<UnassignedQueue>
    {
        public UnassignedQueues(DataTable QueuesTable) : base(QueuesTable) { }

        public static List<UnassignedQueue> TechQueues(ApiUser hdUser)
        {
            Guid OrgId = hdUser.OrganizationId;
            int UserId = hdUser.UserId;
            int DeptId = hdUser.DepartmentId;

           //Instance_Config config = new Instance_Config(hdUser);
            if (hdUser.Role != bigWebApps.bigWebDesk.UserAuth.UserRole.Administrator && bigWebApps.bigWebDesk.Data.GlobalFilters.IsFilterEnabled(OrgId, DeptId, UserId, bigWebApps.bigWebDesk.Data.GlobalFilters.FilterState.LimitToAssignedTickets))
            {
                return new List<UnassignedQueue>();
            }
            string _query = @"SELECT J.id AS Id, Max(J.QueEmailAddress) AS QueEmailAddress, Max(L.FirstName+' '+L.LastName) AS FullName
            ,(SELECT COUNT(t.Id) FROM tbl_ticket t WHERE t.company_id = " + DeptId.ToString() + @" AND t.Technician_Id=J.id AND t.Status <> 'Closed') AS TicketsCount
            FROM tbl_LoginCompanyJunc J INNER JOIN tbl_Logins L ON J.login_id = L.id 
            LEFT OUTER JOIN tbl_ticket T ON T.company_id=" + DeptId.ToString() + @" AND T.Technician_Id=J.id 
            WHERE J.company_id = " + DeptId.ToString() + @" AND J.UserType_id = 4 
            GROUP BY J.id 
            ORDER BY FullName";
            DataTable queuesCount = bigWebApps.bigWebDesk.Data.UnassignedQueues.SelectByQuery(_query, OrgId);

            if (bigWebApps.bigWebDesk.Data.GlobalFilters.IsFilterEnabled(OrgId, DeptId, UserId, bigWebApps.bigWebDesk.Data.GlobalFilters.FilterState.EnabledGlobalFilters) && bigWebApps.bigWebDesk.Data.GlobalFilters.IsFilterEnabled(OrgId, DeptId, UserId, bigWebApps.bigWebDesk.Data.GlobalFilters.FilterType.UnassignedQueue))
            {
                DataTable queuesFilter = bigWebApps.bigWebDesk.Data.GlobalFilters.SelectFilterByType(OrgId, DeptId, UserId, bigWebApps.bigWebDesk.Data.GlobalFilters.FilterType.UnassignedQueue);

                foreach (DataRow queueFilter in queuesFilter.Rows)
                {
                    if (!queueFilter.IsNull("State") && !((bool)queueFilter["State"]))
                    {
                        foreach (DataRow queueCount in queuesCount.Rows)
                        {
                            if ((int)queueCount["Id"] == (int)queueFilter["ID"])
                            {
                                queuesCount.Rows.Remove(queueCount);
                                break;
                            }
                        }
                    }
                }
            }
            UnassignedQueues _queues = new UnassignedQueues(queuesCount);
            return _queues.List;
        }
    }
}
