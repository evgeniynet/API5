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
   /// Summary description for Class
   /// </summary>
    [DataContract(Name = "File")]
    public class File
    {
        protected Micajah.AzureFileService.File _file;
        public File(Micajah.AzureFileService.File file) { _file = file; }

        [DataMember(Name = "id")]
        public string FileId
        {
            get { return _file.FileId; }
            set { _file.FileId = value; }
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get { return _file.Name; }
            set { _file.Name = value; }
        }
        
        [DataMember(Name = "url")]
        public string Url
        {
            get { return _file.Url; }
            set { _file.Url = value; }
        }

        
        [DataMember(Name = "date")]
        public DateTime UpdatedTime
        {
            get
            {
                return _file.LastModified;
            }
            set
            {
                 _file.LastModified = value;
            }
        }
        
        [DataMember(Name = "size")]
        public long SizeInBytes
        {
            get { return _file.Length; }
            set { _file.Length = value; }
        }

        public string ContentType
        {
            get { return _file.ContentType; }
            set { _file.ContentType = value; }
        }

    }
}
