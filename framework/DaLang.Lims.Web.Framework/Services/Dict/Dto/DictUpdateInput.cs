using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Dict.Dto;

/// <summary>
/// 修改
/// </summary>
public class DictUpdateInput : DictAddInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择数据字典")]
    public long Id { get; set; }
}