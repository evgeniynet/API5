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
    [DataContract(Name = "User_Profile")]
    public class UserProfile : ModelItemBase
    {
        public UserProfile(DataRow row) : base(row) { 
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
            get { return Row.GetString("Email"); }
            set { Row["Email"] = value; }
        }

        [DataMember(Name = "mobile_email_type")]
        public string MobileEmailType
        {
            get { return Row.GetString("MobileEmailType"); }
            set { Row["MobileEmailType"] = value; }
        }

        [DataMember(Name = "mobile_email")]
        public string MobileEmail
        {
            get { return Row.GetString("MobileEmail"); }
            set { Row["MobileEmail"] = value; }
        }

        [DataMember(Name = "title")]
        public string Title
        {
            get { return Row.GetString("Title"); }
            set { Row["Title"] = value; }
        }

        [DataMember(Name = "phone")]
        public string Phone
        {
            get { return Row.GetString("phone"); }
            set { Row["phone"] = value; }
        }

        [DataMember(Name = "mobile_phone")]
        public string MobilePhone
        {
            get { return Row.GetString("MobilePhone"); }
            set { Row["MobilePhone"] = value; }
        }

        [DataMember(Name = "skype")]
        public string Skype
        {
            get { return Row.GetString("Skype"); }
            set { Row["Skype"] = value; }
        }

        [DataMember(Name = "firstname")]
        public string FirstName
        {
            get { return Row.GetString("firstname"); }
            set { Row["firstname"] = value; }
        }

        [DataMember(Name = "lastname")]
        public string LastName
        {
            get { return Row["lastname"].ToString(); }
            set { Row["lastname"] = value; }
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
            get { return Row.GetString("ProfileName"); }
            set { Row["ProfileName"] = value; }
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

        public static UserProfile GetProfile(Guid OrgId, int DeptId, int UserId)
        {
            DataRow rUser = bigWebApps.bigWebDesk.Data.Logins.SelectUserDetails(OrgId, DeptId, UserId);
            if (rUser == null)
                return null;

            UserProfile _user = new UserProfile(rUser);
            return _user;
        }

        public static int UpdateProfile(Guid OrgId, int DeptId, int UserId, string phone, string mobile_phone, string skype)
        {
            //UserProfile user = GetProfile(OrgId, DeptId, UserId);
            //if (user == null)
            //    return -1;

            int result = bigWebApps.bigWebDesk.Data.Logins.UpdateProfileLite(OrgId, DeptId,
                            UserId, phone, mobile_phone, skype);

            /*
             if (result == 1)
            {
                vldCstUniqueEmail.IsValid = false;
                return;
            }
            */
            return result;
        }
    }
}
