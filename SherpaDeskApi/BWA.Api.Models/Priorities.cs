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
    [DataContract(Name = "Priorities")]
    public class Priorities : ModelItemCollectionGeneric<Priority>
    {
        public Priorities(DataTable PrioritiesTable) : base(PrioritiesTable) { }

        public static List<Priority> All(Guid OrgId, int DeptId, bool istech)
        {
            
            Priorities _priorities = new Priorities(bigWebApps.bigWebDesk.Data.Priorities.SelectAllFull(OrgId, DeptId));
            var prio = _priorities.List;
            if (!istech)
                prio = prio.FindAll(p => !p.is_tech_only);
            return prio;
        }
    }
}
