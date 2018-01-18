// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk.Data;
using System.Data.SqlClient;
using SherpaDeskApi.ServiceModel;
using ServiceStack;

namespace BWA.Api.Models
{
    /// <summary>
    /// Summary description for TicketLogRecords
    /// </summary>
    [DataContract(Name = "Assets")]
    public class Assets : ModelItemCollectionGeneric<Asset>
    {
        public Assets(DataTable AssetsTable) : base(AssetsTable) { }

        public static Asset GetAsset(ApiUser hdUser, int id)
        {
            Instance_Config config = new Instance_Config(hdUser);

            AssetsConfig assetsconfig = config.Assets;
            
            DataRow dr = GetAssetRow(hdUser, id);

            Asset asset = new Asset(dr);
            List<KeyValuePair<string, string>> unique_fields = new List<KeyValuePair<string, string>>();
            for (int i = 1; i < 8; i++)
                if (!string.IsNullOrEmpty(assetsconfig.Captions[i - 1])) unique_fields.Add(new KeyValuePair<string, string>(assetsconfig.Captions[i - 1] + " (Unique" + i+ ")", dr.GetString("Unique" + i)));
            asset.UniqueFields = unique_fields.ToList();
            return asset;
        }

        public static DataRow GetAssetRow(ApiUser hdUser, int id)
        {
            DataRow dr = bigWebApps.bigWebDesk.Data.Asset.GetAsset(hdUser.OrganizationId, hdUser.DepartmentId, id);
            if (dr == null)
            {
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Assets not found. Please check Id");
            }
            return dr;
        }

        public static List<Asset> GetAssets(ApiUser hdUser, string search, string filter, int user_id, int owner_id, int account_id, int location_id, bool? is_active, int status, int limit, int page)
        {
            Instance_Config config = new Instance_Config(hdUser);

            AssetsConfig assetsconfig = config.Assets;

            bigWebApps.bigWebDesk.Data.Assets.Filter m_Filter = new bigWebApps.bigWebDesk.Data.Assets.Filter(hdUser.DepartmentId, true, hdUser.OrganizationId); ;
            bigWebApps.bigWebDesk.Data.Assets.ColumnsSetting ColSetting = //new bigWebApps.bigWebDesk.Data.Assets.ColumnsSetting();
            //bigWebApps.bigWebDesk.Data.Assets.ColumnsSetting ColSetting = 
            new bigWebApps.bigWebDesk.Data.Assets.ColumnsSetting(hdUser.DepartmentId, hdUser.UserId, true, hdUser.OrganizationId);

            /*
            ColSetting.Column1 = ColSetting.Column1 == bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank ? bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.AssetName : ColSetting.Column1;
            ColSetting.Column2 = ColSetting.Column2 == bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank ? bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.AuditNote : ColSetting.Column2;
            ColSetting.Column3 = ColSetting.Column3 == bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank ? bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.DateUpdated : ColSetting.Column3;
            ColSetting.Column4 = ColSetting.Column4 == bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank ? bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.DateEntered : ColSetting.Column4;
            */
            ColSetting.Column1 = bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank;
            ColSetting.Column2 = bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank;
            ColSetting.Column3 = bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank;
            ColSetting.Column4 = bigWebApps.bigWebDesk.Data.Assets.BrowseColumn.Blank;

            if (!hdUser.IsTechAdmin || filter == "my")
                user_id = owner_id = hdUser.UserId;
            if (!string.IsNullOrEmpty(search))
            {
                m_Filter.SearchText = search;
                //m_Filter.LocationName
            }
            if (user_id > 0)
                m_Filter.CheckedOutID = user_id;
            if (owner_id > 0)
                m_Filter.OwnerID = owner_id;
            if (account_id > 0)
                m_Filter.TicketAccountID = account_id;
            if (location_id > 0)
                m_Filter.LocationID = location_id;
            if (status > 0)
                m_Filter.StatusID = status;
            string custom_sort = "";
            int _assetsCount = 0;
            DataTable dtAssets = bigWebApps.bigWebDesk.Data.Assets.SelectAssetsByFilter(hdUser.OrganizationId, hdUser.DepartmentId, m_Filter, ColSetting, custom_sort, out _assetsCount, null, hdUser.UserId, config, is_active, limit, page);
            return InitializeAssetList(hdUser, assetsconfig, dtAssets);
        }


