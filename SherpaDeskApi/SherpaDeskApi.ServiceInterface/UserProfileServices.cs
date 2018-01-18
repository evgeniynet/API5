// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using bigWebApps.bigWebDesk;
using System.Net;
using Micajah.Common.Bll.Providers;

namespace SherpaDeskApi.ServiceInterface
{
    public class ProfileService : Service
    {
        [Secure("tech")]
        public object Get(Profile request)
        {
			if (request.id <= 0)
                return new HttpError("User Not Found. Id is missing");
            ApiUser hdUser = request.ApiUser;
            UserProfile user = UserProfile.GetProfile(hdUser.OrganizationId, hdUser.DepartmentId, request.id);
            if (user == null)
                return new HttpError("User Not Found. Id is incorrect");
            return user;
        }

        [Secure("tech")]
        public object Put(Profile request)
        {
            if (request.id <= 0)
                return new HttpError("User Not Found. Id is missing");
            ApiUser hdUser = request.ApiUser;
            UserProfile user = UserProfile.GetProfile(hdUser.OrganizationId, hdUser.DepartmentId, request.id);
            if (user == null)
                return new HttpError("User Not Found. Id is incorrect");
            return UserProfile.UpdateProfile(hdUser.OrganizationId, hdUser.DepartmentId, request.id, request.phone, request.mobile_phone, request.skype);
        }
    }
}
