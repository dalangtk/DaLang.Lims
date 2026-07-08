
namespace DaLang.Lims.Pathology.Contracts.ExamPathologyDigitalSlicing.Dto;

/// <summary>
/// 数字切片新增输入
/// </summary>
public class ExamPathologyDigitalSlicingAddInput
{
    /// <summary>
    /// 条码号
    /// </summary>
    public string? Barcode { get; set; }
    /// <summary>
    /// 病理号
    /// </summary>
    public string? SampleNo { get; set; }
    /// <summary>
    /// 切片名称
    /// </summary>
    public string? SlicingName { get; set; }
    /// <summary>
    /// 切片路径
    /// </summary>
    public string? SlicingPath { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; } = true;
}