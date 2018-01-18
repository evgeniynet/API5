// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Collections.Generic;

namespace SherpaDeskApi.ServiceInterface
{
    public class ProjectsService : Service
    {
        [Secure()]
        public object Get(Projects request)
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

			if (string.IsNullOrWhiteSpace (query ["search"])) {
				return base.RequestContext.ToOptimizedResultUsingCache (base.Cache, string.Format ("urn:{0}:{1}{2}", base.Request.GetBasicAuth (), base.Request.PathInfo.Substring (1), (query.Count > 0 ? ":" + query.ToString () : "")),
					new System.TimeSpan (2, 0, 0), () => {
					return GetProjects (request);
				});
			}
            else
                return GetProjects(request);
        }

        private object GetProjects(Projects request)
        {
            ApiUser hdUser = request.ApiUser;
            return Models.Projects.GetProjectDetails(hdUser.OrganizationId, hdUser.DepartmentId, request.id);
        }

        [Secure()]
        public object Get(Projects_List request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
			return request.FilteredResult<Models.Project>(Models.Projects.GetProjects(hdUser.OrganizationId, hdUser.DepartmentId, request.account, hdUser.IsTechAdmin ? request.tech : hdUser.UserId, instanceConfig.AccountManager, request.is_with_statistics ?? true));
        }
    }
}
