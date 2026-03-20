namespace DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;

public class InstrumentItemInfoDto
{
    public string InstrumentItemCode { get; set; }
    public string InstrumentItemName { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string ItemEN { get; set; }
    public string ItemAB { get; set; }
    public string ItemUnit { get; set; }
    public string MethodCode { get; set; }
    public string MethodName { get; set; }
    public bool IsReportShow { get; set; }
    public string DefaultValue { get; set; }
    public string ReportOrder { get; set; }
    public string ItemReportOrder { get; set; }
    public string MethodBasis { get; set; }
    public string ResultType { get; set; }
    public bool IsCalculate { get; set; }
    public string CalcExpression { get; set; }
}
