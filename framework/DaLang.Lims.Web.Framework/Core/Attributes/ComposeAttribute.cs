using System;

namespace DaLang.Lims.Web.Framework.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ComposeAttribute : Attribute
{
    public Type[] SourceTypes { get; }

    public ComposeAttribute(params Type[] sourceTypes)
    {
        SourceTypes = sourceTypes;
    }
}
