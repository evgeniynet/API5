// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;

namespace BWA.Api.Models
{
    [DataContract(Name = "account_users")]
    public class AccountUsers : ModelItemCollectionGeneric<AccountUser>
    {
        public AccountUsers(DataTable AccountUsersTable) : base(AccountUsersTable) { }

        public static List<AccountUser> GetAccountUsers(Guid organizationId, int departmentId, int accountId)
        {
            AccountUsers accountUsers = new AccountUsers(bigWebApps.bigWebDesk.Data.Accounts.SelectUsers(organizationId, departmentId, accountId, "", false));           
            return accountUsers.List;
        }
    }
}
