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
    [DataContract(Name = "Classes")]
    public class Classes : ModelItemCollectionGeneric<Class>
    {
        public Classes(DataTable ClassesTable) : base(ClassesTable) { }

        public static List<Class> UserClasses(Guid OrgId, int DeptId, int UserId, int classId, bool is_global_filters_enabled, bool? get_all_active_for_user)
        {
            DataTable dtClasses;
            if (get_all_active_for_user.HasValue && get_all_active_for_user.Value)
            {
                dtClasses = bigWebApps.bigWebDesk.Data.Classes.SelectAllActiveForUsers(OrgId, DeptId, classId);
            }
            else
            {
                if (is_global_filters_enabled)
                {
                    dtClasses = bigWebApps.bigWebDesk.Data.Classes.SelectByInactiveStatus(OrgId, DeptId, UserId, bigWebApps.bigWebDesk.Data.InactiveStatus.Active, classId);
                }
                else
                {
                    dtClasses = bigWebApps.bigWebDesk.Data.Classes.SelectByInactiveStatus(OrgId, DeptId, bigWebApps.bigWebDesk.Data.InactiveStatus.Active, classId);
                }
            }
            Classes _classes = new Classes(dtClasses);
            if (classId > 0)
            {
                return _classes.List;
            }
            return MakeTreeFromFlatList(_classes.List);
        }

        public static List<Class> ParentClasses(Guid OrgId, int DeptId, int ClassId)
        {
            DataTable dtClasses = bigWebApps.bigWebDesk.Data.Classes.SelectAllParent(OrgId, DeptId, ClassId);
            Classes _classes = new Classes(dtClasses);
            return _classes.List;
        }

        private static List<Class> MakeTreeFromFlatList(IEnumerable<Class> flatList)
        {
            var dic = flatList.ToDictionary(n => n.Id, n => n);
            var rootClasss = new List<Class>();
            foreach (var clas in flatList)
            {
                if (clas.ParentId > 0)
                {
                    Class parent = dic[clas.ParentId];
                    if ( parent.SubClasses == null)
                        parent.SubClasses = new List<Class>();
                    parent.SubClasses.Add(clas);
                }
                else
                {
                    rootClasss.Add(clas);
                }
            }
            return rootClasss;
        }
    }
}
