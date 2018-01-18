// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Runtime.Serialization;

namespace BWA.Api.Models
{
    [DataContract(Name = "prepaid_pack")]
    public class Contract : ModelItemBase
    {
        public Contract(DataRow row)
            : base(row)
        { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "prepaid_pack_id")]
        public int ContractID
        {
            get { return Convert.ToInt32(Row["id"]); }
            set { Row["id"] = value; }
        }

        [DataMember(Name = "prepaid_pack_name")]
        public string ContractName
        {
            get { return Row["Name"].ToString(); }
            set { Row["Name"] = value; }
        }
    }
}
