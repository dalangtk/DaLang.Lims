using System;
using System.Linq;
using System.Text.Json;

namespace DaLang.Lims.Web.Framework.Core.ClayObject;

/// <summary>
///     <see cref="JsonSerializerOptions" /> 拓展类
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    ///     添加 <see cref="Clay" /> JSON 序列化配置
    /// </summary>
    /// <param name="jsonSerializerOptions">
    ///     <see cref="JsonSerializerOptions" />
    /// </param>
    public static void AddClayConverters(this JsonSerializerOptions jsonSerializerOptions)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

        // 处理对象序列化为 Clay 的问题
        if (!jsonSerializerOptions.Converters.OfType<ClayJsonConverter>().Any())
        {
            jsonSerializerOptions.Converters.Add(new ClayJsonConverter());
        }

        // 处理 object/dynamic 类型对象序列化为 Clay 的问题
        if (!jsonSerializerOptions.Converters.OfType<ObjectToClayJsonConverter>().Any())
        {
            jsonSerializerOptions.Converters.Add(new ObjectToClayJsonConverter());
        }
    }
}