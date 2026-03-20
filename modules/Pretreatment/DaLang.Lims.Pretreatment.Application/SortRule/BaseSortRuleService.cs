using DaLang.Lims.BaseData.Domain.Sequence;
using DaLang.Lims.Pretreatment.Contracts.BaseSortRule;
using DaLang.Lims.Pretreatment.Contracts.SortRule.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.SortRule;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Pretreatment.Services.BaseSortRule;

/// <summary>
/// 分拣规则服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class BaseSortRuleService : BaseService, IBaseSortRuleService, IDynamicApi
{
    private IBaseSortRuleRepository _baseSortRuleRep;

    public BaseSortRuleService(IBaseSortRuleRepository baseSortRuleRep)
    {
        _baseSortRuleRep = baseSortRuleRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseSortRuleDto> GetAsync(long id)
    {
        var output = await _baseSortRuleRep.GetAsync(id);
        return output.Adapt<BaseSortRuleDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<BaseSortRuleGetListDto>> GetListAsync(BaseSortRuleQueryInput input)
    {
        var list = await _baseSortRuleRep.AsQueryable()
            .LeftJoin<BaseSequenceEntity>((a, b) => a.SequenceCode == b.SequenceCode)
            .OrderByDescending(a => a.Id)
            .Select((a, b) => new BaseSortRuleGetListDto
            {
                SequenceName = b.SequenceName ?? ""
            }, true)
            .ToListAsync();
        return list;
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseSortRuleGetListDto>> GetPageAsync(PageInput<BaseSortRuleQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseSortRuleRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrWhiteSpace(filter?.SortRuleCode), c => c.SequenceCode!.Contains(filter!.SortRuleCode!) || c.RuleName!.Contains(filter.SortRuleCode!))
            .LeftJoin<BaseSequenceEntity>((a, b) => a.SequenceCode == b.SequenceCode)
            .Select((a, b) => new BaseSortRuleGetListDto
            {
                SequenceName = b.SequenceName ?? ""
            }, true)
            .OrderBy(a => a.Sort)
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseSortRuleGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseSortRuleDto input)
    {
        var aa = input.GroupName;
        input.RuleExpression = JsonHelper.Serialize(input.RuleExpressionObj);
        var entity = Mapper.Map<BaseSortRuleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseSortRuleRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseSortRuleRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseSortRuleDto input)
    {
        var entity = await _baseSortRuleRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("分拣规则不存在！");

        input.RuleExpression = JsonHelper.Serialize(input.RuleExpressionObj);
        Mapper.Map(input, entity);
        await _baseSortRuleRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseSortRuleRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}