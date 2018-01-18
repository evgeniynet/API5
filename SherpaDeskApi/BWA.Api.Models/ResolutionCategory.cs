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
    [DataContract(Name = "ResolutionCategory")]
    public class ResolutionCategory : ModelItemBase
    {
        public ResolutionCategory(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row.GetString("Name"); }
            set { Row["Name"] = value; }
        }
        
        [DataMember(Name = "id")]
        public int Id
        {
            get { return Row.Get<int>("Id"); }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "is_resolved")]
        public bool isResolved
        {
            get { return Row.Get<bool>("btResolved"); }
            set { Row["btResolved"] = value; }
        }

        [DataMember(Name = "is_active")]
        public bool IsActive
        {
            get { return !Row.Get<bool>("btInactive"); }
            set { Row["btInactive"] = !value; }
        }
    }
}
