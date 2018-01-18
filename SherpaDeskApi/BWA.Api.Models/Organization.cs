// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface;

namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for Ticket
   /// </summary>
    [DataContract(Name = "Organization")]
    public class Organization
    {
        readonly Micajah.Common.Bll.Organization _innerOrganization;

        public Organization(Micajah.Common.Bll.Organization org, Micajah.Common.Bll.InstanceCollection insts)
        {
            _innerOrganization = org;
            var i = from element in insts
                    orderby element.Active
                    select element;
            Instances=new List<Instance>(insts.Count);
            foreach (Micajah.Common.Bll.Instance inst in i)
            {
                Instances.Add(new Instance(inst));
            }
        }

       //[DataMember]
        public Guid Id
        {
            get { return _innerOrganization.OrganizationId; }
            set { _innerOrganization.OrganizationId = value; }
        }

        [DataMember(Name = "key")]
        public string Key
        {
            get { return _innerOrganization.PseudoId; }
            set { _innerOrganization.PseudoId = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return _innerOrganization.Name; }
            set { _innerOrganization.Name = value; }
        }

        [DataMember(Name = "is_expired")]
        public bool is_expired
        {
            get { return _innerOrganization.Expired && _innerOrganization.GraceDaysRemaining <= 0; }
            set { var t = value; }
        }


        [DataMember(Name = "is_trial")]
        public bool is_trial
        {
            get { return _innerOrganization.Trial; }
            set { var t = value; }
        }

        [DataMember(Name = "instances")]
        public List<Instance> Instances { get; set; }
    }
}
