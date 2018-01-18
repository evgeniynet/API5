// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using bigWebApps.bigWebDesk;
using System.Net;
using ServiceStack;
using SherpaDeskApi.ServiceModel;

namespace BWA.Api.Models
{
    /// <summary>
    /// Summary description for Class
    /// </summary>
    [DataContract(Name = "Account_Details")]
    public class Account_Details : ModelItemBase
    {
        public Account_Details(DataRow row)
            : base(row)
        {
            if (row == null)
                throw new HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int? Id
        {
            get
            {
                return (Row.Get<int?>("AccountId") ?? Row.Get<int?>("Id")) ?? -1;
            }
            set
            {
                if (Row.Table.Columns.Contains("AccountId"))
                    Row["AccountId"] = value;
                else if (Row.Table.Columns.Contains("Id"))
                    Row["Id"] = value;
            }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get
            {
                return HttpUtility.HtmlDecode(Row.GetString("AcctName")) ?? HttpUtility.HtmlDecode(Row.GetString("Name"));
            }
            set
            {
                if (Row.Table.Columns.Contains("AcctName"))
                    Row["AcctName"] = value;
                else if (Row.Table.Columns.Contains("Name"))
                    Row["Name"] = value;
            }
        }

        [DataMember(Name = "note")]
        public string Note
        {
            get { return Row.GetString("txtNote"); }
            set
            {
                Row["txtNote"] = value;
            }
        }

        [DataMember(Name = "is_active")]
        public bool? Is_Active
        {
            get { return Row.Get<bool?>("btActive"); }
            set
            {
                Row["btActive"] = value;
            }
        }

        [DataMember(Name = "is_organization")]
        public bool? Is_Organization
        {
            get { return Row.Get<bool?>("btOrgAcct"); }
            set
            {
                Row["btOrgAcct"] = value;
            }
        }

        [DataMember(Name = "bwd_number")]
        public int? Bwd_Number
        {
            get { return Row.Get<int?>("intBWDAcctNum"); }
            set { Row["intBWDAcctNum"] = value; }
        }

        [DataMember(Name = "client_contract_id")]
        public int? ClientContractID
        {
            get { return Row.Get<int?>("ClientContractID"); }
            set { Row["ClientContractID"] = value; }
        }

        [DataMember(Name = "number")]
        public string Number
        {
            get { return Row.GetString("vchAcctNum"); }
            set { Row["vchAcctNum"] = value; }
        }

        [DataMember(Name = "ref1")]
        public string Ref1
        {
            get { return Row.GetString("vchRef1Num"); }
            set
            {
                Row["vchRef1Num"] = value;
            }
        }

        [DataMember(Name = "ref2")]
        public string Ref2
        {
            get { return Row.GetString("vchRef2Num"); }
            set
            {
                Row["vchRef2Num"] = value;
            }
        }

        [DataMember(Name = "representative_name")]
        public string Representative_Name
        {
            get { return Row.GetString("AcctRepName"); }
            set
            {
                Row["AcctRepName"] = value;
            }
        }

        [DataMember(Name = "internal_location_name")]
        public string Internal_Location_Name
        {
            get { return Row.GetString("LocationName"); }
            set
            {
                Row["LocationName"] = value;
            }
        }

        [DataMember(Name = "city")]
        public string City
        {
            get { return Row.GetString("City"); }
            set
            {
                Row["City"] = value;
            }
        }

        [DataMember(Name = "state")]
        public string State
        {
            get { return Row.GetString("State"); }
            set
            {
                Row["State"] = value;
            }
        }

        [DataMember(Name = "zipcode")]
        public string ZipCode
        {
            get { return Row.GetString("ZipCode"); }
            set
            {
                Row["ZipCode"] = value;
            }
        }

        [DataMember(Name = "country")]
        public string Country
        {
            get { return Row.GetString("Country"); }
            set
            {
                Row["Country"] = value;
            }
        }

        [DataMember(Name = "phone1")]
        public string Phone1
        {
            get { return Row.GetString("Phone1"); }
            set
            {
                Row["Phone1"] = value;
            }
        }

        [DataMember(Name = "phone2")]
        public string Phone2
        {
            get { return Row.GetString("Phone2"); }
            set
            {
                Row["Phone2"] = value;
            }
        }

        [DataMember(Name = "address1")]
        public string Address1
        {
            get { return Row.GetString("Address1"); }
            set
            {
                Row["Address1"] = value;
            }
        }

        [DataMember(Name = "address2")]
        public string Address2
        {
            get { return Row.GetString("Address2"); }
            set
            {
                Row["Address2"] = value;
            }
        }

        [DataMember(Name = "email_suffix")]
        public string Email_Suffix
        {
            get { return Row.GetString("vchEmailSuffix"); }
            set
            {
                Row["vchEmailSuffix"] = value;
            }
        }

        [DataMember(Name = "fb_client_id")]
        public int FB_Client_Id
        {
            get
            {
                return Row.Get<int>("FBClientId");
            }
            set
            {
                Row["FBClientId"] = value;
            }
        }

        [DataMember(Name = "qb_customer_id")]
        public int QB_Customer_Id
        {
            get
            {
                return Row.Get<int>("QBCustomerId");
            }
            set
            {
                Row["QBCustomerId"] = value;
            }
        }

        [DataMember(Name = "xero_contact_id")]
        public string Xero_Contact_Id
        {
            get {
                return Row.GetString("XeroContactId");
            }
            set
            {
                Row["XeroContactId"] = value;
            }
        }

        [DataMember(Name = "logo")]
        public string Logo
        { get; set; }

        [DataMember(Name = "files")]
        public List<File> Files { get; set; }

        [DataMember(Name = "locations")]
        public List<Location> Locations { get; set; }

        [DataMember(Name = "users")]
        public List<AccountUser> Users { get; set; }

        [DataMember(Name = "projects")]
        public List<Account_Project> Projects { get; set; }

        [DataMember(Name = "assets")]
        public List<Asset> Assets { get; set; }

        [DataMember(Name = "account_statistics")]
        public AccountStats account_statistics { get; set; }

        [DataMember(Name = "primary_contact")]
        public Address Primary_Contact { get; set; }

        [DataMember(Name = "customfields")]
        public List<KeyValuePair<string, string>> CustomFields { get; set; }

        public static Account_Details GetAccountDetails(ApiUser user, int accountId, bool is_with_statistics)
        {
            Models.Account_Details account = null;
            Instance_Config config = new Instance_Config(user);
            DataRow accountDetails = bigWebApps.bigWebDesk.Data.Accounts.SelectOne(user.DepartmentId, accountId, user.OrganizationId);
            if (accountDetails != null) account = new Account_Details(accountDetails);
            else if (accountId == -1 || accountId == 0)
            {
                DataTable table = new DataTable();
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Id", typeof(int));

                account = new Account_Details(table.NewRow());
                account.Name = user.DepartmentName;
                account.Id = -1;
            }
            else
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect account id");
            if (config.LocationTracking)
            {
                account.Locations = new Locations(bigWebApps.bigWebDesk.Data.Accounts.SelectLocations(user.OrganizationId, user.DepartmentId, accountId)).List;
            }

            /*
            if (account.ClientContractID.HasValue)
            {
                DataRow row = bigWebApps.bigWebDesk.Data.Accounts.SelectAccountContract(user.OrganizationId, user.DepartmentId, account.ClientContractID.Value);
                account.contract_renewal_date = Functions.DisplayDate((DateTime)row["EndDate"], 0, false);
            }
            */

            account.Primary_Contact = Address.GetAccountAddress(user.OrganizationId, user.DepartmentId, accountId);

            account.Users = AccountUsers.GetAccountUsers(user.OrganizationId, user.DepartmentId, accountId);

           //Assets
            if (config.AssetTracking)
            {
                account.Assets = Models.Assets.AccountAssets(user, config.Assets, accountId);
            }

            if (config.ProjectTracking)
            {
                account.Projects = AccountProjects.GetAccountProjects(user.OrganizationId, user.DepartmentId, accountId);
            }

           //Custom fields
            if (accountId > 0)
            {
                List<KeyValuePair<string, string>> customFields = new List<KeyValuePair<string, string>>();
                for (int i = 1; i < 16; i++)
                    if ((bool)accountDetails["btCust" + i.ToString() + "On"])
                        customFields.Add(new KeyValuePair<string, string>(accountDetails["vchCust" + i.ToString() + "Cap"].ToString(), accountDetails["vchCust" + i.ToString()].ToString()));

                if (!accountDetails.IsNull("btDateCust1On") && (bool)accountDetails["btDateCust1On"]) customFields.Add(new KeyValuePair<string, string>(accountDetails["vchDateCust1Cap"].ToString(), accountDetails.IsNull("dtCust1") ? "" : Functions.DisplayDate((DateTime)accountDetails["dtCust1"], user.TimeZoneOffset, false)));
                if (!accountDetails.IsNull("btDateCust2On") && (bool)accountDetails["btDateCust2On"]) customFields.Add(new KeyValuePair<string, string>(accountDetails["vchDateCust2Cap"].ToString(), accountDetails.IsNull("dtCust2") ? "" : Functions.DisplayDate((DateTime)accountDetails["dtCust2"], user.TimeZoneOffset, false)));

                account.CustomFields = customFields.ToList();
            }

            account.Files = Models.Files.GetFiles(user.InstanceId, accountId, user.OrganizationId, "accounts-accounts-files");

            string accountLogoUrl = "";
            if (accountId > 0)
            {
                accountLogoUrl = Models.Files.GetAccountLogoUrl(user.InstanceId, accountId, user.OrganizationId);
            }
            else
            {
                accountLogoUrl = string.IsNullOrEmpty(user.InstanceLogoImageUrl) ? user.OrganizationLogoImageUrl : user.InstanceLogoImageUrl;
            }

            account.Logo = accountLogoUrl;

            if (is_with_statistics)
                account.account_statistics = AccountStats.GetStatistics(accountId, user.OrganizationId, user.DepartmentId, user.UserId);

            return account;
        }
    }

