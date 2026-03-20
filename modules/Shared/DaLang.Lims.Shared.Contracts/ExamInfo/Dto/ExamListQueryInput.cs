using DaLang.Lims.Web.Common.Enums;

namespace DaLang.Lims.Shared.Contracts.ExamInfo.Dto;

public class ExamListQueryInput
{
    public string? GroupCode { get; set; }
    public SampleStatusEnum? SampleStatus { get; set; }
    public string? Barcode { get; set; }
    public string? PatientName { get; set; }
    public DateTime? BeginDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? ExamInfoId { get; set; }
}
