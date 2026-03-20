using System;

namespace DaLang.Lims.Web.Framework.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SnowflakeAttribute : Attribute
{
    public bool Enable { get; set; } = true;
}