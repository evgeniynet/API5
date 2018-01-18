// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;
using System.Net;
using bigWebApps.bigWebDesk.Data;

namespace BWA.Api.Models
{
    static class DataRowExtensions
    {
        public static T Get<T>(this DataRow row, string column, T defaultValue = default(T))
        {
            if (row == null)
                return default(T);
            if (row.Table.Columns.Contains(column))
                if (!row.IsNull(column))
                    return row.Field<T>(column);
                else
                    return defaultValue;
            return default(T);
        }

        public static string GetString(this DataRow row, string column, string defaultValue = null)
        {
            if (row == null)
                return string.Empty;
            return row.Table.Columns.Contains(column) ? (!row.IsNull(column) ? row[column].ToString() : defaultValue) : null;
        }

        public static void Set<T>(this DataRow row, string column, T value)
        {
            if (row == null)
                return;
            if (row.Table.Columns.Contains(column) && row.Table.Columns[column].DataType == typeof(T))
            {
                row.SetField<T>(column, value);
            }
        }

        public static void AddDetails(this Models.Invoice invoice, Guid organizationId, int departmentId, DateTime startDate, DateTime endDate, bool isInvoiced = false, string time_logs = "", string travel_logs = "", string expenses = "", string retainers = "", decimal adjustments = 0, string adjustments_note = "")
        {
                if (isInvoiced)
                {
                    DataTable dtTime = bigWebApps.bigWebDesk.Data.Invoice.SelectInvoiceTimeLogs(organizationId, departmentId, invoice.Id.Value);
                    invoice.time_logs = new InvoiceDetails(dtTime).List;
                    DataTable dtTravelTime = bigWebApps.bigWebDesk.Data.Invoice.SelectInvoiceTicketTravelCosts(organizationId, departmentId, invoice.Id.Value);
                    invoice.travel_logs = new InvoiceDetails(dtTravelTime).List;
                    DataTable dtExpense = bigWebApps.bigWebDesk.Data.Invoice.SelectInvoiceExpenses(organizationId, departmentId, invoice.Id.Value);
                    invoice.expenses = new InvoiceDetails(dtExpense).List;
                    DataTable dtRetainers = bigWebApps.bigWebDesk.Data.Invoice.SelectInvoiceRetainers(organizationId, departmentId, invoice.Id.Value);
                    dtRetainers.Columns.Add("Id", typeof(int));
                    int inc = 1;
                    foreach (DataRow r in dtRetainers.Rows)
                        r["Id"] = inc++;
                    invoice.retainers = new InvoiceDetails(dtRetainers).List;
                }
                else
                {
                    DateTime invoiceStartDate = endDate, invoiceEndDate = startDate;
                   //Time
                    DataTable dtTime = bigWebApps.bigWebDesk.Data.Invoice.SelectTimeForInvoice(organizationId, departmentId, startDate, endDate, invoice.AccountId, invoice.ProjectId, true);
                    bool isCorrected = CorrectDetails(ref dtTime, time_logs);
                    invoice.time_logs = new InvoiceDetails(dtTime).List;
                    decimal total_hours = 0, amount = 0, total_amount = 0;
                    for (int i = 0; i < dtTime.Rows.Count; i++)
                    {
                        CalculteInvoiceRange(invoice.time_logs[i].date, ref invoiceStartDate, ref invoiceEndDate);
                        if (isCorrected)
                        {
                            decimal hours = dtTime.Rows[i].Get<decimal>("Hours");
                            total_hours += hours;
                            amount += hours * dtTime.Rows[i].Get<decimal>("BillRate");
                        }
                    }
                    if (isCorrected)
                    {
                        invoice.Amount = amount;
                        invoice.TotalHours = total_hours;
                        total_amount += amount;
                    }
                   //Travel
                    DataTable dtTravelTime = bigWebApps.bigWebDesk.Data.Invoice.SelectTravelTimeForInvoice(organizationId, departmentId, invoice.AccountId,
                                    invoice.ProjectId, startDate, endDate);
                    isCorrected = CorrectDetails(ref dtTravelTime, travel_logs);
                    invoice.travel_logs = new InvoiceDetails(dtTravelTime).List;
                    decimal? travel_cost = 0;
                    for (int i = 0; i < dtTravelTime.Rows.Count; i++)
                    {
                        CalculteInvoiceRange(invoice.travel_logs[i].date, ref invoiceStartDate, ref invoiceEndDate);
                        if (isCorrected) travel_cost += invoice.travel_logs[i].total;
                    }
                    if (isCorrected)
                    {
                        invoice.TravelCost = travel_cost.Value;
                        total_amount += travel_cost.Value;
                    }
                   //Expenses
                    DataTable dtExpense = bigWebApps.bigWebDesk.Data.Invoice.SelectExpenseForInvoice(organizationId, departmentId, invoice.AccountId, invoice.ProjectId, startDate, endDate);
                    if (!string.IsNullOrWhiteSpace(expenses))
                    {
                        isCorrected = CorrectDetails(ref dtExpense, "Convert('" + expenses.Replace(",", "', 'System.Guid'),Convert('") + "', 'System.Guid')");
                    }
                    else
                        isCorrected = false;
                    invoice.expenses = new InvoiceDetails(dtExpense).List;
                    decimal? misc_cost = 0;
                    foreach (var _invoice in invoice.expenses)
                    {
                        CalculteInvoiceRange(_invoice.date, ref invoiceStartDate, ref invoiceEndDate);
                        if (isCorrected) misc_cost += _invoice.total;
                    }
                    if (isCorrected)
                    {
                        invoice.MiscCost = misc_cost.Value;
                        total_amount += misc_cost.Value;
                    }

                    DataTable dtRetainers = bigWebApps.bigWebDesk.Data.Invoice.SelectRetainersForInvoice(organizationId, departmentId, startDate, endDate,
                                        invoice.AccountId, invoice.ProjectId, true);
                    dtRetainers.Columns.Add("Id", typeof(int));
                    int inc = 1;
                    foreach (DataRow r in dtRetainers.Rows)
                        r["Id"] = inc++;
                    isCorrected = CorrectDetails(ref dtRetainers, retainers);
                    invoice.retainers = new InvoiceDetails(dtRetainers).List;
                    decimal? retainer = 0;
                    foreach (var _invoice in invoice.retainers)
                    {
                        CalculteInvoiceRange(_invoice.date, ref invoiceStartDate, ref invoiceEndDate);
                        if (isCorrected) retainer += _invoice.total;
                    }
                    if (isCorrected)
                    {
                        total_amount += retainer.Value;
                    }
                    if (adjustments != 0)
                    {
                        total_amount += adjustments;
                        invoice.adjustments = adjustments;
                    }
                    invoice.adjustments_note = adjustments_note;
                    if (total_amount > 0 && invoice.TotalCost != total_amount)
                        invoice.TotalCost = total_amount;
                    invoice.BeginDate = invoiceStartDate.Date;
                    invoice.EndDate = invoiceEndDate;
                }
        }

