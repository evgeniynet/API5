// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk.Data;
using System.Net;
using ServiceStack.Common.Web;

namespace BWA.Api.Models
{
    [DataContract(Name = "qb_data")]
    public class QBModel
    {
        public static void UpdateData(ApiUser User, int userID, int qb_employee_id, int accountID, int qb_customer_id,
            int taskTypeID, int qb_service_id, int qb_vendor_id)
        {
            QuickBooks.UpdateData(User.OrganizationId, User.DepartmentId, userID, qb_employee_id, accountID, qb_customer_id, taskTypeID,
                qb_service_id, qb_vendor_id);
        }
    }
}
