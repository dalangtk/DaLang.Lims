using DaLang.Lims.Web.Framework.Core.ClayObject;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
///     <see cref="Controller" /> 拓展类
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    ///     创建一个 <see cref="ViewResult" /> 对象，并将视图模型设置为 <see cref="Clay" /> 类型
    /// </summary>
    /// <param name="controller">
    ///     <see cref="Controller" />
    /// </param>
    /// <param name="model">视图模型</param>
    /// <param name="clayOptions">
    ///     <see cref="ClayOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="ViewResult" />
    /// </returns>
    public static ViewResult ViewClay(this Controller controller, object? model, ClayOptions? clayOptions = null) =>
        controller.View(Clay.Parse(model, clayOptions));

    /// <summary>
    ///     创建一个 <see cref="ViewResult" /> 对象，并将视图模型设置为 <see cref="Clay" /> 类型
    /// </summary>
    /// <param name="controller">
    ///     <see cref="Controller" />
    /// </param>
    /// <param name="viewName">视图名称</param>
    /// <param name="model">视图模型</param>
    /// <param name="clayOptions">
    ///     <see cref="ClayOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="ViewResult" />
    /// </returns>
    public static ViewResult ViewClay(this Controller controller, string? viewName, object? model,
        ClayOptions? clayOptions = null) =>
        controller.View(viewName, Clay.Parse(model, clayOptions));
}