using DaLang.Lims.Pretreatment.Core.Enum;
using DaLang.Lims.Shared.Contracts.ApplyInfo.Dto;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.ExamTask.Dto;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

#pragma warning disable CS1591,CS8618

/// <summary>
/// 分拣输入
/// </summary>
public class SortingInput
{
    public string SortInfoCode { get; set; }
    public string Barcode { get; set; }
    public string? SampleTypeCode { get; set; }
    public BarcodeTypeEnum BarcodeType { get; set; } = BarcodeTypeEnum.Barcode;
}
/// <summary>
/// 分拣输出
/// </summary>
public class SortingOutput
{
    public string Barcode { get; set; }
    /// <summary>
    /// 1选择目的检测计划 2选择目的标本类型  3选择分拣标本类型  4分血  5分拣成功
    /// </summary>
    public SortingStatusEnum Status { get; set; }
    public List<SelectPurSampleTypeDto> SelectPurSampleTypeList { get; set; }
    public List<CodeNameDto> SampleTypeList { get; set; }
    public string ShelfName { get; set; }
    public int? HolePosition { get; set; }
    public ApplyInfoDto ApplyInfo { get; set; }
    public List<ApplyPurposeDto> ApplyPurposeList { get; set; }
    public List<ExamTaskDto> ExamTaskList { get; set; }
    public string Message { get; set; }
}
public class SelectPurSampleTypeDto
{
    public ApplyPurposeDto Purpose { get; set; }
    public List<CodeNameDto> SampleTypeList { get; set; }
}
#pragma warning restore CS1591, CS8618