using DaLang.Lims.Agent.Contracts.Dto;
using DaLang.Lims.BaseData.Contracts.Group.Dto;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.BaseData.Domain.Group;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Web.Framework.Repositories;
using SqlSugar;

namespace DaLang.Lims.Agent.Domain;

public class SampleStatisticRepository : AdminRepositoryBase<ExamInfoEntity>, ISampleStatisticRepository
{
    public async Task<List<BaseGroupDto>> GetAllGroups()
    {
        var ret = await Context.Queryable<BaseGroupEntity>().Select(v => new BaseGroupDto
        {
            GroupCode = v.GroupCode,
            GroupName = v.GroupName,
        }).ToListAsync();
        return ret;
    }

    public async Task<List<CustomerSampleCountOutput>> GetReportCountGroupByCusomerAndTestate(CustomerSampleCountInput input)
    {
        var ret = await Context.Queryable<ExamInfoEntity>()
            .LeftJoin<BaseCustomerEntity>((v, c) => v.CustomerCode == c.CustomerCode)
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), v => v.CustomerCode == input.CustomerCode)
            .Where(v => SqlFunc.Between(v.TestDate, input.BeginTime, input.EndTime))
            .GroupBy((v,c) => new { v.CustomerCode,c.CustomerName, v.TestDate })
            .Select((v, c) => new CustomerSampleCountOutput
            {
                CustomerCode = v.CustomerCode,
                CustomerName = c.CustomerName,
                BeginTime = input.BeginTime,
                EndTime = input.EndTime,
                TestDate = v.TestDate,
                Count = SqlFunc.AggregateCount(v.Id)
            }).ToListAsync();
        return ret;
    }
}
