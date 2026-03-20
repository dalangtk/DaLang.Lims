using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DaLang.Lims.Web.Common.Extensions;

namespace DaLang.Lims.Web.Framework.Core.Converters.Json;

/// <summary>
///     <see cref="string" /> JSON 序列化转换器
/// </summary>
/// <remarks>解决 Number 类型和 Boolean 类型转 String 类型时异常。</remarks>
public class StringJsonConverter : JsonConverter<string>
{
    /// <inheritdoc />
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.True or JsonTokenType.False => reader.GetBoolean().ToString(),
            JsonTokenType.Number => reader.ConvertRawValueToString(),
            _ => reader.GetString()
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value);
}