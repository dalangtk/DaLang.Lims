using System;

namespace DaLang.Lims.Web.DynamicApi.Attributes;

[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class NonDynamicApiAttribute:Attribute
{
    
}