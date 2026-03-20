using System.Text.RegularExpressions;

namespace DaLang.Lims.Web.Common.Helpers;

public static class RegexHelper
{
    /// <summary>
    /// 判断字符串是否由字母+数字组成
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsLetterAndNumber(this string str)
    {
        return Regex.IsMatch(str, "^[a-zA-Z0-9]+$");
    }
    /// <summary>
    /// 正则替换
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="pattern">正则</param>
    /// <param name="originalStr">待替换的字符串</param>
    /// <param name="replaceStr">替换字符串</param>
    /// <returns></returns>
    public static string Replace(this string str, string pattern, string originalStr, string replaceStr)
    {
        var newStr = Regex.Replace(str, pattern, match =>
        {
            string content = match.Groups[1].Value;
            string replacedContent = content.Replace(originalStr, replaceStr);
            return $"({replacedContent})";
        });
        return newStr;
    }
    /// <summary>
    /// 是否科学计数法
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsScientificNotation(string input)
    {
        string pattern = @"^[-+]?\d+(\.\d+)?[eE][+-]?\d+$";
        return Regex.IsMatch(input, pattern);
    }
    /// <summary>
    /// 验证字符串是否是 <c>application/x-www-form-urlencoded</c> 格式
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsUrlEncodedFormFormat(this string input)
    {
        string pattern = @"^(?:(?:[a-zA-Z0-9-._~]|%[0-9A-Fa-f]{2})+=(?:[a-zA-Z0-9-._~+]|%[0-9A-Fa-f]{2})*)(?:&(?:[a-zA-Z0-9-._~]|%[0-9A-Fa-f]{2})+=(?:[a-zA-Z0-9-._~+]|%[0-9A-Fa-f]{2})*)*$";
        return Regex.IsMatch(input, pattern);
    }
}
