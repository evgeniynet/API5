// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System.Net;
using System.Data;
using System;
using BWA.Api.Models;

namespace SherpaDeskApi.ServiceInterface
{
    public class QuickBooksServices : Service
    {
        [Secure()]
        public object Post(QB_Data request)
        {
            ApiUser hdUser = request.ApiUser;
            try
            {
                QBModel.UpdateData(hdUser, request.user_id, request.qb_employee_id, request.account_id, request.qb_customer_id, request.task_type_id, request.qb_service_id, request.qb_vendor_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure()]
        public object Post(QB_Time request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            bool isProjectLog = false;
            string notes = "";
            int qb_employee_id = 0;
            int qb_vendor_id = 0;
            DateTime date = DateTime.Now;
            if (request.is_project_log.HasValue)
            {
                isProjectLog = request.is_project_log.Value;
            }
            if (request.qb_employee_id.HasValue)
            {
                qb_employee_id = request.qb_employee_id.Value;
            }
            if (request.qb_vendor_id.HasValue)
            {
                qb_vendor_id = request.qb_vendor_id.Value;
            }
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            decimal time_offset = hdUser.TimeZoneOffset;
            if (request.time_offset.HasValue)
            {
                time_offset = request.time_offset.Value;
            }
            bool is_billable = request.is_billable ?? true;
            bool is_rate_fixed = request.is_rate_fixed ?? false;
            try
            {
                string result = QBTimeActivities.CreateTimeActivity(hdUser, instanceConfig, qb_employee_id, request.qb_customer_id, request.qb_service_id,
                    request.hours, request.hourly_rate, notes, date, request.time_id, isProjectLog, is_billable, request.start_time, request.stop_time, time_offset, 0, -1, false, qb_vendor_id, is_rate_fixed);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Put(QB_Time_Update request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            bool isProjectLog = false;
            bool overwrite_changes = false;
            string notes = "";
            int qb_employee_id = 0;
            int qb_vendor_id = 0;
            DateTime date = DateTime.Now;
            if (request.is_project_log.HasValue)
            {
                isProjectLog = request.is_project_log.Value;
            }
            if (request.qb_employee_id.HasValue)
            {
                qb_employee_id = request.qb_employee_id.Value;
            }
            if (request.qb_vendor_id.HasValue)
            {
                qb_vendor_id = request.qb_vendor_id.Value;
            }
            if (request.overwrite_changes.HasValue)
            {
                overwrite_changes = request.overwrite_changes.Value;
            }
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            decimal time_offset = hdUser.TimeZoneOffset;
            if (request.time_offset.HasValue)
            {
                time_offset = request.time_offset.Value;
            }
            bool is_billable = request.is_billable ?? true;
            bool is_rate_fixed = request.is_rate_fixed ?? false;
            try
            {
                string result = QBTimeActivities.CreateTimeActivity(hdUser, instanceConfig, qb_employee_id, request.qb_customer_id, request.qb_service_id,
                    request.hours, request.hourly_rate, notes, date, request.time_id, isProjectLog, is_billable, request.start_time, request.stop_time, time_offset, request.key,
                    request.qb_sync_token, overwrite_changes, qb_vendor_id, is_rate_fixed);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(QB_Time_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            bool isProjectLog = false;
            if (request.is_project_log.HasValue)
            {
                isProjectLog = request.is_project_log.Value;
            }
            if (request.is_unlink)
            {
                try
                {
                    QBTimeActivities.UnlinkQuickBooksTimeActivity(hdUser, request.key, isProjectLog);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    QBTimeActivities.DeleteQuickBooksTimeActivity(hdUser, instanceConfig, request.key, isProjectLog);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Post(QB_Employee_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            try
            {
                return QBEmployees.CreateEmployee(hdUser, instanceConfig, request.user_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(QB_Customer_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            try
            {
                return QBCustomers.CreateCustomer(hdUser, instanceConfig, request.account_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(QB_Service_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            try
            {
                return QBServices.CreateService(hdUser, instanceConfig, request.task_type_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(QB_Employee_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    QBEmployees.UnlinkQuickBooksEmployee(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    QBEmployees.DeleteQuickBooksEmployee(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Delete(QB_Customer_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    QBCustomers.UnlinkQuickBooksCustomer(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    QBCustomers.DeleteQuickBooksCustomer(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Delete(QB_Service_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    QBServices.UnlinkQuickBooksService(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    QBServices.DeleteQuickBooksService(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Post(QB_Expense request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            int travel_id = 0;
            if (request.travel_id.HasValue)
            {
                travel_id = request.travel_id.Value;
            }
            CheckExpenseAndUpdateQBAccount(hdUser, request.expense_id, request.qb_account_id, travel_id);
            string notes = "";
            string note_internal = "";
            int qb_employee_id = 0;
            int qb_vendor_id = 0;
            int markup = 0;
            DateTime date = DateTime.Now;
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.note_internal != null)
            {
                note_internal = request.note_internal;
            }
            if (request.qb_employee_id.HasValue)
            {
                qb_employee_id = request.qb_employee_id.Value;
            }
            if (request.qb_vendor_id.HasValue)
            {
                qb_vendor_id = request.qb_vendor_id.Value;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            if (request.markup.HasValue)
            {
                markup = request.markup.Value;
            }
            bool qb_is_employee = request.qb_is_employee ?? false;
            bool is_billable = request.is_billable ?? true;
            try
            {
                string result = QBExpenses.CreateExpense(hdUser, instanceConfig, qb_employee_id, request.qb_customer_id, request.qb_service_id, request.qb_account_id,
                    qb_is_employee, request.amount, notes, note_internal, date, is_billable, markup, request.expense_id, 0, -1, false, qb_vendor_id, travel_id);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Put(QB_Expense_Update request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            int travel_id = 0;
            if (request.travel_id.HasValue)
            {
                travel_id = request.travel_id.Value;
            }
            CheckExpenseAndUpdateQBAccount(hdUser, request.expense_id, request.qb_account_id, travel_id);
            string notes = "";
            string note_internal = "";
            int qb_employee_id = 0;
            int qb_vendor_id = 0;
            int markup = 0;
            DateTime date = DateTime.Now;
            bool overwrite_changes = false;
            if (request.overwrite_changes.HasValue)
            {
                overwrite_changes = request.overwrite_changes.Value;
            }
            if (request.notes != null)
            {
                notes = request.notes;
            }
            if (request.note_internal != null)
            {
                note_internal = request.note_internal;
            }
            if (request.qb_employee_id.HasValue)
            {
                qb_employee_id = request.qb_employee_id.Value;
            }
            if (request.qb_vendor_id.HasValue)
            {
                qb_vendor_id = request.qb_vendor_id.Value;
            }
            if (request.date.HasValue)
            {
                date = request.date.Value;
            }
            if (request.markup.HasValue)
            {
                markup = request.markup.Value;
            }
            bool qb_is_employee = request.qb_is_employee ?? false;
            bool is_billable = request.is_billable ?? true;
            try
            {
                string result = QBExpenses.CreateExpense(hdUser, instanceConfig, qb_employee_id, request.qb_customer_id, request.qb_service_id, request.qb_account_id,
                    qb_is_employee, request.amount, notes, note_internal, date, is_billable, markup, request.expense_id, request.key, request.qb_sync_token, overwrite_changes, qb_vendor_id, travel_id);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(QB_Expense_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            bool is_travel = request.is_travel ?? false;
            if (request.is_unlink)
            {
                try
                {
                    if (is_travel)
                    {
                        QBExpenses.UnlinkQuickBooksTravel(hdUser, int.Parse(request.key));
                    }
                    else
                    {
                        QBExpenses.UnlinkQuickBooksExpense(hdUser, request.key);
                    }
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    if (is_travel)
                    {
                        QBExpenses.DeleteQuickBooksTravel(hdUser, instanceConfig, int.Parse(request.key));
                    }
                    else
                    {
                        QBExpenses.DeleteQuickBooksExpense(hdUser, instanceConfig, request.key);
                    }
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Post(QB_Invoice_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            try
            {
                return QBInvoices.CreateInvoice(hdUser, instanceConfig, request.invoice_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(QB_Invoice_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    QBInvoices.UnlinkQuickBooksInvoice(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    QBInvoices.DeleteQuickBooksInvoice(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Post(QB_Vendor_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            try
            {
                return QBVendors.CreateVendor(hdUser, instanceConfig, request.user_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(QB_Vendor_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    QBVendors.UnlinkQuickBooksVendor(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    QBVendors.DeleteQuickBooksVendor(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Post(QB_Bill_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            PaymentsService.CheckPaymentEnable(instanceConfig);
            try
            {
                string result = QBBills.CreateBill(hdUser, instanceConfig, request.bill_id);
                if (result == "ok")
                {
                    return new HttpResult("", HttpStatusCode.OK);
                }
                else
                {
                    throw new HttpError(Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(QB_Bill_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckQBOnlineIntegration(instanceConfig);
            PaymentsService.CheckPaymentEnable(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    QBBills.UnlinkQuickBooksBill(hdUser, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
            else
            {
                try
                {
                    QBBills.DeleteQuickBooksBill(hdUser, instanceConfig, request.key);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        private void CheckQBOnlineIntegration(Instance_Config instanceConfig)
        {
            if (!instanceConfig.QBOnlineIntegration || string.IsNullOrEmpty(instanceConfig.QBAccessToken))
            {
                throw new HttpError("QuickBooks Online is not enabled for this instance.");
            }
        }

        private void CheckExpenseAndUpdateQBAccount(ApiUser hdUser, string expense_id, int qb_account_id, int travel_id)
        {
            if (travel_id > 0)
            {
                DataTable dt = bigWebApps.bigWebDesk.Data.TicketTravelCosts.Select(hdUser.OrganizationId, hdUser.DepartmentId, travel_id);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong Travel Id");
                }
                else
                {
                    if (dt.Rows[0].IsNull("QBAccountID") || (dt.Rows[0]["QBAccountID"].ToString() != qb_account_id.ToString()))
                    {
                        bigWebApps.bigWebDesk.Data.TicketTravelCosts.UpdateTicketTravelCostsQBAccount(hdUser.OrganizationId, hdUser.DepartmentId, travel_id, qb_account_id);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(expense_id))
                {
                    DataRow plRow = bigWebApps.bigWebDesk.Data.Expense.SelectExpense(hdUser.OrganizationId, hdUser.DepartmentId, expense_id);
                    if (plRow == null)
                    {
                        throw new HttpError(HttpStatusCode.NotFound, "Wrong Expense Id");
                    }
                    else
                    {
                        if (plRow.IsNull("QBAccountID") || (plRow["QBAccountID"].ToString() != qb_account_id.ToString()))
                        {
                            bigWebApps.bigWebDesk.Data.Expense.UpdateExpenseQBAccount(hdUser.OrganizationId, hdUser.DepartmentId, expense_id, qb_account_id);
                        }
                    }
                }
            }
        }
    }
}