        public static int AddAsset(ApiUser hdUser, string txtSerialNumber, int AssetCategoryId, int AssetTypeId, int MakeId, int ModelId, string txtUnique1Value, string txtUnique2Value, string txtUnique3Value, string txtUnique4Value, string txtUnique5Value, string txtUnique6Value, string txtUnique7Value, string txtUniqueMotherboard, string txtUniqueBios, string txtAssetName, string txtAssetDescription, int LocationId,
        int status, bool is_bulk, bool is_force_dublicate, int? user_id, string note, DateTime? entered_date)
        {
            int dub_count = CheckDublicateAssets(hdUser, txtSerialNumber,
                                                                    txtUnique1Value, txtUnique2Value,
                                                                    txtUnique3Value, txtUnique4Value,
                                                                    txtUnique5Value, txtUnique6Value,
                                                                    txtUnique7Value,
                                                                    txtUniqueMotherboard, txtUniqueBios);

            int CreatedAssetID = 0;

            if (dub_count == 0 || is_force_dublicate)
            {
                try
                {
                    if (0 == user_id)
                        user_id = null;
                    //else
                    //    user_id = user_id.HasValue && user_id.Value > 0 ? user_id : hdUser.UserId;
                    if (!is_bulk)
                    {
                        CreatedAssetID = bigWebApps.bigWebDesk.Data.Asset.InsertAsset(hdUser.DepartmentId, user_id, null, txtSerialNumber, AssetCategoryId, AssetTypeId, MakeId, ModelId, txtUnique1Value, txtUnique2Value, txtUnique3Value, txtUnique4Value, txtUnique5Value, txtUnique6Value, txtUnique7Value, txtUniqueMotherboard, txtUniqueBios, txtAssetName, txtAssetDescription, LocationId, false, hdUser.OrganizationId);
                        if (entered_date.HasValue && entered_date.Value > DateTime.MinValue || !string.IsNullOrEmpty(note) || status > 0)
                        {
                            string query = "";
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(new SqlParameter("@AssetId", CreatedAssetID));
                            parameters.Add(new SqlParameter("@DepartmentId", hdUser.DepartmentId));
                            if (entered_date.HasValue && entered_date.Value > DateTime.MinValue)
                            {
                                query += "DateEntered=@DateEntered";
                                parameters.Add(new SqlParameter("@DateEntered", entered_date.Value));
                            }
                            if (!string.IsNullOrEmpty(note))
                            {
                                if (!string.IsNullOrEmpty(query)) query += ",";
                                query += "Notes=@Notes";
                                parameters.Add(new SqlParameter("@Notes", note));
                            }
                            if (status > 0)
                            {
                                if (!string.IsNullOrEmpty(query)) query += ",";
                                query += "StatusId=@StatusId";
                                parameters.Add(new SqlParameter("@StatusId", status));

                            }
                            DBAccess.UpdateByQuery("UPDATE Assets SET " + query + " WHERE DepartmentId = @DepartmentId AND Id = @AssetId", parameters.ToArray(), hdUser.OrganizationId);
                        }
                    }
                    else
                        CreatedAssetID = bigWebApps.bigWebDesk.Data.AssetBulk.InsertBulkAsset(hdUser.DepartmentId, user_id ?? hdUser.UserId, AssetCategoryId, AssetTypeId, txtAssetName, txtAssetDescription, hdUser.OrganizationId);

                    /*
                    FileServiceUpload.ObjectType = TaggedAssetMode ? "assets-assets-picture" : "assets-bulk-picture";
                    FileServiceUpload.ObjectId = CreatedAssetID.ToString();
                    FileServiceUpload.AcceptChanges();
                    */
                    //UserSetting.GetSettings(TempSettingName).Value = String.Format("{0}|{1}|{2}|{3}", AssetCategoryId.ToString(), AssetTypeId.ToString(), ddlMakeList.SelectedValue, ddlModelList.SelectedValue);
                    //res = CreatedAssetID;
                }
                catch
                {
                    throw new HttpError("Can't insert asset.");
                }
            }
            else
                throw new HttpError("Can't insert dublicate asset. Please set is_force_dublicate = true");
            return CreatedAssetID;
        }

