namespace DaLang.Lims.Shared.Contracts.ExamSpecialResult.Dto;

/// <summary>
/// 特检结果新增输入
/// </summary>
public class ExamSpecialResultAddInput
{
    /// <summary>
    ///
    ///</summary>
    public long ExamInfoId { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
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
    public DateTime TestDate { get; set; }
    /// <summary>
    ///字段
    ///</summary>
    public string FieldCode { get; set; }
    /// <summary>
    ///字段名
    ///</summary>
    public string? FieldName { get; set; }
    /// <summary>
    ///字段值
    ///</summary>
    public string? FieldValue { get; set; }
    /// <summary>
    /// 结果类型 1初诊 2复诊
    /// </summary>
    public int ResultType { get; set; }
}
