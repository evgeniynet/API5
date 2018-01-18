// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;
using System.Data;
using System;
using System.Web;

namespace SherpaDeskApi.ServiceInterface
{
    public class ExpensesServices : Service
    {
        [Secure()]
        public object Get(GET_Expenses request)
        {
            ApiUser hdUser = request.ApiUser;
            if (string.IsNullOrEmpty(request.type))
                request.type = "recent";
            if (!(request.type.ToLower() == "recent" || request.type.ToLower() == "linked_fb" || request.type.ToLower() == "unlinked_fb"
                || request.type.ToLower() == "invoiced" || request.type.ToLower() == "not_invoiced" || request.type.ToLower() == "unlinked_fb_billable"
                || request.type.ToLower() == "not_invoiced_billable" || request.type.ToLower() == "not_invoiced_nonbillable" || request.type.ToLower() == "linked_qb"
                || request.type.ToLower() == "unlinked_qb" || request.type.ToLower() == "unlinked_qb_billable" || request.type.ToLower() == "hidden_from_invoice"))
            {
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect Type");
            }

            int accountID = request.account ?? 0;
            int projectID = request.project ?? 0;
            int techID = request.tech ?? 0;

            return request.FilteredResult<Expense>(Models.Expenses.GetExpenses(hdUser.OrganizationId, hdUser.DepartmentId, request.type.ToLower(), accountID, projectID, techID));
        }

        [Secure()]
        public object Get(GET_Expense request)
        {
            ApiUser hdUser = request.ApiUser;
            if (!string.IsNullOrEmpty(request.expense_id))
            {
                DataRow plRow = bigWebApps.bigWebDesk.Data.Expense.SelectExpense(hdUser.OrganizationId, hdUser.DepartmentId, request.expense_id);
                if (plRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "No data found");
                }
                return new Expense(plRow);
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect request");
            }
        }

        [Secure()]
        public object Put(Expense_Billable request)
        {
            ApiUser hdUser = request.ApiUser;
            try
            {
                if (request.is_billable.HasValue)
                {
                    bigWebApps.bigWebDesk.Data.Expense.SetBillableExpense(hdUser.OrganizationId, hdUser.DepartmentId, request.key, request.is_billable.Value);
                }
                if (request.hidden_from_invoice.HasValue)
                {
                    bigWebApps.bigWebDesk.Data.Expense.HideFromInvoiceExpense(hdUser.OrganizationId, hdUser.DepartmentId, request.key, request.hidden_from_invoice.Value);
                }
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Get(GET_ExpenseCategory request)
        {
            ApiUser hdUser = request.ApiUser;
            if (!string.IsNullOrEmpty(request.category_id))
            {
                DataRow plRow = bigWebApps.bigWebDesk.Data.Expense.SelectExpenseCategory(hdUser.OrganizationId, hdUser.DepartmentId, request.category_id);
                if (plRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "No data found");
                }
                return new ExpenseCategory(plRow);
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect request");
            }
        }