        public static void UpdateAsset(ApiUser hdUser, int id, DataRow oldasset, int? user_id, int? owner_id, int? account_id, string txtSerialNumber, int? AssetCategoryId, int? AssetTypeId, int? MakeId, int? ModelId, string txtUnique1Value, string txtUnique2Value, string txtUnique3Value, string txtUnique4Value, string txtUnique5Value, string txtUnique6Value, string txtUnique7Value, string txtUniqueMotherboard, string txtUniqueBios, string txtAssetName, string txtAssetDescription, string note, int? LocationId, bool is_bulk, bool is_force_dublicate, bool? is_active, int status, DateTime? entered_date)
        {
           //var t =  string.Join("\n", oldasset.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray());

           double _ValueCurrent = (double)Math.Max(-1, oldasset.Get<decimal>("ValueCurrent"));
           double _ValueReplacement = (double)Math.Max(-1, oldasset.Get<decimal>("ValueReplacement"));
           double _ValueDepreciated = (double)Math.Max(-1, oldasset.Get<decimal>("ValueDepreciated"));
           double _ValueSalvage = (double)Math.Max(-1, oldasset.Get<decimal>("ValueSalvage"));
           double _DisposalCost = (double)Math.Max(-1, oldasset.Get<decimal>("DisposalCost"));
            int _AssetSort = Math.Max(-1, int.Parse(oldasset.GetString("AssetSort")));
            //int _FundingSource=-1;

            //if (txtFundingSource.Text.Length > 0)
            //    _FundingSource = int.Parse(txtFundingSource.Text);

            if (!string.IsNullOrEmpty(note))
                bigWebApps.bigWebDesk.Data.Asset.UpdateAssetNotes(hdUser.DepartmentId, id, note, hdUser.OrganizationId);

            int _category_id = AssetCategoryId ?? oldasset.Get<int>("CategoryId");
            int _type_id = AssetTypeId ?? oldasset.Get<int>("TypeId");
            int _make_id = MakeId ?? oldasset.Get<int>("MakeId");
            int _model_id = ModelId ?? oldasset.Get<int>("ModelId");

            if (AssetCategoryId.HasValue || AssetTypeId.HasValue || MakeId.HasValue || ModelId.HasValue)
            {
                bigWebApps.bigWebDesk.Data.Asset.UpdateAssetTypeMakeModel(hdUser.DepartmentId, id, _category_id, _type_id, _make_id, _model_id, hdUser.OrganizationId);
            }

            int _owner = owner_id ?? oldasset.Get<int?>("OwnerId") ?? hdUser.UserId;
            if (owner_id.HasValue)
                bigWebApps.bigWebDesk.Data.Asset.UpdateAssetOwner(hdUser.DepartmentId, _owner, id, hdUser.FullName, hdUser.OrganizationId);
            int _checkout = user_id ?? oldasset.Get<int?>("CheckedOutId") ?? hdUser.UserId;
            if (user_id.HasValue)
                bigWebApps.bigWebDesk.Data.Asset.UpdateAssetCheckout(hdUser.DepartmentId, _checkout, id, hdUser.FullName, hdUser.OrganizationId);
            int _purchase_vendor_id = -1;
            if (oldasset.Get<int>("VendorId") <= 0)
                _purchase_vendor_id = -1;

            int _warranty_vendor_id = -1;
            if (oldasset.Get<int?>("WarrantyVendor") <= 0)
                _warranty_vendor_id = -1;

            int _warranty_labor = -1;
            if (oldasset.GetString("LaborWarrantyLength", "").Length > 0)
                _warranty_labor = Int32.Parse(oldasset.GetString("LaborWarrantyLength", ""));

            int _warranty_part = -1;
            int _locationId = LocationId ?? oldasset.Get<int>("LocationId");
            string txtFundingSource = oldasset.GetString("FundingSource", "");
            DateTime? dateAcquired = oldasset.Get<DateTime?>("DateAquired");
            DateTime? datePurchased = oldasset.Get<DateTime?>("DatePurchase");
            DateTime? dateDeployed = oldasset.Get<DateTime?>("DateDeployed");
            DateTime? dateOutOfService = oldasset.Get<DateTime?>("DateOutOfService");
            DateTime? dateEntered =  entered_date ?? oldasset.Get<DateTime?>("DateEntered");
            DateTime? dateReceived = oldasset.Get<DateTime?>("DateReceived");
            DateTime? dateDisposed = oldasset.Get<DateTime?>("DateDisposed");
            string txtFundingCode = oldasset.GetString("FundingCode", "");
            string txtPONumber = oldasset.GetString("PONumber", "");
            if (oldasset.GetString("PartsWarrantyLength", "").Length > 0)
                _warranty_part = Int32.Parse(oldasset.GetString("PartsWarrantyLength", ""));

            double _value = -1;
            if (oldasset.GetString("Value", "").Length > 0)
                _value = double.Parse(oldasset.GetString("Value", ""));

            int accountId = account_id ?? oldasset.Get<int?>("AccountId") ?? hdUser.AccountId;

            accountId = accountId <= 0 ? -1 : accountId;

            //need to swith stauses /active - inactive
            status = status > 0 ? status : oldasset.Get<int>("Status");

            int _update_return = bigWebApps.bigWebDesk.Data.Asset.UpdateAsset(
                                hdUser.DepartmentId,
                                hdUser.UserId,
                                id,
                                _purchase_vendor_id,
                                _warranty_vendor_id,
                                accountId,
                                _locationId,
                                txtAssetName ?? oldasset.GetString("AssetName") ?? oldasset.GetString("Name"),
                                txtAssetDescription ?? oldasset.GetString("Description"),
                                _value,
                                _ValueCurrent,
                                _ValueReplacement,
                                _ValueDepreciated,
                                _ValueSalvage,
                                _DisposalCost,
                                _AssetSort,
                                txtFundingSource,
                                dateAcquired,
                                datePurchased,
                                dateDeployed,
                                dateOutOfService,
                                dateEntered,
                                dateReceived,
                                dateDisposed,
                                _warranty_labor,
                                _warranty_part,
                                txtPONumber,
                                txtFundingCode,
                                txtSerialNumber ?? oldasset.GetString("SerialNumber"),
                                txtUnique1Value ?? oldasset.GetString("Unique1Value") ?? oldasset.GetString("Unique1"),
                                txtUnique2Value ?? oldasset.GetString("Unique2Value") ?? oldasset.GetString("Unique2"),
                                txtUnique3Value ?? oldasset.GetString("Unique3Value") ?? oldasset.GetString("Unique3"),
                                txtUnique4Value ?? oldasset.GetString("Unique4Value") ?? oldasset.GetString("Unique4"),
                                txtUnique5Value ?? oldasset.GetString("Unique5Value") ?? oldasset.GetString("Unique5"),
                                txtUnique6Value ?? oldasset.GetString("Unique6Value") ?? oldasset.GetString("Unique6"),
                                txtUnique7Value ?? oldasset.GetString("Unique7Value") ?? oldasset.GetString("Unique7"),
                                status, //status active
                                oldasset.Get<int>("Status"), //old status
                                true,
                                hdUser.OrganizationId
                                );
        }

