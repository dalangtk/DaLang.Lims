using System;

namespace DaLang.Lims.Web.Framework.Core.ClayObject;

/// <summary>
///     <see cref="Clay" /> 对象事件参数
/// </summary>
public sealed class ClayEventArgs : EventArgs
{
    /// <summary>
    ///     <inheritdoc cref="ClayEventArgs" />
    /// </summary>
    /// <param name="identifier">标识符，可以是键（字符串）或索引（整数）或索引运算符（Index）或范围运算符（Range）</param>
    /// <param name="isFound">指示标识符是否存在</param>
    internal ClayEventArgs(object identifier, bool isFound)
    {
        Identifier = identifier;
        IsFound = isFound;
    }

    /// <summary>
    ///     标识符，可以是键（字符串）或索引（整数）或索引运算符（Index）或范围运算符（Range）
    /// </summary>
    public object Identifier { get; }

    /// <summary>
    ///     指示标识符是否存在
    /// </summary>
    public bool IsFound { get; }
}