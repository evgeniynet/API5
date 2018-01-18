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
    [DataContract(Name = "ExpenseCategory")]
    public class ExpenseCategory : ModelItemBase
    {
        public ExpenseCategory(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        public int Id
        {
            get;
            set;
        }

        [DataMember(Name = "category_id")]
        public string CategoryID
        {
            get
            {
                return Row["Id"].ToString();
            }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row["Name"].ToString(); }
            set { Row["Name"] = value; }
        }

        [DataMember(Name = "fb_category_id")]
        public int FBCategoryID
        {
            get
            {
                if (!Row.IsNull("FBCategoryID"))
                {
                    return Convert.ToInt32(Row["FBCategoryID"]);
                }
                return 0;
            }
            set { Row["FBCategoryID"] = value; }
        }

        [DataMember(Name = "qb_service_id")]
        public int QBServiceID
        {
            get
            {
                if (!Row.IsNull("QBServiceID"))
                {
                    return Convert.ToInt32(Row["QBServiceID"]);
                }
                return 0;
            }
            set { Row["QBServiceID"] = value; }
        }
    }
}