        [Secure()]
        public object Delete(Expense_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (!instanceConfig.MiscCosts) throw new HttpError("Expenses is not enabled for this instance.");
            try
            {
                int ticketID = 0;
                DataRow plRow = bigWebApps.bigWebDesk.Data.Expense.SelectExpense(hdUser.OrganizationId, hdUser.DepartmentId, request.expense_id);
                if (plRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong Expense Id");
                }
                else
                {
                    if(int.Parse(plRow["InvoiceId"].ToString()) > 0)
                    {
                        throw new HttpError("Expense has been invoiced and can not be removed.");
                    }
                    ticketID = int.Parse(plRow["TicketId"].ToString());
                }
                bigWebApps.bigWebDesk.Data.Expense.DeleteExpense(hdUser.OrganizationId, hdUser.DepartmentId, request.expense_id, ticketID);
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(POST_Expense request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (!instanceConfig.MiscCosts) throw new HttpError("Expenses is not enabled for this instance.");
            if (!string.IsNullOrEmpty(request.expense_id))
            {
                DataRow plRow = bigWebApps.bigWebDesk.Data.Expense.SelectExpense(hdUser.OrganizationId, hdUser.DepartmentId, request.expense_id);
                if (plRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong Expense Id");
                }
            }
            bool is_billable = request.is_billable ?? true;
            bool is_technician_payment = request.is_technician_payment ?? true;
            int techID = request.tech_id > 0 ? request.tech_id : hdUser.UserId;
            bigWebApps.bigWebDesk.CustomNames _cNames = bigWebApps.bigWebDesk.CustomNames.GetCustomNames(hdUser.OrganizationId, hdUser.DepartmentId);
            int ticketID = 0;
            if (!string.IsNullOrEmpty(request.ticket_key))
            {
                ticketID = Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket_key);
                if(ticketID == 0)
                {
                    throw new HttpError("Incorrect " + _cNames.Ticket.abbreviatedSingular + " key.");
                }
            }
            string categoryName = string.Empty;
            if (!string.IsNullOrEmpty(request.category_id))
            {
                DataRow ecRow = bigWebApps.bigWebDesk.Data.Expense.SelectExpenseCategory(hdUser.OrganizationId, hdUser.DepartmentId, request.category_id);
                if (ecRow == null)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong Category Id");
                }
                else
                {
                    if (!(bool)ecRow["Active"])
                    {
                        throw new HttpError(HttpStatusCode.NotFound, "Category Is Inactive");
                    }
                    categoryName = ecRow["Name"].ToString();
                }
            }
            string logNote = "";
            string additionalNote = "";
            string note = "";
            string note_internal = "";
            if (!string.IsNullOrEmpty(request.note))
            {
                note = request.note;
            }
            if (!string.IsNullOrEmpty(request.note_internal))
            {
                note_internal = request.note_internal;
            }
            if (ticketID > 0)
            {
                logNote = "Misc Cost ";
                if (!string.IsNullOrEmpty(request.expense_id))
                {
                    logNote += "edited";
                }
                else
                {
                    logNote += "added";
                }
                logNote += " " + instanceConfig.Currency + request.amount.ToString("0.00") + (note.Length > 0 ? " - "
                    + note : string.Empty);
                if (!string.IsNullOrEmpty(categoryName))
                {
                    logNote += "<br>Category: " + categoryName + ".";
                }
                if (!string.IsNullOrEmpty(request.vendor))
                {
                    logNote += "<br>Vendor: " + request.vendor + ".";
                }
                logNote += " Billable: " + is_billable.ToString() + ".";
                logNote += " " + _cNames.Technician.FullSingular + " Payment: " + is_technician_payment.ToString() + ".";
                if (request.markup > 0)
                {
                    logNote += "<br>Markup: " + request.markup + "%.";
                }
                logNote += "<br>The Misc Cost was ";
                if (!string.IsNullOrEmpty(request.expense_id))
                {
                    logNote += "edited";
                }
                else
                {
                    logNote += "added";
                }
                logNote += " by " + hdUser.FirstName + " " + hdUser.LastName + ".";
            }
            else
            {
                if (techID != hdUser.UserId)
                {
                    additionalNote = " The expense was ";
                    if (!string.IsNullOrEmpty(request.expense_id))
                    {
                        additionalNote += "edited";
                    }
                    else
                    {
                        additionalNote += "input";
                    }
                    additionalNote +=" by " + hdUser.FirstName + " " + hdUser.LastName + ".";
                }
            }
            string vendor = (request.vendor != null ? request.vendor : "");
            DateTime date = DateTime.UtcNow;
            if (request.date.HasValue && request.date > DateTime.MinValue)
            {
                date = (DateTime)request.date;
            }
            if (!string.IsNullOrEmpty(request.expense_id))
            {
                bigWebApps.bigWebDesk.Data.Expense.UpdateExpense(hdUser.OrganizationId, hdUser.DepartmentId, request.expense_id, ticketID, request.account_id,
                    request.project_id, techID, request.amount, HttpUtility.HtmlDecode(note), HttpUtility.HtmlDecode(note_internal) + additionalNote, vendor, is_billable,
                    request.markup, request.category_id, request.qb_account_id, date, is_technician_payment);
            }
            else
            {
                bigWebApps.bigWebDesk.Data.Expense.InsertExpense(hdUser.OrganizationId, hdUser.DepartmentId, ticketID, request.account_id,
                    request.project_id, techID, request.amount, HttpUtility.HtmlDecode(note), HttpUtility.HtmlDecode(note_internal) + additionalNote, vendor, is_billable,
                    request.markup, request.category_id, request.qb_account_id, date, is_technician_payment);
            }
            if (ticketID > 0)
            {
                bigWebApps.bigWebDesk.Data.Ticket _tkt = new bigWebApps.bigWebDesk.Data.Ticket(hdUser.OrganizationId, hdUser.DepartmentId, ticketID, true);
                _tkt.TicketLogs.Insert(0, new bigWebApps.bigWebDesk.Data.Ticket.LogEntry(hdUser.UserId, hdUser.LoginEmail, hdUser.FirstName, hdUser.LastName, DateTime.UtcNow, "Misc Costs", logNote));
                bigWebApps.bigWebDesk.Data.NotificationRules.RaiseNotificationEvent(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId,
                    bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.EnterMiscCosts, _tkt);
            }
            return new HttpResult("", HttpStatusCode.OK);
        }
    }
}
