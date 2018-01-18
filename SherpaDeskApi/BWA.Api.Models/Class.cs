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
    [DataContract(Name = "Class")]
    public class Class : ModelItemBase
    {
        public Class(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        private List<Class> _subClasses = null;

        [DataMember(Name = "name")]
        public string Name
        {
            get { return HttpUtility.HtmlDecode(Row.GetString("Name")); }
            set { Row["Name"] = value; }
        }
        
        [DataMember(Name = "id")]
        public int Id
        {
            get { return Row.Get<int>("Id"); }
            set { Row["Id"] = value; }
        }
        
        [DataMember(Name = "parent_id")]
        public int ParentId
        {
            get { return Row.Get<int>("ParentId"); }
            set { Row["ParentId"] = value; }
        }

        [DataMember(Name = "hierarchy_level")]
        public int HierarchyLevel
        {
            get { return Row.Get<int>("Level"); }
            set { Row["Level"] = value; }
        }

        [DataMember(Name = "sub")]
        public List<Class> SubClasses
        {
            get { return _subClasses; }
            set
            {
                _subClasses = value;
            }
        }

        [DataMember(Name = "is_lastchild")]
        public bool IsLastChild
        {
            get { return Row.Get<bool>("IsLastChild"); }
            set { Row["IsLastChild"] = value; }
        }

        [DataMember(Name = "is_restrict_to_techs")]
        public bool IsRestrictToTechs
        {
            get { return Row.Get<bool>("bitRestrictToTechs"); }
            set { Row["bitRestrictToTechs"] = value; }
        }

        [DataMember(Name = "is_active")]
        public bool IsActive
        {
            get { return !Row.Get<bool>("btInactive"); }
            set { Row["btInactive"] = !value; }
        }

        [DataMember(Name = "priority_id")]
        public int PriorityId
        {
            get 
            { 
                return Row.Get<int>("intPriorityId");
            }
            set { Row["intPriorityId"] = value; }
        }

        [DataMember(Name = "level_override")]
        public byte LevelOverride
        {
            get
            {
                return Row.Get<byte>("tintLevelOverride");
            }
            set { Row["tintLevelOverride"] = value; }
        }
    }
}
