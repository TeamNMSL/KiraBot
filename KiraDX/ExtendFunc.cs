using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX
{

    static class ExtendFunc
    {
        static public string format(this string str) => (str.Trim().ToLower());
        /// <summary>
        /// 字符反转
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string rvs(this string str) {
            string _ = str;
            string __ = "";
            for (int i = _.Length - 1; i >= 0; i--)
                __ += _[i];
            str = __;
            return str;
        }
    }
}
