// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Collections.Generic;
using System.Net;

namespace SherpaDeskApi.ServiceInterface
{
    public class LocationsService : Service
    {
        [Secure()]
        public object Get(Locations request)
        {
			var query = base.Request.QueryString;
			string search = query["search"];
			if (!string.IsNullOrWhiteSpace (search)) {
				search = search.Trim ();
				if (search == "*"){
                    search = "";
                    query = System.Web.HttpUtility.ParseQueryString(query.ToString());
					query.Remove ("search");
				}
                request.search = search;
			}

			if (string.IsNullOrWhiteSpace (query["search"])  && string.IsNullOrWhiteSpace(query["name"]))
                return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (query.Count > 0 ? ":" + query.ToString() : "")),
                    new System.TimeSpan(2, 0, 0), () =>
                    {
                        return GetLocations(request);
                    });
            else
                return GetLocations(request);
        }

        private object GetLocations(Locations request)
        {
            ApiUser hdUser = request.ApiUser;
           //if (request.account.HasValue)
            string search = string.IsNullOrWhiteSpace(request.name) ? request.search : request.name;
            if (request.is_tree ?? false)
            {
                return Models.Locations.GetLocationsTree(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, request.account ?? 0, request.parent_id <= 0 ? 0 : request.parent_id);
            }
            return request.FilteredResult<Models.Location>(Models.Locations.GetAccountLocations(hdUser.OrganizationId, hdUser.DepartmentId, request.account ?? -1, request.parent_id <= 0 ? 0 : request.parent_id, search, request.is_active ?? true));
        }

        [Secure()]
        public object Get(Location request)
        {
            ApiUser hdUser = request.ApiUser;
            return Models.Location.GetLocation(hdUser.OrganizationId, hdUser.DepartmentId, request.location_id);
        }

        [Secure()]
        public object Post(LocationsPost request)
        {
            ApiUser hdUser = request.ApiUser;
            return Models.Location.AddLocation(hdUser, request.name, request.description, request.parent_location_id, request.type_id, request.is_active, request.auditor_id, request.audit_days);
        }

        [Secure()]
        public object Put(LocationEdit request)
        {
            ApiUser hdUser = request.ApiUser;
            Models.Location loc = Models.Location.GetLocation(hdUser.OrganizationId, hdUser.DepartmentId, request.location_id);
            return Models.Location.EditLocation(hdUser, loc, request.location_id, request.name, request.description, request.parent_location_id, request.type_id, request.is_active, request.auditor_id, request.audit_days);
        }

        [Secure()]
        public object Get(LocationTypes request)
        {
            ApiUser hdUser = request.ApiUser;
            return Models.LocationTypes.GetLocationTypes(hdUser.OrganizationId, hdUser.DepartmentId);
        }

    }
}
