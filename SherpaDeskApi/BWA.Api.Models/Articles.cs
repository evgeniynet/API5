// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "Articles")]
    public class Articles : ModelItemCollectionGeneric<Article>
    {
        public Articles(DataTable ArticlesTable) : base(ArticlesTable) { }

        public static List<Article> GetArticles(Guid organizationId, int departmentId, int userId)
        {
            Articles _articles = new Articles(bigWebApps.bigWebDesk.Data.KnowledgeBase.BrowseKnowledgeBase(organizationId, departmentId, false, 0, 0));
            return _articles.List;
        }
    }
}
