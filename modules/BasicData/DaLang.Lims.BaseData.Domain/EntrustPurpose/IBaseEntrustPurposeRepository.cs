using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Web.BaseData.Domain.EntrustPurpose;

public interface IBaseEntrustPurposeRepository : ISqlSugarRepository<BaseEntrustPurposeEntity>
{
    /// <summary>
    /// 获取委托目的
    /// </summary>
    /// <param name="purCode"></param>
    /// <param name="sampleTypeCode"></param>
    /// <param name="receiveTime"></param>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    Task<List<BaseEntrustPurposeEntity>> GetEntrustPurpose(string purCode, string sampleTypeCode, DateTime receiveTime, string customerCode);
}
