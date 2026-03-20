using System;

namespace DaLang.Lims.Web.Framework.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class OrderGuidAttribute : Attribute
{
    public bool Enable { get; set; } = true;
}