
using System;

namespace DaLang.Lims.Web.Framework.Core.ClayObject;

/// <summary>
///     流变对象模型绑定特性
/// </summary>
/// <remarks>示例代码：<c>[Clay] dynamic input</c>。</remarks>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ClayAttribute : Attribute;