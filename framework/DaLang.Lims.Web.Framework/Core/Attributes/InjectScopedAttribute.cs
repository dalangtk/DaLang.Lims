using System;

namespace DaLang.Lims.Web.Framework.Core.Attributes;

/// <summary>
/// 作用域注入
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class InjectScopedAttribute : Attribute
{
}