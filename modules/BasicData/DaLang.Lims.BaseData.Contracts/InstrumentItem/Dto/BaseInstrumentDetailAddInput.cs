namespace DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;

public class BaseInstrumentDetailAddInput
{
    public string InstrumentItemCode { get; set; }
    public string ItemCode { get; set; }
    public bool IsValid { get; set; }
}
