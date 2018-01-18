// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "Article")]
    public class Article : ModelItemBase
    {
        public Article(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        public int Id
        {
            get { return Convert.ToInt32(Row["Id"]); }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "key")]
        public string PseudoId
        {
            get { return Row["PseudoId"].ToString(); }
            set { Row["PseudoId"] = value; }
        }

        [DataMember(Name = "subject")]
        public string Subject
        {
            get { return Row["subject"].ToString(); }
            set { Row["subject"] = value; }
        }

        [DataMember(Name = "category")]
        public string Category
        {
            get { return Row["Category"].ToString(); }
            set { Row["Category"] = value; }
        }

        [DataMember(Name = "lastupdated_date")]
        public DateTime? LastMod
        {
            get
            {
                if (Row.Table.Columns.Contains("LastMod") && !Row.IsNull("LastMod")) return (DateTime)Row["LastMod"];
                else return null;
            }
            set
            {
                if (Row.Table.Columns.Contains("LastMod")) Row["LastMod"] = value;
            }
        }

        public static Ticket GetArticle(Guid organizationId, int departmentId, int articleId, Guid InstanceId)
        {
            Ticket article = new Ticket(organizationId, departmentId, articleId, InstanceId);
            return article;
        }

    }
}
