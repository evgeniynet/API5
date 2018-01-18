// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using System.Data;
using System.Linq.Dynamic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;
using ServiceStack.Web;
using ServiceStack.Host;




namespace SherpaDeskApi.ServiceModel
{

   

    public class PagedApiRequest : ApiRequest
    {
        private int _limit = 25;
        private string _sort_order = "desc";
        private DateTime? _start_date;
        private DateTime? _end_date;

        public string exclude_fields { get; set; }
        public string query { get; set; }
        public int limit
        {
            get { if (_limit <= 0) _limit = 25; return _limit; }
            set { _limit = value; }
        }
        public int page { get; set; }
        public string sort_order
        {
            get { if (string.IsNullOrWhiteSpace(_sort_order)) _sort_order = "desc"; return _sort_order; }
            set
            {
                if (string.IsNullOrWhiteSpace(_sort_order)) _sort_order = "desc";
                else
                    _sort_order = value;
            }
        }

        public string sort_by { get; set; }

        public DateTime? start_date
        {
            get
            {
                if (_start_date.HasValue && _start_date.Value.TimeOfDay.TotalSeconds > 0)
                    return _start_date.Value.Date;
                return _start_date;
            }
            set { _start_date = value; }
        }

        public DateTime? end_date
        {
            get
            {
                if (_end_date.HasValue && _end_date.Value.TimeOfDay.TotalSeconds == 0)
                    return _end_date.Value.Date.AddDays(1).AddSeconds(-1);
                return _end_date;
            }
            set { _end_date = value; }
        }

        public List<T> FilteredResult<T>(List<T> list)
        {
            if (list == null || list.Count <= 1)
                return list;
            if (!start_date.HasValue && !end_date.HasValue && string.IsNullOrEmpty(sort_by) && string.IsNullOrEmpty(query))
                return list.Skip(page * limit).Take(limit).ToList<T>();
            var temp = list.AsQueryable();
            string dateName = null;
            if (start_date.HasValue)
            {
                dateName = GetPropertyFrom(list[0], "date") ?? "createtime";//for tickets
                if (!string.IsNullOrEmpty(dateName))
                    temp = temp.Where(string.Format("{0} >= @0", dateName), start_date);
            }
            if (end_date.HasValue)
            {
                dateName = dateName ?? (GetPropertyFrom(list[0], "date") ?? "createtime");//for tickets
                if (!string.IsNullOrEmpty(dateName))
                    temp = temp.Where(string.Format("{0} <= @0", dateName), end_date);
            }
            if (!string.IsNullOrEmpty(query))
            {
                var test = Regex.Matches(query, @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}Z");
                if (test.Count > 0)
                {
                    int i = 0;
                    object[] dates = new object[test.Count];

                    foreach (Match m in test)
                    {
                        dates[i] = DateTime.Parse(m.Value);
                        query = query.Replace(m.Value, "@" + i++);
                    }
                    temp = temp.Where(query, dates);
                }
                else
                    temp = temp.Where(query);
            }

            if (!string.IsNullOrEmpty(sort_by))
            {
               //TODO: rename properties to datamember name
                string property_sort_by = GetPropertyFrom(list[0], sort_by);
                try
                {
                    property_sort_by = property_sort_by ?? sort_by;
                    temp = temp.OrderBy(property_sort_by + " " + sort_order);
                }
                catch { }
            }
            return temp.Cast<T>().Skip(page * limit).Take(limit).ToList();
        }

        public List<T> SortedResult<T>(List<T> list)
        {
            if (list == null || list.Count <= 1)
                return list;
            if (string.IsNullOrEmpty(sort_by))
                return list.Skip(page * limit).Take(limit).ToList<T>();
            var temp = list.AsQueryable();

           //TODO: rename properties to datamember name

                string property_sort_by = GetPropertyFrom(list[0], sort_by);
                try
                {
                    property_sort_by = property_sort_by ?? sort_by;
                    temp = temp.OrderBy(property_sort_by + " " + sort_order);
                }
                catch { }

            return temp.Cast<T>().Skip(page * limit).Take(limit).ToList();
        }

        public List<T> QueryResult<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return list;

