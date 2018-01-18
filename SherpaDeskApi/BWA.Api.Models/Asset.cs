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
    [DataContract(Name = "Asset")]
    public class Asset : ModelItemBase
    {
        public Asset(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int id
        {
            get { return Row.Get<int>("Id"); }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "serial_tag_number")]
        public string serial_tag_number
        {
            get { return Row.GetString("SerialTagNumber"); }
            set { Row["SerialTagNumber"] = value; }
        }

        [DataMember(Name = "category")]
        public string category_name
        {
            get { return Row.GetString("AssetCategoryName"); }
            set { Row["AssetCategoryName"] = value; }
        }


        [DataMember(Name = "name")]
        public string name
        {
            get { return Row.GetString("Name") ?? Row.GetString("AssetName"); }
            set { Row["Name"] = value; }
        }

        [DataMember(Name = "note")]
        public string note
        {
            get { return Row.GetString("Notes"); }
            set { Row["Notes"] = value; }
        }

        [DataMember(Name = "Description")]
        public string description
        {
            get { return Row.GetString("Description"); }
            set { Row["Description"] = value; }
        }

        [DataMember(Name = "updated_date")]
        public DateTime? DateUpdated
        {
            get
            {
                return Row.Get<DateTime?>("DateUpdated") ?? Row.Get<DateTime?>("UpdatedDate") ?? Row.Get<DateTime?>("dtUpdated");
            }
            set
            {
                if (Row.Table.Columns.Contains("DateUpdated")) Row["DateUpdated"] = value;
                else if (Row.Table.Columns.Contains("dtUpdated")) Row["dtUpdated"] = value;
            }
        }


        [DataMember(Name = "entered_date")]
        public DateTime? DateEntered
        {
            get
            {
                if (Row.Table.Columns.Contains("DateEntered") && !Row.IsNull("DateEntered"))
                    return Convert.ToDateTime(Row["DateEntered"]);
                else
                    return null;
            }
            set
            {
                if (Row.Table.Columns.Contains("DateEntered")) Row["DateEntered"] = value;
            }
        }

        [DataMember(Name = "category_id")]
        public int category_id
        {
            get { return Row.Get<int?>("AssetCategoryId") ?? Row.Get<int>("CategoryId"); }
            set { Row["AssetCategoryId"] = value; }
        }

        [DataMember(Name = "type")]
        public string type_name
        {
            get { return Row.GetString("AssetTypeName"); }
            set { Row["AssetTypeName"] = value; }
        }

        [DataMember(Name = "type_id")]
        public int type_id
        {
            get { return Row.Get<int?>("AssetTypeId") ?? Row.Get<int>("TypeId"); }
            set { Row["AssetTypeId"] = value; }
        }

        [DataMember(Name = "make")]
        public string make_name
        {
            get { return Row.GetString("AssetMakeName"); }
            set { Row["AssetMakeName"] = value; }
        }


        [DataMember(Name = "make_id")]
        public int make_id
        {
            get { return Row.Get<int?>("AssetMakeId") ?? Row.Get<int>("MakeId"); }
            set { Row["AssetMakeId"] = value; }
        }

        [DataMember(Name = "model")]
        public string model_name
        {
            get { return Row["AssetModelName"].ToString(); }
            set { Row["AssetModelName"] = value; }
        }

        [DataMember(Name = "model_id")]
        public int model_id
        {
            get { return Row.Get<int?>("AssetModelId") ?? Row.Get<int>("ModelId"); }
            set { Row["AssetModelId"] = value; }
        }

        [DataMember(Name = "serial")]
        public string serial_number
        {
            get { return Row["SerialNumber"].ToString(); }
            set { Row["SerialNumber"] = value; }
        }

        [DataMember(Name = "unique_fields")]
        public List<KeyValuePair<string, string>> UniqueFields { get; set; }

        [DataMember(Name = "checkout_name")]
        public string checkout_name
        {
            get { return Row.GetString("CheckoutName") ?? Row.GetString("CheckedOutName"); }
            set { if (Row.Table.Columns.Contains("CheckoutName")) Row["CheckoutName"] = value; }
        }

        [DataMember(Name = "checkout_id")]
        public int checkout_id
        {
            get { return Row.Get<int>("CheckedOutId"); }
            set { Row["CheckedOutId"] = value; }
        }

        [DataMember(Name = "owner_id")]
        public int owner_id
        {
            get { return Row.Get<int>("OwnerId"); }
            set { Row["OwnerId"] = value; }
        }

        [DataMember(Name = "owner_name")]
        public string owner_name
        {
            get { return Row.GetString("CheckoutName") ?? Row.GetString("CheckedOutName"); }
            set { if (Row.Table.Columns.Contains("CheckoutName")) Row["CheckoutName"] = value; }
        }

        [DataMember(Name = "status")]
        public string status
        {
            get { return Row.GetString("StatusName") ?? Row.GetString("vchStatus"); }
            set { if (Row.Table.Columns.Contains("StatusName")) Row["StatusName"] = value; }
        }

        int _status_id = 0;
        [DataMember(Name = "status_id")]
        public int status_id
        {
            get { return Row.Get<int>("StatusId"); }
            set { _status_id = value; }
        }

        [DataMember(Name = "location_id")]
        public int location_id
        {
            get { return Row.Get<int>("LocationId"); }
            set { Row["LocationId"] = value; }
        }

        private string _location_name = "";

        [DataMember(Name = "location_name")]
        public string location_name
        {
            get { return Row.GetString("AssetLocationName") ?? _location_name; }
            set { _location_name = value; }
        }

        private int _counts = 0;

       //[DataMember(Name = "count")]
        public int ItemsCount
        {
            get { return _counts; }
            set { _counts = value; }
        }

    }
}
