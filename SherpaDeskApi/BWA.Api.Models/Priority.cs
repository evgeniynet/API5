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
   /// Summary description for Priority
   /// </summary>
    [DataContract(Name = "Priority")]
    public class Priority : ModelItemBase
    {
        public Priority(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["ID"]); }
            set { Row["ID"] = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row["tintPriority"].ToString() + "-" + Row["Name"].ToString(); }
            set { Row["Name"] = value; }
        }

        [DataMember(Name = "priority_level")]
        public byte PriorityLevel
        {
            get { return Row.Get<byte>("tintPriority"); }
            set { Row["tintPriority"] = value; }
        }

        [DataMember(Name = "description")]
        public string Description
        {
            get { return Row.GetString("Description"); }
            set { Row["Description"] = value; }
        }

        [DataMember(Name = "is_tech_only")]
        public bool is_tech_only
        {
            get { return Row.Get<bool>("btRstrctUsr"); }
            set { Row["btRstrctUsr"] = value; }
        }

    }
}