    [DataContract(Name = "account_statistics")]
    public class AccountStats
    {
        [DataMember(Name = "ticket_counts")]
        public AccountTicketCount ticket_counts { get; set; }

        [DataMember(Name = "timelogs")]
        public int timelogs { get; set; }

        [DataMember(Name = "invoices")]
        public int invoices { get; set; }

        [DataMember(Name = "hours")]
        public decimal? hours { get; set; }

        [DataMember(Name = "expenses")]
        public decimal? expenses { get; set; }

        public static AccountStats GetStatistics(int accountId, Guid organizationId, int departmentId, int userId)
        {
            var account_stat = new AccountStats();
            account_stat.ticket_counts = new AccountTicketCount(bigWebApps.bigWebDesk.Data.Accounts.SelecttTicketStats(organizationId, departmentId, accountId));
           //List<CommonTimeLog> timelogs = Models.CommonTimeLogs.GetCommonTimeLog (organizationId, departmentId, "recent", accountId, 0, 0, null, null);
           //account_stat.timelogs = timelogs.Count;
           //List<Invoice> invoices = Models.Invoices.GetInvoices (organizationId, departmentId, userId, DateTime.UtcNow.Date.AddDays (-365), DateTime.UtcNow.Date.AddDays (1).AddSeconds (-1), null, accountId);
            string query = string.Format("SELECT count(*) FROM Invoice "
               //+ "INNER JOIN tbl_company ON tbl_company.company_id = {0} "
               //+ "LEFT OUTER JOIN Accounts A ON A.DId={0} AND A.Id=I.AccountId "
                + "WHERE DId={0} "
                + "AND Date >= '{2}' AND Date <= '{3}' AND Archived = 0 AND AccountID {4} "
                , departmentId, userId, DateTime.UtcNow.Date.AddDays(-365).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"), accountId < 1 ? " is NULL " : (" = " + accountId));
            var invoices = bigWebApps.bigWebDesk.Data.DBAccess.SelectByQuery(query, organizationId);
            account_stat.invoices = Convert.ToInt32(invoices.Rows[0][0]);
            account_stat.hours = account_stat.ticket_counts.Hours;

            query = string.Format("SELECT SUM(Amount) "
                    + "FROM Expense "
                    + "WHERE DId = {0} AND InvoiceId IS NULL "
                    + "AND (ISNULL(AccountId, -1) = {1}) "
                    , departmentId, accountId < 1 ? -1 : accountId);
            var expenses = bigWebApps.bigWebDesk.Data.DBAccess.SelectByQuery(query, organizationId);
            decimal sum = 0;
            if (expenses.Rows[0][0] != DBNull.Value)
                sum = (decimal)expenses.Rows[0][0];
            account_stat.expenses = sum;
            return account_stat;
        }
    }
}
