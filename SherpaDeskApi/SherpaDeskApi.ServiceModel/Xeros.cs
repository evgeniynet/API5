using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/xero", "POST, OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Data : ApiRequest
    {
        public int user_id { get; set; }
        public int account_id { get; set; }
        public string xero_contact_id { get; set; }
    }

    [Route("/xero/contact", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Contact_Create : ApiRequest
    {
        public int? user_id { get; set; }
        public int? account_id { get; set; }
    }

    [Route("/xero/customer/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Customer_Unlink : ApiRequest
    {
        public int key { get; set; }
    }

    [Route("/xero/contact/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Contact_Unlink : ApiRequest
    {
        public int key { get; set; }
    }

    [Route("/xero/invoice", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Invoice_Create : ApiRequest
    {
        public int invoice_id { get; set; }
    }

    [Route("/xero/invoice/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Invoice_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }

    [Route("/xero/bill", "POST OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Bill_Create : ApiRequest
    {
        public int bill_id { get; set; }
    }

    [Route("/xero/bill/{key}", "DELETE OPTIONS")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class Xero_Bill_Delete : ApiRequest
    {
        public int key { get; set; }
        public bool is_unlink { get; set; }
    }
}