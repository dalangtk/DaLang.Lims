using DaLang.Lims.Api.Contracts.Dto;

namespace DaLang.Lims.Api.Contracts;

public interface IEntrustService
{
    /// <summary>
    /// 推送数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> PushEntrustData(List<PushDataInput> input);
}
