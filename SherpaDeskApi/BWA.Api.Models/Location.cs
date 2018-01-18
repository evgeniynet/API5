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
    [DataContract(Name = "Location")]
    public class Location : ModelItemBase
    {
        public Location(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        private List<Location> _subLocations = null;

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
            get { return Row.GetString("LocationName") ?? Row.GetString("Name"); }
            set
            {
                if (Row.Table.Columns.Contains("LocationName"))
                    Row["LocationName"] = value;
                else
                    Row["Name"] = value;
            }
        }


        [DataMember(Name = "description")]
        public string description
        {
            get { return Row.GetString("Description"); }
            set { Row["Description"] = value; }
        }

        [DataMember(Name = "type")]
        public string Type
        {
            get { return Row.GetString("TypeName"); }
            set { Row["TypeName"] = value; }
        }


        [DataMember(Name = "is_default")]
        public bool is_default
        {
            get { return Row.Get<bool>("IsDefault"); }
            set { Row["IsDefault"] = value; }
        }

        [DataMember(Name = "is_lastchild")]
        public bool is_last
        {
            get { return Row.Get<bool>("IsLastChildNode"); }
            set { Row["IsLastChildNode"] = value; }
        }

        [DataMember(Name = "parent_id")]
        public int ParentId
        {
            get { return Row.Get<int>("ParentId"); }
            set { Row["ParentId"] = value; }
        }

        [DataMember(Name = "sub")]
        public List<Location> SubLocations
        {
            get { return _subLocations; }
            set
            {
                _subLocations = value;
            }
        }

        [DataMember(Name = "type_id")]
        public int TypeId
        {
            get { return Row.Get<int>("TypeId"); }
            set { Row["TypeId"] = value; }
        }


        [DataMember(Name = "account_id")]
        public int AccountId
        {
            get { return Row.Get<int>("AccountId"); }
            set { Row["AccountId"] = value; }
        }

        [DataMember(Name = "auditor_id")]
        public int AuditorId
        {
            get { return Row.Get<int>("AuditorId"); }
            set { Row["AuditorId"] = value; }
        }

        [DataMember(Name = "audit_days")]
        public int AuditDays
        {
            get { return Row.Get<int>("AuditDays"); }
            set { Row["AuditDays"] = value; }
        }

        [DataMember(Name = "is_active")]
        public bool IsActive
        {
            get { return Row.Get<bool>("IsActive"); }
            set { Row["IsActive"] = value; }
        }

        public static Location GetLocation(Guid organizationId, int departmentId, int locationId)
        {
            var loc = bigWebApps.bigWebDesk.Data.Locations.SelectOne(organizationId, departmentId, locationId);
            if (loc == null)
                throw new ServiceStack.Common.Web.HttpError("Cannot find Location with id="+ locationId);
            return new Location(loc);
        }

        public static object AddLocation(ApiUser hdUser, string name, string description, int? parent_location_id, int? type_id, bool? is_active, int? auditor_id, int? audit_days)
        {
            return ChangeLocation( hdUser,  0, name,  description,  parent_location_id,  type_id,  is_active,  auditor_id,  audit_days);
        }

        public static object EditLocation(ApiUser hdUser, Location loc, int id, string name, string description, int? parent_location_id, int? type_id, bool? is_active, int? auditor_id, int? audit_days)
        {
            return ChangeLocation(hdUser, id, name, description, parent_location_id, type_id, is_active, auditor_id, audit_days, loc);
        }

        static object ChangeLocation(ApiUser hdUser, int id, string name, string description, int? parent_location_id, int? type_id, bool? is_active, int? auditor_id, int? audit_days, Models.Location old_loc = null)
        {
            Guid organizationId = hdUser.OrganizationId;
            int departmentId = hdUser.DepartmentId;
            int m_ID = id;
            bool _isNew = m_ID == 0 ? true : false;

            bool? Status = null;

            int accountId = hdUser.AccountId;

            Instance_Config instanceConfig = new Models.Instance_Config(hdUser);
           
            if (!_isNew)
            {
                auditor_id = !auditor_id.HasValue ? old_loc.AuditorId : auditor_id;
                auditor_id = !audit_days.HasValue ? old_loc.AuditDays : audit_days;
                accountId = accountId == 0 ? old_loc.AccountId : accountId;
                Status = is_active.HasValue ? is_active : old_loc.IsActive;
                parent_location_id = parent_location_id.HasValue ? parent_location_id : old_loc.ParentId;
                type_id = type_id.HasValue ? type_id : old_loc.TypeId; 
            }
            else
            {
                accountId = accountId == 0 ? -1 : accountId;
                Status = is_active ?? true;
                parent_location_id = parent_location_id ?? 0;
                type_id = type_id ?? 0;
            }

            /*if (!instanceConfig.EnableAssetAuditor || !instanceConfig.AssetTracking)
           {
               throw new HttpError("Assets is not enabled for this instance.");
           }*/

            if (bigWebApps.bigWebDesk.Data.Locations.IsNameExists(organizationId, departmentId, accountId, parent_location_id.Value, type_id, name, m_ID))
            {
                throw new ServiceStack.Common.Web.HttpError("The " + instanceConfig.Names.location.s + " name \"" + name + "\" is already exists. Please, enter another name.");
            }


            bool LocationAuditEnabled =  auditor_id.HasValue && auditor_id.Value > 0;
            int? AuditorId = auditor_id;
            int? AditPeriodDays = audit_days;
            if (LocationAuditEnabled)// && instanceConfig.)
            {
                AuditorId = auditor_id;
                AditPeriodDays = audit_days;
            }


            try
            {
                m_ID = bigWebApps.bigWebDesk.Data.Locations.Update(organizationId, departmentId, m_ID, parent_location_id.Value, accountId, type_id, name, Status,
                    description, false, LocationAuditEnabled, AuditorId, AditPeriodDays);
            }
            catch
            {
                throw new ServiceStack.Common.Web.HttpError("The " + instanceConfig.Names.location.s + " name \"" + name + "\" is already exists. Please, enter another name.");
            }
            /*if (_isNew)
            {
                DataTable dt = bigWebApps.bigWebDesk.Data.Locations.LocationAliases.SelectAll(organizationId, departmentId, m_ID);
                foreach (DataRow _row in dt.Rows) bigWebApps.bigWebDesk.Data.Locations.LocationAliases.Update(organizationId, departmentId, 0, m_ID, _row["LocationAliasName"].ToString());
            }
            */
            return m_ID;
        }
    }
}
