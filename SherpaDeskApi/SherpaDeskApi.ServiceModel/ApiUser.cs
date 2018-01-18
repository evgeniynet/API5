// This is an independent project of an individual developer. Dear PVS-Studio, please check it.// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using bigWebApps.bigWebDesk;
using bigWebApps.bigWebDesk.Data;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using ServiceStack;
using ServiceStack.Common;
using ServiceStack.Host;
using ServiceStack.Web;
using System;
using System.Data;
using System.Net;

namespace SherpaDeskApi.ServiceModel
{
    static class DataRowExtensions
    {
        public static T Get<T>(this DataRow row, string column, T defaultValue = default(T))
        {
            if (row == null)
                return default(T);
            return row.Table.Columns.Contains(column) ? (!row.IsNull(column) ? row.Field<T>(column) : defaultValue) : default(T);
        }

        public static string GetString(this DataRow row, string column, string defaultValue = null)
        {
            if (row == null)
                return string.Empty;
            return row.Table.Columns.Contains(column) ? (!row.IsNull(column) ? row[column].ToString() : defaultValue) : null;
        }

        public static void Set<T>(this DataRow row, string column, T value)
        {
            if (row == null)
                return;
            if (row.Table.Columns.Contains(column) && row.Table.Columns[column].DataType == typeof(T))
            {
                row.SetField<T>(column, value);
            }
        }
    }

   /// <summary>
   /// Summary description for ApiUser
   /// </summary>
    public class ApiUser
    {
        protected Guid m_LoginId;
        protected Guid m_OrganizationId;
        protected string m_OrganizationLogoImageUrl;
        protected Guid m_InstanceId;
        protected string m_InstanceLogoImageUrl;
        protected int m_DepartmentId;
        protected string m_DepartmentName;
        protected int m_UserId;
        protected int m_AccountId;
        protected string m_AccountName;
        protected string m_LoginEmail;
        protected bool m_IsTechAdmin;
        protected bool m_IsUseWorkDaysTimer;
        protected string m_FirstName;
        protected string m_LastName;
        protected string m_Skype;
        protected string[] m_Roles;
        protected int m_TimeZoneOffset;
        protected int m_TimeFormat = 0;
        protected int m_DateFormat = 0;
        protected string m_TimeZoneId;
        const int DefaultTimeZoneOffset = -5;
        protected byte m_tintTicketTimer;
        protected bigWebApps.bigWebDesk.UserAuth.UserRole m_Role;
        protected bool m_IsAdmin;

        public ApiUser(string apiKey)
        {
            m_LoginId = Guid.Empty;
            m_OrganizationId = Guid.Empty;
            m_OrganizationLogoImageUrl = "";
            m_InstanceId = Guid.Empty;
            m_InstanceLogoImageUrl = "";
            m_DepartmentId = 0;
            m_DepartmentName = "";
            m_UserId = 0;
            m_LoginEmail = string.Empty;
            m_IsTechAdmin = false;
            m_IsAdmin = false;
            m_IsUseWorkDaysTimer = false;
            m_Roles = new string[1];
            Micajah.Common.Bll.Providers.LoginProvider lp = new Micajah.Common.Bll.Providers.LoginProvider();
            DataRowView userRow = lp.GetLoginByToken(apiKey);
            if (userRow == null) throw new HttpError(HttpStatusCode.NotFound, "User with this token was not found.");
            m_LoginId = (Guid)userRow["LoginId"];
            OrganizationCollection _orgs = lp.GetOrganizationsByLoginId(m_LoginId);
            if (_orgs.Count == 0) throw new HttpError(HttpStatusCode.NotFound, "No assigned organizations found for this user.");
            m_LoginEmail = userRow["LoginName"].ToString();
           //m_LoginId = userRow.UserId;
            m_FirstName = userRow["FirstName"].ToString();
            m_LastName = userRow["LastName"].ToString();
        }

