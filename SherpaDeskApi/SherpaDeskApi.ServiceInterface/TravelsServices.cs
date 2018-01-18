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
    public class TravelsServices : Service
    {
        [Secure()]
        public object Get(GET_Travels request)
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

            return request.FilteredResult<Travel>(Models.Travels.GetTravels(hdUser.OrganizationId, hdUser.DepartmentId, request.type.ToLower(), accountID, projectID, techID));
        }

        [Secure()]
        public object Get(GET_Travel request)
        {
            ApiUser hdUser = request.ApiUser;
            if (request.travel_id > 0)
            {
                DataTable dt = bigWebApps.bigWebDesk.Data.TicketTravelCosts.Select(hdUser.OrganizationId, hdUser.DepartmentId, request.travel_id);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "No data found");
                }
                return new Travel(dt.Rows[0]);
            }
            else
            {
                throw new HttpError(HttpStatusCode.NotFound, "Incorrect request");
            }
        }

        [Secure()]
        public object Put(Travel_Billable request)
        {
            ApiUser hdUser = request.ApiUser;
            try
            {
                bigWebApps.bigWebDesk.Data.TicketTravelCosts.SetBillableTravel(hdUser.OrganizationId, hdUser.DepartmentId, request.travel_id, request.is_billable);
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Delete(Travel_Delete request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (!instanceConfig.TravelCosts) throw new HttpError("Travels is not enabled for this instance.");
            try
            {
                DataTable dt = bigWebApps.bigWebDesk.Data.TicketTravelCosts.Select(hdUser.OrganizationId, hdUser.DepartmentId, request.travel_id);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong Travel Id");
                }
                else
                {
                    if (dt.Rows[0]["InvoiceId"] != null && !string.IsNullOrEmpty(dt.Rows[0]["InvoiceId"].ToString()) && int.Parse(dt.Rows[0]["InvoiceId"].ToString()) > 0)
                    {
                        throw new HttpError("Travel has been invoiced and can not be removed.");
                    }
                }
                bigWebApps.bigWebDesk.Data.TicketTravelCosts.Delete(hdUser.OrganizationId, hdUser.DepartmentId, request.travel_id);
                return new HttpResult("", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpError(Utils.ClearString(ex.Message));
            }
        }

        [Secure()]
        public object Post(POST_Travel request)
        {
            ApiUser hdUser = request.ApiUser;
            Instance_Config instanceConfig = new Instance_Config(hdUser);
            if (!instanceConfig.TravelCosts) throw new HttpError("Travels is not enabled for this instance.");
            if (request.travel_id > 0)
            {
                DataTable dt = bigWebApps.bigWebDesk.Data.TicketTravelCosts.Select(hdUser.OrganizationId, hdUser.DepartmentId, request.travel_id);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "Wrong Travel Id");
                }
            }
            bool is_billable = request.is_billable ?? true;
            bool is_technician_payment = request.is_technician_payment ?? true;
            int techID = request.tech_id > 0 ? request.tech_id : hdUser.UserId;
            int ticketID = 0;
            if (!string.IsNullOrEmpty(request.ticket_key))
            {
                ticketID = Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.ticket_key);
                if (ticketID == 0)
                {
                    bigWebApps.bigWebDesk.CustomNames _cNames = bigWebApps.bigWebDesk.CustomNames.GetCustomNames(hdUser.OrganizationId, hdUser.DepartmentId);
                    throw new HttpError("Incorrect " + _cNames.Ticket.abbreviatedSingular + " key.");
                }
            }            
            string note = string.Empty;            
            if (!string.IsNullOrEmpty(request.note))
            {
                note = request.note;
            }

            DateTime date = DateTime.UtcNow;
            if (request.date.HasValue && request.date > DateTime.MinValue)
            {
                date = (DateTime)request.date;
            }
            if (request.travel_id > 0)
            {
                bigWebApps.bigWebDesk.Data.TicketTravelCosts.Update(hdUser.OrganizationId, hdUser.DepartmentId, ticketID, request.account_id, request.start_location ?? string.Empty, request.end_location ?? string.Empty,
                        request.distance, request.distance_rate, date, request.travel_id, HttpUtility.HtmlDecode(note), is_billable, techID, is_technician_payment, request.qb_account_id, request.project_id);
            }
            else
            {
                bigWebApps.bigWebDesk.Data.TicketTravelCosts.Insert(hdUser.OrganizationId, hdUser.DepartmentId, ticketID, request.account_id, request.start_location ?? string.Empty, request.end_location ?? string.Empty,
                   request.distance, request.distance_rate, date, HttpUtility.HtmlDecode(note), is_billable, techID, is_technician_payment, request.qb_account_id,request.project_id);
            }

            return new HttpResult("", HttpStatusCode.OK);
        }
    }
}
