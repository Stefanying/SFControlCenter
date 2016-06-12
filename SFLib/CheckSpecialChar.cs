using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SFLib
{
    public class CheckSpecialChar
    {
        public static bool CheckParams(string arg)
        {
            string[] Lawlesses = { "=", "'", "?", "%", "@", "<", ">", "(", ")", "~", "$" };
            if (Lawlesses == null || Lawlesses.Length <= 0) return true;
            //构造正则表达式
            string str_Regex = ".*[";
            for (int i = 0; i < Lawlesses.Length - 1; i++)
                str_Regex += Lawlesses[i] + "|";
            str_Regex += Lawlesses[Lawlesses.Length - 1] + "].*";

            if (Regex.Matches(arg.ToString(), str_Regex).Count > 0)
                return false;
            
            return true;
        }
    }
}