        public static bool CorrectDetails(ref DataTable details, string logs)
        {
            if (string.IsNullOrWhiteSpace(logs))
                return false;
            DataView dw = details.DefaultView;
            dw.RowFilter = "Id NOT IN (" + logs + ")";
            details = dw.ToTable();
            return true;
        }

        static void CalculteInvoiceRange(DateTime? dateTime, ref DateTime StartDate, ref DateTime EndDate)
        {
            if (!dateTime.HasValue)
                return;
            if (dateTime < StartDate)
            {
                StartDate = dateTime.Value;
            }
            if (dateTime > EndDate)
            {
                EndDate = dateTime.Value;
            }
        }
    }

   /// <summary>
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "Invoice")]
    public class Invoice : ModelItemBase
    {
        public Invoice(DataRow row) : base(row) { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int? Id
        {
            get { return Row.Get<int?>("Id"); }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "key")]
        public string Key
        {
            get { return Row.Get<string>("PseudoId"); }
            set { Row["PseudoId"] = value; }
        }

        [DataMember(Name = "project_id")]
        public int ProjectId
        {
            get {
                int? project_id = Row.Get<int?>("ProjectId", -1);
                if (!project_id.HasValue)
                {
                    string complexId = Row.Get<string>("ComplexID");
                    if (!string.IsNullOrEmpty(complexId))
                    {
                        int projectId = 0;
                        if (int.TryParse(complexId.Split('|')[1], out projectId))
                            project_id = projectId;
                    }
                    else
                        project_id = -1;
                }
                return project_id.Value;
            }
            set { Row["ProjectId"] = value; }
        }

        [DataMember(Name = "account_id")]
        public int AccountId
        {
            get
            {
                int? account_id = Row.Get<int?>("AccountId", -1);
                if (!account_id.HasValue)
                {
                    string complexId = Row.Get<string>("ComplexID");
                    if (!string.IsNullOrEmpty(complexId))
                    {
                        int accountId = 0;
                        if (int.TryParse(complexId.Split('|')[0], out accountId))
                            account_id = accountId;
                    }
                    else
                        account_id = -1;
                }
                return account_id.Value;
            }
            set { Row["AccountId"] = value; }
        } 

        [DataMember(Name = "customer")]
        public string Customer
        {
            get { return Row["Customer"].ToString(); }
            set { Row["Customer"] = value; }
        }
        
        [DataMember(Name = "date")]
        public DateTime? Date
        {
            get
            {
                return Row.Get<DateTime?>("Date");
            }
            set
            {
                if (Row.Table.Columns.Contains("Date")) Row["Date"] = value;
            }
        }

        private DateTime? _BeginDate;

        [DataMember(Name = "start_date")]
        public DateTime? BeginDate
        {
            get
            {
                _BeginDate = _BeginDate ?? Row.Get<DateTime?>("BeginDate") ?? Row.Get<DateTime?>("LastRangeBegin");
                return _BeginDate;
            }
            set
            {
                _BeginDate = value;
            }
        }

        DateTime? _EndDate;
        [DataMember(Name = "end_date")]
        public DateTime? EndDate
        {
            get
            {
                _EndDate = _EndDate ?? (Row.Get<DateTime?>("EndDate") ?? Row.Get<DateTime?>("LastRangeEnd"));
                return _EndDate;
            }
            set
            {
                _EndDate = value;
            }
        }

		[DataMember(Name = "timelogs_count")]
		public int TimeLogsCount
		{
			get { return Row.Get<int>("TimeLogsCount"); }
			set { Row["TimeLogsCount"] = value; }
		}

        decimal? _total_hours = null;

        [DataMember(Name = "total_hours")]
        public decimal TotalHours
        {
            get { return _total_hours ?? Row.Get<decimal>("TotalHours"); }
            set { _total_hours = value; Row.Set<decimal>("TotalHours", value); }
        }

        decimal? _amount = null;

        [DataMember(Name = "amount")]
        public decimal Amount
        {
            get { return _amount ?? Row.Get<decimal>("Amount"); }
            set
            {
                _amount = value;
                Row.Set<decimal>("Amount", value);
            }
        }

        decimal? _travel_cost = null;
        [DataMember(Name = "travel_cost")]
        public decimal TravelCost
        {
            get { return _travel_cost ?? Row.Get<decimal>("TravelCost"); }
            set
            {
                _travel_cost = value;
                Row.Set<decimal>("TravelCost", value);
            }
        }

        decimal? _misc_cost = null;

        [DataMember(Name = "misc_cost")]
        public decimal MiscCost
        {
            get { return _misc_cost ?? Row.Get<decimal>("MiscCost"); }
            set
            {
                _misc_cost = value;
                Row.Set<decimal>("MiscCost", value);
            }
        }

        decimal? _total_cost = null;

        [DataMember(Name = "total_cost")]
        public decimal TotalCost
        {
            get
            {
                if (_total_cost.HasValue)
                    return _total_cost.Value;
                decimal miscCost = Row.Get<decimal>("MiscCost");
                decimal? totalCost = Row.Get<decimal?>("TotalCost") ?? (Row.Get<decimal>("Amount") + Row.Get<decimal>("TravelCost"));
				return totalCost.Value + miscCost;
            }
            set
            {
                _total_cost = value;
                Row.Set<decimal>("TotalCost", value);
            }
        }

        decimal? _adjustments = null;

        [DataMember(Name = "adjustments")]
        public decimal adjustments
        {
            get { return _adjustments ?? Row.Get<decimal>("Adjustments"); }
            set
            {
                _adjustments = value;
                Row.Set<decimal>("Adjustments", value);
            }
        }

        string _adjustments_note = null;

        [DataMember(Name = "adjustments_note")]
        public string adjustments_note
        {
            get { return _adjustments_note ?? Row.GetString("AdjustmentsNote"); }
            set
            {
                _adjustments_note = value;
                Row.Set<string>("AdjustmentsNote", value);
            }
        }

        [DataMember(Name = "is_fb_exported")]
        public bool? FBExported
        {
            get { return Row.Get<bool?>("FBExported"); }
            set { Row["FBExported"] = value; }
        }

        [DataMember(Name = "is_qb_exported")]
        public bool? QBExported
        {
            get { return Row.Get<bool?>("QBExported"); }
            set { Row["QBExported"] = value; }
        }

        [DataMember(Name = "is_no_rate_plan")]
        public bool? is_no_rate_plan
        {
            get { return Row.Get<bool?>("NoRatePlan"); }
            set { Row["NoRatePlan"] = value; }
        }

        [DataMember(Name = "account_name")]
        public string AccountName
        {
            get
            {
				return Row.GetString ("AccountName");
            }
            set { Row["AccountName"] = value; }
        }

		[DataMember(Name = "project_name")]
		public string ProjectName
		{
			get
			{
				return Row.GetString ("ProjectName");
			}
			set { Row["ProjectName"] = value; }
		}

        [DataMember(Name = "qb_invoice_id")]
        public int QBInvoiceID
        {
            get
            {
				return Row.Get<int>("QBInvoiceID");
            }
            set { Row["QBInvoiceID"] = value; }
        }

        [DataMember(Name = "qb_customer_id")]
        public int QBCustomerId
        {
            get
            {
				return Row.Get<int>("QBCustomerId");
            }
            set { Row["QBCustomerId"] = value; }
        }

        [DataMember(Name = "xero_invoice_id")]
        public string XeroInvoiceID
        {
            get { return Row.GetString("XeroInvoiceID"); }
            set { Row["XeroInvoiceID"] = value; }
        }

        [DataMember(Name = "xero_contact_id")]
        public string Xero_Contact_Id
        {
            get { return Row.GetString("XeroContactId"); }
            set { Row["XeroContactId"] = value; }
        }

        [DataMember(Name = "recipients")]
        public List<AccountUser> recipients { get; set; }

        [DataMember(Name = "time_logs")]
        public List<InvoiceDetail> time_logs { get; set; }

        [DataMember(Name = "travel_logs")]
        public List<InvoiceDetail> travel_logs { get; set; }
        
        [DataMember(Name = "expenses")]
        public List<InvoiceDetail> expenses { get; set; }

        [DataMember(Name = "retainers")]
        public List<InvoiceDetail> retainers { get; set; }

        internal static Models.Invoice GetInvoice(Guid organizationId, int departmentId, string invoiceId, bool isFullDetails = true)
        {
            int invoice_id = 0;
            if (!int.TryParse(invoiceId, out invoice_id))
            {
                DataRow iRow = bigWebApps.bigWebDesk.Data.Invoice.SelectInvoiceByPseudoId(organizationId, departmentId, invoiceId);
                if (iRow != null)
                {
                    invoice_id = int.Parse(iRow["Id"].ToString());
                }
            }
            if (invoice_id > 0)
            {
                DataRow iRow = bigWebApps.bigWebDesk.Data.Invoice.SelectInvoice(organizationId, departmentId, invoice_id);
                if (iRow != null)
                {
                    var invoice = new Invoice(iRow);
					invoice.recipients = AccountUsers.GetAccountUsers(organizationId, departmentId, invoice.AccountId);
                    if (isFullDetails) invoice.AddDetails(organizationId, departmentId, DateTime.UtcNow.AddDays(-365), DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1), true);
                    return invoice;
                }
            }
            throw new HttpError(HttpStatusCode.NotFound, "Invoice not found");
        }

        internal static Models.Invoice CreateInvoice(ApiUser hdUser, int AccountId, int ProjectId, DateTime? StartDate, DateTime? EndDate, string time_logs = "", string travel_logs = "", string expenses = "", string retainers = "", decimal adjustments = 0, string adjustments_note = "")
        {
            Guid organizationId = hdUser.OrganizationId;
            int departmentId = hdUser.DepartmentId;
            int userId = hdUser.UserId;
            DateTime startDate, endDate;
            if (!StartDate.HasValue || !EndDate.HasValue)
            {
                bigWebApps.bigWebDesk.Data.Invoice.CalculateStartSopDates(organizationId, departmentId, out startDate, out endDate);

            }
            else
            {
                startDate = StartDate.Value;
                endDate = EndDate.Value;
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            int invoiceID = bigWebApps.bigWebDesk.Data.Invoice.CreateInvoice(organizationId, departmentId, userId, AccountId, ProjectId, startDate, endDate, true, true, false);
            if (invoiceID > 0)
            {
                DataRow iRow = bigWebApps.bigWebDesk.Data.Invoice.SelectInvoice(organizationId, departmentId, invoiceID);
                if (iRow != null)
                {
                    int billingMethodId = 0;
                    if (iRow["BillingMethodID"] != DBNull.Value)
                    {
                        billingMethodId = int.Parse(iRow["BillingMethodID"].ToString());
                    }
                    if (billingMethodId == 6)
                    {
                        bigWebApps.bigWebDesk.Data.Invoice.UpdateInvoiceBilledHours(organizationId, departmentId, invoiceID);
                        bigWebApps.bigWebDesk.Data.Invoice.UpdateAccumulatedHours(organizationId, departmentId, AccountId, ProjectId, invoiceID);
                    }
                    bigWebApps.bigWebDesk.Data.Invoice.UpdateInvoiceContractsBilledHours(organizationId, departmentId, invoiceID);
                    var invoice = new Invoice(iRow);
                   //invoice.AddDetails(organizationId, departmentId, startDate, endDate, true, time_logs, travel_logs, expenses, retainers, adjustments, adjustments_note);
                    return invoice;
                }
            }
            throw new HttpError(HttpStatusCode.NotFound, "");
        }

        internal static object SendInvoice(ApiUser hdUser, string invoice_id, string recipients, bool isPDFOnly = false)
        {
            Guid organizationId = hdUser.OrganizationId;
            int departmentId = hdUser.DepartmentId;
            int userId = hdUser.UserId;
            string Email = hdUser.LoginEmail;
            string userName = hdUser.FullName;
            string department = hdUser.DepartmentName;
            Models.Invoice invoice = GetInvoice(organizationId, departmentId, invoice_id, false);
            int AccountId = invoice.AccountId;
            List<int> intUserIds = new List<int>();
            string[] emails = recipients.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
            recipients = "";
            invoice.recipients = AccountUsers.GetAccountUsers(organizationId, departmentId, invoice.AccountId);
            foreach (string email in emails)
            {
                if (!Utils.IsValidEmail(email))
                {
                    continue;
                }

                bool isAccountingContact = true;
                int userID = 0;
                string new_email = "";
                AccountUser user = invoice.recipients.Find(r => r.Email.ToLower() == email.ToLower());
                if (user != null)
                {
                    isAccountingContact = user.AccountingContact;
                    userID = user.Id;
                    new_email = user.Email;
                }
                else
                {
                    userId = bigWebApps.bigWebDesk.Data.Accounts.InsertUserIntoAccount(hdUser.OrganizationId, hdUser.DepartmentId, AccountId, email, 0, false);
                    new_email = email;
                    isAccountingContact = false;
                }
                if (!isAccountingContact)
                    bigWebApps.bigWebDesk.Data.Accounts.UpdateAccountContact(hdUser.OrganizationId, hdUser.DepartmentId, AccountId, userID, true);
                recipients += new_email + ";";
            }

            int ProjectId = invoice.ProjectId;
            int invoiceID = invoice.Id.Value;
            string subject = invoice.Customer + " | Invoice #" + invoiceID;
            string from = "\"" + userName + " - " + department + "\"<" + Email + ">";
   
            if (!string.IsNullOrWhiteSpace(recipients))
            {
                Instance_Config instanceConfig = new Models.Instance_Config(hdUser);
                string currency = string.IsNullOrWhiteSpace(instanceConfig.Currency) ? "$" : instanceConfig.Currency;
                try
                {
                    string filename = "Invoice-" + invoiceID.ToString() + ".pdf";
                    string logoURL = string.Empty;
                    string logoImageUrl = Files.GetOrganizationLargeLogoUrl(organizationId);
                    if (!String.IsNullOrEmpty(logoImageUrl))
                        logoURL = logoImageUrl;
                    logoImageUrl = Files.GetInstanceLargeLogoUrl(hdUser.InstanceId);
                    if (!String.IsNullOrEmpty(logoImageUrl))
                        logoURL = logoImageUrl;
                    byte[] pdfBytes = null;
                    string body = "";
                    try
                    {
                        pdfBytes = bigWebApps.bigWebDesk.Data.Invoice.ExportPDF(organizationId, hdUser.InstanceId, departmentId, userId, invoiceID, "https://app.sherpadesk.com", currency,
                            instanceConfig.Names.tech.a, instanceConfig.Names.ticket.a, instanceConfig.ProjectTracking, instanceConfig.Names.user.a, instanceConfig.QBUseQBInvoiceNumber, logoURL);
                        if (isPDFOnly)
                        {
                            var sfile = new System.IO.MemoryStream(pdfBytes);
                            return new BWA.bigWebDesk.Api.Services.FilesService.FileResult(sfile, "application/pdf", filename);
                        }
                        body = bigWebApps.bigWebDesk.Data.Invoice.ExportHtml(organizationId, hdUser.InstanceId, departmentId, userId, invoiceID, "https://app.sherpadesk.com", currency, instanceConfig.Names.tech.a,
                            instanceConfig.Names.ticket.a, instanceConfig.ProjectTracking, instanceConfig.Names.user.a, instanceConfig.QBUseQBInvoiceNumber);
                    }
                    catch
                    {
                        throw new HttpError(HttpStatusCode.NotFound, "Cannot create invoice with provided data.");
                    }
                    MailNotification _mail_notification = new MailNotification(organizationId, departmentId, userId, from, recipients, subject, body);
                    if (pdfBytes != null)
                    {
                        bigWebApps.bigWebDesk.Data.FileItem[] _files = new bigWebApps.bigWebDesk.Data.FileItem[1];
                        _files[0] = new bigWebApps.bigWebDesk.Data.FileItem(0, filename, pdfBytes.Length, DateTime.Now, string.Empty, pdfBytes);
                        _mail_notification.AttachedFiles = _files;
                    }
                    string _return_string = _mail_notification.Commit(true);
                }
                catch
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Email error.");
                }
            }
            else
                throw new HttpError(HttpStatusCode.NotFound, "No recepients selected.");
            return invoice;
        }
    }
}
