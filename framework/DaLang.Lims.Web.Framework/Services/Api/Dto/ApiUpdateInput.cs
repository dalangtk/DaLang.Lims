using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Api.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class ApiUpdateInput : ApiAddInput
{
    /// <summary>
    /// 接口Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择接口")]
    public long Id { get; set; }
}