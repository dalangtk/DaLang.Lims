using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.UserGroup;

/// <summary>
/// 用户组别 实体类
/// </summary>
/// <remarks>用户组别</remarks>
[SugarTable(TableName = "base_user_group")]
public partial class BaseUserGroupEntity : EntityTenant
{
    /// <summary>
    /// 用户Id
    /// </summary>
    /// <remarks>用户Id</remarks>
    [SugarColumn(ColumnName = "UserId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20, IsNullable = false)]
    public long UserId { get; set; }
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16, IsNullable = false)]
    public string GroupCode { get; set; }
    /// <summary>
    /// 检验
    /// </summary>
    /// <remarks>检验</remarks>
    [SugarColumn(ColumnName = "CanTest", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanTest { get; set; } = 0;
    /// <summary>
    /// 初审
    /// </summary>
    /// <remarks>初审</remarks>
    [SugarColumn(ColumnName = "CanFirstCheck", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanFirstCheck { get; set; } = 0;
    /// <summary>
    /// 复审
    /// </summary>
    /// <remarks>复审</remarks>
    [SugarColumn(ColumnName = "CanSecondCheck", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanSecondCheck { get; set; } = 0;
    /// <summary>
    /// 反审
    /// </summary>
    /// <remarks>反审</remarks>
    [SugarColumn(ColumnName = "CanUnCheck", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanUnCheck { get; set; } = 0;
    /// <summary>
    /// 打印后反审
    /// </summary>
    /// <remarks>打印后反审</remarks>
    [SugarColumn(ColumnName = "CanPrintedUnCheck", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanPrintedUnCheck { get; set; } = 0;
    /// <summary>
    /// 取消检测
    /// </summary>
    /// <remarks>取消检测</remarks>
    [SugarColumn(ColumnName = "CanCancelTest", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanCancelTest { get; set; } = 0;
    /// <summary>
    /// 反取消检测
    /// </summary>
    /// <remarks>反取消检测</remarks>
    [SugarColumn(ColumnName = "CanDisCancel", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanDisCancel { get; set; } = 0;
    /// <summary>
    /// 修改信息
    /// </summary>
    /// <remarks>修改信息</remarks>
    [SugarColumn(ColumnName = "CanModifiedInfo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public int CanModifiedInfo { get; set; } = 0;
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

