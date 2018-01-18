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
   /// Summary description for Level
   /// </summary>
    [DataContract(Name = "Level")]
    public class Level : ModelItemBase
    {
        public Level(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["ID"]); }
            set { Row["ID"] = value; }
        }

        [DataMember(Name="name")]
        public string Name
        {
            get { return Row["LevelName"].ToString(); }
            set { Row["LevelName"] = value; }
        }

        [DataMember(Name="description")]
        public string Description
        {
            get { return Row["Description"].ToString(); }
            set { Row["Description"] = value; }
        }
    }
}
