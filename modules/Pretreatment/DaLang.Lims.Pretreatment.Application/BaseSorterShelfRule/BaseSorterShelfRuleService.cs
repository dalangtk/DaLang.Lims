using DaLang.Lims.BaseData.Contracts.BaseSorterShelfRule;
using DaLang.Lims.BaseData.Domain.BaseSorterShelfRule;
using DaLang.Lims.BaseData.Domain.Sequence;
using DaLang.Lims.Pretreatment.Contracts.BaseSorterShelfRule.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.SortRule;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;


namespace DaLang.Lims.BaseData.Services.BaseSorterShelfRule;

/// <summary>
/// 架子规则服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class BaseSorterShelfRuleService : BaseService, IBaseSorterShelfRuleService, IDynamicApi
{
    private IBaseSorterShelfRuleRepository _baseSorterShelfRuleRep;

    public BaseSorterShelfRuleService(IBaseSorterShelfRuleRepository baseSorterShelfRuleRep)
    {
        _baseSorterShelfRuleRep = baseSorterShelfRuleRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseSorterShelfRuleDto> GetAsync(long id)
    {
        var output = await _baseSorterShelfRuleRep.GetAsync(id);
        return output.Adapt<BaseSorterShelfRuleDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<BaseSorterShelfRuleDto>> GetListAsync(BaseSorterShelfRuleQueryInput input)
    {
        if (input == null || string.IsNullOrWhiteSpace(input?.SorterCode))
            throw ResultOutput.Exception("参数有误！");

        var list = await _baseSorterShelfRuleRep.AsQueryable()
            .LeftJoin<BaseSortRuleEntity>((a, b) => a.SortRuleCode == b.RuleCode && b.IsValid && !b.IsDeleted)
            .LeftJoin<BaseSequenceEntity>((a, b, c) => b.SequenceCode == c.SequenceCode && c.IsValid && !c.IsDeleted)
            //.WhereIF(input.ShelfPosition != null, a => a.SorterCode == input.SorterCode && a.ShelfPosition == input.ShelfPosition && a.IsDeleted == false)
            .Where(a => a.IsDeleted == false)
            .WhereIF(!string.IsNullOrWhiteSpace(input.SorterCode), a => a.SorterCode == input.SorterCode)
            .WhereIF(input.ShelfPosition != null, a => a.ShelfPosition == input.ShelfPosition)
            .OrderBy(a => a.Sort)
            .Select((a, b, c) => new BaseSorterShelfRuleDto
            {
                SortRuleName = b.RuleName,
                SequenceCode = c.SequenceCode,
                SequenceName = c.SequenceName
            }, true)
            .ToListAsync();
        return list;
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseSorterShelfRuleDto input)
    {
        var entity = Mapper.Map<BaseSorterShelfRuleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseSorterShelfRuleRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseSorterShelfRuleRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseSorterShelfRuleDto input)
    {
        var entity = await _baseSorterShelfRuleRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("架子规则不存在！");

        Mapper.Map(input, entity);
        await _baseSorterShelfRuleRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseSorterShelfRuleRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}