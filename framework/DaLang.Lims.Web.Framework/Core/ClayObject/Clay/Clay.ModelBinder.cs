using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.ClayObject;

/// <summary>
///     流变对象
/// </summary>
/// <remarks>
///     <para>为最小 API 提供模型绑定。</para>
///     <para>参考文献：https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-9.0#custom-binding。</para>
/// </remarks>
public partial class Clay
{
    /// <summary>
    ///     为最小 API 提供模型绑定
    /// </summary>
    /// <remarks>由运行时调用。</remarks>
    /// <param name="httpContext"><c>HttpContext</c> 实例</param>
    /// <param name="parameter">
    ///     <see cref="ParameterInfo" />
    /// </param>
    /// <returns>
    ///     <see cref="Clay" />
    /// </returns>
    public static async ValueTask<Clay?> BindAsync(HttpContext httpContext, ParameterInfo parameter) =>
        await ClayBinder.BindAsync(httpContext, parameter)!;
}