            if (!string.IsNullOrEmpty(exclude_fields))
            {
                var excludes = exclude_fields.Split(',');
                foreach (var ex in excludes)
                {
                    System.Reflection.PropertyInfo propertyInfo = propertyInfo = GetPropertyFromName(list[0], ex);//list[0].GetType().GetProperty(ex);
                    

                    if (propertyInfo != null)
                        list.ForEach(i => propertyInfo.SetValue(i, null));
                }
            }

            if (list.Count <= 1 || (string.IsNullOrEmpty(query) && string.IsNullOrEmpty(sort_by)))
                return list;
            var temp = list.AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                temp = temp.Where(query);
            }
            if (!string.IsNullOrEmpty(sort_by))
            {
                string property_sort_by = GetPropertyFrom(list[0], sort_by);
                try
                {
                    property_sort_by = property_sort_by ?? sort_by;
                    temp = temp.OrderBy(property_sort_by + " " + sort_order.ToUpper());
                }
                catch { }
            }
            return temp.Cast<T>().ToList();
        }

        private static string GetPropertyFrom(object item, string name)
        {
            PropertyInfo property = GetPropertyFromName(item, name);
            if (property != null)
            {
                return property.Name;
            }
            return null;
        }

        private static PropertyInfo GetPropertyFromName(object item, string name)
        {
            if (item == null || string.IsNullOrEmpty(name))
                return null;
            var properties = item.GetType().GetProperties();
           // This replaces all the iteration above:
            return properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(DataMemberAttribute), false)
                                   .Any(a => ((DataMemberAttribute)a).Name != null && ((DataMemberAttribute)a).Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));

        }

    }

   /// <summary>
   /// Summary description for ApiRequest
   /// </summary>
    public class ApiRequest
    {
        public ApiRequest()
        {
        }

        protected internal ApiUser ApiUser { get; set; }
        protected internal string api_token { get; set; }
    }

    public class SecureAttribute : RequestFilterAttribute
    {
        public SecureAttribute()
        {
            ClientType = "";
        }

        public SecureAttribute(string clientType)
        {
            ClientType = clientType;
        }

        public string ClientType { get; set; }

        public override void Execute(IHttpRequest httpReq, IHttpResponse httpResp, object request)
        {
            var basicAuth = httpReq.GetBasicAuthUserAndPassword();
            ApiRequest apiRequest = request as ApiRequest;
            if (apiRequest == null)
            {
               //Custom Auth needed
                return;
            }

            string api_token = "";

            if (basicAuth == null)
            {
                api_token = httpReq.QueryString["api_token"];

                if (string.IsNullOrEmpty(api_token))
                {
                    httpResp.AddHeader(HttpHeaders.WwwAuthenticate, "Basic realm=\"/login\"");
                    throw new HttpError(HttpStatusCode.Unauthorized, "Invalid BasicAuth credentials");
                }
                else if (api_token.Length != 32)
                    throw new HttpError(HttpStatusCode.Forbidden, "Token is not correct.");
                var apiUser = new ApiUser(api_token);
                if ((ClientType == "super" && !apiUser.Roles.Equals("super") && !apiUser.IsTechAdmin) || (ClientType == "tech" && !apiUser.IsTechAdmin))
                    throw new HttpError(HttpStatusCode.Forbidden, "Access Denied for user.");
                else
                    apiRequest.ApiUser = apiUser;
            }
            else
            {
                string key = basicAuth.Value.Key;
                string password = basicAuth.Value.Value;
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(password))
                    throw new HttpError(HttpStatusCode.Forbidden, "Token is not correct.");
                if (key == "x")
                {
                    if (password.Length != 32)
                        throw new HttpError(HttpStatusCode.Forbidden, "Token is not correct.");
                    apiRequest.api_token = password;
                }
                else
                {
                    var apiUser = ApiUser.getUser(httpReq);
                    if (ClientType == "tech" && !apiUser.IsTechAdmin)
                        throw new HttpError(HttpStatusCode.Forbidden, "Access Denied for user.");
                    else
                        apiRequest.ApiUser = apiUser;
                }
            }
        }

        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
