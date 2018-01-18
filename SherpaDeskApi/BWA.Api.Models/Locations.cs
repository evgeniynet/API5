// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "Locations")]
    public class Locations : ModelItemCollectionGeneric<Location>
    {
        public Locations(DataTable LocationsTable) : base(LocationsTable) { }

        public static List<Location> GetAccountLocations(Guid organizationId, int departmentId, int accountId, int parent_id, string search, bool isActive)
        {
            Locations _accountLocations = new Locations(bigWebApps.bigWebDesk.Data.Locations.SelectLocationsWithTicketCount(organizationId, departmentId, accountId, parent_id, search, isActive ? bigWebApps.bigWebDesk.Data.InactiveStatus.Active : bigWebApps.bigWebDesk.Data.InactiveStatus.Inactive));
            return _accountLocations.List;
        }

        public static List<Location> GetLocationsTree(Guid organizationId, int departmentId, int userId, int accountId, int parent_id)
        {
            Locations _locations = GetLocations(organizationId, departmentId, userId, accountId, parent_id);
            /*if (classId > 0)
            {
                return _locations.List;
            }*/
            return MakeTreeFromFlatList(_locations.List, organizationId, departmentId, userId, accountId, 0);
        }

        static Locations GetLocations(Guid organizationId, int departmentId, int userId, int accountId, int parent_id)
        {
            if (accountId < 0) accountId = 0;
            DataTable tree;
            if (bigWebApps.bigWebDesk.Data.GlobalFilters.IsFilterEnabled(organizationId, departmentId, userId, bigWebApps.bigWebDesk.Data.GlobalFilters.FilterState.EnabledGlobalFilters))
                tree = bigWebApps.bigWebDesk.Data.Locations.SelectTreeActiveFiltered(departmentId, userId, accountId, parent_id, string.Empty, organizationId);
            else
                tree = bigWebApps.bigWebDesk.Data.Locations.SelectTreeActive(departmentId, accountId, parent_id, string.Empty, organizationId);

            return new Locations(tree);
        }

        private static List<Location> MakeTreeFromFlatList(List<Location> rootLocations, Guid organizationId, int departmentId, int userId, int accountId, int parent_id)
        {
           //var dic = flatList.ToDictionary(n => n.Id, n => n);
           //var rootLocations = new List<Location>();
            foreach (var loc in rootLocations)
            {
                if (!loc.is_last)
                {
                    if (loc.SubLocations == null)
                        loc.SubLocations = MakeTreeFromFlatList(GetLocations(organizationId, departmentId, userId, accountId, loc.Id).List, organizationId, departmentId, userId, accountId, loc.Id);
                   //parent.SubLocations.Add(loc);
                }
            }
            return rootLocations;
        }
    }
}