        private void CompleteInitObject()
        {
            var CurrOrganization = Micajah.Common.Bll.Providers.OrganizationProvider.GetOrganization(m_OrganizationId);
            byte _graceDays = (byte)CurrOrganization.GraceDays;
            DateTime _expire = DateTime.UtcNow.AddMonths(1);
            if (CurrOrganization.ExpirationTime != null)
            {
                _expire = (DateTime)CurrOrganization.ExpirationTime;
            }
            DateTime _now = DateTime.UtcNow;
            _expire = _expire.AddDays(_graceDays);
            if (_expire < _now)
            {
                throw new HttpError(HttpStatusCode.Forbidden, "Your organization's account has expired or inactivated. Contact SherpaDesk for assistance. Email: support@sherpadesk.com Phone: +1 (866) 996-1200, then press 2");
            }

            DataRow _row = bigWebApps.bigWebDesk.Data.Companies.SelectOne(m_OrganizationId, m_InstanceId);
            if (_row == null) throw new HttpError(HttpStatusCode.NotFound, "Can't find department for OrganizationId/InstanceId=" + OrganizationId.ToString() + "/" + InstanceId.ToString());
            m_DepartmentId = (int)_row["company_id"];
            m_DepartmentName = _row["company_name"].ToString();
            int _userStatus = Logins.SelectLoginExists(m_OrganizationId, m_DepartmentId, m_LoginEmail, out m_UserId, out m_AccountId);
            if (_userStatus == 1) throw new HttpError(HttpStatusCode.NotFound, "Can't find User \"" + m_LoginEmail + "\" in the bigWebApps database.");
            else if (_userStatus == 2) throw new HttpError(HttpStatusCode.Forbidden, "User \"" + m_LoginEmail + "\" is not associated with OrganizationId/InstanceId=" + OrganizationId.ToString() + "/" + InstanceId.ToString() + ".");
            _row = Logins.SelectUserDetails(m_OrganizationId, m_DepartmentId, m_UserId);
            if (_row == null) throw new HttpError(HttpStatusCode.Forbidden, "The user account is not associated with this Department.");
            if ((bool)_row["btUserInactive"]) throw new HttpError(HttpStatusCode.Forbidden, "The user account is Inactive in this Department.");
            m_Skype = _row["Skype"].ToString();
            m_AccountName = ((string)_row["AccountName"]);
            if (string.IsNullOrWhiteSpace(m_AccountName))
                m_AccountName = m_DepartmentName;
            Micajah.Common.Dal.ClientDataSet.UserRow _uRow = UserProvider.GetUserRow(m_LoginEmail, m_OrganizationId);
            string TimeZoneId = null;

            if (_uRow != null)
            {
                m_TimeFormat = _uRow.IsTimeFormatNull() ? 0 : _uRow.TimeFormat;
                m_DateFormat = _uRow.IsTimeFormatNull() ? 0 : _uRow.DateFormat;

                if (!_uRow.IsTimeFormatNull())
                    TimeZoneId = _uRow.TimeZoneId;
                UpdateLastLoginDate(m_OrganizationId, _uRow.UserId);
            }
            if (string.IsNullOrEmpty(TimeZoneId) && !_row.IsNull("InstanceTimeZoneId"))
                TimeZoneId = _row["InstanceTimeZoneId"].ToString();
                
            try
            {
                m_TimeZoneOffset = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId).GetUtcOffset(new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero)).Hours;
                m_TimeZoneId = TimeZoneId;
            }
            catch
            {
                m_TimeZoneOffset = DefaultTimeZoneOffset;
            }

            if (!_row.IsNull("tintTicketTimer")) m_tintTicketTimer = (byte)_row["tintTicketTimer"];
            else m_tintTicketTimer = (byte)_row["tintDTicketTimer"];

            bool isOrgAdmin = _row.Get<bool?>("OrganizationAdministrator") ?? false;

            m_IsUseWorkDaysTimer = tintTicketTimer > 0;

            int userType = (int)_row ["UserType_Id"];
            if (userType == 2 || userType == 3  || userType == 6 || isOrgAdmin) m_IsTechAdmin = true;
            if (userType == 3 || isOrgAdmin) m_IsAdmin = true;
            m_Roles[0] = userType == 3 ? "admin" : "";
            m_Roles[0] = userType == 5 ? "super" : m_Roles[0];
			m_Role = (bigWebApps.bigWebDesk.UserAuth.UserRole) Enum.Parse(typeof (bigWebApps.bigWebDesk.UserAuth.UserRole), userType.ToString());

            m_OrganizationLogoImageUrl = BWA.Api.Models.Files.GetOrganizationLogoUrl(OrganizationId);
            m_InstanceLogoImageUrl = BWA.Api.Models.Files.GetInstanceLogoUrl(m_InstanceId);

