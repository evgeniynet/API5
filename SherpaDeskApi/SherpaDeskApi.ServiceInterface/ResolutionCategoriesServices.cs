// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;

namespace SherpaDeskApi.ServiceInterface
{
    public class ResolutionCategoriesService : Service
    {
        [Secure()]
        public object Get(ResolutionCategories request)
        {
            ApiUser hdUser = request.ApiUser;
            return Models.ResolutionCategories.GetResolutionCategories(hdUser.OrganizationId, hdUser.DepartmentId);
        }
    }
}
