using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Customer;

/// <summary>
/// 客户 实体类
/// </summary>
/// <remarks>客户</remarks>
[SugarTable(TableName = "base_customer")]
public partial class BaseCustomerEntity : EntityTenant
{
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", ColumnDataType = "varchar", Length = 8)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 客户名称
    /// </summary>
    /// <remarks>客户名称</remarks>
    [SugarColumn(ColumnName = "CustomerName", ColumnDataType = "varchar", Length = 64)]
    public string? CustomerName { get; set; }
    /// <summary>
    /// 客户简称
    /// </summary>
    /// <remarks>客户简称</remarks>
    [SugarColumn(ColumnName = "CustomerNameAB", ColumnDataType = "varchar", Length = 32)]
    public string? CustomerNameAB { get; set; }
    /// <summary>
    /// 区域
    /// </summary>
    /// <remarks>区域</remarks>
    [SugarColumn(ColumnName = "Area", ColumnDataType = "varchar", Length = 16)]
    public string? Area { get; set; }
    /// <summary>
    /// 详细地址
    /// </summary>
    /// <remarks>详细地址</remarks>
    [SugarColumn(ColumnName = "Address", ColumnDataType = "varchar", Length = 255)]
    public string? Address { get; set; }
    /// <summary>
    /// 经度
    /// </summary>
    /// <remarks>经度</remarks>
    [SugarColumn(ColumnName = "Longitude", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? Longitude { get; set; }
    /// <summary>
    /// 纬度
    /// </summary>
    /// <remarks>纬度</remarks>
    [SugarColumn(ColumnName = "Latitude", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? Latitude { get; set; }
    /// <summary>
    /// 客户等级
    /// </summary>
    /// <remarks>客户等级</remarks>
    [SugarColumn(ColumnName = "CustomerLevel", ColumnDataType = "varchar", Length = 8)]
    public string? CustomerLevel { get; set; }
    /// <summary>
    /// 客户类型
    /// </summary>
    /// <remarks>客户类型</remarks>
    [SugarColumn(ColumnName = "CustomerType", ColumnDataType = "varchar", Length = 8)]
    public string? CustomerType { get; set; }
    /// <summary>
    /// 客户性质
    /// </summary>
    /// <remarks>客户性质</remarks>
    [SugarColumn(ColumnName = "CustomerNature", ColumnDataType = "varchar", Length = 8)]
    public string? CustomerNature { get; set; }
    /// <summary>
    /// 合作模式
    /// </summary>
    /// <remarks>合作模式</remarks>
    [SugarColumn(ColumnName = "CollaborationMode", ColumnDataType = "varchar", Length = 8)]
    public string? CollaborationMode { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    /// <remarks>联系人</remarks>
    [SugarColumn(ColumnName = "Contacts", ColumnDataType = "varchar", Length = 16)]
    public string? Contacts { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>联系电话</remarks>
    [SugarColumn(ColumnName = "ContactPhone", ColumnDataType = "varchar", Length = 32)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 危急值联系人
    /// </summary>
    /// <remarks>危急值联系人</remarks>
    [SugarColumn(ColumnName = "CriticalContacts", ColumnDataType = "varchar", Length = 16)]
    public string? CriticalContacts { get; set; }
    /// <summary>
    /// 危急值联系电话
    /// </summary>
    /// <remarks>危急值联系电话</remarks>
    [SugarColumn(ColumnName = "CriticalContactPhone", ColumnDataType = "varchar", Length = 32)]
    public string? CriticalContactPhone { get; set; }
    /// <summary>
    /// 信息录入方式
    /// </summary>
    /// <remarks>信息录入方式</remarks>
    [SugarColumn(ColumnName = "DoubleInputType", ColumnDataType = "varchar", Length = 8)]
    public string? DoubleInputType { get; set; }
    /// <summary>
    /// 合作方式
    /// </summary>
    /// <remarks>合作方式</remarks>
    [SugarColumn(ColumnName = "CooperateType", ColumnDataType = "varchar", Length = 8)]
    public string? CooperateType { get; set; }
    /// <summary>
    /// 首次合作时间
    /// </summary>
    /// <remarks>首次合作时间</remarks>
    [SugarColumn(ColumnName = "FirstCooperateTime", ColumnDataType = "datetime")]
    public DateTime? FirstCooperateTime { get; set; }
    /// <summary>
    /// 客户分类
    /// </summary>
    /// <remarks>客户分类</remarks>
    [SugarColumn(ColumnName = "CustomerClassification", ColumnDataType = "varchar", Length = 8)]
    public string? CustomerClassification { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
    /// <summary>
    /// 归属机构
    /// </summary>
    [SugarColumn(ColumnName = "BelongToTenant")]
    public long? BelongToTenant { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }


    ///// <summary>
    ///// logo
    ///// </summary>
    ///// <remarks>logo</remarks>
    //[SugarColumn(ColumnName = "CustomerLogo", ColumnDataType = "varchar", Length = 128)]
    //public string? CustomerLogo { get; set; }
}

#pragma warning restore CS8618

