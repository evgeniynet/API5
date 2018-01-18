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
   /// Summary description for Levels
   /// </summary>
    [DataContract(Name = "Levels")]
    public class Levels : ModelItemCollectionGeneric<Level>
    {
        public Levels(DataTable LevelsTable) : base(LevelsTable) { }

        public static List<Level> UserLevels(Guid OrgId, int DeptId, int UserId)
        {
            Levels _levels = new Levels(bigWebApps.bigWebDesk.Data.Levels.SelectAll(OrgId, DeptId, UserId));
            return _levels.List;
        }
    }
}
