namespace DaLang.Lims.Web.Framework.Core.ClayObject;

/// <summary>
///     流变对象的基本类型
/// </summary>
/// <remarks>用于区分是单一对象还是集合或数组形式。</remarks>
public enum ClayType
{
    /// <summary>
    ///     单一对象
    /// </summary>
    /// <remarks>缺省值。</remarks>
    Object = 0,

    /// <summary>
    ///     集合或数组形式
    /// </summary>
    Array
}