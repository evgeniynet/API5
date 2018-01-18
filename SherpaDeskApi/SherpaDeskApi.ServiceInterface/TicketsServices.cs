// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using ServiceStack;
using SherpaDeskApi.ServiceModel;
using bigWebApps.bigWebDesk;
using System.Data;

namespace SherpaDeskApi.ServiceInterface
{
    public class TicketsService : Service
    {
        [Secure()]
        public object Get(Tickets_List request)
        {
            ApiUser  hdUser = request.ApiUser;
            int user_id = hdUser.UserId;
			if (request.user > 0) {
				if (hdUser.IsTechAdmin || Array.IndexOf (hdUser.Roles, "super") > -1)
					user_id = request.user;
			}
			if (Array.IndexOf (hdUser.Roles, "super") > -1 && request.user > 0)
				request.account = hdUser.AccountName;
           //v3
            if (!string.IsNullOrEmpty(request.query))
            {
                request.role = request.query;
                request.query = "";
            }
            if (!string.IsNullOrEmpty(request.role) && request.role.ToLower() == "user_tech")
            {
                return request.QueryResult<TicketSearchResult>(Models.TicketSearchResults.GetTickets(hdUser.OrganizationId, hdUser.DepartmentId, request.account, request.project));
            }
            if ("undefined" == request.location)
            {
                request.location = "";
            }
            var tickets = WorklistTickets.GetTickets(hdUser, hdUser.OrganizationId, hdUser.DepartmentId, user_id, request.status, request.role, request.Class, request.account, request.location, request.project, hdUser.IsTechAdmin, hdUser.IsUseWorkDaysTimer, request.sort_order, request.sort_by, request.search, request.page, request.limit, request.start_date, request.end_date);
            return request.QueryResult<WorklistTicket>(tickets);
        }

        [Secure()]
        public object Get(Tickets request)
        {
            ApiUser hdUser = request.ApiUser;
            request.key = request.key ?? "";
            bool isBulkInfo = request.key.Contains(",");
            if (isBulkInfo)
            {
                List<Ticket> tickets = new List<Ticket>();
                string[] ticketKeys = request.key.Split(',');
                foreach (string key in ticketKeys)
                {
                    if (!string.IsNullOrWhiteSpace(key))
                        try
                        {
                            var tktg = getTicket(hdUser, key, false || !string.IsNullOrWhiteSpace(request.search));
                            tickets.Add(tktg);
                        }
                        catch { /* skip incorrect ticket keys*/ }
                }
                return tickets;
            }

            return getTicket(hdUser, request.key, false);
        }

        Ticket getTicket(ApiUser hdUser, string key, bool isCoreInfo)
        {
            Ticket tkt = new Ticket(hdUser.OrganizationId, hdUser.DepartmentId, key, hdUser.InstanceId, isCoreInfo, hdUser.IsTechAdmin ? 0 : hdUser.UserId);

            tkt.DaysOldInMinutes = Utils.GetDaysOldInMinutes(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.IsUseWorkDaysTimer, tkt.TicketStatusString, tkt.CreateTime.Value, tkt.ClosedTime ?? DateTime.UtcNow);
            tkt.days_old = bigWebApps.bigWebDesk.Functions.DisplayDateDuration(tkt.DaysOldInMinutes);
            if (!isCoreInfo)
                bigWebApps.bigWebDesk.Data.Ticket.UpdateNewPostIcon(hdUser.OrganizationId, hdUser.DepartmentId, tkt.ID, hdUser.UserId, false);

            return tkt;
        }


        [Secure()]
        public object Post(Ticket request)
        {
            ApiUser hdUser = ApiUser.getUser(base.Request);
            int _newTktId = Ticket.CreateNew(hdUser, request);
            if (_newTktId > 0)
            {
                if (request.Users != null)
                    foreach (TicketAssignee _ta in request.Users) Ticket.AttachAlternateUser(hdUser, _newTktId, _ta.UserId);

                if (request.Technicians != null)
                    foreach (TicketAssignee _ta in request.Technicians) Ticket.AttachAlternateTechnician(hdUser, _newTktId, _ta.UserId);

                if (!string.IsNullOrWhiteSpace(request.Note) && hdUser.IsTechAdmin)
                    Ticket.UpdateTechNote(hdUser.OrganizationId, _newTktId, request.Note);

                return new HttpResult(new Ticket(hdUser.OrganizationId, hdUser.DepartmentId, _newTktId, hdUser.InstanceId)) { StatusCode = HttpStatusCode.Created };
            }

