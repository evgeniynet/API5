// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using ServiceStack;
using SherpaDeskApi.ServiceModel;
using System;
using System.Net;
using System.Data;
using System.Text;

namespace SherpaDeskApi.ServiceInterface
{
    public class StatisticsService : Service
	{
		[Secure ("tech")]
		public object Get (Statistics request)
		{
			ApiUser hdUser = request.ApiUser;
			DateTime StartDate = request.start_date.Value;
			DateTime EndDate = request.end_date.Value;
			return new StatResponse{
				AvgOpenedData = AvgOpenedData(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, StartDate, EndDate),
				AvgClosedData = AvgClosedData(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, StartDate, EndDate),
				MyOpenedData = MyOpenedData(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, StartDate, EndDate),
				MyClosedData = MyClosedData(hdUser.OrganizationId, hdUser.DepartmentId, hdUser.UserId, StartDate, EndDate)
			};
		}

		private string AvgOpenedData (Guid OrgId, int DeptId, int UserId, DateTime StartDate, DateTime EndDate)
		{
			DataTable dt = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketsCountForChart (OrgId, DeptId, StartDate, EndDate, UserId);
			return ConvertDataTabelToString (dt, EndDate);
		}

		private string MyOpenedData (Guid OrgId, int DeptId, int UserId, DateTime StartDate, DateTime EndDate)
		{
			DataTable dt = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketsCountForChart (OrgId, DeptId, StartDate, EndDate, UserId);
			return ConvertDataTabelToString (dt, EndDate);
		}

		private string AvgClosedData (Guid OrgId, int DeptId, int UserId, DateTime StartDate, DateTime EndDate)
		{
			DataTable dt = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketsCountForChart (OrgId, DeptId, StartDate, EndDate, UserId);
			return ConvertDataTabelToString (dt, EndDate);
		}

		string MyClosedData (Guid OrgId, int DeptId, int UserId, DateTime StartDate, DateTime EndDate)
		{
			DataTable dt = bigWebApps.bigWebDesk.Data.Tickets.SelectTicketsCountForChart (OrgId, DeptId, StartDate, EndDate, UserId);
			return ConvertDataTabelToString (dt, EndDate);
		}

		public string ConvertDataTabelToString (DataTable dt, DateTime EndDate)
		{
			StringBuilder sb = new StringBuilder ();
			if (dt != null) {
				foreach (DataRow row in dt.Rows) {
					sb.AppendFormat ("{2} [({0}), {1}]", GetJavascriptTimestamp (row ["Date"] as DateTime?, EndDate), row ["Count"], sb.Length > 0 ? "," : string.Empty);
				}
			}
			return sb.ToString ();
		}

		public long GetJavascriptTimestamp (System.DateTime? input, DateTime EndDate)
		{
			if (!input.HasValue) {
				input = EndDate;
			}
			System.TimeSpan span = new System.TimeSpan (System.DateTime.Parse ("1/1/1970").Ticks);
			System.DateTime time = input.Value.Subtract (span);
			return (long)(time.Ticks / 10000);
		}
	}
}
