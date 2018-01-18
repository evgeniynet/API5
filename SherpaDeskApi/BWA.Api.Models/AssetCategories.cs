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

    [DataContract(Name = "AssetCategories")]
    public class AssetCategories : ModelItemCollectionGeneric<AssetCategory>
    {
        public AssetCategories(DataTable AssetCategoriesTable) : base(AssetCategoriesTable) { }

        public static List<AssetCategory> GetAssetCategories(Guid OrgId, int DeptId)
        {
            return (new AssetCategories(bigWebApps.bigWebDesk.Data.AssetCategories.SelectAssetCategories(OrgId, DeptId))).List;
        }
    }

    [DataContract(Name = "AssetStatuses")]
    public class AssetStatuses : ModelItemCollectionGeneric<AssetCategory>
    {
        public AssetStatuses(DataTable AssetTypesTable) : base(AssetTypesTable) { }

        public static List<AssetCategory> GetAssetStatuses(Guid OrgId, int DeptId, bool? is_active)
        {
            var statuses = (new AssetCategories(bigWebApps.bigWebDesk.Data.Asset.SelectCompanyAssetStatuses(DeptId, OrgId))).List;
            if (is_active.HasValue)
                return statuses.FindAll(s => s.is_checked == is_active);
            return statuses;
        }
    }

    [DataContract(Name = "AssetTypes")]
    public class AssetTypes : ModelItemCollectionGeneric<AssetCategory>
    {
        public AssetTypes(DataTable AssetTypesTable) : base(AssetTypesTable) { }

        public static List<AssetCategory> GetAssetTypes(Guid OrgId, int DeptId, int category_id)
        {
            return (new AssetCategories(bigWebApps.bigWebDesk.Data.AssetCategories.SelectAssetTypes(OrgId, DeptId, category_id))).List;
        }
    }


    [DataContract(Name = "AssetMakes")]
    public class AssetMakes : ModelItemCollectionGeneric<AssetCategory>
    {
        public AssetMakes(DataTable AssetMakesTable) : base(AssetMakesTable) { }

        public static List<AssetCategory> GetAssetMakes(Guid OrgId, int DeptId, int type_id)
        {
            return (new AssetCategories(bigWebApps.bigWebDesk.Data.AssetCategories.SelectAssetMakes(OrgId, DeptId, type_id))).List;
        }
    }


    [DataContract(Name = "AssetModels")]
    public class AssetModels : ModelItemCollectionGeneric<AssetCategory>
    {
        public AssetModels(DataTable AssetModelsTable) : base(AssetModelsTable) { }

        public static List<AssetCategory> GetAssetModels(Guid OrgId, int DeptId, int make_id)
        {
            return (new AssetCategories(bigWebApps.bigWebDesk.Data.AssetCategories.SelectAssetModels(OrgId, DeptId, make_id))).List;
        }
    }

   /// <summary>
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "AssetCategory")]
    public class AssetCategory : ModelItemBase
    {
        public AssetCategory(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return Row.GetString("Name") ?? Row.GetString("Make") ?? Row.GetString("Model") ?? Row.GetString("vchStatus"); }
            set { Row["Name"] = value; }
        }
        
        [DataMember(Name = "id")]
        public int Id
        {
            get { return Row.Get<int>("Id"); }
            set { Row["Id"] = value; }
        }


        [DataMember(Name = "count")]
        public int Count
        {
            get { return Row.Get<int>("count"); }
            set { Row["count"] = value; }
        }

        [DataMember(Name = "is_active")]
        public bool? is_active
        {
            get {
                bool? non = Row.Get<bool?>("NonActive");
                if (non.HasValue)
                    return !non.Value;
                return non; }
            set { Row["NonActive"] = value; }
        }

        public bool is_checked
        {
            get
            {
                return Row.Get<int>("AssetStatusIsChecked") == 1;
            }
            set { Row["AssetStatusIsChecked"] = value; }
        }
    }
}
