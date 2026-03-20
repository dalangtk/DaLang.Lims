using DaLang.Lims.Web.Common.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace DaLang.Lims.Web;

public static class StringExtension
{
    /// <summary>
    /// 判断字符串是否为Null、空
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsNull(this string s)
    {
        return string.IsNullOrWhiteSpace(s);
    }

    /// <summary>
    /// 判断字符串是否不为Null、空
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool NotNull(this string s)
    {
        return !string.IsNullOrWhiteSpace(s);
    }

    /// <summary>
    /// 与字符串进行比较，忽略大小写
    /// </summary>
    /// <param name="s"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string s, string value)
    {
        return s.Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 首字母转小写
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string FirstCharToLower(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        string str = s.First().ToString().ToLower() + s.Substring(1);
        return str;
    }

    /// <summary>
    /// 首字母转大写
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string FirstCharToUpper(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        string str = s.First().ToString().ToUpper() + s.Substring(1);
        return str;
    }

    /// <summary>
    /// 转为Base64，UTF-8格式
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToBase64(this string s)
    {
        return s.ToBase64(Encoding.UTF8);
    }

    /// <summary>
    /// 转为Base64
    /// </summary>
    /// <param name="s"></param>
    /// <param name="encoding">编码</param>
    /// <returns></returns>
    public static string ToBase64(this string s, Encoding encoding)
    {
        if (s.IsNull())
            return string.Empty;

        var bytes = encoding.GetBytes(s);
        return bytes.ToBase64();
    }

    public static string ToPath(this string s)
    {
        if (s.IsNull())
            return string.Empty;

        return s.Replace(@"\", "/");
    }

    public static string Format(this string str, object obj)
    {
        if (str.IsNull())
        {
            return str;
        }
        string s = str;
        if (obj.GetType().Name == "JObject")
        {
            foreach (var item in (Newtonsoft.Json.Linq.JObject)obj)
            {
                var k = item.Key.ToString();
                var v = item.Value.ToString();
                s = Regex.Replace(s, "\\{" + k + "\\}", v, RegexOptions.IgnoreCase);
            }
        }
        else
        {
            foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
            {
                var xx = p.Name;
                var yy = p.GetValue(obj).ToString();
                s = Regex.Replace(s, "\\{" + xx + "\\}", yy, RegexOptions.IgnoreCase);
            }
        }
        return s;
    }

    /// <summary>
    ///     将 <c>application/x-www-form-urlencoded</c> 格式的字符串解析为 <see cref="JsonObject" />
    /// </summary>
    /// <param name="formData">URL 编码的表单数据字符串</param>
    /// <returns>
    ///     <see cref="JsonObject" />
    /// </returns>
    public static JsonObject? ParseUrlEncodedFormToJsonObject(this string? formData)
    {
        // 尝试移除开头的 ?
        formData = formData?.TrimStart('?');

        // 空检查
        if (string.IsNullOrWhiteSpace(formData))
        {
            return null;
        }

        // 初始化 JsonObject 实例
        var root = new JsonObject();

        // 按 & 分割每个键值对
        foreach (var part in formData.Split('&'))
        {
            // 查找第一个 =
            var eqIndex = part.IndexOf('=');

            // 键名为空或不存在 = 则跳过
            if (eqIndex <= 0)
            {
                continue;
            }

            // URL 解码键和值
            var key = WebUtility.UrlDecode(part[..eqIndex]);
            var value = WebUtility.UrlDecode(part[(eqIndex + 1)..]);

            // 将键名（如 user[0][name]）拆分为 token：["user", "0", "name"]
            var tokens = key.Replace("]", "").Split('[');
            var current = root;
            var i = 0;

            // 逐层构建嵌套结构
            while (i < tokens.Length)
            {
                // 空检查
                var token = tokens[i];
                if (string.IsNullOrEmpty(token))
                {
                    i++;
                    continue;
                }

                // 检查是否是最后一项
                if (i == tokens.Length - 1)
                {
                    current[token] = value;
                    break;
                }

                // 下一项为数组索引，按数组处理
                var nextToken = tokens[i + 1];
                if (int.TryParse(nextToken, out var index) && index >= 0)
                {
                    // 确保当前 token 是一个 JsonArray
                    if (current[token] is not JsonArray array)
                    {
                        array = [];
                        current[token] = array;
                    }

                    // 扩容数组
                    while (array.Count <= index)
                    {
                        array.Add(new JsonObject());
                    }

                    // 进入该数组元素并跳过数组名和索引
                    current = (JsonObject)array[index]!;
                    i += 2;
                }
                else
                {
                    // 下一个 token 是普通属性名则视为对象名
                    if (current[token] is not JsonObject child)
                    {
                        child = new JsonObject();
                        current[token] = child;
                    }

                    current = child;
                    i++;
                }
            }
        }

        return root;
    }
}