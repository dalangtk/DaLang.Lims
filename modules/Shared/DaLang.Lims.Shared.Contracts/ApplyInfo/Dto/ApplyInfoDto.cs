namespace DaLang.Lims.Shared.Contracts.ApplyInfo.Dto;

///<summary>
///申请单信息查询结果输出
///</summary>
public class ApplyInfoDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///客户名称
    ///</summary>
    public string? CustomerName { get; set; }
    /// <summary>
    ///客户条码
    ///</summary>
    public string? CustomerBarcode { get; set; }
    /// <summary>
    ///病人类别代码
    ///</summary>
    public string? PatientTypeCode { get; set; }
    /// <summary>
    ///病人类别名称
    ///</summary>
    public string? PatientTypeName { get; set; }
    /// <summary>
    ///门诊号
    ///</summary>
    public string? PatientId { get; set; }
    /// <summary>
    ///病人姓名
    ///</summary>
    public string? PatientName { get; set; }
    /// <summary>
    ///性别代码
    ///</summary>
    public string? GenderCode { get; set; }
    /// <summary>
    ///性别
    ///</summary>
    public string? GenderName { get; set; }
    /// <summary>
    ///年龄1
    ///</summary>
    public string? Age1 { get; set; }
    /// <summary>
    ///年龄单位代码1
    ///</summary>
    public string? AgeUnit1 { get; set; }
    /// <summary>
    ///年龄单位1
    ///</summary>
    public string? AgeUnitName1 { get; set; }
    /// <summary>
    ///年龄2
    ///</summary>
    public string? Age2 { get; set; }
    /// <summary>
    ///年龄单位代码2
    ///</summary>
    public string? AgeUnit2 { get; set; }
    /// <summary>
    ///年龄单位2
    ///</summary>
    public string? AgeUnitName2 { get; set; }
    /// <summary>
    /// 年龄描述
    /// </summary>
    public string AgeDesc
    {
        get
        {
            var ageDesc = string.Empty;

            if (!string.IsNullOrWhiteSpace(Age1))
            {
                ageDesc += Age1 + (string.IsNullOrWhiteSpace(AgeUnitName1) ? "" : AgeUnitName1);
            }
            else
            {
                ageDesc = "/";
                return ageDesc;
            }

            if (!string.IsNullOrWhiteSpace(Age2))
            {
                ageDesc += Age2 + (string.IsNullOrWhiteSpace(AgeUnitName1) ? " " : "") + (string.IsNullOrWhiteSpace(AgeUnitName2) ? "" : AgeUnitName2);
            }

            return ageDesc;
        }
    }
    /// <summary>
    ///证件类型代码
    ///</summary>
    public string? CardTypeCode { get; set; }
    /// <summary>
    ///证件类型名称
    ///</summary>
    public string? CardTypeName { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    ///生日
    ///</summary>
    public DateTime? BirthDay { get; set; }
    /// <summary>
    ///是否绝经
    ///</summary>
    public int? IsMenoPause { get; set; } = 0;
    /// <summary>
    ///末次月经
    ///</summary>
    public DateTime? LastMenstrualPeriod { get; set; }
    /// <summary>
    ///身高
    ///</summary>
    public string? Height { get; set; }
    /// <summary>
    ///体重
    ///</summary>
    public string? Weight { get; set; }
    /// <summary>
    ///NT检查结果
    ///</summary>
    public string? NTTestResult { get; set; }
    /// <summary>
    ///头臀长
    ///</summary>
    public string? CRL { get; set; }
    /// <summary>
    ///双顶径
    ///</summary>
    public string? BPD { get; set; }
    /// <summary>
    ///孕周
    ///</summary>
    public string? GestationalWeeks { get; set; }
    /// <summary>
    ///地址
    ///</summary>
    public string? HomeAddress { get; set; }
    /// <summary>
    ///科室
    ///</summary>
    public string? Department { get; set; }
    /// <summary>
    ///病区
    ///</summary>
    public string? Ward { get; set; }
    /// <summary>
    ///送检医生
    ///</summary>
    public string? Doctor { get; set; }
    /// <summary>
    ///病床号
    ///</summary>
    public string? BedNo { get; set; }
    /// <summary>
    /// 临床诊断
    /// </summary>
    public string? ClinicalDiagnosis { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string? PurCodes { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string? PurNames { get; set; }
    /// <summary>
    ///标本类型代码
    ///</summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    ///标本类型名称
    ///</summary>
    public string? SampleTypeName { get; set; }
    /// <summary>
    ///采集时间
    ///</summary>
    public DateTime? CollectTime { get; set; }
    /// <summary>
    ///接收时间
    ///</summary>
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    ///标本状态代码
    ///</summary>
    public int? SampleStatus { get; set; }
    /// <summary>
    ///标本状态
    ///</summary>
    public string? SampleStatusName { get; set; }
    /// <summary>
    ///收费类型
    ///</summary>
    public string? ChargeType { get; set; }
    /// <summary>
    ///加急
    ///</summary>
    public int? IsUrgent { get; set; } = 0;
    /// <summary>
    /// 数据来源
    /// </summary>
    public int DataSource { get; set; } = 0;
    /// <summary>
    ///来源机构
    ///</summary>
    public long? FromTenant { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}
