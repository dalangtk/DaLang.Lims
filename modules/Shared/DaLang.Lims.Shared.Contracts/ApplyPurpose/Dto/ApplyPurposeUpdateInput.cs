namespace DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;

public class ApplyPurposeUpdateInput
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///组别名称
    ///</summary>
    public string GroupName { get; set; }
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///套餐代码
    ///</summary>
    public string? ComboCode { get; set; }
    /// <summary>
    ///套餐名称
    ///</summary>
    public string? ComboName { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string PurName { get; set; }
    /// <summary>
    ///个性化目的名称
    ///</summary>
    public string? PurNamePersonalize { get; set; }
    /// <summary>
    ///目的数量
    ///</summary>
    public int PurAmount { get; set; } = 1;
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string? InstrumentItemCodes { get; set; }
    /// <summary>
    ///工作流
    ///</summary>
    public string? WorkFlowType { get; set; }
    /// <summary>
    ///检测计划代码
    ///</summary>
    public string? TestExamPlanCode { get; set; }
    /// <summary>
    ///标本类型
    ///</summary>
    public string SampleTypeCode { get; set; }
    /// <summary>
    ///标本类型名称
    ///</summary>
    public string SampleTypeName { get; set; }
    /// <summary>
    ///预计检测日期
    ///</summary>
    public DateTime? EstimateTestDate { get; set; }
    /// <summary>
    ///计划报告日期
    ///</summary>
    public DateTime? EstimateReportTime { get; set; }
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
    public int? EntrustStatus { get; set; }
    /// <summary>
    ///预计问询时间
    ///</summary>
    public DateTime? EstimateAskTime { get; set; }
    /// <summary>
    ///原始组别代码
    ///</summary>
    public string OriginalGroupCode { get; set; }
    /// <summary>
    ///原始组别名称
    ///</summary>
    public string OriginalGroupName { get; set; }
    /// <summary>
    ///增项
    ///</summary>
    public int? AddType { get; set; } = 0;
    /// <summary>
    ///样本状态
    ///</summary>
    public int? SampleStatus { get; set; }
    /// <summary>
    ///样本状态名称
    ///</summary>
    public string? SampleStatusName { get; set; }
    /// <summary>
    ///接收人Id
    ///</summary>
    public long? ReceiveId { get; set; }
    /// <summary>
    ///接收人姓名
    ///</summary>
    public string? ReceiveName { get; set; }
    /// <summary>
    ///接收时间
    ///</summary>
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    ///标本数量
    ///</summary>
    public int? SampleCnt { get; set; } = 1;
    /// <summary>
    ///检测班次
    ///</summary>
    public string? TestShift { get; set; }
    /// <summary>
    ///检测系列
    ///</summary>
    public string? TestSeries { get; set; }
    /// <summary>
    ///分拣状态
    ///</summary>
    public int? SortStatus { get; set; } = 0;
    /// <summary>
    ///数据来源
    ///</summary>
    public int? DataSource { get; set; }
    /// <summary>
    ///批次号
    ///</summary>
    public long? BatchNo { get; set; }
    /// <summary>
    ///批次扫码顺序
    ///</summary>
    public int? BatchScanOrder { get; set; }
    /// <summary>
    ///检验任务Id
    ///</summary>
    public long? TaskId { get; set; }
    /// <summary>
    /// 是否计费
    /// </summary>
    public bool IsCharging { get; set; } = true;
    /// <summary>
    /// 财务生成标记
    /// </summary>
    public int FinanceStatus { get; set; } = 0;
}
