// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using Micajah.Common.Bll;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{
    [Route("/organizations")]
    public class Organizations : ApiRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string url { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
        public string how { get; set; }
        public bool? is_send_hubSpot { get; set; }
        public bool is_force_registration { get; set; }
        public bool is_force_redirect { get; set; }
        public string note { get; set; }
        public string external_id { get; set; }
    }

    [Route("/organization_delete")]
    public class DeleteOrganization : ApiRequest
    {
        public string id { get; set; }
    }

    [Route("/validate_organization")]
    public class AddOrganization : ApiRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string url { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
        public string how { get; set; }
        public bool is_force_registration { get; set; }
        public bool is_force_redirect { get; set; }

       //public string invite_code { get; set; }

        public string note { get; set; }
    }

    public class VerifyOrganization
    {
        public VerifyOrganization()
        {
        }

        public bool isNameExists { get; set; }
        public bool isEmailExists { get; set; }
        public bool isUrlExists { get; set; }
        public bool isPasswordCorrect { get; set; }
        public bool isPasswordsMatch { get; set; }
        public string ErrorDescription { get; set; }
    }

    public class NewOrganizationsResponse
    {
        public string api_token { get; set; }
        public string organization { get; set; }
        public string instance { get; set; }
        public List<Organization> organizations { get; set; }

    }
}
