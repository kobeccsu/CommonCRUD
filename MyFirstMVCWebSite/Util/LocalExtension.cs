using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstMVCWebSite.Util
{
    public static class LocalExtension
    {
        public static int GetMassWord(this string ttt, int temp)
        {
            return temp + 1;
        }
    }
}