using DressUp_Scl_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DressUp.Scl.Model
{
    public class JurisdictionTree
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        //[JsonProperty(PropertyName = "CName")]
        public bool @checked { get; set; }

        public bool open = true;
    }
}