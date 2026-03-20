using System;

namespace DaLang.Lims.Web.Framework.Core.Attributes;

/// <summary>
/// 排序增长步长
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SortGrowStepAttribute : Attribute
{
    public int Step = 1;
    /// <summary>
    /// 步长
    /// </summary>
    /// <param name="step">步长</param>
    public SortGrowStepAttribute(int step = 1)
    {
        this.Step = step;
    }
}
