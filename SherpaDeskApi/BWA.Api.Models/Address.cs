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
    [DataContract(Name = "Address")]
    public class Address : ModelItemBase
    {
        public Address(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "fullname")]
        public string userFullName
        {
            get { return Row.Table.Columns.Contains("userFullName") ? Row["userFullName"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("userFullName"))
                    Row["userFullName"] = value;
            }
        }

        [DataMember(Name = "address1")]
        public string Address1
        {
            get { return Row.Table.Columns.Contains("Address1") ? Row["Address1"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("Address1"))
                    Row["Address1"] = value;
            }
        }

        [DataMember(Name = "address2")]
        public string Address2
        {
            get { return Row.Table.Columns.Contains("Address2") ? Row["Address2"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("Address2"))
                    Row["Address2"] = value;
            }
        }

        [DataMember(Name = "city")]
        public string City
        {
            get { return Row.Table.Columns.Contains("City") ? Row["City"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("City"))
                    Row["City"] = value;
            }
        }

        [DataMember(Name = "state")]
        public string State
        {
            get { return Row.Table.Columns.Contains("State") ? Row["State"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("State"))
                    Row["State"] = value;
            }
        }

        [DataMember(Name = "zipcode")]
        public string ZipCode
        {
            get { return Row.Table.Columns.Contains("ZipCode") ? Row["ZipCode"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("ZipCode"))
                    Row["ZipCode"] = value;
            }
        }

        [DataMember(Name = "country")]
        public string Country
        {
            get { return Row.Table.Columns.Contains("Country") ? Row["Country"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("Country"))
                    Row["Country"] = value;
            }
        }

        [DataMember(Name = "phone1")]
        public string Phone1
        {
            get { return Row.Table.Columns.Contains("Phone1") ? Row["Phone1"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("Phone1"))
                    Row["Phone1"] = value;
            }
        }

        [DataMember(Name = "phone2")]
        public string Phone2
        {
            get { return Row.Table.Columns.Contains("Phone2") ? Row["Phone2"].ToString() : ""; }
            set
            {
                if (Row.Table.Columns.Contains("Phone2"))
                    Row["Phone2"] = value;
            }
        }

        public static Address GetAccountAddress(Guid organizationId, int departmentId, int accountId)
        {
            DataTable dt = bigWebApps.bigWebDesk.Data.Accounts.SelectAccountPrimaryContact(organizationId, departmentId, accountId);
            Models.Address account = null;
            if (dt != null && dt.Rows.Count > 0)
                account = new Address(dt.Rows[0]);
            return account;
        }
    }
}
