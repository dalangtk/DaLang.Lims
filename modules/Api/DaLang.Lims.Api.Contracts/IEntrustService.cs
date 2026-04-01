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
    /// <summary>
    /// 根据条码获取结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<EntrustResultDto>> GetResultByBarcode(EntrustResultQueryInpupt input);
    /// <summary>
    /// 根据时间段获取结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<EntrustResultDto>> GetResultByTime(EntrustResultQueryInpupt input);
}
