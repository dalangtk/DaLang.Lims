using DaLang.Lims.BaseData.Contracts.AskRule.Dto;
using DaLang.Lims.BaseData.Contracts.ExamPlan.Dto;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.BaseData.Domain.BaseAskRuleDetail;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Web.BaseData.Services.BaseAskRuleDetail;

/// <summary>
/// 问询规则明细服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseAskRuleDetailService : BaseService, IBaseAskRuleDetailService, IDynamicApi
{
    private IBaseAskRuleDetailRepository _baseAskRuleDetailRep;

    public BaseAskRuleDetailService(IBaseAskRuleDetailRepository baseAskRuleDetailRep)
    {
        _baseAskRuleDetailRep = baseAskRuleDetailRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseAskRuleDetailDto> GetAsync(long id)
    {
        var output = await _baseAskRuleDetailRep.GetAsync(id);
        return output.Adapt<BaseAskRuleDetailDto>();
    }

    /// <summary>
    /// 根据代码查询明细
    /// </summary>
    /// <param name="askRuleCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BaseAskRuleDetailDto>> GetAllAsync(string askRuleCode)
    {
        var list = await _baseAskRuleDetailRep.GetListAsync(a => a.AskRuleCode == askRuleCode);
        return list.Adapt<List<BaseAskRuleDetailDto>>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseAskRuleDetailDto>> GetPageAsync(PageInput<BaseAskRuleDetailQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseAskRuleDetailRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseAskRuleDetailDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseAskRuleDetailDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseAskRuleDetailAddInput input)
    {
        var entity = Mapper.Map<BaseAskRuleDetailEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseAskRuleDetailRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseAskRuleDetailRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseAskRuleDetailUpdateInput input)
    {
        var entity = await _baseAskRuleDetailRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("问询规则明细不存在！");

        Mapper.Map(input, entity);
        await _baseAskRuleDetailRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseAskRuleDetailRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}