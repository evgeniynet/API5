using ServiceStack;
using SherpaDeskApi.ServiceModel;
using ServiceStack.Common.Web;
using System.Net;
using System.Data;
using System;
using bigWebApps.bigWebDesk.Data;

namespace SherpaDeskApi.ServiceInterface
{
    public class XeroServices : Service
    {
        [Secure()]
        public object Post(Xero_Data request)
        {
            ApiUser hdUser = request.ApiUser;
            try
            {
                Xero.UpdateXeroData(hdUser.OrganizationId, hdUser.DepartmentId, request.user_id, request.account_id, request.xero_contact_id);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure()]  
        public object Post(Xero_Contact_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckXeroIntegration(instanceConfig);
            int user_id = 0;
            int account_id = 0;
            if (request.user_id.HasValue)
            {
                user_id = request.user_id.Value;
            }
            if (request.account_id.HasValue)
            {
                account_id = request.account_id.Value;
            }
            try
            {
                XeroContact xeroContact;
                string result = Xero.CreateContact(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.XeroAuthConsumerKey, user_id, account_id, out xeroContact,
                    instanceConfig.XeroRSAPrivateKey);
                if (result == "ok")
                {
                    return xeroContact;
                }
                else
                {
                    throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        private void CheckXeroIntegration(Instance_Config instanceConfig)
        {
            if (!instanceConfig.XeroIntegration || string.IsNullOrEmpty(instanceConfig.XeroAccessToken))
            {
                throw new HttpError("Xero is not enabled for this instance.");
            }
        }

        [Secure()]
        public object Delete(Xero_Customer_Unlink request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckXeroIntegration(instanceConfig);
            try
            {
                Xero.UnlinkXeroCustomer(hdUser.OrganizationId, hdUser.DepartmentId, request.key);
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(Xero_Contact_Unlink request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckXeroIntegration(instanceConfig);
            try
            {
                Xero.UnlinkXeroContact(hdUser.OrganizationId, hdUser.DepartmentId, request.key);
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(Xero_Invoice_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckXeroIntegration(instanceConfig);
            try
            {
                XeroInvoice xeroInvoice;
                string result = Xero.CreateInvoice(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.XeroAuthConsumerKey, request.invoice_id, out xeroInvoice, 
                    instanceConfig.XeroRSAPrivateKey);
                if (result != "ok")
                {
                    throw new HttpError(HttpStatusCode.NotFound, Utils.ClearString(result));
                }
                return xeroInvoice;
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(Xero_Invoice_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckXeroIntegration(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    Xero.UnlinkXeroInvoice(hdUser.OrganizationId, hdUser.DepartmentId, request.key);
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
                    Xero.DeleteAndUnlinkInvoice(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.XeroAuthConsumerKey, request.key, instanceConfig.XeroRSAPrivateKey);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }

        [Secure()]
        public object Post(Xero_Bill_Create request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckXeroIntegration(instanceConfig);
            try
            {
                string result = Xero.CreateBill(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.XeroAuthConsumerKey, request.bill_id, instanceConfig.XeroRSAPrivateKey);
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
        public object Delete(Xero_Bill_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            CheckXeroIntegration(instanceConfig);
            if (request.is_unlink)
            {
                try
                {
                    Xero.UnlinkXeroBill(hdUser.OrganizationId, hdUser.DepartmentId, request.key);
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
                    Xero.DeleteAndUnlinkBill(hdUser.OrganizationId, hdUser.DepartmentId, instanceConfig, instanceConfig.XeroAuthConsumerKey, request.key, instanceConfig.XeroRSAPrivateKey);
                    return new HttpResult("", HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    throw new HttpError(Utils.ClearString(ex.Message));
                }
            }
        }
    }
}