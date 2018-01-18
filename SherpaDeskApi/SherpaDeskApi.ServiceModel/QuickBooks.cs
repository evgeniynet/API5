// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/quickbooks", "POST, OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Data : ApiRequest
    {
        public int user_id { get; set; }
        public int qb_employee_id { get; set; }
        public int account_id { get; set; }
        public int qb_customer_id { get; set; }
        public int task_type_id { get; set; }
        public int qb_service_id { get; set; }
        public int qb_vendor_id { get; set; }
    }

    [Route("/quickbooks/time", "POST, OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Time : ApiRequest
    {
        public int? qb_employee_id { get; set; }
        public int qb_customer_id { get; set; }
        public int qb_service_id { get; set; }
        public decimal hours { get; set; }
        public decimal hourly_rate { get; set; }
        public string notes { get; set; }
        public DateTime? date { get; set; }
        public int time_id { get; set; }
        public bool? is_project_log { get; set; }
        public bool? is_billable { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? stop_time { get; set; }
        public int? qb_vendor_id { get; set; }
        public bool? is_rate_fixed { get; set; }
        public decimal? time_offset { get; set; }
    }

    [Route("/quickbooks/time/{key}", "PUT OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Time_Update : ApiRequest
    {
        public int key { get; set; }
        public int qb_sync_token { get; set; }
        public int? qb_employee_id { get; set; }
        public int qb_customer_id { get; set; }
        public int qb_service_id { get; set; }
        public decimal hours { get; set; }
        public decimal hourly_rate { get; set; }
        public string notes { get; set; }
        public DateTime? date { get; set; }
        public int time_id { get; set; }
        public bool? is_project_log { get; set; }
        public bool? is_billable { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? stop_time { get; set; }
        public bool? overwrite_changes { get; set; }
        public int? qb_vendor_id { get; set; }
        public bool? is_rate_fixed { get; set; }
        public decimal? time_offset { get; set; }
    }

    [Route("/quickbooks/time/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Time_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
        public bool? is_project_log { get; set; }
    }

    [Route("/quickbooks/employee", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Employee_Create : ApiRequest
    {
        public int user_id { get; set; }
    }

    [Route("/quickbooks/employee/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Employee_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/quickbooks/customer", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Customer_Create : ApiRequest
    {
        public int account_id { get; set; }
    }

    [Route("/quickbooks/customer/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Customer_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/quickbooks/service", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Service_Create : ApiRequest
    {
        public int task_type_id { get; set; }
    }

    [Route("/quickbooks/service/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Service_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/quickbooks/expense", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Expense : ApiRequest
    {
        public int? qb_employee_id { get; set; }
        public int qb_service_id { get; set; }
        public int qb_account_id { get; set; }
        public int qb_customer_id { get; set; }
        public bool? qb_is_employee { get; set; }
        public decimal amount { get; set; }
        public string notes { get; set; }
        public string note_internal { get; set; }
        public DateTime? date { get; set; }
        public bool? is_billable { get; set; }
        public string expense_id { get; set; }
        public int? travel_id { get; set; }
        public int? markup { get; set; }
        public int? qb_vendor_id { get; set; }
    }

    [Route("/quickbooks/expense/{key}", "PUT OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Expense_Update : ApiRequest
    {
        public int key { get; set; }
        public int qb_sync_token { get; set; }
        public int? qb_employee_id { get; set; }
        public int qb_service_id { get; set; }
        public int qb_account_id { get; set; }
        public int qb_customer_id { get; set; }
        public bool? qb_is_employee { get; set; }
        public decimal amount { get; set; }
        public string notes { get; set; }
        public string note_internal { get; set; }
        public DateTime? date { get; set; }
        public bool? is_billable { get; set; }
        public bool? overwrite_changes { get; set; }
        public string expense_id { get; set; }
        public int? travel_id { get; set; }
        public int? markup { get; set; }
        public int? qb_vendor_id { get; set; }
    }

    [Route("/quickbooks/expense/del/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Expense_Delete : ApiRequest
    {
        public string key { get; set; }
        public bool is_unlink { get; set; }
        public bool? is_travel { get; set; }
    }

    [Route("/quickbooks/invoice", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Invoice_Create : ApiRequest
    {
        public int invoice_id { get; set; }
    }

    [Route("/quickbooks/invoice/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Invoice_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/quickbooks/vendor", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Vendor_Create : ApiRequest
    {
        public int user_id { get; set; }
    }

    [Route("/quickbooks/vendor/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Vendor_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/quickbooks/bill", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Bill_Create : ApiRequest
    {
        public int bill_id { get; set; }
    }

    [Route("/quickbooks/bill/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QB_Bill_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }
}
