using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DaLang.Lims.Web.Framework.Core.ClayObject;

public static class ClayObjectMvcBuilderExtensions
{
    /// <summary>
    ///     添加 <see cref="Clay" /> 配置
    /// </summary>
    /// <param name="builder">
    ///     <see cref="IMvcBuilder" />
    /// </param>
    /// <returns>
    ///     <see cref="IMvcBuilder" />
    /// </returns>
    public static IMvcBuilder AddClayOptions(this IMvcBuilder builder) => builder.AddClayOptions(_ => { });

    /// <summary>
    ///     添加 <see cref="Clay" /> 配置
    /// </summary>
    /// <param name="builder">
    ///     <see cref="IMvcBuilder" />
    /// </param>
    /// <param name="configure">自定义配置委托</param>
    /// <returns>
    ///     <see cref="IMvcBuilder" />
    /// </returns>
    public static IMvcBuilder AddClayOptions(this IMvcBuilder builder, Action<ClayOptions> configure)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(configure);

        // 配置 JsonOptions 选项，添加 ClayJsonConverter 和 ObjectToClayJsonConverter 转换器
        builder.Services.Configure<JsonOptions>(options => options.JsonSerializerOptions.AddClayConverters());

        // 配置 ClayOptions 选项服务
        builder.Services.Configure(configure);

        // 添加 Clay 模型绑定提供器
        builder.Services.Configure<MvcOptions>(options =>
        {
            if (!options.ModelBinderProviders.OfType<ClayBinderProvider>().Any())
            {
                options.ModelBinderProviders.Insert(0, new ClayBinderProvider());
            }
        });

        return builder;
    }
}


