namespace DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch.Dto;

public class PurposeMatchQueryInput
{
    public string? CustomerCode { get; set; }
}
public class PurposeMatchSingleQueryInput : PurposeMatchQueryInput
{
    public string? CustomerPurCode { get; set; }
}
public class PurposeMatchMutilQueryInput : PurposeMatchQueryInput
{
    public List<string> CustomerPurCodes { get; set; }
}