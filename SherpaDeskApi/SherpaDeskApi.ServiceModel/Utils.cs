// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Web;
using System.Text.RegularExpressions;

namespace SherpaDeskApi.ServiceModel
{
    public class Utils
    {
        public static string ClearString(string inputString)
        {
            return inputString.Replace("\n", " ").Replace("\r", " ");
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+(\.[^@\s]+)+$",
                    RegexOptions.IgnoreCase);
        }
        
        public static bool IsValidPassword(string pass)
        {
			if (string.IsNullOrWhiteSpace(pass) || pass.Length < 5)
                return false;
			return true;
        }

		static bool IsDigitsOnly(string str)
		{
			foreach (char c in str)
			{
				if (c < '0' || c > '9')
					return false;
			}

			return true;
		}

        public static string FormatTicketNote(string note)
        {
            
            return HttpUtility.HtmlDecode(note).Replace("<br>", "\r\n");
        }

        public static int GetDaysOldInMinutes(Guid OrganizationId, int DepartmentId, bool IsUseWorkDaysTimer, string status, DateTime CreateTime, DateTime ClosedTime)
        {
            DateTime closedtime = status == "Closed" ? ClosedTime : DateTime.UtcNow;
            if (IsUseWorkDaysTimer)
                return bigWebApps.bigWebDesk.Data.Tickets.SelectTicketSLATime(OrganizationId, DepartmentId, CreateTime, closedtime);
            return Convert.ToInt32((closedtime - CreateTime).TotalMinutes);
        }

        public static string NormalizeString(string inStr)
        {
            string res = Regex.Replace(inStr, "<br>|<BR>|<br/>|<BR/>|<br />|<BR />|&lt;br&gt;|&lt;BR&gt;|&lt;br/&gt;|&lt;BR/&gt;|&lt;br /&gt;|&lt;BR /&gt;", Environment.NewLine);
            res = Regex.Replace(res.Replace("<", "&lt;").Replace(">", "&gt;").Replace(Environment.NewLine, "<br/>").Replace("\r\n", "<br/>").Replace("\n", "<br/>"),
                                    @"((ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\=\?\,\'\/\\\+&amp;%\$#_]*)?([a-zA-Z0-9]))", "<a href=\"$1\" target=\"_blank\">$1</a>");
            return res;
        }
    }
}