using System.Collections.Generic;

namespace DaLang.Lims.Web.Framework.Services.Permission.Dto;

public class PermissionAddDotInput
{
    /// <summary>
    /// 父级节点
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 关联接口
    /// </summary>
    public List<long> ApiIds { get; set; } = new List<long>();

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 权限编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; }
    /// <summary>
    /// 是否api权限点
    /// </summary>
    public bool IsApiDot { get; set; } = false;
}