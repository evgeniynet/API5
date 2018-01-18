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
    [DataContract(Name = "Account_User")]
    public class AccountUser : ModelItemBase
    {
        [DataMember(Name = "id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["ID"]); }
            set { Row["ID"] = value; }
        }

        public AccountUser(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "email")]
        public string Email
        {
            get { return Row["vchEmail"].ToString(); }
            set { Row["vchEmail"] = value; }
        }

        [DataMember(Name = "fullname")]
        public string FullName
        {
            get { return Row["vchFullName"].ToString(); }
            set { Row["vchFullName"] = value; }
        }

        [DataMember(Name = "phone")]
        public string LastName
        {
            get { return Row["vchPhone"].ToString(); }
            set { Row["vchPhone"] = value; }
        }

        [DataMember(Name = "type")]
        public string Type
        {
            get { return GetUserType(Row["vchUserType"].ToString()); }
            set { Row["vchUserType"] = value; }
        }

        [DataMember(Name = "is_accounting_contact")]
        public bool AccountingContact
        {
            get { return (bool)Row["AccountingContact"]; }
            set { Row["AccountingContact"] = value; }
        }  
        
        private string GetUserType(string userType)
        {
            switch (userType)
            {
                case "Standard User": return "user";
                case "Super User": return "superuser";
                case "Administrator": return "admin";
            }
            return "tech";
        }
    }
}
