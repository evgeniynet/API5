// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Collections.Generic;

namespace SherpaDeskApi.ServiceInterface
{
    public class ArticlesService : Service
    {
        [Secure()]
        public object Get(Article request)
        {
            ApiUser hdUser = request.ApiUser;
            return  Models.Article.GetArticle(hdUser.OrganizationId, hdUser.DepartmentId, Models.Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key), hdUser.InstanceId);
        }

        [Secure()]
        public object Get(Articles request)
        {
            ApiUser hdUser = request.ApiUser;
            return request.FilteredResult<Models.Article>(Models.Articles.GetArticles(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId));
        }
    }
}
