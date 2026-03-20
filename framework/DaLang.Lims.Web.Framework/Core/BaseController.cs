using Microsoft.AspNetCore.Mvc;
using DaLang.Lims.Web.Framework.Core.Attributes;

namespace DaLang.Lims.Web.Framework.Core;

/// <summary>
/// 基础控制器
/// </summary>
[Route("api/[area]/[controller]/[action]")]
[ApiController]
[ValidatePermission]
[ValidateInput]
public abstract class BaseController : ControllerBase
{
}