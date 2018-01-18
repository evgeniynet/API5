// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;

namespace SherpaDeskApi.ServiceInterface
{
    public class AssetsService : Service
    {
        [Secure()]
        public object Get(GetAssets request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckAssets(hdUser);

            var query = base.Request.QueryString;
            string search = query["search"];
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                if (search == "*")
                {
                    search = "";
                    query = System.Web.HttpUtility.ParseQueryString(query.ToString());
                    query.Remove("search");
                }
                request.search = search;
            }

            return GetAssets(request);

            if (string.IsNullOrWhiteSpace(query["search"]))
            {
                return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(0, 5, 0), () =>
                {
                    return GetAssets(request);
                });
            }
            else
                return GetAssets(request);
        }

        public object GetAssets(GetAssets request)
        {
            return request.QueryResult<Asset>(Models.Assets.GetAssets(request.ApiUser, request.search, request.filter, request.user_id, request.owner_id, request.account_id, request.location_id, request.is_active, request.status, request.limit, request.page));
        }

        [Secure()]
        public object Get(GetAsset request)
        {
            if (request.id < 1)
            {
                throw new HttpError(HttpStatusCode.NotFound, "Assets not found. Please check Id");
            }
            ApiUser hdUser = request.ApiUser;
            CheckAssets(hdUser);

            var query = base.Request.QueryString;
            
                return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(0, 1, 0), () =>
                {
                    return GetAsset(request);
                });
            
        }

        public object GetAsset(GetAsset request)
        {
            CheckAssets(request.ApiUser);
            return Models.Assets.GetAsset(request.ApiUser, request.id);
        }

        public object Post(PostAsset request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config config = CheckAssets(hdUser);
            int account_id = request.account_id;
            if (request.location_id > 0 && config.LocationTracking && config.AccountManager)
            {
                string account_name = "";
                if (account_id == 0)
                {
                    account_id = hdUser.AccountId;
                    account_name = hdUser.AccountName;
                }
                else
                    account_name = Models.Account_Details.GetAccountDetails(hdUser, request.account_id > 0 ? request.account_id : hdUser.AccountId, false).Name;

                var location = Models.Location.GetLocation(hdUser.OrganizationId, hdUser.DepartmentId, request.location_id);
                if (account_id == -1)
                    account_id = 0;
                if (location.AccountId != account_id)
                    throw new HttpError($"Cannot insert Asset to LocationId = {request.location_id}! This location don't exists in Account: {account_name} with Account Id: {account_id}.");
            }
            int CreatedAssetID = Models.Assets.AddAsset(hdUser, request.serial_number,
        request.category_id,
        request.type_id,
        request.make_id,
        request.model_id,
        request.unique1_value,
        request.unique2_value,
        request.unique3_value,
        request.unique4_value,
        request.unique5_value,
        request.unique6_value,
        request.unique7_value,
        request.unique_motherboard,
        request.unique_bios,
        request.name,
        request.description,
        request.location_id,
        request.status_id,
        request.is_bulk,
        request.is_force_dublicate,
        request.checkout_id,
        request.note,
        request.entered_date);
            return new HttpResult(CreatedAssetID, HttpStatusCode.OK);
        }

        public object Put(PutAsset request)
        {
            if (request.id < 1)
            {
                throw new HttpError(HttpStatusCode.NotFound, "Asset not found. Please check Id");
            }

            var req = base.Request;
            int formKeysCount = req.FormData.Count;
            int queryKeysCount = req.QueryString.Count;
            if (req.QueryString.GetValues("format") != null)
                queryKeysCount--;

            if (formKeysCount == 0 && queryKeysCount == 0)
            {
                throw new HttpError(HttpStatusCode.NotAcceptable, "No parameters provided to Asset");
            }

            ApiUser hdUser = request.ApiUser;
            Instance_Config config = CheckAssets(hdUser);
            DateTime? entered_date = null;
            if (request.entered_date.HasValue && request.entered_date > DateTime.MinValue)
            {
                 entered_date = request.entered_date.Value.AddHours(-1 * hdUser.TimeZoneOffset);
            }
            System.Data.DataRow drasset = Models.Assets.GetAssetRow(request.ApiUser, request.id);

            if (request.location_id.HasValue && request.location_id.Value > 0 && config.LocationTracking && config.AccountManager)
            {
                int account_id = request.account_id ?? hdUser.AccountId;
                string account_name = Models.Account_Details.GetAccountDetails(hdUser, account_id, false).Name;

                var location = Models.Location.GetLocation(hdUser.OrganizationId, hdUser.DepartmentId, request.location_id.Value);
                if (account_id == -1)
                    account_id = 0;
                if (location.AccountId != account_id)
                    throw new HttpError($"Cannot move Asset to LocationId = {request.location_id}! This location don't exists in Account: {account_name} with Account Id: {account_id}.");
            }

            Models.Assets.UpdateAsset(hdUser, request.id, drasset, request.checkout_id, request.owner_id, request.account_id, request.serial_number,
        request.category_id,
        request.type_id,
        request.make_id,
        request.model_id,
        request.unique1_value,
        request.unique2_value,
        request.unique3_value,
        request.unique4_value,
        request.unique5_value,
        request.unique6_value,
        request.unique7_value,
        request.unique_motherboard,
        request.unique_bios,
        request.name,
        request.description,
        request.note,
        request.location_id,
        request.is_bulk,
        request.is_force_dublicate,
        request.is_active,
        request.status_id, 
        entered_date);
            return new HttpResult("", HttpStatusCode.OK);
        }


        public object Get(AssetStatuses request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckAssets(hdUser);
            return Models.AssetStatuses.GetAssetStatuses(hdUser.OrganizationId, hdUser.DepartmentId, request.is_active ?? true);
        }

        public object Get(AssetCategories request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckAssets(hdUser);
            return Models.AssetCategories.GetAssetCategories(hdUser.OrganizationId, hdUser.DepartmentId);
        }

        public object Get(AssetTypes request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckAssets(hdUser);
            return Models.AssetTypes.GetAssetTypes(hdUser.OrganizationId, hdUser.DepartmentId, request.category_id);
        }

        public object Get(AssetMakes request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckAssets(hdUser);
            return Models.AssetMakes.GetAssetMakes(hdUser.OrganizationId, hdUser.DepartmentId, request.type_id);
        }

        public object Get(AssetModels request)
        {
            ApiUser hdUser = request.ApiUser;
            CheckAssets(hdUser);
            return Models.AssetModels.GetAssetModels(hdUser.OrganizationId, hdUser.DepartmentId, request.make_id);
        }

        private Instance_Config CheckAssets(ApiUser hdUser)
        {
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (!instanceConfig.EnableAssetAuditor || !instanceConfig.AssetTracking)
            {
                throw new HttpError("Assets is not enabled for this instance.");
            }
            return instanceConfig;
        }
    }
}
