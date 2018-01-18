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
    [DataContract(Name = "User_Account")]
    public class UserAccount : ModelItemBase
    {
        public UserAccount(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int? Id
        {
            get { return Row.Get<int?>("ID1") ?? Row.Get<int?>("ID"); }
            set { Row["ID"] = value; }
        }

        [DataMember(Name = "email")]
        public string Email
        {
            get { return Row["Email"].ToString(); }
            set { Row["Email"] = value; }
        }

        [DataMember(Name = "firstname")]
        public string FirstName
        {
            get { return Row["FirstName"].ToString(); }
            set { Row["FirstName"] = value; }
        }

        [DataMember(Name = "lastname")]
        public string LastName
        {
            get { return Row["LastName"].ToString(); }
            set { Row["LastName"] = value; }
        }

        [DataMember(Name = "type")]
        public string Type
        {
            get { return Row.Table.Columns.Contains("UserType_Id") ? GetUserType((int)Row["UserType_Id"]) : "tech"; }
            set { Row["UserType_Id"] = value.ToString(); }
        }

        [DataMember(Name = "fb_staff_id")]
        public int FBStaffID
        {
            get
            {
                if (Row.Table.Columns.Contains("FBStaffID") && !Row.IsNull("FBStaffID"))
                {
                    return Convert.ToInt32(Row["FBStaffID"]);
                }
                return 0;
            }
            set { Row["FBStaffID"] = value; }
        }

        [DataMember(Name = "qb_employee_id")]
        public int QBEmployeeID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBEmployeeID") && !Row.IsNull("QBEmployeeID"))
                {
                    return Convert.ToInt32(Row["QBEmployeeID"]);
                }
                return 0;
            }
            set { Row["QBEmployeeID"] = value; }
        }

        [DataMember(Name = "qb_vendor_id")]
        public int QBVendorID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBVendorID") && !Row.IsNull("QBVendorID"))
                {
                    return Convert.ToInt32(Row["QBVendorID"]);
                }
                return 0;
            }
            set { Row["QBVendorID"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int? account_id
        {
            get { return Row.Get<int?>("company_id"); }
            set { Row["company_id"] = value; }
        }

        [DataMember(Name = "account_name")]
        public string account_name
        {
            get { return Row.GetString("AccountName"); }
            set { Row["AccountName"] = value; }
        }

        [DataMember(Name = "location_id")]
        public int? location_id
        {
            get { return Row.Get<int?>("location_id") ?? Row.Get<int?>("locationid"); }
            set { Row["location_id"] = value; }
        }

        [DataMember(Name = "location_name")]
        public string location_name
        {
            get { return Row.GetString("LocationName"); }
            set { Row["LocationName"] = value; }
        }

        [DataMember(Name = "xero_contact_id")]
        public string Xero_Contact_Id
        {
            get { return Row.GetString("XeroContactId"); }
            set { Row["XeroContactId"] = value; }
        }

        private string GetUserType(int userType)
        {
            switch (userType)
            {
                case 1:
                case 5: return "user";
                case 2: return "tech";
                case 3: return "admin";
            }
            return "queue";
        }
    }
}
