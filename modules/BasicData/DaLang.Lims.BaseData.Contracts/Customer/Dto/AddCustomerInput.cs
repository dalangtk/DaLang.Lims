namespace DaLang.Lims.BaseData.Contracts.Customer.Dto;

public class AddCustomerInput
{
    public BaseCustomerAddInput Customer { get; set; }
    public BaseCustomerReportExtendAddInput ReportExtend { get; set; }
}

public class UpdateCustomerInput
{
    public BaseCustomerUpdateInput Customer { get; set; }
    public BaseCustomerReportExtendUpdateInput ReportExtend { get; set; }
}
