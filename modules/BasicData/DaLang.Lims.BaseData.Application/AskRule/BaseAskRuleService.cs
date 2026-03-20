using DaLang.Lims.BaseData.Contracts.AskRule;
using DaLang.Lims.BaseData.Contracts.AskRule.Dto;
using DaLang.Lims.BaseData.Domain.BaseAskRule;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.BaseAskRule;

/// <summary>
/// 问询规则服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseAskRuleService : BaseService, IBaseAskRuleService, IDynamicApi
{
    private IBaseAskRuleRepository _baseAskRuleRep;

    public BaseAskRuleService(IBaseAskRuleRepository baseAskRuleRep)
    {
        _baseAskRuleRep = baseAskRuleRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseAskRuleDto> GetAsync(long id)
    {
        var output = await _baseAskRuleRep.GetAsync(id);
        return output.Adapt<BaseAskRuleDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseAskRuleDto>> GetPageAsync(PageInput<BaseAskRuleQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseAskRuleRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseAskRuleDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseAskRuleDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseAskRuleAddInput input)
    {
        var entity = Mapper.Map<BaseAskRuleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseAskRuleRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseAskRuleRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseAskRuleUpdateInput input)
    {
        var entity = await _baseAskRuleRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("问询规则不存在！");

        Mapper.Map(input, entity);
        await _baseAskRuleRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseAskRuleRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}