            CustomNames _cNames = CustomNames.GetCustomNames(hdUser.OrganizationId, hdUser.DepartmentId);
            string errMsg = "Ticket Not Saved. ";
            switch (_newTktId)
            {
                case -1:
                    errMsg += "Input level is not setup for this class.";
                    break;
                case -2:
                    errMsg += "No routing options are enabled. No route found. Must choose " + _cNames.Technician.AbbreviatedSingular + " specifically.";
                    break;
                case -3:
                    errMsg += "No Route Found. Routing configuration must be modified.";
                    break;
                case -4:
                    errMsg += "Level does not exists.";
                    break;
                case -5:
                    errMsg += "Route found but " + _cNames.Technician.AbbreviatedSingular + " could not be returned. Please check routing order for errors.";
                    break;
            }
            throw new HttpError(HttpStatusCode.NotFound, new ArgumentException(errMsg));
        }

        [Secure("tech")]
        public object Delete(Tickets request)
        {
            ApiUser hdUser = request.ApiUser;
            bigWebApps.bigWebDesk.Data.Tickets.DeleteTicket(hdUser.OrganizationId, hdUser.DepartmentId, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key));
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure()]
        public object Put(Ticket_Actions request)
        {
            ApiUser hdUser = request.ApiUser;

            request.note_text = request.note_text ?? "";
            request.status = request.status ?? "";
            request.status = request.status.Replace("_", "");
            int ticket_id = Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key);

            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, true);
            bigWebApps.bigWebDesk.Data.Ticket _tktNew = null;
            Ticket.Status status;

            if (Enum.TryParse<Ticket.Status>(request.status, true, out status))
            {
                if (_tktOld.TicketStatus != status)
                {
                    switch (status)
                    {
                        case Ticket.Status.OnHold:
                            Ticket.OnHold(hdUser, ticket_id, request.note_text, ref _tktNew);
                            break;
                        case Ticket.Status.PartsOnOrder:
                            throw new HttpError(HttpStatusCode.NotFound, "Incorrect status");
                        case Ticket.Status.Closed:
                            Ticket.Close(hdUser, ticket_id, request.note_text, request.is_send_notifications, request.resolved, request.resolution_id, request.confirmed, request.confirm_note, ref _tktNew);
                            break;
                        case Ticket.Status.Open:
                            Ticket.ReOpen(hdUser, ticket_id, request.note_text, ref _tktNew);
                            break;
                    }
                }
            }

            bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.TicketResponse;

            if (request.action == "transfer" || (request.user_id > 0 && _tktOld.UserId != request.user_id))
            {
                Ticket.TransferToUser(hdUser, ticket_id, request.user_id, request.note_text, ref _tktNew);
                tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.ChangeEndUser;
            }

            if (request.action == "transfer" || (request.tech_id > 0 && _tktOld.TechnicianId != request.tech_id))
            {
                Ticket.TransferToTech(hdUser, ticket_id, request.tech_id, request.note_text, request.keep_attached, ref _tktNew);
                if (_tktNew.TechnicianId == hdUser.UserId) bigWebApps.bigWebDesk.Data.Ticket.UpdateNewPostIcon(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, hdUser.UserId, false);
                else if (_tktNew.TechnicianId != hdUser.UserId) bigWebApps.bigWebDesk.Data.Ticket.UpdateNewPostIcon(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, hdUser.UserId, true);
                tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.TransferTicket;
            }

            if (request.priority_id > 0 && _tktOld.PriorityId != request.priority_id)
            {
                Ticket.UpdatePriority(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, request.priority_id);
                tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.ChangePriority;
            }
            /** not implemented
             *  need to add parameters LocationId, Transfermode, is_transfer_user_to_account
             *  look page https://support.sherpadesk.com/home/accts/assignticket.aspx
             *  Transfermode: 
					0 - This ticket only (default)
					1 - Only unassigned tickets for this user
					2 - All tickets for this user
             */
            if (request.account_id != 0 && _tktOld.AccountId != request.account_id)
            {
                Ticket.UpdateAccount(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, request.account_id, request.account_location_id, 0, request.is_transfer_user_to_account);
                tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.ChangeAccount;
            }
            if (request.location_id != 0 && _tktOld.LocationId != request.location_id)
            {
                Ticket.UpdateLocation(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, request.location_id);
                tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.ChangeLocation;
            }
			if (request.project_id != 0 && _tktOld.ProjectId != request.project_id)
			{
				Ticket.UpdateProject(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, request.project_id);
				tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.ChangeProject;
			}
            if (request.level_id > 0 && _tktOld.Level != request.level_id)
            {
                Ticket.UpdateLevel(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, request.level_id, hdUser.UserId, request.note_text);
                tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.ChangeLevel;
            }
            if (request.class_id > 0 && _tktOld.ClassId != request.class_id)
            {
                Ticket.UpdateClass(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, request.class_id);
                tktEvent = bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.ChangeClass;
            }

            if (tktEvent != bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.TicketResponse)
            {
                _tktNew = _tktNew ?? new bigWebApps.bigWebDesk.Data.Ticket(hdUser.OrganizationId, hdUser.DepartmentId, ticket_id, true);
                bigWebApps.bigWebDesk.Data.NotificationRules.RaiseNotificationEvent(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, tktEvent, _tktNew);
            }

            if (!string.IsNullOrEmpty(request.action))
            {
                switch (request.action.ToLower())
                {
                    case "pickup": Ticket.PickUp(hdUser, ticket_id, request.note_text, ref _tktNew);
                        break;
                    case "confirm": Ticket.Confirm(hdUser, ticket_id, request.note_text, ref _tktNew);
                        break;
                    case "escalate":
                        Ticket.EscalateTicketByLevel(hdUser, ticket_id, true);
                        return getTicket(hdUser, request.key, false);
                        break;
                    case "deescalate":
                        Ticket.EscalateTicketByLevel(hdUser, ticket_id, false);
                        return getTicket(hdUser, request.key, false);
                        break;
                }
            }
            if (_tktNew != null)
                return TicketLogRecords.TicketLog(hdUser.OrganizationId, hdUser.DepartmentId, _tktNew.ID, _tktNew.PseudoID);
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure()]
        public object Post(Ticket_Actions request)
        {
            ApiUser hdUser = request.ApiUser;
            bigWebApps.bigWebDesk.Data.Ticket _tktNew = null;
            if (request.action == "response" || !string.IsNullOrEmpty(request.note_text))
            {
                Ticket.Response(hdUser, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key), request.note_text, request.is_waiting_on_response, ref _tktNew, request.files, request.user_id);
                //return new HttpResult("", HttpStatusCode.OK);
            }
            else if (request.action == "workpad")
            {
                string workpad = HttpUtility.HtmlDecode(request.workpad + "");
                Ticket.UpdateTicketWorkpad(hdUser.OrganizationId, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key), workpad);
                //return new HttpResult("", HttpStatusCode.OK);
            }
            else if (request.action == "note" && hdUser.IsTechAdmin) {
				string note =request.note + "";
				Ticket.UpdateTechNote(hdUser.OrganizationId, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key), note);
				//return new HttpResult("", HttpStatusCode.OK);
			}
            else if (request.action == "add_tech" || request.tech_id > 0)
                Ticket.AttachAlternateTechnician(hdUser, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key), request.tech_id);
            else if (request.action == "add_user" || request.user_id > 0)
                Ticket.AttachAlternateUser(hdUser, Ticket.GetId(hdUser.OrganizationId, hdUser.DepartmentId, request.key), request.user_id);

            if (_tktNew != null)
                return TicketLogRecords.TicketLog(hdUser.OrganizationId, hdUser.DepartmentId, _tktNew.ID, _tktNew.PseudoID);
            return new HttpResult("", HttpStatusCode.OK);
        }

        [Secure("tech")]
        public object Get(Tickets_Counts request)
        {
            return base.RequestContext.ToOptimizedResultUsingCache(base.Cache, string.Format("urn:{0}:{1}{2}", base.Request.GetBasicAuth(), base.Request.PathInfo.Substring(1), (base.Request.QueryString.Count > 0 ? ":" + base.Request.QueryString.ToString() : "")),
                new System.TimeSpan(0, 2, 0), () =>
                {
                    return GetTickets_Counts(request);
                });
        }

        private object GetTickets_Counts(Tickets_Counts request)
        {
            ApiUser hdUser = request.ApiUser;
            request.status = request.status ?? "";
               
            TicketCount tk = new TicketCount(bigWebApps.bigWebDesk.Data.Tickets.SelectTicketCounts(hdUser.OrganizationId, hdUser.DepartmentId, 1, hdUser.UserId));
            switch (request.status.ToLower())
            {
               //v1
                case "new": return tk.New;
                case "open": return tk.Open;
                case "total": return tk.Open;
               //v2
                case "new_mesages": return tk.New;
                case "open_all": return tk.Open;
                case "open_as_tech": return tk.OpenAsTech;
                case "open_as_alttech": return tk.OpenAsAltTech;
                case "open_as_user": return tk.OpenAsUser;
                case "onhold": return tk.OnHold;
                case "reminder": return tk.Reminder;
                case "parts_on_order": return tk.PartsOnOrder;
                case "unconfirmed": return tk.Unconfirmed;
                case "waiting": return tk.Waiting;
                default: return tk;
            }
            return new HttpResult("", HttpStatusCode.OK);
        }
    }
}
