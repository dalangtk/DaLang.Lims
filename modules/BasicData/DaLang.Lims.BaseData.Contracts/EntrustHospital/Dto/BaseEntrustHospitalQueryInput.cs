namespace DaLang.Lims.BaseData.Contracts.EntrustHospital.Dto;

/// <summary>
/// 委托医院分页查询条件输入
/// </summary>
public partial class BaseEntrustHospitalQueryInput
{

    /// <summary>
    /// 委托医院代码
    /// </summary>       
    public string? EntrustHospitalCode { get; set; }
}
