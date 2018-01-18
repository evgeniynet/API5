// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;
using ServiceStack.Common.Web;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description of AccountProjects
   /// </summary>
    [DataContract(Name = "AccountProjects")]
    public class AccountProjects : ModelItemCollectionGeneric<Account_Project>
    {
        public AccountProjects(DataTable AccountProjectsTable) : base(AccountProjectsTable) { }

        public static List<Account_Project> GetAccountProjects(Guid organizationId, int departmentId, int accountId)
        {
            AccountProjects _AccountProjects = new AccountProjects(bigWebApps.bigWebDesk.Data.Accounts.SelectProjects(organizationId, departmentId, accountId));
            return _AccountProjects.ToList();
        }

    }
}
