namespace DaLang.Lims.BaseData.Contracts.Customer.Dto;

/// <summary>
/// 客户报告扩展更新数据输入
/// </summary>
public class BaseCustomerReportExtendUpdateInput : BaseCustomerReportExtendAddInput
{
    public long Id { get; set; }
}
