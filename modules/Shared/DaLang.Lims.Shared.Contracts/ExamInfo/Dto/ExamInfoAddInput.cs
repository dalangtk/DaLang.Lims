namespace DaLang.Lims.Shared.Contracts.ExamInfo.Dto;

public class ExamInfoAddInput
{
    public long? TaskId { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string? GroupCode { get; set; }
    /// <summary>
    ///组别名称
    ///</summary>
    public string? GroupName { get; set; }
    /// <summary>
    ///条码
    ///</summary>
    public string? Barcode { get; set; }
    /// <summary>
    ///样本号
    ///</summary>
    public string? SampleNo { get; set; }
    /// <summary>
    ///
    ///</summary>
    public string? VirtualSampleNo { get; set; }
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
    ///检测日期
    ///</summary>
    public DateTime? TestDate { get; set; }
    /// <summary>
    ///工作流
    ///</summary>
    public string? WFCode { get; set; }
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
    /// 年龄值
    /// </summary>
    public int? AgeValue { get; set; }
    /// <summary>
    ///证件类型代码
    ///</summary>
    public string? CardTypeCode { get; set; }
    /// <summary>
    ///证件类型名称
    ///</summary>
    public string? CardTypeName { get; set; }
    /// <summary>
    ///联系电话
    ///</summary>
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
    ///临床诊断
    ///</summary>
    public string? ClinicalDiagnosis { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
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
    ///标本性状代码
    ///</summary>
    public string? SamplePropertyCode { get; set; }
    /// <summary>
    ///标本性状名称
    ///</summary>
    public string? SamplePropertyName { get; set; }
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
    public int IsUrgent { get; set; } = 0;
    /// <summary>
    ///检验时间
    ///</summary>
    public DateTime? InTestTime { get; set; }
    /// <summary>
    ///检验者Id
    ///</summary>
    public long? InspectorId { get; set; }
    /// <summary>
    ///检验授权人Id
    ///</summary>
    public long? InspectorAuthorizedId { get; set; }
    /// <summary>
    ///检验者
    ///</summary>
    public string? InspectorName { get; set; }
    /// <summary>
    ///初审人Id
    ///</summary>
    public long? FirstAuditId { get; set; }
    /// <summary>
    ///初审授权人Id
    ///</summary>
    public long? FirstAuditAuthorizedId { get; set; }
    /// <summary>
    ///初审人
    ///</summary>
    public string? FirstAuditName { get; set; }
    /// <summary>
    ///复审人Id
    ///</summary>
    public long? SecondAuditId { get; set; }
    /// <summary>
    ///复审授权人Id
    ///</summary>
    public long? SecondAuditAuthorizedId { get; set; }
    /// <summary>
    ///复审人
    ///</summary>
    public string? SecondAuditName { get; set; }
    /// <summary>
    ///批准人Id
    ///</summary>
    public long? ApproverId { get; set; }
    /// <summary>
    ///批准授权人Id
    ///</summary>
    public long? ApproverAuthorizedId { get; set; }
    /// <summary>
    ///批准人
    ///</summary>
    public string? ApproverName { get; set; }
    /// <summary>
    ///结果说明
    ///</summary>
    public string? ResultDescription { get; set; }
    /// <summary>
    ///建议解释
    ///</summary>
    public string? Suggestion { get; set; }
    /// <summary>
    ///初审时间
    ///</summary>
    public DateTime? FirstAuditTime { get; set; }
    /// <summary>
    ///报告时间
    ///</summary>
    public DateTime? SecondAuditTime { get; set; }
    /// <summary>
    ///复审时间
    ///</summary>
    public DateTime? CreateReportTime { get; set; }
    /// <summary>
    ///检查类型 0默认1加做 2复查
    ///</summary>
    public int TestType { get; set; } = 0;
    /// <summary>
    ///PDF报告
    ///</summary>
    public bool IsPdfReport { get; set; } = false;
    /// <summary>
    ///委托医院代码
    ///</summary>
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    ///委托医院名称
    ///</summary>
    public string? EntrustHospitalName { get; set; }
    /// <summary>
    ///委托状态
    ///</summary>
    public int? EntrustStatus { get; set; } = 0;
    /// <summary>
    ///异常结果
    ///</summary>
    public int IsExceptionResult { get; set; } = 0;
    /// <summary>
    ///打印时间
    ///</summary>
    public DateTime? PrintReportTime { get; set; }
    /// <summary>
    ///下载结果标志
    ///</summary>
    public int? DownloadFlag { get; set; }
    /// <summary>
    ///入库状态
    ///</summary>
    public int? StorageStatus { get; set; }
    /// <summary>
    ///入库位置
    ///</summary>
    public string? StorageLocation { get; set; }
    /// <summary>
    /// 内注
    /// </summary>
    public string? InternalRemarks { get; set; }
}
