// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Secure()]
    [Route("/assets", "GET, OPTIONS")]
    public class GetAssets : PagedApiRequest
    {
        public string search { get; set; }
        public string filter { get; set; }
        public int status { get; set; }
        public int user_id { get; set; }
        public int owner_id { get; set; }
        public int account_id { get; set; }
        public int location_id { get; set; }
        public int? asset_id { get; set; }
        public bool? is_active { get; set; }
        public bool? get_all_active_for_user { get; set; }
    }

    [Secure()]
    [Route("/assets/{id}", "GET, OPTIONS")]
    public class GetAsset : ApiRequest
    {
        public int id { get; set; }
    }

    [Secure()]
    [Route("/assets", "POST, OPTIONS")]
    public class PostAsset : ApiRequest
    {
        public bool is_bulk { get; set; }
        public int? checkout_id { get; set; }
        public bool is_force_dublicate { get; set; }
        public string serial_number { get; set; }
        public int category_id { get; set; }
        public int type_id { get; set; }
        public int make_id { get; set; }
        public int model_id { get; set; }
        public string unique1_value { get; set; }
        public string unique2_value { get; set; }
        public string unique3_value { get; set; }
        public string unique4_value { get; set; }
        public string unique5_value { get; set; }
        public string unique6_value { get; set; }
        public string unique7_value { get; set; }
        public string unique_motherboard { get; set; }
        public string unique_bios { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int account_id { get; set; }
        public int location_id { get; set; }
        public int status_id { get; set; }
        public DateTime? entered_date { get; set; }
        public string note { get; set; }
    }

    [Secure()]
    [Route("/assets/{id}", "PUT, OPTIONS")]
    public class PutAsset : ApiRequest
    {
        public int id { get; set; }
        public bool is_bulk { get; set; }
        public bool is_force_dublicate { get; set; }
        public int? checkout_id { get; set; }
        public int? owner_id { get; set; }
        public int? account_id { get; set; }
        public string serial_number { get; set; }
        public int? category_id { get; set; }
        public int? type_id { get; set; }
        public int? make_id { get; set; }
        public int? model_id { get; set; }
        public string unique1_value { get; set; }
        public string unique2_value { get; set; }
        public string unique3_value { get; set; }
        public string unique4_value { get; set; }
        public string unique5_value { get; set; }
        public string unique6_value { get; set; }
        public string unique7_value { get; set; }
        public string unique_motherboard { get; set; }
        public string unique_bios { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int? location_id { get; set; }
        public bool? is_active { get; set; }
        public int status_id { get; set; }
        public DateTime? entered_date { get; set; }
    }

    [Secure()]
    [Route("/asset_categories")]
    public class AssetCategories : PagedApiRequest
    {
       //public int category_id { get; set; }
    }


    [Secure()]
    [Route("/asset_types")]
    public class AssetTypes : PagedApiRequest
    {
        public int category_id { get; set; }
    }


    [Secure()]
    [Route("/asset_makes")]
    public class AssetMakes : PagedApiRequest
    {
        public int type_id { get; set; }
    }

    [Secure()]
    [Route("/asset_models")]
    public class AssetModels : PagedApiRequest
    {
        public int make_id { get; set; }
    }


    [Secure()]
    [Route("/asset_statuses")]
    public class AssetStatuses : PagedApiRequest
    {
        public bool? is_active { get; set; }
    }
}
