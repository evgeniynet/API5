// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;

namespace SherpaDeskApi.ServiceInterface
{
    public class PrioritiesService : Service
    {
        [Secure()]
        public object Get(Priorities request)
        {
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1)),
                    new System.TimeSpan(2, 0, 0), () =>
                    {
                        return GetPriorities(request);
                    });   
        }

        public object GetPriorities (Priorities request)
        {
            ApiUser hdUser = request.ApiUser;
            return Models.Priorities.All(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.IsTechAdmin);
        }
    }
}
