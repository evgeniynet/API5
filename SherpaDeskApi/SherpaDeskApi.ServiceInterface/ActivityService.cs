// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Collections.Generic;

namespace SherpaDeskApi.ServiceInterface
{
    public class ActivityService : Service
    {
        [Secure()]
        public object Any(Activity request)
        {
            ApiUser hdUser = request.ApiUser;
           //v1
            if (request.user_id > 0)
                request.user = hdUser.IsTechAdmin ? request.user_id : hdUser.UserId;
           //v2
            int userId = hdUser.IsTechAdmin && request.user > 0 ? request.user : hdUser.UserId;
            return request.FilteredResult<Models.ActivityLog>(Models.ActivityLogs.GetAllActivity(hdUser.OrganizationId, hdUser.DepartmentId, userId, request.sort_order, request.sort_by));
        }
    }
}
