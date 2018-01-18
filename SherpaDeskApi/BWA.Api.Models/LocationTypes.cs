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
    [DataContract(Name = "LocationType")]
    public class LocationType : ModelItemBase
    {
        public LocationType(DataRow row) : base(row)
        {
            if (row == null)
                throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return Row.Get<int?>("LocationId") ?? Row.Get<int>("Id"); }
            set
            {
                if (Row.Table.Columns.Contains("LocationId"))
                    Row["LocationId"] = value;
                else
                    Row["Id"] = value;
            }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row.GetString("Name"); }
            set
            {
               Row["Name"] = value;
            }
        }

        [DataMember(Name = "hierarchy_level")]
        public int HierarchyLevel
        {
            get { return Row.Get<int>("HierarchyLevel"); }
            set { Row["HierarchyLevel"] = value; }
        }


        [DataMember(Name = "is_deleted")]
        public bool is_deleted
        {
            get { return Row.Get<bool>("Deleted"); }
            set { Row["Deleted"] = value; }
        }
    }
    /// <summary>
    /// Summary description for TicketLogRecords
    /// </summary>
    [DataContract(Name = "LocationTypes")]
    public class LocationTypes : ModelItemCollectionGeneric<LocationType>
    {
        public LocationTypes(DataTable LocationsTable) : base(LocationsTable) { }

        public static List<LocationType> GetLocationTypes(Guid organizationId, int departmentId)
        {
            return new LocationTypes(bigWebApps.bigWebDesk.Data.Locations.LocationTypes.SelectAll(organizationId, departmentId)).List;
        }
    }
}
