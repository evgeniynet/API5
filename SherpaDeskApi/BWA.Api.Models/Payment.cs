// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;
using System.Net;
using bigWebApps.bigWebDesk.Data;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "Payment")]
    public class Payment : ModelItemBase
    {
        public Payment(DataRow row)
            : base(row)
        { 
        if (row == null)
            throw new ServiceStack.Common.Web.HttpError(System.Net.HttpStatusCode.OK, "No results");
        }

        [DataMember(Name = "id")]
        public int Id
        {
            get { return Convert.ToInt32(Row["Id"]); }
            set { Row["Id"] = value; }
        }

        [DataMember(Name = "date")]
        public DateTime? Date
        {
            get
            {
                return Row.Get<DateTime?>("Date");
            }
            set
            {
                if (Row.Table.Columns.Contains("Date")) Row["Date"] = value;
            }
        }

        [DataMember(Name = "start_date")]
        public DateTime? BeginDate
        {
            get
            {
                return Row.Get<DateTime?>("BeginDate");
            }
            set
            {
                Row["BeginDate"] = value;
            }
        }

        [DataMember(Name = "end_date")]
        public DateTime? EndDate
        {
            get
            {
                return Row.Get<DateTime?>("EndDate");
            }
            set
            {
                Row["EndDate"] = value;
            }
        }

        [DataMember(Name = "user_id")]
        public int UserID
        {
            get { return Convert.ToInt32(Row["TechID"]); }
            set { Row["TechID"] = value; }
        }

        [DataMember(Name = "user_name")]
        public string UserName
        {
            get { return Row["FullName"].ToString(); }
            set { Row["FullName"] = value; }
        }

        [DataMember(Name = "total_hours")]
        public decimal? TotalHours
        {
            get { return Row.Get<decimal?>("TotalHours"); }
            set { Row["TotalHours"] = value; }
        }

        [DataMember(Name = "amount")]
        public decimal? Amount
        {
            get { return Row.Get<decimal?>("Amount"); }
            set { Row["Amount"] = value; }
        }

        [DataMember(Name = "qb_bill_id")]
        public int QBBillID
        {
            get
            {
                return Row.Get<int>("QBBillID");
            }
            set { Row["QBBillID"] = value; }
        }

        [DataMember(Name = "qb_employee_id")]
        public int QBEmployeeID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBEmployeeID") && !Row.IsNull("QBEmployeeID"))
                {
                    return Convert.ToInt32(Row["QBEmployeeID"]);
                }
                return 0;
            }
            set { Row["QBEmployeeID"] = value; }
        }

        [DataMember(Name = "qb_vendor_id")]
        public int QBVendorID
        {
            get
            {
                if (Row.Table.Columns.Contains("QBVendorID") && !Row.IsNull("QBVendorID"))
                {
                    return Convert.ToInt32(Row["QBVendorID"]);
                }
                return 0;
            }
            set { Row["QBVendorID"] = value; }
        }

        [DataMember(Name = "xero_bill_id")]
        public string XeroBillID
        {
            get { return Row.GetString("XeroBillID"); }
            set { Row["XeroBillID"] = value; }
        }

        [DataMember(Name = "xero_contact_id")]
        public string Xero_Contact_Id
        {
            get { return Row.GetString("XeroContactId"); }
            set { Row["XeroContactId"] = value; }
        }

        internal static Models.Payment GetPayment(Guid organizationId, int departmentId, int timeBillId)
        {
            if (timeBillId > 0)
            {
                DataRow iRow = bigWebApps.bigWebDesk.Data.TimeBill.SelectTimeBill(organizationId, departmentId, timeBillId);
                if (iRow != null)
                {
                    return new Payment(iRow);
                }
            }
            throw new HttpError(HttpStatusCode.NotFound, "Payment not found");
        }
    }
}
