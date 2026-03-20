namespace DaLang.Lims.Shared.Contracts.ExamResult.Dto;

public class ExamResultAddInput
{
    /// <summary>
    ///
    ///</summary>
    public long TaskDetailId { get; set; }
    public long ExamInfoId { get; set; }
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
    public string Barcode { get; set; }
    /// <summary>
    ///样本号
    ///</summary>
    public string SampleNo { get; set; }
    /// <summary>
    ///检测日期
    ///</summary>
    public DateTime? TestDate { get; set; }
    /// <summary>
    ///套餐代码
    ///</summary>
    public string? ComboCode { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    public string PurName { get; set; }
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string ItemCode { get; set; }
    /// <summary>
    ///项目名称
    ///</summary>
    public string ItemName { get; set; }
    /// <summary>
    ///个性化项目名称
    ///</summary>
    public string? ItemNamePersonalize { get; set; }
    /// <summary>
    ///英文名称
    ///</summary>
    public string? ItemNameEN { get; set; }
    /// <summary>
    ///项目简称
    ///</summary>
    public string? ItemNameAB { get; set; }
    /// <summary>
    ///项目单位
    ///</summary>
    public string? ItemUnit { get; set; }
    /// <summary>
    ///检验结果
    ///</summary>
    public string? ItemResult { get; set; }
    /// <summary>
    ///高低标记
    ///</summary>
    public string? HLFlag { get; set; }
    /// <summary>
    ///参考范围
    ///</summary>
    public string? ItemReference { get; set; }
    /// <summary>
    ///警告值范围
    ///</summary>
    public string? WarningRange { get; set; }
    /// <summary>
    ///危急值范围
    ///</summary>
    public string? CriticalRange { get; set; }
    /// <summary>
    ///报告显示范围
    ///</summary>
    public string? DisplayRange { get; set; }
    /// <summary>
    ///试剂代码
    ///</summary>
    public string? ReagentCode { get; set; }
    /// <summary>
    ///试剂名称
    ///</summary>
    public string? ReagentName { get; set; }
    /// <summary>
    ///仪器代码
    ///</summary>
    public string? InstrumentCode { get; set; }
    /// <summary>
    ///仪器名称
    ///</summary>
    public string? InstrumentName { get; set; }
    /// <summary>
    ///方法学代码
    ///</summary>
    public string? MethodCode { get; set; }
    /// <summary>
    ///方法学
    ///</summary>
    public string? MethodName { get; set; }
    /// <summary>
    ///结果来源1输入 2回传
    ///</summary>
    public int? ResultSource { get; set; }
    /// <summary>
    ///原始结果
    ///</summary>
    public string? OriginalValue { get; set; }
    /// <summary>
    ///报告显示
    ///</summary>
    public bool? IsReportShow { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public string? ReportOrder { get; set; }
    /// <summary>
    ///方法依据
    ///</summary>
    public string? MethodBasis { get; set; }
    /// <summary>
    ///cutoff值
    ///</summary>
    public string? CutOffValue { get; set; }
    /// <summary>
    ///od值
    ///</summary>
    public string? ODValue { get; set; }
    /// <summary>
    ///s/co
    ///</summary>
    public string? SpecificValue { get; set; }
    /// <summary>
    ///结果类型，定量、定性等
    ///</summary>
    public string? ResultType { get; set; }
    /// <summary>
    ///是否计算项0否1是
    ///</summary>
    public bool? IsCalculate { get; set; }
    /// <summary>
    ///通用计算公式
    ///</summary>
    public string? CalcExpression { get; set; }
    /// <summary>
    ///上机次数
    ///</summary>
    public int? InTestCount { get; set; } = 0;
    /// <summary>
    ///委托状态
    ///</summary>
    public int? EntrustStatus { get; set; }
    /// <summary>
    ///增项
    ///</summary>
    public int? AddType { get; set; }
    /// <summary>
    ///复查
    ///</summary>
    public int? ReviewStatus { get; set; } = 0;

}