        private static int CheckDublicateAssets(ApiUser hdUser, string txtSerialNumber, string txtUnique1Value, string txtUnique2Value, string txtUnique3Value, string txtUnique4Value, string txtUnique5Value, string txtUnique6Value, string txtUnique7Value, string txtUniqueMotherboard, string txtUniqueBios)
        {
            int assetDuplicateResult = 0;
            DataTable dtAssetDuplicates = bigWebApps.bigWebDesk.Data.Asset.SelectDuplicates(hdUser.DepartmentId, 0, txtSerialNumber,
                                                                    txtUnique1Value, txtUnique2Value,
                                                                    txtUnique3Value, txtUnique4Value,
                                                                    txtUnique5Value, txtUnique6Value,
                                                                    txtUnique7Value, out assetDuplicateResult,
                                                                    txtUniqueMotherboard, txtUniqueBios, hdUser.OrganizationId);
            /*
                if (assetDuplicateResult != 0)
            {
                plchDuplicateAssets.Visible = true;
                trButton.Visible = false;
                ctlDuplicateAssets.AssetProfileId = AssetProfileId;
                ctlDuplicateAssets.DataSource = dtAssetDuplicates;
                ctlDuplicateAssets.DataBind();
            }
             */
            return assetDuplicateResult;
        }

