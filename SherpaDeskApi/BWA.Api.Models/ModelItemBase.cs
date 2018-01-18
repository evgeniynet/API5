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
   /// Summary description for ModelItemBase
   /// </summary>
    [DataContract(Name = "ModelItemBase")]
    public class ModelItemBase : ModelItemBaseInterface
    {
        protected DataRow m_Row;
        protected string IdFieldName;

        public ModelItemBase(DataRow row)
        {
            m_Row = row;
            IdFieldName = "Id";
        }

        #region ModelItemBaseInterface Members

       //[DataMember(Name = "id")]
        public int Id
        {
            get { return (!Row.Table.Columns.Contains(IdFieldName) ||  Row.IsNull(IdFieldName)) ? 0 : (int)Row[IdFieldName]; }
            set { if (Row.Table.Columns.Contains(IdFieldName)) Row[IdFieldName] = value; }
        }

        public DataRow Row
        {
            get { return m_Row; }
        }

        #endregion
    }
}