            bigWebApps.bigWebDesk.Data.Logins.UpdateUserLastLoginDate(m_OrganizationId, m_UserId);
        }

        private void UpdateLastLoginDate(Guid OrgId, Guid UserId)
        {
            bigWebApps.bigWebDesk.Data.Logins.UpdateUserLastLoginDate(OrgId, UserId);
        }

        public static bool ValidateStatic(string userName, string password)
        {
            var _lp = new LoginProvider();
            return _lp.ValidateLogin(userName, password);
        }

        public static ApiUser getUser(IHttpRequest request)
        {
            var basicAuth = request.GetBasicAuthUserAndPassword();
            string key = basicAuth.Value.Key;
            string api_token = basicAuth.Value.Value;
            if (key.IndexOf('-') < 6)
                throw new HttpError(HttpStatusCode.Forbidden, "Org/Instance is not correct.");
            string[] split = key.Split('-');
            string org_key = split[0];
            string instance_key = split[1];
            return getUser(api_token, org_key, instance_key);
        }

        public static ApiUser getUser(string api_token, string org_key, string instance_key)
        {
            if (api_token.Length != 32)
                throw new HttpError(HttpStatusCode.Forbidden, "Token is not correct.");
            ApiUser apiUser = new ApiUser(api_token);
            if (!apiUser.ValidateAccess(org_key, instance_key))
                throw new HttpError(HttpStatusCode.Forbidden, "User is Inactive or does not have access to this Organization/Instance");
            return apiUser;
        }

        public static bool IsExists(string userName, Guid orgId)
        {
            Micajah.Common.Dal.ClientDataSet.UserRow _uRow = UserProvider.GetUserRow(userName, orgId);
            if (_uRow == null) return false;
            return true;
        }

        internal static object GetUser(IHttpRequest httpReq)
        {
            throw new NotImplementedException();
        }

        public bool ValidateAccess(string orgKey, string instKey)
        {
            if (string.IsNullOrEmpty(orgKey)) throw new HttpError(HttpStatusCode.InternalServerError, new ArgumentException("OrgKey parameter can't be null or empty"));

            if (!string.IsNullOrEmpty(instKey))
            {
                Micajah.Common.Bll.Organization _org = Micajah.Common.Bll.Providers.OrganizationProvider.GetOrganizationByPseudoId(orgKey);
                if (_org == null) throw new HttpError(HttpStatusCode.NotFound, "Can't find organization with OrganizationKey=" + orgKey);

                Micajah.Common.Bll.Instance _inst = Micajah.Common.Bll.Providers.InstanceProvider.GetInstanceByPseudoId(instKey, _org.OrganizationId);
                if (_inst == null) throw new HttpError(HttpStatusCode.NotFound, "Can't find instance with InstanceKey=" + instKey);

                if (UserProvider.UserIsActiveInInstance(LoginId, _inst.InstanceId, _org.OrganizationId))
                {
                    m_OrganizationId = _org.OrganizationId;
                    m_InstanceId = _inst.InstanceId;
                    CompleteInitObject();
                    return true;
                }
            }
            else
            {
                var _lp = new LoginProvider();
                Micajah.Common.Bll.OrganizationCollection _orgsMc = _lp.GetOrganizationsByLoginId(LoginId);
                foreach (Organization _org in _orgsMc)
                {
                    if (_org.PseudoId == orgKey)
                    {
                        m_OrganizationId = _org.OrganizationId;
                        return true;
                    }
                }
            }
            return false;
        }

        public byte tintTicketTimer
        {
            get { return m_tintTicketTimer; }
        }

        public bigWebApps.bigWebDesk.UserAuth.UserRole Role
        {
            get { return m_Role; }
        }

        public Guid LoginId
        {
            get { return m_LoginId; }
        }

        public string LoginEmail
        {
            get { return m_LoginEmail; }
        }

        public Guid OrganizationId
        {
            get { return m_OrganizationId; }
        }

        public Guid InstanceId
        {
            get { return m_InstanceId; }
        }

        public int DepartmentId
        {
            get { return m_DepartmentId; }
        }

        public string DepartmentName
        {
            get { return m_DepartmentName; }
        }

        public string OrganizationLogoImageUrl
        {
            get { return m_OrganizationLogoImageUrl; }
        }

        public string InstanceLogoImageUrl
        {
            get { return m_InstanceLogoImageUrl; }
        }
        public int UserId
        {
            get { return m_UserId; }
        }
        public int AccountId
        {
            get { return m_AccountId; }
        }
        public string AccountName
        {
            get { return m_AccountName; }
        }
        public bool IsTechAdmin
        {
            get { return m_IsTechAdmin; }
        }

        public bool IsAdmin
        {
            get { return m_IsAdmin; }
        }

        public bool IsUseWorkDaysTimer
        {
            get { return m_IsUseWorkDaysTimer; }
        }

        public int TimeZoneOffset
        {
            get { return m_TimeZoneOffset; }
        }

        public int TimeFormat
        {
            get { return m_TimeFormat; }
        }

        public int DateFormat
        {
            get { return m_DateFormat; }
        }

        public string TimeZoneId
        {
            get { return m_TimeZoneId; }
        }

        public string FirstName
        {
            get { return m_FirstName; }
        }

        public string LastName
        {
            get { return m_LastName; }
        }

        public string Skype
        {
            get { return m_Skype; }
        }

        public string FullName
        {
            get { return m_FirstName + " " + m_LastName; }
        }

        public string[] Roles
        {
            get { return m_Roles; }
        }
    }
}
