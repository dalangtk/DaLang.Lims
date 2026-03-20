using Microsoft.Extensions.Localization;
using DaLang.Lims.Web.Framework.Core.Attributes;

namespace DaLang.Lims.Web.Framework.Resources;

/// <summary>
/// Admin国际化
/// </summary>
[InjectSingleton]
public class AdminLocalizer: ModuleLocalizer
{
    public AdminLocalizer(IStringLocalizer<AdminLocalizer> localizer) : base(localizer)
    {
    }
}
