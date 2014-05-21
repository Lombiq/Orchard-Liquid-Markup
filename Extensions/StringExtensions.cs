using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class StringExtensions
    {
        public static string TrimParameter(this string input)
        {
            return input.Trim().Trim('"', '\'');
        }
    }
}