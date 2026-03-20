using DaLang.Lims.Web.Framework.Core.Attributes;
using SqlSugar;
using System;
using System.Reflection;

namespace DaLang.Lims.Web.Framework.Core.Entities;

public abstract class EntityBaseId
{
    /// <summary>
    /// 雪花Id
    /// </summary>
    [SugarColumn(ColumnName = "Id", ColumnDescription = "主键Id", IsPrimaryKey = true, IsIdentity = false, CreateTableFieldSort = -1, IsOnlyIgnoreUpdate = true)]
    public virtual long Id { get; set; }
}

/// <summary>
/// 框架实体基类
/// </summary>
public abstract class EntityBase : EntityBaseId, IDeletedFilter
{

    /// <summary>
    /// 创建者Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者Id", IsOnlyIgnoreUpdate = true, CreateTableFieldSort = 801)]
    public virtual long? ProId { get; set; }

    /// <summary>
    /// 创建者姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者姓名", Length = 64, IsOnlyIgnoreUpdate = true, CreateTableFieldSort = 802)]
    public virtual string? ProName { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsOnlyIgnoreUpdate = true, InsertServerTime = true, CreateTableFieldSort = 803)]
    public virtual DateTime ProTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 修改者Id
    /// </summary>
    [SugarColumn(ColumnDescription = "修改者Id", CreateTableFieldSort = 804)]
    public virtual long? ModId { get; set; }

    /// <summary>
    /// 修改者姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "修改者姓名", Length = 64, CreateTableFieldSort = 805)]
    public virtual string? ModName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", IsOnlyIgnoreInsert = true, UpdateServerTime = true, CreateTableFieldSort = 806)]
    public virtual DateTime? ModTime { get; set; }
    /// <summary>
    /// 已修改
    /// </summary>
    [SugarColumn(ColumnDescription = "已修改", DefaultValue = "0", CreateTableFieldSort = 807)]
    public virtual bool IsModified { get; set; } = false;

    /// <summary>
    /// 已删除
    /// </summary>
    [SugarColumn(ColumnDescription = "已删除", DefaultValue = "0", CreateTableFieldSort = 808)]
    public virtual bool IsDeleted { get; set; } = false;
    /// <summary>
    /// 设置排序
    /// </summary>
    /// <param name="s"></param>
    public virtual void SetSort(int s)
    {
        var p = this.GetType().GetProperty("Sort");
        if (p is not null)
        {
            int step = 1;
            var attr = p.GetCustomAttribute(typeof(SortGrowStepAttribute));
            if (attr != null)
            {
                step = (attr as SortGrowStepAttribute).Step;
                p.SetValue(this, s + step);
            }

        }
    }
}

/// <summary>
/// 业务数据实体基类（数据权限）
/// </summary>
//public abstract class EntityBaseData : EntityBase, IOrgIdFilter
//{
//    /// <summary>
//    /// 创建者部门Id
//    /// </summary>
//    [SugarColumn(ColumnDescription = "创建者部门Id", IsOnlyIgnoreUpdate = true)]
//    public virtual long? CreateOrgId { get; set; }

//    /// <summary>
//    /// 创建者部门
//    /// </summary>
//    [Newtonsoft.Json.JsonIgnore]
//    [System.Text.Json.Serialization.JsonIgnore]
//    [Navigate(NavigateType.OneToOne, nameof(CreateOrgId))]
//    public virtual SysOrg CreateOrg { get; set; }

//    /// <summary>
//    /// 创建者部门名称
//    /// </summary>
//    [SugarColumn(ColumnDescription = "创建者部门名称", Length = 64, IsOnlyIgnoreUpdate = true)]
//    public virtual string? CreateOrgName { get; set; }
//}

/// <summary>
/// 租户实体基类
/// </summary>
public abstract class EntityTenant : EntityBase, ITenantIdFilter
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true, CreateTableFieldSort = 800)]
    public virtual long? TenantId { get; set; }
}


///// <summary>
///// 租户实体基类 + 业务数据（数据权限）
///// </summary>
//public abstract class EntityTenantBaseData : EntityBase, ITenantIdFilter
//{
//    /// <summary>
//    /// 租户Id
//    /// </summary>
//    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
//    public virtual long? TenantId { get; set; }
//}