using DaLang.Lims.Pretreatment.Contracts.Entrust.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.Entrust;

/// <summary>
/// 委托服务
/// </summary>

public interface IEntrustService
{
    /// <summary>
    /// 计算委托信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<CalcEntrustDto>> CalcEntrustAsync(CalcEntrustInput input);
}
