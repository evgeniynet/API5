// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;
namespace SherpaDeskApi.ServiceModel
{
    [Route("/expenses", "GET, OPTIONS")]
    public class GET_Expenses : PagedApiRequest
    {
        public int? project { get; set; }
        public string type { get; set; }
        public int? account { get; set; }
        public int? tech { get; set; }
    }

    [Route("/expenses", "POST, OPTIONS")]
    public class POST_Expense : ApiRequest
    {
        public string expense_id { get; set; }
        public string ticket_key { get; set; }
        public int tech_id { get; set; }
        public int account_id { get; set; }
        public int project_id { get; set; }
        public bool? is_billable { get; set; }
        public string note { get; set; }
        public string note_internal { get; set; }
        public decimal amount { get; set; }
        public string category_id { get; set; }
        public string vendor { get; set; }
        public int markup { get; set; }
        public int qb_account_id { get; set; }
        public DateTime? date { get; set; }
        public bool? is_technician_payment { get; set; }
    }

    [Route("/expenses/{expense_id}", "GET, OPTIONS")]
    public class GET_Expense : ApiRequest
    {
        public string expense_id { get; set; }
    }

    [Route("/expenses/{key}", "PUT, OPTIONS")]
    public class Expense_Billable : ApiRequest
    {
        public string key { get; set; }
        public bool? is_billable { get; set; }
        public bool? hidden_from_invoice { get; set; }
    }

    [Route("/expenses/{expense_id}", "DELETE OPTIONS")]
    public class Expense_Delete : ApiRequest
    {
        public string expense_id { get; set; }
    }

    [Route("/expenses/categories/{category_id}", "GET, OPTIONS")]
    public class GET_ExpenseCategory : ApiRequest
    {
        public string category_id { get; set; }
    }
}
