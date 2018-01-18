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
    [DataContract(Name = "fb_data")]
    public class FBModel
    {
        public static void UpdateData(ApiUser User, int userID, int fbStaffID, int accountID, int fbClientId, int projectID,
            int fbProjectID, int taskTypeID, int fbTaskTypeID, string category_id, int fb_category_id)
        {
            FreshBooks.UpdateData(User.OrganizationId, User.DepartmentId, userID, fbStaffID, accountID, fbClientId, projectID, fbProjectID, taskTypeID, 
                fbTaskTypeID, category_id, fb_category_id);
        }
    }
}