        public static List<Asset> TicketAssets(ApiUser hdUser, AssetsConfig assetsconfig, int TicketId)
        {
            DataTable dtAssets = bigWebApps.bigWebDesk.Data.Tickets.SelectAssets(hdUser.OrganizationId, hdUser.DepartmentId, TicketId);
            return InitializeAssetList(hdUser, assetsconfig, dtAssets);
        }

        public static List<Asset> AccountAssets(ApiUser hdUser, AssetsConfig assetsconfig, int AccountId)
        {
            var sqlquery = string.Format("SELECT Assets.Id,  Assets.CategoryId as AssetCategoryId, AssetCategories.Name as AssetCategoryName,  Assets.TypeId as AssetTypeId, AssetTypes.Name as AssetTypeName,  Assets.MakeId as AssetMakeId, AssetMakes.Make as AssetMakeName,  Assets.ModelId as AssetModelId, AssetModels.Model as AssetModelName,  Assets.AccountId as AccountId,  ISNULL(Accounts.vchName,ISNULL(tbl_company.company_name, '')) AS AccountName,  Assets.AssetGUID, Assets.SerialNumber, Assets.Unique1, Assets.Unique2, Assets.Unique3, Assets.Unique4, Assets.Unique5, Assets.Unique6, Assets.Unique7, CASE WHEN LEN(Assets.SerialNumber)>0 THEN Assets.SerialNumber END AS SerialTagNumber,  dbo.fxGetUserName(lo_checkout.FirstName, lo_checkout.LastName, lo_checkout.Email) as CheckoutName, AssetStatus.vchStatus as StatusName, Assets.dtUpdated AS UpdatedDate, dbo.fxGetUserName2(lo_updated.FirstName,lo_updated.LastName,lo_updated.Email) AS UpdatedByName FROM Assets  INNER JOIN tbl_company ON tbl_company.company_id={0}  INNER JOIN AssetCategories ON AssetCategories.DepartmentId={0} and Assets.CategoryId = AssetCategories.Id  INNER JOIN AssetTypes ON AssetTypes.DepartmentId={0} and Assets.TypeId = AssetTypes.Id  INNER JOIN AssetMakes ON AssetMakes.DepartmentId={0} and Assets.MakeId = AssetMakes.Id  INNER JOIN AssetModels ON AssetModels.DepartmentId={0} and Assets.ModelId = AssetModels.Id  INNER JOIN AssetStatus ON (AssetStatus.DId is NULL OR AssetStatus.DId={0}) and Assets.StatusId = AssetStatus.Id  LEFT OUTER JOIN Accounts ON Accounts.DId={0} and Assets.AccountId = Accounts.Id  LEFT OUTER JOIN Locations ON Locations.DId={0} and Assets.LocationId = Locations.Id  LEFT OUTER JOIN tbl_vendors Vendors ON Vendors.company_id={0} and Assets.VendorId = Vendors.Id  LEFT OUTER JOIN tbl_vendors WarrantyVendors ON WarrantyVendors.company_id={0} and Assets.WarrantyVendor = WarrantyVendors.Id  LEFT OUTER JOIN tbl_LoginCompanyJunc tlj_owner ON tlj_owner.company_id={0} and Assets.OwnerId=tlj_owner.id LEFT OUTER JOIN tbl_Logins lo_owner ON lo_owner.id=tlj_owner.login_id  LEFT OUTER JOIN tbl_LoginCompanyJunc tlj_checkout ON tlj_checkout.company_id={0} and Assets.CheckedOutId=tlj_checkout.id LEFT OUTER JOIN tbl_Logins lo_checkout ON lo_checkout.id=tlj_checkout.login_id  LEFT OUTER JOIN tbl_LoginCompanyJunc tlj_updated ON tlj_updated.company_id={0} and Assets.intUpdatedBy=tlj_updated.id LEFT OUTER JOIN tbl_Logins lo_updated ON lo_updated.id=tlj_updated.login_id  LEFT OUTER JOIN AssetStatusCompany ON AssetStatusCompany.DId={0} and AssetStatusCompany.AssetStatusID=Assets.StatusId  WHERE Assets.DepartmentId = 3 AND Assets.StatusId<>17 AND (AssetStatusCompany.NonActive=0 OR (AssetStatusCompany.NonActive IS NULL))  AND Assets.AccountId = {1} ORDER BY AssetCategoryName,AssetTypeName,AssetMakeName,AssetModelName",
                hdUser.DepartmentId, AccountId);
            return InitializeAssetList(hdUser, assetsconfig, DBAccess.SelectByQuery(sqlquery, hdUser.OrganizationId));
        }

