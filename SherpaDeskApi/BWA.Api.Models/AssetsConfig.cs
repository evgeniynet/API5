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
    [DataContract(Name = "Assets_Config")]
    public class AssetsConfig
    {
        public AssetsConfig(bigWebApps.bigWebDesk.AssetsConfig assetsCfg)
        {
            Captions = new string[7];
            Captions[0] = Unique1Caption = assetsCfg.Unique1Caption;
            Captions[1] = Unique2Caption = assetsCfg.Unique2Caption;
            Captions[2] = Unique3Caption = assetsCfg.Unique3Caption;
            Captions[3] = Unique4Caption = assetsCfg.Unique4Caption;
            Captions[4] = Unique5Caption = assetsCfg.Unique5Caption;
            Captions[5] = Unique6Caption = assetsCfg.Unique6Caption;
            Captions[6] = Unique7Caption = assetsCfg.Unique7Caption;
        }

        public string[] Captions
        { get; set; }

        [DataMember(Name = "unique1_caption")]
        public string Unique1Caption
        { get; set; }

        [DataMember(Name = "unique2_caption")]
        public string Unique2Caption
        { get; set; }

        [DataMember(Name = "unique3_caption")]
        public string Unique3Caption
        { get; set; }

        [DataMember(Name = "unique4_caption")]
        public string Unique4Caption
        { get; set; }

        [DataMember(Name = "unique5_caption")]
        public string Unique5Caption
        { get; set; }

        [DataMember(Name = "unique6_caption")]
        public string Unique6Caption
        { get; set; }

        [DataMember(Name = "unique7_caption")]
        public string Unique7Caption
        { get; set; }
    }
}
