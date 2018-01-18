// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using BWA.Api.Models;

namespace SherpaDeskApi.ServiceInterface
{
    public class ContractsService : Service
    {
        [Secure()]
        public object Get(GET_Contracts request)
        {
            ApiUser hdUser = request.ApiUser;
            bigWebApps.bigWebDesk.CustomNames _cNames = bigWebApps.bigWebDesk.CustomNames.GetCustomNames(hdUser.OrganizationId, hdUser.DepartmentId);
            int accountID = request.account ?? -1;
            int projectID = request.project ?? 0;
            if (accountID > 0)
            {
                DataRow aRow = bigWebApps.bigWebDesk.Data.Accounts.SelectOne(hdUser.DepartmentId, accountID, hdUser.OrganizationId);
                if (aRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Incorrect " + _cNames.Account.FullSingular + " Id.");
                }
                else
                {
                    if (!(bool)aRow["btActive"])
                    {
                        throw new HttpError(HttpStatusCode.NotFound, _cNames.Account.FullSingular + " Is Inactive.");
                    }
                }
            }
            if (projectID > 0)
            {
                DataRow pRow = bigWebApps.bigWebDesk.Data.Project.Select(hdUser.OrganizationId, hdUser.DepartmentId, projectID);
                if (pRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Incorrect Project Id.");
                }
                else
                {
                    if (pRow.IsNull("Active") || !(bool)pRow["Active"])
                    {
                        throw new HttpError(HttpStatusCode.NotFound, "Project Is Inactive.");
                    }
                }
            }
            DateTime date = DateTime.UtcNow;
            if (request.date.HasValue && request.date > DateTime.MinValue)
            {
                date = (DateTime)request.date;
            }
            return request.FilteredResult<Contract>(Contracts.GetContracts(hdUser.OrganizationId, hdUser.DepartmentId, accountID, date, projectID));
        }

        [Secure()]
        public object Get(GET_Contract request)
        {
            ApiUser hdUser = request.ApiUser;
            if (request.prepaid_pack_id > 0)
            {
                DataRow acRow = bigWebApps.bigWebDesk.Data.Accounts.SelectAccountContract(hdUser.OrganizationId, hdUser.DepartmentId, request.prepaid_pack_id);
                if (acRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "No data found.");
                }
                return new Contract(acRow);
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect request.");
            }
        }
    }
}
