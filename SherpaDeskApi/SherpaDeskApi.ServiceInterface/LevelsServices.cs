// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;

namespace SherpaDeskApi.ServiceInterface
{
    public class LevelsService : Service
    {
        [Secure()]
        public object Get(Levels request)
        {
            ApiUser hdUser = request.ApiUser;
            return Models.Levels.UserLevels(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId);
        }
    }
}
