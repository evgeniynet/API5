// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/freshbooks/clients", "GET OPTIONS")]
    public class FB_Clients : PagedApiRequest
    {
    }

    [Route("/freshbooks/clients", "POST OPTIONS")]
    public class FB_Clients_Create : ApiRequest
    {
        public int account_id { get; set; }
    }

    [Route("/freshbooks/clients/{key}", "DELETE OPTIONS")]
    public class FB_Client_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/freshbooks/staff", "GET OPTIONS")]
    public class FB_Staff : PagedApiRequest
    {
    }

    [Route("/freshbooks/staff/{key}", "DELETE OPTIONS")]
    public class FB_Staff_Delete : ApiRequest
    {
        public int key { get; set; }
    }

    [Route("/freshbooks/projects", "GET OPTIONS")]
    public class FB_Projects : PagedApiRequest
    {
        public string client { get; set; }
        public string staff { get; set; }
    }

    [Route("/freshbooks/projects", "POST OPTIONS")]
    public class FB_Projects_Create : ApiRequest
    {
        public int project_id { get; set; }
        public int fb_client_id { get; set; }
        public int fb_staff_id { get; set; }
        public int account_id { get; set; }
    }

    [Route("/freshbooks/projects/{key}", "DELETE OPTIONS")]
    public class FB_Project_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/freshbooks/tasks", "GET OPTIONS")]
    public class FB_Tasks : PagedApiRequest
    {
        public int? project { get; set; }
        public string name { get; set; }
    }

    [Route("/freshbooks/tasks", "POST OPTIONS")]
    public class FB_Tasks_Create : ApiRequest
    {
        public int task_type_id { get; set; }
        public int fb_project_id { get; set; }
    }

    [Route("/freshbooks/tasks/{key}", "DELETE OPTIONS")]
    public class FB_Task_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/freshbooks", "POST, OPTIONS")]
    public class FB_Data : ApiRequest
    {
        public int user_id { get; set; }
        public int fb_staff_id { get; set; }
        public int account_id { get; set; }
        public int fb_client_id { get; set; }
        public int project_id { get; set; }
        public int fb_project_id { get; set; }
        public int task_type_id { get; set; }
        public int fb_task_type_id { get; set; }
        public string category_id { get; set; }
        public int fb_category_id { get; set; }
    }

    [Route("/freshbooks/time", "POST, OPTIONS")]
    public class FB_Time : ApiRequest
    {
        public int? fb_staff_id { get; set; }
        public int fb_project_id { get; set; }
        public int fb_task_type_id { get; set; }
        public decimal hours { get; set; }
        public string notes { get; set; }
        public DateTime? date { get; set; }
        public int time_id { get; set; }
        public bool? is_project_log { get; set; }
    }

    [Route("/freshbooks/time/{key}", "PUT OPTIONS")]
    public class FB_Time_Update : ApiRequest
    {
        public int key { get; set; }
        public int? fb_staff_id { get; set; }
        public int fb_project_id { get; set; }
        public int fb_task_type_id { get; set; }
        public decimal hours { get; set; }
        public string notes { get; set; }
        public DateTime? date { get; set; }
    }

    [Route("/freshbooks/time/{key}", "DELETE OPTIONS")]
    public class FB_Time_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
        public bool? is_project_log { get; set; }
    }

    [Route("/freshbooks/expense", "POST OPTIONS")]
    public class FB_Expense : ApiRequest
    {
        public int fb_staff_id { get; set; }
        public int fb_category_id { get; set; }
        public int? fb_project_id { get; set; }
        public int? fb_client_id { get; set; }
        public decimal amount { get; set; }
        public string vendor { get; set; }
        public string notes { get; set; }
        public DateTime? date { get; set; }
        public string expense_id { get; set; }
    }

    [Route("/freshbooks/expense/{key}", "PUT OPTIONS")]
    public class FB_Expense_Update : ApiRequest
    {
        public int key { get; set; }
        public int fb_staff_id { get; set; }
        public int fb_category_id { get; set; }
        public int? fb_project_id { get; set; }
        public int? fb_client_id { get; set; }
        public decimal amount { get; set; }
        public string vendor { get; set; }
        public string notes { get; set; }
        public DateTime? date { get; set; }
    }

    [Route("/freshbooks/expense/del/{key}", "DELETE OPTIONS")]
    public class FB_Expense_Delete : ApiRequest
    {
        public string key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/freshbooks/category", "GET OPTIONS")]
    public class FB_Categories : ApiRequest
    {
    }

    [Route("/freshbooks/category", "POST OPTIONS")]
    public class FB_Category_Create : ApiRequest
    {
        public string category_id { get; set; }
    }

    [Route("/freshbooks/category/del/{key}", "DELETE OPTIONS")]
    public class FB_Category_Delete : ApiRequest
    {
        public string key { get; set; }
        public bool is_unlink { get; set; }
    }
}
