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
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "ResolutionCategories")]
    public class ResolutionCategories : ModelItemCollectionGeneric<ResolutionCategory>
    {
        public ResolutionCategories(DataTable ResolutionCategoriesTable) : base(ResolutionCategoriesTable) { }

        public static List<ResolutionCategory> GetResolutionCategories(Guid OrgId, int DeptId)
        {
            return (new ResolutionCategories(bigWebApps.bigWebDesk.Data.ResolutionCategories.SelectAll(OrgId, DeptId))).List;
        }
            
    }
}
