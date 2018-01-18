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
   /// Summary description for Ticket
   /// </summary>
    [DataContract(Name = "Instance")]
    public class Instance
    {
        protected Micajah.Common.Bll.Instance InnerInstance;

        public Instance(Micajah.Common.Bll.Instance inst)
        {
            InnerInstance = inst;
        }

       //[DataMember]
        public Guid Id
        {
            get { return InnerInstance.InstanceId; }
            set { InnerInstance.InstanceId = value; }
        }

        [DataMember(Name = "key")]
        public string Key
        {
            get { return InnerInstance.PseudoId; }
            set { InnerInstance.PseudoId = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return InnerInstance.Name; }
            set { InnerInstance.Name = value; }
        }

        [DataMember(Name = "is_expired")]
        public bool is_expired
        {
            get { return !InnerInstance.Active || (InnerInstance.CanceledTime.HasValue && InnerInstance.CanceledTime > DateTime.MinValue); }
            set { var t = value; }
        }

        [DataMember(Name = "is_trial")]
        public bool is_trial
        {
            get { return InnerInstance.Trial; }
            set { var t = value; }
        }
    }
}
