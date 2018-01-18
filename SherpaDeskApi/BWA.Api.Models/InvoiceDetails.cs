// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for TicketLogRecords
   /// </summary>
    [DataContract(Name = "InvoiceDetails")]
    public class InvoiceDetails : ModelItemCollectionGeneric<InvoiceDetail>
    {
        public InvoiceDetails(DataTable InvoiceDetailsTable) : base(InvoiceDetailsTable) { }
    }
}
