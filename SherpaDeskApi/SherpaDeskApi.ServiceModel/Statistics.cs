// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ServiceStack;

namespace SherpaDeskApi.ServiceModel
{

	[Route ("/stat")]
	public class Statistics : ApiRequest
	{
		private DateTime? _start_date;
		private DateTime? _end_date;

		public DateTime? start_date
		{
			get
			{
				if (_start_date.HasValue && _start_date.Value.TimeOfDay.TotalSeconds > 0)
					return _start_date.Value.Date;
				return DateTime.UtcNow.Date.AddDays(-30);
			}
			set { _start_date = value; }
		}

		public DateTime? end_date         
		{
			get
			{
				if (_end_date.HasValue && _end_date.Value.TimeOfDay.TotalSeconds == 0)
					return _end_date.Value.Date.AddDays(1).AddSeconds(-1);
				return DateTime.UtcNow;
			}
			set { _end_date = value; }
		}
	}

	public class StatResponse
	{

		public string AvgOpenedData { get; set; }

		public string AvgClosedData { get; set; }

		public string MyOpenedData { get; set; }

		public string MyClosedData { get; set; }
	}
}
