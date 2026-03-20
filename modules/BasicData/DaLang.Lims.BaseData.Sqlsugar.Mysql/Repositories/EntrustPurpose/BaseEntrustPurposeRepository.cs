using DaLang.Lims.Web.BaseData.Domain.EntrustPurpose;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.EntrustPurpose;

public class BaseEntrustPurposeRepository : AdminRepositoryBase<BaseEntrustPurposeEntity>, IBaseEntrustPurposeRepository
{
    public BaseEntrustPurposeRepository()
    {
    }

    /// <summary>
    /// 获取委托目的
    /// </summary>
    /// <param name="purCode"></param>
    /// <param name="sampleTypeCode"></param>
    /// <param name="receiveTime"></param>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    public async Task<List<BaseEntrustPurposeEntity>> GetEntrustPurpose(string purCode, string sampleTypeCode, DateTime receiveTime, string customerCode)
    {
        string sql = @"select t.*
                        from base_entrust_purpose t
                        where t.PurCode = @purCode
                          And t.BeginTime <= @receiveTime
                          And t.EndTime >= @receiveTime
                          And (InStr(t.CustomerCode, @customerCode) > 0 Or
                               t.CustomerCode Is Null Or
                               (InStr(t.CustomerCode, @customerCode) <= 0 And
                                t.IsCustomerReverse = 1))
                          And (InStr(t.SampleTypeCode, @sampleTypeCode) > 0 Or
                               t.SampleTypeCode Is Null)
                          And t.IsValid = 1
                          And t.IsDeleted = 0
                        Order By case t.CustomerCode when null then '0' else '1' end Desc,
                                 t.IsCustomerReverse Desc,
                                 case t.Purcode when Null then '0' else '1' end Desc,
                                 case t.SampleTypeCode when Null then '0' else '1' end Desc,
                                 t.Sort Asc";
        return await Context.Ado.SqlQueryAsync<BaseEntrustPurposeEntity>(sql,
            new
            {
                purCode,
                sampleTypeCode,
                receiveTime,
                customerCode
            }
        );
    }
}
