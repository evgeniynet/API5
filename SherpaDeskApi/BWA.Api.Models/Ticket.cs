// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;
using bigWebApps.bigWebDesk;
using bigWebApps.bigWebDesk.Data;
using System.Net;
using ServiceStack.ServiceHost;
using System.Data.SqlClient;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for Ticket
   /// </summary>
         
    [Route("/tickets", "POST")]
    [DataContract(Name = "ticket")]
    public class Ticket : bigWebApps.bigWebDesk.Data.Ticket, ModelItemBaseInterface
    {
        protected DataRow m_Row;

        public Ticket(DataRow row) : base(row)
        {
            m_Row = row;
        }

        public Ticket(Guid OrgId, int DeptId, string TktPseudoId, Guid InstanceId, bool isCoreInfo = false, int userId = 0)
        {
            int TktId = GetId(OrgId, DeptId, TktPseudoId);
            TicketFactory(OrgId, DeptId, TktId, InstanceId, isCoreInfo, userId);
        }

        public Ticket(Guid OrgId, int DeptId, int TktId, Guid InstanceId, int userId = 0)
        {
            TicketFactory(OrgId, DeptId, TktId, InstanceId, false, userId);
        }

        public void TicketFactory(Guid OrgId, int DeptId, int TktId, Guid InstanceId, bool isCoreInfo = false, int userId = 0)
        {
            m_Row = Tickets.SelectOne(OrgId, DeptId, TktId);

            if (m_Row == null)
                throw new HttpError(HttpStatusCode.NotFound, "key not found");

            if (userId > 0 && userId != (int)m_Row["technician_id"] && userId != (int)m_Row["user_id"])
            {
                Users = TicketAssignments.TicketUsers(OrgId, DeptId, TktId);
                if (!Users.Exists(x => x.UserId == userId))
                {
                    throw new HttpError(HttpStatusCode.NotFound, "user's ticket not found");
                }
            }

           //m_Row["intSLAResponseUsed"] = !m_Row.IsNull("dtSLAResponse") ? bigWebApps.bigWebDesk.Data.Tickets.SelectTicketSLATime(OrgId, DeptId, DateTime.UtcNow, (DateTime)m_Row["dtSLAResponse"]) : 0;
            InitTicket(m_Row);
            if (!m_Row.IsNull("dtSLAResponse"))
                this.SLAResponseUsed = Tickets.SelectTicketSLATime(OrgId, DeptId, DateTime.UtcNow, (DateTime)m_Row["dtSLAResponse"]);
            else
                this.SLAResponseUsed = 0;
            if (this.DaysOldInMinutes == 0)
                this.DaysOldInMinutes = Tickets.SelectTicketSLATime(OrgId, DeptId, DateTime.UtcNow, (DateTime)m_Row["CreateTime"]);
            OrganizationId = OrgId;
            DepartmentID = DeptId;
            TktId = this.Id;
            Config _cfg = new Config(OrganizationId, InstanceId);
            if (!isCoreInfo)
            {
                if (_cfg.ResolutionTracking)
                    ResolutionCategories = Models.ResolutionCategories.GetResolutionCategories(OrganizationId, DepartmentID);
                Users = TicketAssignments.TicketUsers(OrgId, DeptId, TktId);
               //Technicians = TicketAssignments.TicketTechnicians(OrgId, DeptId, TktId);
                TicketLogs = TicketLogRecords.TicketLog(OrgId, DeptId, TktId, m_Row["PseudoId"].ToString());
               //var assetsConfig = new AssetsConfig(bigWebApps.bigWebDesk.Config.GetConfig(OrgId, InstanceId).Assets);
               //Assets = Models.Assets.TicketAssets(assetsConfig, OrgId, DeptId, TktId);
                Files = Models.Files.GetFiles(InstanceId, TktId, OrgId);
                //ParentClasses = Models.Classes.ParentClasses(OrgId, DeptId, Convert.ToInt32(m_Row["class_id"] != DBNull.Value ? m_Row["class_id"] : "0"));
                if (_cfg.TimeTracking)
                    TimeLogs = TicketTimeLogs.GetTicketTimeLogs(OrgId, DeptId, TktId);
                if (_cfg.TktLevels)
                {
                    int desc_val = 0;
                    int esc_val = 0;
                    GetEscalate(OrganizationId, DepartmentID, TktId, LocationId, ClassId, Level, out desc_val, out esc_val);
                    LevelDescalate = desc_val;
                    LevelEscalate = esc_val;
                }
            }
        }

        private static void GetEscalate(Guid OrgId, int DeptId, int TktID, int LocationId, int ClassId, int Level, out int desc_val, out int esc_val)
        {
            desc_val = 0;
            esc_val = 0;
            SqlParameter _pLocationId = new SqlParameter("@LocationId", SqlDbType.Int);
            if (LocationId != 0) _pLocationId.Value = LocationId;
            else _pLocationId.Value = DBNull.Value;
            SqlParameter _pClassId = new SqlParameter("@ClassId", SqlDbType.Int);
            if (ClassId != 0) _pClassId.Value = ClassId;
            else _pClassId.Value = DBNull.Value;
            SqlParameter _pbtEscalate = new SqlParameter("@btEscalate", SqlDbType.Bit);
            _pbtEscalate.Direction = ParameterDirection.Output;
            _pbtEscalate.Value = false;
            SqlParameter _pbtDescalate = new SqlParameter("@btDescalate", SqlDbType.Bit);
            _pbtDescalate.Direction = ParameterDirection.Output;
            _pbtDescalate.Value = false;
            bigWebApps.bigWebDesk.Data.DBAccess.UpdateData("sp_SelectEscalationAvail", new SqlParameter[] { new SqlParameter("@DId", DeptId), new SqlParameter("@TId", TktID), _pLocationId, _pClassId, new SqlParameter("@tintLevel", Level), _pbtEscalate, _pbtDescalate }, OrgId);
            if ((bool)_pbtDescalate.Value)
            {
                desc_val = Level - 1;
            }
            if ((bool)_pbtEscalate.Value)
            {
                esc_val = Level + 1;
            }
        }

        #region Ticket Static Actions
        public static int CreateNew(ApiUser User, Ticket Tkt)
        {
            int _initPostId = 0;
            Tkt.AccountLocationId = Tkt.LocationId;
            //Tkt.LocationId = Math.Max(Tkt.LocationId, Tkt.AccountLocationId);
            if (Tkt.AccountLocationId == 0) Tkt.AccountLocationId = -1;
            Tkt.LocationId = 0;
            int user_id = User.IsTechAdmin && Tkt.UserId > 0 ? Tkt.UserId : User.UserId;
            return Tickets.CreateNew(User.OrganizationId, User.DepartmentId, user_id,
                                                         Tkt.TechnicianId, user_id, DateTime.UtcNow, Tkt.AccountId,
                                                         Tkt.AccountLocationId, false, Tkt.LocationId, Tkt.ClassId,
                                                         Tkt.Level, Tkt.SubmissionCategory, Tkt.IsHandleByCallCentre,
                                                         Tkt.CreationCategoryId, false, Tkt.PriorityId,
                                                         Tkt.RequestCompletionDate.HasValue
                                                             ? Tkt.RequestCompletionDate.Value
                                                             : DateTime.MinValue, Tkt.RequestCompletionNote,
                                                         Tkt.SerialNumber, null, Tkt.IDMethod, Tkt.CustomFieldsXML,
                                                         Tkt.Subject, Tkt.InitialPost, null, Tkt.TicketStatus.ToString(),
                                                         out _initPostId, Tkt.ProjectId, Tkt.FolderId, 0,
                                                         Tkt.EstimatedTime);
        }

        public static int GetId (Guid OrgId, int DeptId, string TktPseudoId)
        {
            int TktId = 0, tempTktId = 0;
            if (!int.TryParse(TktPseudoId, out TktId))
                TktId = Tickets.GetTicketIDByPseudoID(OrgId, DeptId, TktPseudoId);
            else
            {
                tempTktId = Tickets.GetTicketIDByNumber(OrgId, DeptId, TktPseudoId);
            }
            if (tempTktId > 0)
                TktId = tempTktId;
            return TktId;
        }

        public static string GetPseudoId(Guid organizationId, int departmentId, int TktId)
        {
            return Tickets.SelectOne(organizationId, departmentId, TktId)["PseudoId"].ToString();
        }

        public static void AttachAlternateUser(ApiUser User, int TktId, int UserId)
        {
            AttachAlternateAssignee(User.OrganizationId, User.DepartmentId, TktId, UserId, TicketAssignmentType.User, true);
        }

        public static void AttachAlternateTechnician(ApiUser User, int TktId, int TechId)
        {
            AttachAlternateAssignee(User.OrganizationId, User.DepartmentId, TktId, TechId, TicketAssignmentType.Technician, true);
        }

        public static void EscalateTicketByLevel(ApiUser User, int TktId, bool escalate)
        {
            string error = "";
            int _errCode = UpdateEscalateByLevel(User.OrganizationId, User.DepartmentId, TktId, User.UserId, escalate, "", true);

            if (_errCode < 0)
            {
                throw new HttpError("No correct technician returned from routing routine.");
            }
        }

        public static void PickUp(ApiUser User, int TktId, string NoteText, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew)
        {
            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);

            if (_tktOld.TechnicianType == Logins.UserType.Queue)//Queue PickUp
            {
                UpdateTechnician(User.OrganizationId, User.DepartmentId, TktId, User.UserId, false);
                InsertLogMessage(User.OrganizationId, User.DepartmentId, TktId, User.UserId, "Queue Pickup", NoteText, "Picked Up from " + _tktOld.TechnicianFirstName + " " + _tktOld.TechnicianLastName + " by " + User.FullName);
                _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
                NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, User.UserId, NotificationRules.TicketEvent.PickUpTicket, _tktNew, _tktOld, 0, DateTime.MinValue, null);
            }
            else
            {
                Ticket.UpdateTechnician(User.OrganizationId, User.DepartmentId, TktId, User.UserId, true);
                if (_tktOld.TicketStatus == Status.OnHold) Ticket.UpdateStatus(User.OrganizationId, User.DepartmentId, TktId, Ticket.Status.Open);
                CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);
                string sysGeneratedText = User.FullName + " picked up this " + _cNames.Ticket.FullSingular + " from " + _tktOld.TechnicianFirstName + " " + _tktOld.TechnicianLastName;
                sysGeneratedText += "<br>This " + _cNames.Ticket.FullSingular + " was transferred by choosing route by " + _cNames.Technician.FullSingular + ".";
                if (_tktOld.TicketStatus == Status.OnHold) sysGeneratedText += "<br>This " + _cNames.Ticket.FullSingular + " was set back to \"Open\" status from \"On Hold\" status.";
                InsertLogMessage(User.OrganizationId, User.DepartmentId, TktId, User.UserId, "Transfer", NoteText, sysGeneratedText);
                _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
                NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, User.UserId, NotificationRules.TicketEvent.TransferTicket, _tktNew, _tktOld, 0, DateTime.MinValue, null);
            }
        }

        public static void Response(ApiUser User, int TktId, string NoteText, bool? Waiting, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew, string[] Files, int user_id)
        {
            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);
            string _toReceipts = string.Empty;
            List<Models.TicketAssignee> _tktUsers = Models.TicketAssignments.TicketUsers(User.OrganizationId, User.DepartmentId, TktId);
            foreach (Models.TicketAssignee _ta in _tktUsers)
            {
                if (_ta.UserId == User.UserId) continue;
                _toReceipts += _ta.UserFullName + ", ";
            }
            List<Models.TicketAssignee> _tktTechs = Models.TicketAssignments.TicketTechnicians(User.OrganizationId, User.DepartmentId, TktId);
            foreach (Models.TicketAssignee _ta in _tktTechs)
            {
                if (_ta.UserId == User.UserId) continue;
                _toReceipts += _ta.UserFullName + ", ";
            }

            if (_tktOld.Waiting)
                Waiting = null;

            user_id = User.IsTechAdmin && user_id > 0 ? user_id : User.UserId; 

            InsertResponse(User.OrganizationId, User.DepartmentId, TktId, user_id, false, _tktOld.UserId == user_id, false, NoteText, string.Empty, 0, Files, _toReceipts, false, Waiting);
            _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
            foreach (TicketAssignee _usrTkt in _tktNew.Users) _usrTkt.SendResponse = true;
            foreach (TicketAssignee _usrTkt in _tktNew.Technicians) _usrTkt.SendResponse = true;
            FileItem[] attachments = null;

            if (Files != null && Files.Length > 0)
            {
                attachments = Models.Files.GetTicketFiles(User.InstanceId, TktId, Files).ToArray();
            }
            NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, user_id, NotificationRules.TicketEvent.TicketResponse, _tktNew, null, 0, DateTime.MinValue, attachments);
        }

        public static void Close(ApiUser User, int TktId, string NoteText, bool SendNotifications, bool resolved, int resolution_id, bool confirmed, string confirm_note, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew)
        {
            Config _cfg = new Config(User.OrganizationId, User.InstanceId);
            CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);
            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);

            if (_cfg.ResolutionTracking)
            {
                UpdateResolution(User.OrganizationId, User.DepartmentId, TktId, resolution_id, resolved);
            }

            if (_cfg.ConfirmationTracking)
                UpdateConfirmation(User.OrganizationId, User.DepartmentId, TktId, User.UserId, confirmed, confirm_note + " "); 

            string sysGeneratedText = _cNames.Ticket.FullSingular + " was CLOSED by " + User.FullName + ".";
            CloseTicket(User.OrganizationId, User.DepartmentId, TktId, User.UserId, NoteText, sysGeneratedText, DateTime.UtcNow, string.Empty);
            if (SendNotifications)
            {
                _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true)
                                  {IsSendNotificationEmail = true};
                foreach (TicketAssignee _ta in _tktNew.Users) _ta.SendResponse = true;
                foreach (TicketAssignee _ta in _tktNew.Technicians) _ta.SendResponse = true;
                NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, User.UserId,
                                                         NotificationRules.TicketEvent.CloseTicket, _tktNew, _tktOld, 0,
                                                         DateTime.MinValue, null);
            }
        }

        public static void ReOpen(ApiUser User, int TktId, string NoteText, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew)
        {
            CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);
            Ticket _tktOld = new Ticket(User.OrganizationId, User.DepartmentId, TktId, User.InstanceId);

            if (_tktOld.TicketStatus != Ticket.Status.Closed) throw new HttpError(_cNames.Ticket.FullSingular + " is not Closed now to ReOpen it.");
            InsertResponse(User.OrganizationId, User.DepartmentId, TktId, User.UserId, false, _tktOld.UserId == User.UserId, false, NoteText, string.Empty, 0, string.Empty, true);
            _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
            foreach (TicketAssignee _ta in _tktNew.Users) _ta.SendResponse = true;
            foreach (TicketAssignee _ta in _tktNew.Technicians) _ta.SendResponse = true;
            NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, User.UserId, NotificationRules.TicketEvent.ReOpenTicket, _tktNew, null, 0, DateTime.MinValue, null);
        }

        public static void TransferToUser(ApiUser User, int TktId, int UserId, string NoteText, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew)
        {
            DataRow _row = Logins.SelectUserDetails(User.OrganizationId, User.DepartmentId, UserId);
            CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);
            if (_row == null) throw new HttpError("The " + _cNames.EndUser.fullSingular + " account Id=" + UserId.ToString() + " is not associated with this Department.");

            string userFullName = _row["FirstName"].ToString() + " " + _row["LastName"].ToString();

            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);

            UpdateEndUser(User.OrganizationId, User.DepartmentId, TktId, UserId);

            string _logText = "This " + _cNames.Ticket.FullSingular + " was transferred from " + _cNames.EndUser.fullSingular  + "  " + _tktOld.UserFirstName + " " + _tktOld.UserLastName + " to " + userFullName + " by " + User.FirstName + " " + User.LastName + ".";
            InsertLogMessage(User.OrganizationId, User.DepartmentId, TktId, User.UserId, "End User Changed", string.Empty, _logText);
            _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
        }

        public static void TransferToTech(ApiUser User, int TktId, int TechId, string NoteText, bool KeepTechAttached, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew)
        {
            DataRow _row = Logins.SelectUserDetails(User.OrganizationId, User.DepartmentId, TechId);
            if (_row == null) throw new HttpError("The user account Id=" + TechId.ToString() + " is not associated with this Department.");

            string techFullName = _row["FirstName"].ToString() + " " + _row["LastName"].ToString();
            CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);

            if ((int)_row["UserType_Id"] != 2 && (int)_row["UserType_Id"] != 3 && (int)_row["UserType_Id"] != 4) throw new HttpError("User " + techFullName + " is not " + _cNames.Technician.FullSingular + " or Administrator or Queue.");

            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);

            UpdateTechnician(User.OrganizationId, User.DepartmentId, TktId, TechId, KeepTechAttached);
            if (_tktOld.TicketStatus == Status.OnHold) UpdateStatus(User.OrganizationId, User.DepartmentId, TktId, Status.Open);
            string sysGeneratedText = User.FullName;
            if (TechId == User.UserId)
                sysGeneratedText += " picked up this " + _cNames.Ticket.FullSingular + " from " + _tktOld.TechnicianFirstName + " " + _tktOld.TechnicianLastName;
            else
                sysGeneratedText += " transfered this " + _cNames.Ticket.FullSingular + " to " + techFullName + ".";
            sysGeneratedText += "<br>This " + _cNames.Ticket.FullSingular + " was transferred by choosing route by " + _cNames.Technician.FullSingular + ".";
            if (_tktOld.TicketStatus == Status.OnHold) sysGeneratedText += "<br>This " + _cNames.Ticket.FullSingular + " was set back to \"Open\" status from \"On Hold\" status.";
            InsertLogMessage(User.OrganizationId, User.DepartmentId, TktId, User.UserId, "Transfer", NoteText, sysGeneratedText);
            _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
        }

        public static void EditTime(ApiUser User,  int ticketTimeID, DateTime? start_date, DateTime? stop_date, bool? is_billable, DateTime? date, decimal? Hours=null, int TaskTypeId=0,
            string NoteText = "", decimal? remainHours = null, int? complete = null, int? contract_id = null, string contractName = "")
        { 
            int TktId = 0;
            int TechId = 0;
            int HoursOffset = User.TimeZoneOffset;
            Config _cfg = new Config(User.OrganizationId, User.InstanceId);
            int _contract_id = contract_id ?? 0;

            if (!_cfg.TimeTracking) throw new HttpError("Time Tracking is not enabled for this instance.");

            Ticket_Time_Log timeEntry = null;

            if (ticketTimeID > 0)
            {
                DataRow row = Tickets.SelectTicketTimeByID(User.OrganizationId, User.DepartmentId, ticketTimeID);
                if (row == null)
                {
                    throw new HttpError(System.Net.HttpStatusCode.NotFound, "Wrong Ticket Time Id");
                }
                else
                {
                    timeEntry = new Ticket_Time_Log(row);
                    if (timeEntry.Invoice_ID > 0)
                    {
                        throw new HttpError("Time log has been invoiced and can not be edited.");
                    }
                    if (timeEntry.PaymentId > 0)
                    {
                        throw new HttpError("Time log has been paid and can not be edited.");
                    }
                }
            }
            else
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Missing Ticket Time Id");

            TechId = timeEntry.User_ID;
            TktId = timeEntry.TicketID;
            int _taskTypeId = timeEntry.Task_Type_ID;
            string _taskTypeName = timeEntry.Task_Type_Name;

            if (TaskTypeId > 0)
            {
                _taskTypeId = TaskTypeId;
                DataRow _rowTaskType = bigWebApps.bigWebDesk.Data.TaskType.SelectTaskType(User.OrganizationId, User.DepartmentId, _taskTypeId);

                if (_rowTaskType == null)
                    throw new HttpError("No Task Types found for TaskTypeId=" + _taskTypeId.ToString() + ".");
                _taskTypeName = _rowTaskType["TaskTypeName"].ToString();
            }

            string _hoursFull = "";
            Hours = Hours ?? timeEntry.Hours;
            decimal hours = Hours ?? 0;

            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);
            string sysGeneratedText = User.FullName;
            NoteText = string.IsNullOrWhiteSpace(NoteText) ? timeEntry.Note : NoteText;
            remainHours = remainHours ?? timeEntry.Hours_Remaining;
            int _complete = complete.HasValue ? complete.Value : timeEntry.Complete;
            bool _is_billable = is_billable.HasValue ? is_billable.Value : timeEntry.Is_Billable;
            if (hours > 0)
            {
                decimal _hRate = Logins.SelectTechHourlyRate(User.OrganizationId, User.DepartmentId, User.UserId, _taskTypeId);
                DateTime startDate = DateTime.MinValue;
                DateTime stopDate = DateTime.MinValue;
                if (start_date.HasValue && start_date > DateTime.MinValue)
                    startDate = start_date.Value;//.AddHours(HoursOffset);
                else if (timeEntry.Start_Time.HasValue && timeEntry.Start_Time > DateTime.MinValue)
                {
                    startDate = timeEntry.Start_Time.Value;
                }

                if (stop_date.HasValue && stop_date > DateTime.MinValue)
                {
                    stopDate = stop_date.Value;//.AddHours(HoursOffset);
                }
                else if (startDate != DateTime.MinValue)
                {
                    stopDate = startDate;//.AddHours((double)Hours.Value);
                }
                else if (timeEntry.Stop_Time.HasValue && timeEntry.Stop_Time > DateTime.MinValue)
                {
                    stopDate = timeEntry.Stop_Time.Value;
                }

                if (hours == 0 && startDate > DateTime.MinValue && stopDate > DateTime.MinValue)
                    hours = Convert.ToDecimal((stopDate - startDate).TotalHours);

                if (hours >= 1)
                {
                    _hoursFull = ((int)hours).ToString();
                    if ((int)hours == 1) _hoursFull += " hour ";
                    else _hoursFull += " hours ";
                }

                string _minutes = string.Format("{0:00}", hours * 60 % 60).TrimStart('0');

                if (!string.IsNullOrEmpty(_minutes))
                {
                    _hoursFull += _minutes;
                    if (_minutes == "1") _hoursFull += " minute";
                    else _hoursFull += " minutes";
                }

                if (!string.IsNullOrEmpty(_hoursFull)) _hoursFull = "(" + _hoursFull.Trim() + ")";

                sysGeneratedText += "edited " + hours.ToString("0.00") + " hours " + _hoursFull + " as " + _taskTypeName + " task type";
                if (_contract_id > 0)
                {
                    sysGeneratedText += " on " + contractName + " contract";
                }
                sysGeneratedText += ".";
                DateTime dateTimeLog = timeEntry.Date.Value;
                if (date.HasValue && date > DateTime.MinValue)
                {
                    dateTimeLog = (DateTime)date;
                }
                Tickets.UpdateTicketTime(User.OrganizationId, ticketTimeID, User.DepartmentId, TktId, TechId, dateTimeLog, hours,
                    NoteText + "", _hRate, startDate, stopDate, _taskTypeId, remainHours ?? 0, DateTime.UtcNow, User.UserId, HoursOffset, _complete, true, _is_billable, _contract_id);
                CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);
                bigWebApps.bigWebDesk.Data.Ticket _tkt = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
                bigWebApps.bigWebDesk.Data.Ticket.InsertLogMessage(User.OrganizationId, User.DepartmentId, TktId, User.UserId, "Log Entry", NoteText, sysGeneratedText, string.Empty, string.Empty, string.Empty, ticketTimeID);
                if (NoteText.Length > 0) sysGeneratedText = NoteText + "<br><br>" + sysGeneratedText;
                _tkt.TicketLogs.Insert(0, new bigWebApps.bigWebDesk.Data.Ticket.LogEntry(User.UserId, User.LoginEmail, User.FirstName, User.LastName, DateTime.UtcNow, _cNames + " Costs", sysGeneratedText));
                foreach (bigWebApps.bigWebDesk.Data.Ticket.TicketAssignee ta in _tkt.Technicians)
                    ta.SendResponse = true;
                foreach (bigWebApps.bigWebDesk.Data.Ticket.TicketAssignee ta in _tkt.Users)
                    ta.SendResponse = true;
                bigWebApps.bigWebDesk.Data.NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, User.UserId,
                    bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.EnterLaborCosts, _tkt, null, null);
            }
        }


        public static void InputTime(ApiUser User, int TktId, int TaskTypeId, DateTime? start_date, DateTime? stop_date, decimal Hours, int HoursOffset,
            string NoteText, bool is_billable, DateTime? date, int ticketTimeID, int TechId, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew, int? complete, decimal? remainHours,
            int? contract_id, string toDoItemId)
        {
            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);
            DateTime dateTimeLog = DateTime.UtcNow;
            if (date.HasValue && date > DateTime.MinValue)
            {
                dateTimeLog = (DateTime)date;
            }
            int _contract_id = contract_id ?? 0;
            string contractName = Contracts.CheckAccountContract(User, _tktOld.AccountId, _contract_id, dateTimeLog, ticketTimeID, 0, _tktOld.ProjectId);
            if (ticketTimeID > 0)
            {
                Ticket.EditTime(User, ticketTimeID, start_date, stop_date, is_billable, date, Hours, TaskTypeId, NoteText + "", remainHours, complete, contract_id, contractName);
                return;
            }

            Config _cfg = new Config(User.OrganizationId, User.InstanceId);
            CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);

            if (!_cfg.TimeTracking) throw new HttpError("Time Tracking is not enabled for this instance.");

            int _taskTypeId = 0;
            string _taskTypeName = string.Empty;

            if (TaskTypeId == 0)
            {
                DataTable _dtTaskTypes = bigWebApps.bigWebDesk.Data.TaskType.SelectTicketAssignedTaskTypes(User.OrganizationId, User.DepartmentId, TechId, TktId);

                if (_dtTaskTypes.Rows.Count == 0)
                    throw new HttpError("No Assigned Task Types found for this " + _cNames.Ticket.fullSingular + ".");

                _taskTypeId = (int) _dtTaskTypes.Rows[0]["ttID"];
                _taskTypeName = _dtTaskTypes.Rows[0]["TaskTypeName"].ToString();
            }
            else
            {
                _taskTypeId = TaskTypeId;
                DataRow _rowTaskType = bigWebApps.bigWebDesk.Data.TaskType.SelectTaskType(User.OrganizationId, User.DepartmentId, _taskTypeId);

                if (_rowTaskType == null)
                    throw new HttpError("No Task Types found for TaskTypeId="+ _taskTypeId.ToString() + ".");
                _taskTypeName = _rowTaskType["TaskTypeName"].ToString();
            }

            string _hoursFull = "";

            if (Hours >= 1)
            {
                _hoursFull = ((int)Hours).ToString();
                if ((int)Hours == 1) _hoursFull += " hour ";
                else _hoursFull += " hours ";
            }

            string _minutes = string.Format("{0:00}", Hours * 60 % 60).TrimStart('0');

            if (!string.IsNullOrEmpty(_minutes))
            {
                _hoursFull += _minutes;
                if (_minutes == "1") _hoursFull += " minute";
                else _hoursFull += " minutes";
            }

            if (!string.IsNullOrEmpty(_hoursFull)) _hoursFull = "(" + _hoursFull.Trim() + ")";

            string sysGeneratedText = User.FullName;
            int _timeLogId = 0;

            if (Hours > 0)
            {
                sysGeneratedText +=  " logged " + Hours.ToString("0.00") + " hours " + _hoursFull + " as " + _taskTypeName + " task type";
                if (_contract_id > 0)
                {
                    sysGeneratedText += " on " + contractName + " contract";
                }
                sysGeneratedText += ".";
                decimal _hRate = Logins.SelectTechHourlyRate(User.OrganizationId, User.DepartmentId, TechId, _taskTypeId);
                DateTime startDate = DateTime.MinValue;
                DateTime stopDate = DateTime.MinValue;
                if (start_date.HasValue && start_date > DateTime.MinValue)
                    startDate = start_date.Value;//.AddHours(HoursOffset);
                if (stop_date.HasValue && stop_date > DateTime.MinValue)
                {
                    stopDate = stop_date.Value;//.AddHours(HoursOffset);
                }
                else if (startDate != DateTime.MinValue)
                {
                    stopDate = startDate.AddHours((double)Hours);
                }

                _timeLogId = Tickets.InsertTime(User.OrganizationId, User.DepartmentId, TktId, TechId, dateTimeLog, Hours, NoteText + "", _hRate, startDate, stopDate,
                    _taskTypeId, remainHours ?? 0, DateTime.UtcNow, TechId, HoursOffset, false, complete ?? 0, true, is_billable, _contract_id, toDoItemId);
            }
            InsertResponse(User.OrganizationId, User.DepartmentId, TktId, TechId, false, _tktOld.UserId == TechId, false, NoteText + "", sysGeneratedText, _timeLogId, string.Empty, false);
            _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
            foreach (TicketAssignee _ta in _tktNew.Users) _ta.SendResponse = true;
            foreach (TicketAssignee _ta in _tktNew.Technicians) _ta.SendResponse = true;
            NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, TechId, NotificationRules.TicketEvent.EnterLaborCosts, _tktNew, _tktOld, 0, DateTime.MinValue, null);
        }

        public static void OnHold(ApiUser User, int TktId, string NoteText, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew)
        {
            Config _cfg = new Config(User.OrganizationId, User.InstanceId);
            CustomNames _cNames = CustomNames.GetCustomNames(User.OrganizationId, User.DepartmentId);

            if (!_cfg.OnHoldStatus) throw new HttpError("On Hold " + _cNames.Ticket.fullSingular + " statuses are not enabled for this instance.");

            var _tktOld = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId);
            InsertResponse(User.OrganizationId, User.DepartmentId, TktId, User.UserId, false, false, true, NoteText+"", string.Empty, 0, string.Empty, false);
            NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, User.UserId, NotificationRules.TicketEvent.PlaceOnHoldTicket, new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true), _tktOld, 0, DateTime.MinValue, null);
            _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
        }

        public static void Confirm(ApiUser User, int TktId, string NoteText, ref bigWebApps.bigWebDesk.Data.Ticket _tktNew)
        {
            Config _cfg = new Config(User.OrganizationId, User.InstanceId);

            if (!_cfg.ConfirmationTracking) throw new HttpError("Confirmation Tracking is not enabled for this instance.");

            UpdateConfirmation(User.OrganizationId, User.DepartmentId, TktId, User.UserId, true, NoteText + "");
            _tktNew = new bigWebApps.bigWebDesk.Data.Ticket(User.OrganizationId, User.DepartmentId, TktId, true);
            foreach (TicketAssignee _ta in _tktNew.Users) _ta.SendResponse = true;
            foreach (TicketAssignee _ta in _tktNew.Technicians) _ta.SendResponse = true;
            NotificationRules.RaiseNotificationEvent(User.OrganizationId, User.DepartmentId, User.UserId, NotificationRules.TicketEvent.TicketConfirmation, _tktNew, null, 0, DateTime.MinValue, null);
        }

        public static void DeleteTicketTime(ApiUser user, int tktTimeID)
        {
            DataRow row = Tickets.SelectTicketTimeByID(user.OrganizationId, user.DepartmentId, tktTimeID);
            if (row == null)
            {
                throw new HttpError(System.Net.HttpStatusCode.NotFound, "Wrong Time Id");
            }
            else
            {
                if (!row.IsNull("InvoiceId"))
                {
                    if(int.Parse(row["InvoiceId"].ToString()) > 0)
                    {
                        throw new HttpError("Time log has been invoiced and can not be removed.");
                    }
                }
            }
            Tickets.DeleteTicketTime(user.OrganizationId, tktTimeID, user.DepartmentId, int.Parse(row["TicketId"].ToString()), true);
            string timeLogEntry = user.FullName + " deleted " + Convert.ToDecimal(row["Hours"].ToString()).ToString("0.00") + " time, " + row["TaskTypeName"].ToString() + " task type.";
            var tktId = int.Parse(row["TicketID"].ToString());
            CustomNames _cNames = CustomNames.GetCustomNames(user.OrganizationId, user.DepartmentId);
            bigWebApps.bigWebDesk.Data.Ticket _tkt = new bigWebApps.bigWebDesk.Data.Ticket(user.OrganizationId, user.DepartmentId, tktId, true);
            bigWebApps.bigWebDesk.Data.Ticket.InsertLogMessage(user.OrganizationId, user.DepartmentId, tktId, user.UserId, "Log Entry", string.Empty, timeLogEntry, string.Empty, string.Empty, string.Empty, tktTimeID);
            _tkt.TicketLogs.Insert(0, new bigWebApps.bigWebDesk.Data.Ticket.LogEntry(user.UserId, user.LoginEmail, user.FirstName, user.LastName, DateTime.UtcNow, _cNames + " Costs", timeLogEntry));
            bigWebApps.bigWebDesk.Data.NotificationRules.RaiseNotificationEvent(user.OrganizationId, user.DepartmentId, user.UserId, bigWebApps.bigWebDesk.Data.NotificationRules.TicketEvent.EnterLaborCosts,
                _tkt, null, null);
        }

        #endregion

        #region ModelItemBaseInterface Members

        public DataRow Row
        {
            get { return m_Row; }
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get {return base.ID;}
            set {base.ID = value;}
        }
        #endregion

        [DataMember(Name = "key")]
        new public string PseudoID
        {
            get { return base.PseudoID; }
            set { base.PseudoID = value; }
        }

        [DataMember(Name = "created_time")]
        new public DateTime? CreateTime
        {
            get { if (base.CreateTime != DateTime.MinValue) return base.CreateTime; else return null; }
            set { base.CreateTime = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "closed_time")]
        new public DateTime? ClosedTime
        {
            get { if (base.ClosedTime != DateTime.MinValue) return base.ClosedTime; else return null; }
            set { base.ClosedTime = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "request_completion_date")]
        new public DateTime? RequestCompletionDate
        {
            get { if (base.RequestCompletionDate != DateTime.MinValue) return base.RequestCompletionDate; else return null; }
            set { base.RequestCompletionDate = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "is_waiting_on_response")]
        new public bool Waiting
        {
            get { return base.Waiting; }
            set { }
        }

        [DataMember(Name = "waiting_date")]
        new public DateTime? WaitingDate
        {
            get { return base.WaitingDate; }
            set { base.WaitingDate = value; }
        }

        [DataMember(Name = "waiting_minutes")]
        new public int WaitingUsed
        {
            get { return base.WaitingUsed; }
            set { base.WaitingUsed = value; }
        }

        [DataMember(Name = "followup_date")]
        new public DateTime? FollowUpDate
        {
            get { if (base.FollowUpDate != DateTime.MinValue) return base.FollowUpDate; else return null; }
            set { base.FollowUpDate = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "sla_complete_date")]
        new public DateTime? SLAComplete
        {
            get { if (base.SLAComplete != DateTime.MinValue) return base.SLAComplete; else return null; }
            set { base.SLAComplete = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "sla_response_date")]
        new public DateTime? SLARespose
        {
            get { if (base.SLARespose != DateTime.MinValue) return base.SLARespose; else return null; }
            set { base.SLARespose = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "level_deescalate")]
        public int LevelDescalate
        {
            get; set;
        }


        [DataMember(Name = "level_escalate")]
        public int LevelEscalate
        {
            get; set;
        }

        [DataMember(Name = "confirmed_date")]
        new public DateTime? ConfirmedDate
        {
            get { if (base.ConfirmedDate != DateTime.MinValue) return base.ConfirmedDate; else return null; }
            set { base.ConfirmedDate = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "next_step_date")]
        new public DateTime? NextStepDate
        {
            get { if (base.NextStepDate != DateTime.MinValue) return base.NextStepDate; else return null; }
            set { base.NextStepDate = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "updated_time")]
        new public DateTime? UpdatedTime
        {
            get { if (base.UpdatedTime != DateTime.MinValue) return base.UpdatedTime; else return null; }
            set { base.UpdatedTime = value.HasValue ? value.Value : DateTime.MinValue; }
        }

        [DataMember(Name = "organization_key")]
        new public Guid OrganizationId
        {
            get { return base.OrganizationId; }
            set { base.OrganizationId = value; }
        }

        [DataMember(Name = "department_key")]
        new public int DepartmentID
        {
            get { return base.DepartmentID; }
            set { base.DepartmentID = value; }
        }

        [DataMember(Name = "is_deleted")]
        new public bool IsDeleted
        {
            get { return base.IsDeleted; }
            set { base.IsDeleted = value; }
        }

        [DataMember(Name = "user_id")]
        new public int UserId
        {
            get { return base.UserId; }
            set { base.UserId = value; }
        }

        [DataMember(Name = "user_title")]
        new public string UserTitle
        {
            get { return base.UserTitle; }
            set { base.UserTitle = value; }
        }

        [DataMember(Name = "user_firstname")]
        new public string UserFirstName
        {
            get { return base.UserFirstName; }
            set { base.UserFirstName = value; }
        }

        [DataMember(Name = "user_lastname")]
        new public string UserLastName
        {
            get { return base.UserLastName; }
            set { base.UserLastName = value; }
        }

        [DataMember(Name = "user_email")]
        new public string UserEmail
        {
            get { return base.UserEmail; }
            set { base.UserEmail = value; }
        }

       //[DataMember]
        new public string UserMobileEmail
        {
            get { return base.UserMobileEmail; }
            set { base.UserMobileEmail = value; }
        }

       //[DataMember]
        new public bigWebApps.bigWebDesk.Data.Logins.EmailType UserMobileEmailType
        {
            get { return base.UserMobileEmailType; }
            set { base.UserMobileEmailType = value; }
        }

       //[DataMember]
        new public string UserPhone
        {
            get { return base.UserPhone; }
            set { base.UserPhone = value; }
        }

       //[DataMember]
        new public string UserMobilePhone
        {
            get { return base.UserMobilePhone; }
            set { base.UserMobilePhone = value; }
        }

        [DataMember(Name = "tech_id")]
        new public int TechnicianId
        {
            get { return base.TechnicianId; }
            set { base.TechnicianId = value; }
        }

        [DataMember(Name = "tech_firstname")]
        new public string TechnicianFirstName
        {
            get { return base.TechnicianFirstName; }
            set { base.TechnicianFirstName = value; }
        }

        [DataMember(Name = "tech_lastname")]
        new public string TechnicianLastName
        {
            get { return base.TechnicianLastName; }
            set { base.TechnicianLastName = value; }
        }

        [DataMember(Name = "tech_email")]
        new public string TechnicianEmail
        {
            get { return base.TechnicianEmail; }
            set { base.TechnicianEmail = value; }
        }

       //[DataMember]
        new public string TechnicianMobileEmail
        {
            get { return base.TechnicianMobileEmail; }
            set { base.TechnicianMobileEmail = value; }
        }

       //[DataMember]
        new public Logins.EmailType TechnicianMobileEmailType
        {
            get { return base.TechnicianMobileEmailType; }
            set { base.TechnicianMobileEmailType = value; }
        }

       //[DataMember]
        new public string TechnicianPhone
        {
            get { return base.TechnicianPhone; }
            set { base.TechnicianPhone = value; }
        }

       //[DataMember]
        new public string TechnicianMobilePhone
        {
            get { return base.TechnicianMobilePhone; }
            set { base.TechnicianMobilePhone = value; }
        }

        [DataMember(Name = "priority")]
        new public int Priority
        {
            get { return base.Priority; }
            set { base.Priority = value; }
        }

        [DataMember(Name = "priority_name")]
        new public string PriorityName
        {
            get { return base.PriorityName; }
            set { base.PriorityName = value; }
        }

        [DataMember(Name = "priority_id")]
        new public int PriorityId
        {
            get { return base.PriorityId; }
            set { base.PriorityId = value; }
        }

        [DataMember(Name = "user_created_id")]
        new public int UserCreatedId
        {
            get { return base.UserCreatedId; }
            set { base.UserCreatedId = value; }
        }

        [DataMember(Name = "user_created_firstname")]
        new public string UserCreatedFirstName
        {
            get { return base.UserCreatedFirstName; }
            set { base.UserCreatedFirstName = value; }
        }

        [DataMember(Name = "user_created_lastname")]
        new public string UserCreatedLastName
        {
            get { return base.UserCreatedLastName; }
            set { base.UserCreatedLastName = value; }
        }

        [DataMember(Name = "user_created_email")]
        new public string UserCreatedEmail
        {
            get { return base.UserCreatedEmail; }
            set { base.UserCreatedEmail = value; }
        }

       //[DataMember(Name = "status")]
        new public Status TicketStatus
        {
            get { return base.TicketStatus; }
            set { base.TicketStatus = value; }
        }

        [DataMember(Name = "status")]
        new public string TicketStatusString
        {
            get { if (base.Waiting) return "Waiting On Response";
                return base.TicketStatus.ToString(); }
            set {  }
        }

        [DataMember(Name = "location_id")]
        new public int LocationId
        {
            get { return base.LocationId; }
            set { base.LocationId = value; }
        }

        [DataMember(Name = "location_name")]
        new public string LocationName
        {
            get { return base.LocationName; }
            set { base.LocationName = value; }
        }

        [DataMember(Name = "class_id")]
        new public int ClassId
        {
            get { return base.ClassId; }
            set { base.ClassId = value; }
        }

        [DataMember(Name = "class_name")]
        new public string ClassName
        {
            get { return base.ClassName; }
            set { base.ClassName = value; }
        }

        [DataMember(Name = "project_id")]
        new public int ProjectId
        {
            get { return base.ProjectId; }
            set { base.ProjectId = value; }
        }

        [DataMember(Name = "project_name")]
        new public string ProjectName
        {
            get { return base.ProjectName; }
            set { base.ProjectName = value; }
        }

        [DataMember(Name = "serial_number")]
        new public string SerialNumber
        {
            get { return base.SerialNumber; }
            set { base.SerialNumber = value; }
        }

        [DataMember(Name = "folder_id")]
        new public int FolderId
        {
            get { return base.FolderId; }
            set { base.FolderId = value; }
        }

        [DataMember(Name = "folder_path")]
        new public string FolderPath
        {
            get { return base.FolderPath; }
            set { base.FolderPath = value; }
        }

        [DataMember(Name = "creation_category_id")]
        new public int CreationCategoryId
        {
            get { return base.CreationCategoryId; }
            set { base.CreationCategoryId = value; }
        }

        [DataMember(Name = "creation_category_name")]
        new public string CreationCategoryName
        {
            get { return base.CreationCategoryName; }
            set { base.CreationCategoryName = value; }
        }

        [DataMember(Name = "subject")]
        new public string Subject
        {
            get { return base.Subject; }
            set { base.Subject = value; }
        }

        [DataMember(Name = "note")]
        new public string Note
        {
            get { return base.Note; }
            set { base.Note = value; }
        }

        [DataMember(Name = "number")]
        new public int TicketNumberInt
        {
            get { return Convert.ToInt32(base.TicketNumber); }
            set { base.TicketNumber = value.ToString(); }
        }

       //[DataMember]
        new public string TicketNumber
        {
            get;
            set;
        }

        [DataMember(Name = "prefix")]
        new public string TicketNumberPrefix
        {
            get { return base.TicketNumberPrefix; }
            set { base.TicketNumberPrefix = value; }
        }

        [DataMember(Name = "customfields_xml")]
        new public string CustomFieldsXML
        {
            get { return base.CustomFieldsXML; }
            set { base.CustomFieldsXML = value; }
        }

        [DataMember(Name = "parts_cost")]
        new public decimal PartsCost
        {
            get { return base.PartsCost; }
            set { base.PartsCost = value; }
        }

        [DataMember(Name = "labor_cost")]
        new public decimal LaborCost
        {
            get { return base.LaborCost; }
            set { base.LaborCost = value; }
        }

        [DataMember(Name = "total_time_in_minutes")]
        new public int TotalTimeMinutes
        {
            get { return base.TotalTimeMinutes; }
            set { base.TotalTimeMinutes = value; }
        }

        [DataMember(Name = "misc_cost")]
        new public decimal MiscCost
        {
            get { return base.MiscCost; }
            set { base.MiscCost = value; }
        }

        [DataMember(Name = "travel_cost")]
        new public decimal TravelCost
        {
            get { return base.TravelCost; }
            set { base.TravelCost = value; }
        }

        [DataMember(Name = "request_completion_note")]
        new public string RequestCompletionNote
        {
            get { return base.RequestCompletionNote; }
            set { base.RequestCompletionNote = value; }
        }

        [DataMember(Name = "followup_note")]
        new public string FollowUpNote
        {
            get { return base.FollowUpNote; }
            set { base.FollowUpNote = value; }
        }

        [DataMember(Name = "initial_response")]
        new public bool InitResponse
        {
            get { return base.InitResponse; }
            set { base.InitResponse = value; }
        }

        [DataMember(Name = "sla_complete_used")]
        new public int SLACompleteUsed
        {
            get { return base.SLACompleteUsed; }
            set { base.SLACompleteUsed = value; }
        }

        [DataMember(Name = "sla_response_used")]
        new public int SLAResponseUsed
        {
            get { return base.SLAResponseUsed; }
            set { base.SLAResponseUsed = value; }
        }

        [DataMember(Name = "level")]
        new public int Level
        {
            get { return base.Level; }
            set { base.Level = value; }
        }

        [DataMember(Name = "level_name")]
        new public string LevelName
        {
            get { return base.LevelName; }
            set { base.LevelName = value; }
        }

        [DataMember(Name = "is_via_email_parser")]
        new public bool IsViaEmailParser
        {
            get { return base.IsViaEmailParser; }
            set { base.IsViaEmailParser = value; }
        }

        [DataMember(Name = "account_id")]
        new public int AccountId
        {
            get { return base.AccountId; }
            set { base.AccountId = value; }
        }

        [DataMember(Name = "account_name")]
        new public string AccountName
        {
            get { return base.AccountName; }
            set { base.AccountName = value; }
        }

        [DataMember(Name = "account_location_id")]
        new public int AccountLocationId
        {
            get { return base.AccountLocationId; }
            set { base.AccountLocationId = value; }
        }

        [DataMember(Name = "account_location_name")]
        new public string AccountLocationName
        {
            get { return base.AccountLocationName; }
            set { base.AccountLocationName = value; }
        }

        [DataMember(Name = "resolution_category_id")]
        new public int ResolutionCategoryId
        {
            get { return base.ResolutionCategoryId; }
            set { base.ResolutionCategoryId = value; }
        }

        [DataMember(Name = "resolution_category_name")]
        new public string ResolutionCategoryName
        {
            get { return base.ResolutionCategoryName; }
            set { base.ResolutionCategoryName = value; }
        }

        [DataMember(Name = "is_resolved")]
        new public bool IsResolved
        {
            get { return base.IsResolved; }
            set { base.IsResolved = value; }
        }

        [DataMember(Name = "confirmed_by_name")]
        new public string ConfirmedBy
        {
            get { return base.ConfirmedBy; }
            set { base.ConfirmedBy = value; }
        }

        [DataMember(Name = "is_confirmed")]
        new public bool IsConfirmed
        {
            get { return base.IsConfirmed; }
            set { base.IsConfirmed = value; }
        }

        [DataMember(Name = "confirmed_note")]
        new public string ConfirmedNote
        {
            get { return base.ConfirmedNote; }
            set { base.ConfirmedNote = value; }
        }

        [DataMember(Name = "support_group_id")]
        new public int SupportGroupId
        {
            get { return base.SupportGroupId; }
            set { base.SupportGroupId = value; }
        }

        [DataMember(Name = "support_group_name")]
        new public string SupportGroupName
        {
            get { return base.SupportGroupName; }
            set { base.SupportGroupName = value; }
        }

        [DataMember(Name = "is_handle_by_callcentre")]
        new public bool IsHandleByCallCentre
        {
            get { return base.IsHandleByCallCentre; }
            set { base.IsHandleByCallCentre = value; }
        }

        [DataMember(Name = "submission_category")]
        new public string SubmissionCategory
        {
            get { return base.SubmissionCategory; }
            set { base.SubmissionCategory = value; }
        }

        [DataMember(Name = "is_user_inactive")]
        new public bool IsUserInactive
        {
            get { return base.IsUserInactive; }
            set { base.IsUserInactive = value; }
        }

        [DataMember(Name = "next_step")]
        new public string NextStep
        {
            get { return base.NextStep; }
            set { base.NextStep = value; }
        }

        [DataMember(Name = "total_hours")]
        new public decimal TotalHours
        {
            get { return base.TotalHours; }
            set { base.TotalHours = value; }
        }

        [DataMember(Name = "remaining_hours")]
        new public decimal RemainingHours
        {
            get { return base.RemainingHours; }
            set { base.RemainingHours = value; }
        }

        [DataMember(Name = "estimated_time")]
        new public decimal EstimatedTime
        {
            get { return base.EstimatedTime; }
            set { base.EstimatedTime = value; }
        }

		[DataMember(Name = "percentage_complete")]
		public decimal PercentageComplete
		{
			get { return base.EstimatedTime > 0 && base.EstimatedTime >= base.TotalHours ? Math.Round(100 * base.TotalHours/ base.EstimatedTime) : 0;}
                        set {}
		}

        [DataMember(Name = "workpad")]
        new public string Workpad
        {
            get { return base.Workpad; }
            set { base.Workpad = value; }
        }

        [DataMember(Name = "scheduled_ticket_id")]
        new public int SchedTicketID
        {
            get { return base.SchedTicketID; }
            set { base.SchedTicketID = value; }
        }

        [DataMember(Name = "kb")]
        new public bool KB
        {
            get { return base.KB; }
            set { base.KB = value; }
        }

        [DataMember(Name = "kb_type")]
        new public int KBType
        {
            get { return base.KBType; }
            set { base.KBType = value; }
        }

        [DataMember(Name = "kb_publish_level")]
        new public int KBPublishLevel
        {
            get { return base.KBPublishLevel; }
            set { base.KBPublishLevel = value; }
        }

        [DataMember(Name = "kb_search_desc")]
        new public string KBSearchDesc
        {
            get { return base.KBSearchDesc; }
            set { base.KBSearchDesc = value; }
        }

        [DataMember(Name = "kb_alternate_id")]
        new public string KBAlternateId
        {
            get { return base.KBAlternateId; }
            set { base.KBAlternateId = value; }
        }

        [DataMember(Name = "kb_helpful_count")]
        new public int KBHelpfulCount
        {
            get { return base.KBHelpfulCount; }
            set { base.KBHelpfulCount = value; }
        }

        [DataMember(Name = "kb_portal_alias")]
        new public string KBPortalAlias
        {
            get { return base.KBPortalAlias; }
            set { base.KBPortalAlias = value; }
        }

        [DataMember(Name = "initial_post")]
        new public string InitialPost
        {
            get { return base.InitialPost; }
            set { base.InitialPost = value; }
        }

        [DataMember(Name = "is_sent_notification_email")]
        new public bool IsSendNotificationEmail
        {
            get { return base.IsSendNotificationEmail; }
            set { base.IsSendNotificationEmail = value; }
        }

        [DataMember(Name = "email_cc")]
        new public string EmailCC
        {
            get { return base.EmailCC; }
            set { base.EmailCC = value; }
        }

        [DataMember(Name = "related_tickets_count")]
        new public int RelatedTicketsCount
        {
            get { return base.RelatedTicketsCount; }
            set { base.RelatedTicketsCount = value; }
        }

        [DataMember(Name = "days_old_in_minutes")]
        new public int DaysOldInMinutes
        {
            get { return base.DaysOldInMinutes; }
            set { base.DaysOldInMinutes = value; }
        }

        [DataMember(Name = "days_old")]
        public string days_old
        {
            get;  
            set;
        }

       //[DataMember]
        new public string IDMethod
        {
            get { return base.IDMethod; }
            set { base.IDMethod = value; }
        }

        [DataMember(Name = "tech_type")]
        new public Logins.UserType TechnicianType
        {
            get { return base.TechnicianType; }
            set { base.TechnicianType = value; }
        }

       //[DataMember]
        new public string UserCreatedMobileEmail
        {
            get { return base.UserCreatedMobileEmail; }
            set { base.UserCreatedMobileEmail = value; }
        }

       //[DataMember]
        new public Logins.EmailType UserCreatedMobileEmailType
        {
            get { return base.UserCreatedMobileEmailType; }
            set { base.UserCreatedMobileEmailType = value; }
        }

       //[DataMember]
        new public string UserCreatedPhone
        {
            get { return base.UserCreatedPhone; }
            set { base.UserCreatedPhone = value; }
        }

       //[DataMember]
        new public string UserCreatedMobilePhone
        {
            get { return base.UserCreatedMobilePhone; }
            set { base.UserCreatedMobilePhone = value; }
        }

        new public bool NoAccount
        {
            get { return base.NoAccount; }
            set { base.NoAccount = value; }
        }

        [DataMember(Name = "resolution_categories")]
        public new List<ResolutionCategory> ResolutionCategories { get; set; }
		[DataMember(Name = "users")]
		public new List<BWA.Api.Models.TicketAssignee> Users { get; set; }
		[DataMember(Name = "technicians")]
		public new List<BWA.Api.Models.TicketAssignee> Technicians { get; set; }
		[DataMember(Name = "ticketlogs")]
		public new List<BWA.Api.Models.TicketLogRecord> TicketLogs { get; set; }
		[DataMember(Name = "assets")]
		public List<BWA.Api.Models.Asset> Assets { get; set; }
		[DataMember(Name = "attachments")]
		public List<File> Files { get; set; }
		[DataMember(Name = "classes")]
		public List<BWA.Api.Models.Class> ParentClasses { get; set; }
        [DataMember(Name = "timelogs")]
        public new List<BWA.Api.Models.Ticket_Time_Log> TimeLogs { get; set; }
    }
}
