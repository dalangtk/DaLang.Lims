using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Shared.Domain.ApplyInfo;

/// <summary>
/// 申请单信息 实体类
/// </summary>
/// <remarks>申请信息</remarks>
[SugarTable(TableName = "apply_info")]
public partial class ApplyInfoEntity : EntityTenant
{
    /// <summary>
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 客户名称
    /// </summary>
    /// <remarks>客户名称</remarks>
    [SugarColumn(ColumnName = "CustomerName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? CustomerName { get; set; }
    /// <summary>
    /// 客户条码
    /// </summary>
    /// <remarks>客户条码</remarks>
    [SugarColumn(ColumnName = "CustomerBarcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? CustomerBarcode { get; set; }
    /// <summary>
    /// 病人类别代码
    /// </summary>
    /// <remarks>病人类别代码</remarks>
    [SugarColumn(ColumnName = "PatientTypeCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? PatientTypeCode { get; set; }
    /// <summary>
    /// 病人类别名称
    /// </summary>
    /// <remarks>病人类别名称</remarks>
    [SugarColumn(ColumnName = "PatientTypeName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? PatientTypeName { get; set; }
    /// <summary>
    /// 门诊号
    /// </summary>
    /// <remarks>门诊号</remarks>
    [SugarColumn(ColumnName = "PatientId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? PatientId { get; set; }
    /// <summary>
    /// 病人姓名
    /// </summary>
    /// <remarks>病人姓名</remarks>
    [SugarColumn(ColumnName = "PatientName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? PatientName { get; set; }
    /// <summary>
    /// 性别代码
    /// </summary>
    /// <remarks>性别代码</remarks>
    [SugarColumn(ColumnName = "GenderCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? GenderCode { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    /// <remarks>性别</remarks>
    [SugarColumn(ColumnName = "GenderName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? GenderName { get; set; }
    /// <summary>
    /// 年龄1
    /// </summary>
    /// <remarks>年龄1</remarks>
    [SugarColumn(ColumnName = "Age1", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? Age1 { get; set; }
    /// <summary>
    /// 年龄单位代码1
    /// </summary>
    /// <remarks>年龄单位代码1</remarks>
    [SugarColumn(ColumnName = "AgeUnit1", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? AgeUnit1 { get; set; }
    /// <summary>
    /// 年龄单位1
    /// </summary>
    /// <remarks>年龄单位1</remarks>
    [SugarColumn(ColumnName = "AgeUnitName1", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? AgeUnitName1 { get; set; }
    /// <summary>
    /// 年龄2
    /// </summary>
    /// <remarks>年龄2</remarks>
    [SugarColumn(ColumnName = "Age2", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? Age2 { get; set; }
    /// <summary>
    /// 年龄单位代码2
    /// </summary>
    /// <remarks>年龄单位代码2</remarks>
    [SugarColumn(ColumnName = "AgeUnit2", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? AgeUnit2 { get; set; }
    /// <summary>
    /// 年龄单位2
    /// </summary>
    /// <remarks>年龄单位2</remarks>
    [SugarColumn(ColumnName = "AgeUnitName2", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? AgeUnitName2 { get; set; }
    /// <summary>
    /// 证件类型代码
    /// </summary>
    /// <remarks>证件类型代码</remarks>
    [SugarColumn(ColumnName = "CardTypeCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? CardTypeCode { get; set; }
    /// <summary>
    /// 证件类型名称
    /// </summary>
    /// <remarks>证件类型名称</remarks>
    [SugarColumn(ColumnName = "CardTypeName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? CardTypeName { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>联系电话</remarks>
    [SugarColumn(ColumnName = "Phone", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? Phone { get; set; }
    /// <summary>
    /// 生日
    /// </summary>
    /// <remarks>生日</remarks>
    [SugarColumn(ColumnName = "BirthDay", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "date")]
    public DateTime? BirthDay { get; set; }
    /// <summary>
    /// 是否绝经
    /// </summary>
    /// <remarks>是否绝经</remarks>
    [SugarColumn(ColumnName = "IsMenoPause", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0", DecimalDigits = 2)]
    public int? IsMenoPause { get; set; } = 0;
    /// <summary>
    /// 末次月经
    /// </summary>
    /// <remarks>末次月经</remarks>
    [SugarColumn(ColumnName = "LastMenstrualPeriod", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "date")]
    public DateTime? LastMenstrualPeriod { get; set; }
    /// <summary>
    /// 身高
    /// </summary>
    /// <remarks>身高</remarks>
    [SugarColumn(ColumnName = "Height", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? Height { get; set; }
    /// <summary>
    /// 体重
    /// </summary>
    /// <remarks>体重</remarks>
    [SugarColumn(ColumnName = "Weight", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? Weight { get; set; }
    /// <summary>
    /// NT检查结果
    /// </summary>
    /// <remarks>NT检查结果</remarks>
    [SugarColumn(ColumnName = "NTTestResult", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? NTTestResult { get; set; }
    /// <summary>
    /// 头臀长
    /// </summary>
    /// <remarks>头臀长</remarks>
    [SugarColumn(ColumnName = "CRL", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? CRL { get; set; }
    /// <summary>
    /// 双顶径
    /// </summary>
    /// <remarks>双顶径</remarks>
    [SugarColumn(ColumnName = "BPD", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? BPD { get; set; }
    /// <summary>
    /// 孕周
    /// </summary>
    /// <remarks>孕周</remarks>
    [SugarColumn(ColumnName = "GestationalWeeks", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? GestationalWeeks { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    /// <remarks>地址</remarks>
    [SugarColumn(ColumnName = "HomeAddress", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? HomeAddress { get; set; }
    /// <summary>
    /// 科室
    /// </summary>
    /// <remarks>科室</remarks>
    [SugarColumn(ColumnName = "Department", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? Department { get; set; }
    /// <summary>
    /// 病区
    /// </summary>
    /// <remarks>病区</remarks>
    [SugarColumn(ColumnName = "Ward", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? Ward { get; set; }
    /// <summary>
    /// 送检医生
    /// </summary>
    /// <remarks>送检医生</remarks>
    [SugarColumn(ColumnName = "Doctor", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? Doctor { get; set; }
    /// <summary>
    /// 病床号
    /// </summary>
    /// <remarks>病床号</remarks>
    [SugarColumn(ColumnName = "BedNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? BedNo { get; set; }
    /// <summary>
    /// 临床诊断
    /// </summary>
    /// <remarks>临床诊断</remarks>
    [SugarColumn(ColumnName = "ClinicalDiagnosis", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? ClinicalDiagnosis { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCodes", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? PurCodes { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    /// <remarks>目的名称</remarks>
    [SugarColumn(ColumnName = "PurNames", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? PurNames { get; set; }
    /// <summary>
    /// 标本类型代码
    /// </summary>
    /// <remarks>标本类型代码</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
    /// <remarks>标本类型名称</remarks>
    [SugarColumn(ColumnName = "SampleTypeName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? SampleTypeName { get; set; }
    /// <summary>
    /// 采集时间
    /// </summary>
    /// <remarks>采集时间</remarks>
    [SugarColumn(ColumnName = "CollectTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? CollectTime { get; set; }
    /// <summary>
    /// 接收时间
    /// </summary>
    /// <remarks>接收时间</remarks>
    [SugarColumn(ColumnName = "ReceiveTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    /// 标本状态代码
    /// </summary>
    /// <remarks>标本状态代码</remarks>
    [SugarColumn(ColumnName = "SampleStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", Length = 4)]
    public int? SampleStatus { get; set; }
    /// <summary>
    /// 标本状态
    /// </summary>
    /// <remarks>标本状态</remarks>
    [SugarColumn(ColumnName = "SampleStatusName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? SampleStatusName { get; set; }
    /// <summary>
    /// 收费类型
    /// </summary>
    /// <remarks>收费类型</remarks>
    [SugarColumn(ColumnName = "ChargeType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? ChargeType { get; set; }
    /// <summary>
    /// 加急
    /// </summary>
    /// <remarks>加急</remarks>
    [SugarColumn(ColumnName = "IsUrgent", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0", DecimalDigits = 2)]
    public int IsUrgent { get; set; } = 0;
    /// <summary>
    /// 数据来源,0excel导入 1api对接 2直接录入
    /// </summary>
    /// <remarks>数据来源</remarks>
    [SugarColumn(ColumnName = "DataSource", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, DefaultValue = "0")]
    public int DataSource { get; set; } = 0;
    /// <summary>
    /// 来源机构
    /// </summary>
    /// <remarks>来源机构</remarks>
    [SugarColumn(ColumnName = "FromTenant", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? FromTenant { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
}

#pragma warning restore CS8618

