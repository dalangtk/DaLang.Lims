namespace DaLang.Lims.Pretreatment.Contracts.SampleInput.Dto;

public class InputInfoDto
{
    /// <summary>
    /// 条码
    /// </summary>
    public string Barcode { get; set; }
    /// <summary>
    /// 姓名
    /// </summary>
    public string PatientName { get; set; }
    /// <summary>
    /// 性别代码
    /// </summary>
    public string? GenderCode { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public string? GenderName { get; set; }
    /// <summary>
    /// 年龄1
    /// </summary>
    public string? Age1 { get; set; }
    /// <summary>
    /// 年龄单位代码1
    /// </summary>
    public string? AgeUnit1 { get; set; }
    /// <summary>
    /// 年龄单位1
    /// </summary>
    public string? AgeUnitName1 { get; set; }
    /// <summary>
    /// 年龄2
    /// </summary>
    public string? Age2 { get; set; }
    /// <summary>
    /// 年龄单位代码2
    /// </summary>
    public string? AgeUnit2 { get; set; }
    /// <summary>
    /// 年龄单位2
    /// </summary>
    public string? AgeUnitName2 { get; set; }
    /// <summary>
    /// 加急
    /// </summary>
    public int IsUrgent { get; set; } = 0;
    /// <summary>
    /// 科室
    /// </summary>
    public string? Department { get; set; }
    /// <summary>
    /// 病区
    /// </summary>
    public string? Ward { get; set; }
    /// <summary>
    /// 医生
    /// </summary>
    public string? Doctor { get; set; }
    /// <summary>
    /// 床号
    /// </summary>
    public string? BedNo { get; set; }
    /// <summary>
    /// 病员号
    /// </summary>
    public string? PatientId { get; set; }
    /// <summary>
    /// 客户条码
    /// </summary>
    public string? CustomerBarcode { get; set; }
    /// <summary>
    /// 采集时间
    /// </summary>
    public DateTime CollectTime { get; set; }
    /// <summary>
    /// 送检类型代码
    /// </summary>
    public string? PatientTypeCode { get; set; }
    /// <summary>
    /// 送检类型
    /// </summary>
    public string? PatientTypeName { get; set; }
    /// <summary>
    /// 电话
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}