        private static List<Asset> InitializeAssetList(ApiUser hdUser, AssetsConfig assetsconfig, DataTable dtAssets)
        {
            List<Asset> assets = new List<Asset>();

            /*
            int counts = 0;
            List<Models.Location> locations = null;
            if (hdUser != null && dtAssets.Rows.Count > 0)
                locations = Models.Locations.GetAccountLocations(hdUser.OrganizationId, hdUser.DepartmentId, -1, 0, "", true);

            List<AssetCategory> statuses = null;
            if (hdUser != null && dtAssets.Rows.Count > 0)
                statuses = AssetStatuses.GetAssetStatuses(hdUser.OrganizationId, hdUser.DepartmentId, null);
            */

            foreach (DataRow row_asset in dtAssets.Rows)
            {
                int id = row_asset.Get<int>("Id");
                if (id < 1)
                {
                    //counts = row_asset.Get<int>("ItemsCount");
                    continue;
                }
                var asset = GetAsset(hdUser, id);
                assets.Add(asset);
            }
            return assets;
        }
    }
}

/*
 DepartmentId
Id
OwnerId
CheckedOutId
TypeId
MakeId
ModelId
location_id
LocationId
VendorId
WarrantyVendor
Name
SerialNumber
Description
Value
DateAquired
LaborWarrantyLength
PartsWarrantyLength
Notes
Room
PONumber
Active
FundingCode
CategoryId
StatusId
AssetSort
DatePurchased
DateDeployed
DateOutOfService
DateEntered
DateReceived
DateDisposed
ValueCurrent
ValueReplacement
ValueDepreciated
ValueSalvage
DisposalCost
FundingSource
dtUpdated
intUpdatedBy
AccountId
AssetNumber
AssetGUID
Unique1
Unique2
Unique3
Unique4
Unique5
Unique6
Unique7
MergedId
Lost
LostOn
AuditNote
CategoryId1
AssetGUID1
AssetNumber1
SerialNumber1
Unique11
Unique31
Unique32
Unique41
Unique51
Unique61
Unique71
Room1
AssetCategoryName
AssetTypeName
AssetProfileId
AssetModelName
AssetMakeName
AssetLocationName
CheckedOutName
Active1
DepartmentId1
OwnerId1
CheckedOutId1
TypeId1
MakeId1
ModelId1
AccountId1
LocationId1
StatusId1
vchStatus
 */
