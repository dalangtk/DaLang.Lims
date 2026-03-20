using System;
using System.Collections;

namespace DaLang.Lims.Web.Common.Helpers
{
    public static class CheckHelper
    {
        public static bool CheckNull(this IList list)
        {
            return list is null || list.Count == 0;
        }
        public static bool CheckNull(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static void ThrowNotSupportedException(string message)
        {
            message = message.CheckNull() ? new NotSupportedException().Message : message;
            throw new Exception("NotSupportedException：" + message);
        }

        public static void ArgumentNullException(object checkObj, string message)
        {
            if (checkObj == null)
                throw new Exception("ArgumentNullException：" + message);
        }

        public static void ArgumentNullException(object[] checkObj, string message)
        {
            if (checkObj == null || checkObj.Length == 0)
                throw new Exception("ArgumentNullException：" + message);
        }

        public static void Exception(bool isException, string message, params string[] args)
        {
            if (isException)
                throw new Exception(string.Format(message, args));
        }
        public static void ExceptionEasy(string message)
        {
            throw new Exception(message);
        }
        public static void ExceptionEasy(bool isException, string message)
        {
            if (isException)
                throw new Exception(message);
        }
    }
}
