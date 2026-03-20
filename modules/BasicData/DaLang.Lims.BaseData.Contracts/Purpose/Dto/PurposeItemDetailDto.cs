namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

/// <summary>
/// 目的-上机项目-项目
/// </summary>
public class PurposeWithDetailListDto
{
    public int RowNum { get; set; }
    public string GroupCode { get; set; }
    public string GroupName { get; set; }
    public string PurCode { get; set; }
    public string PurName { get; set; }
    public string PurNamePersonalize { get; set; }
    public string ExamPlan { get; set; }
    public string SampleTypeCode { get; set; }
    public string SampleTypeName { get; set; }
    public string InstrumentItemCode { get; set; }
    public string InstrumentItemName { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string ItemNamePersonalize { get; set; }
}
