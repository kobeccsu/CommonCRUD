using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstMVCWebSite.Util
{
    public class FieldList
    {
       public string FieldName { get; set; }
       public Type FieldType { get; set; }

       public bool FieldPK { get; set; }
    }
}
