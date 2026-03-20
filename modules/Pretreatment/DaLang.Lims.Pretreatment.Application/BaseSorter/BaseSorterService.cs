using DaLang.Lims.BaseData.Domain.BaseSorterShelfRule;
using DaLang.Lims.Pretreatment.Contracts.BaseSorter;
using DaLang.Lims.Pretreatment.Contracts.BaseSorter.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.BaseSorter;
using DaLang.Lims.Pretreatment.Domain.BaseSorterShelf;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Enums;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pretreatment.Services.BaseSorter;

/// <summary>
/// 分拣仪器服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class BaseSorterService : BaseService, IBaseSorterService, IDynamicApi
{
    private IBaseSorterRepository _baseSorterRep;
    private IBaseSorterShelfRepository _baseSorterShelfRep;
    private IBaseSorterShelfRuleRepository _baseSorterShelfRuleRep;

    public BaseSorterService(IBaseSorterRepository baseSorterRep,
        IBaseSorterShelfRepository baseSorterShelfRep,
        IBaseSorterShelfRuleRepository baseSorterShelfRuleRep)
    {
        _baseSorterRep = baseSorterRep;
        _baseSorterShelfRep = baseSorterShelfRep;
        _baseSorterShelfRuleRep = baseSorterShelfRuleRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseSorterDto> GetAsync(long id)
    {
        var output = await _baseSorterRep.GetAsync(id);
        return output.Adapt<BaseSorterDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseSorterGetListDto>> GetPageAsync(PageInput<BaseSorterQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseSorterRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseSorterGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseSorterGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<long> AddAsync(SorterAndDetailAddInput input)
    {
        var entity = Mapper.Map<BaseSorterEntity>(input.SorterInfo);
        if (string.IsNullOrWhiteSpace(entity.SorterCode))
            throw ResultOutput.Exception("代码不能为空");
        if (entity.Sort == 0)
        {
            var sort = await _baseSorterRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseSorterRep.InsertReturnSnowflakeIdAsync(entity);

        var sorterShelf = Mapper.Map<List<BaseSorterShelfEntity>>(input.SorterShelfList);
        if (sorterShelf.Exists(v => string.IsNullOrWhiteSpace(v.SorterCode)))
            sorterShelf.ForEach(v => v.SorterCode = entity.SorterCode);
        _baseSorterShelfRep.InsertRange(sorterShelf);

        var sorterShelfRule = Mapper.Map<List<BaseSorterShelfRuleEntity>>(input.SorterShelfRuleList);
        if (sorterShelfRule.Exists(v => string.IsNullOrWhiteSpace(v.SorterCode)))
            sorterShelfRule.ForEach(v => v.SorterCode = entity.SorterCode);
        _baseSorterShelfRuleRep.InsertRange(sorterShelfRule);

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [AdminTransaction]
    public async Task UpdateAsync(SorterAndDetailUpdateInput input)
    {
        var sortInfo = input.SorterInfo;
        var sortShelf = input.SorterShelfList;
        var sortShelfRule = input.SorterShelfRuleList;

        var entity = await _baseSorterRep.GetAsync(sortInfo.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("分拣仪器不存在！");

        Mapper.Map(input, entity);
        await _baseSorterRep.UpdateAsync(entity);

        #region SortShelf
        var modifiedShelf = sortShelf.FindAll(v => v.RowStatus == RowStatusEnum.Modified);
        if (modifiedShelf.Any())
        {
            var shelfIds = modifiedShelf.Select(v => v.Id).Distinct();
            var shelfList = await _baseSorterShelfRep.GetListAsync(v => shelfIds.Contains(v.Id));

            foreach (var item in shelfIds)
            {
                var modified = modifiedShelf.FirstOrDefault(v => v.Id == item);
                var original = shelfList.FirstOrDefault(v => v.Id == item);
                if (modified != null && original != null)
                    Mapper.Map(modified, original);
            }
            await _baseSorterShelfRep.UpdateRangeAsync(shelfList);
        }

        var deleteShelf = sortShelf.FindAll(v => v.RowStatus == RowStatusEnum.Deleted);
        if (deleteShelf.Any())
        {
            var shelfIds = deleteShelf.Select(v => v.Id).Distinct();
            await _baseSorterShelfRep.SetColumnUpdateable(v => v.IsDeleted == true)
                .Where(v => shelfIds.Contains(v.Id))
                .ExecuteCommandAsync();
        }

        var addShelf = sortShelf.FindAll(v => v.RowStatus == RowStatusEnum.Added);
        if (addShelf.Any())
        {
            var addInputList = addShelf.Adapt<List<BaseSorterShelfEntity>>();
            await _baseSorterShelfRep.InsertRangeAsync(addInputList);
        }
        #endregion

        #region SortShelfRule
        var modifiedShelfRule = sortShelfRule.FindAll(v => v.RowStatus == RowStatusEnum.Modified);
        if (modifiedShelfRule.Any())
        {
            var shelfRuleIds = modifiedShelfRule.Select(v => v.Id).Distinct();
            var shelfRuleList = await _baseSorterShelfRuleRep.GetListAsync(v => shelfRuleIds.Contains(v.Id));

            foreach (var item in shelfRuleIds)
            {
                var modified = modifiedShelfRule.FirstOrDefault(v => v.Id == item);
                var original = shelfRuleList.FirstOrDefault(v => v.Id == item);
                if (modified != null && original != null)
                    Mapper.Map(modified, original);
            }
            await _baseSorterShelfRuleRep.UpdateRangeAsync(shelfRuleList);
        }

        var deleteShelfRule = sortShelfRule.FindAll(v => v.RowStatus == RowStatusEnum.Deleted);
        if (deleteShelfRule.Any())
        {
            var shelfRuleIds = deleteShelfRule.Select(v => v.Id).Distinct();
            await _baseSorterShelfRuleRep.SetColumnUpdateable(v => v.IsDeleted == true)
                .Where(v => shelfRuleIds.Contains(v.Id))
                .ExecuteCommandAsync();
        }

        var addShelfRule = sortShelfRule.FindAll(v => v.RowStatus == RowStatusEnum.Added);
        if (addShelfRule.Any())
        {
            var addInputList = addShelfRule.Adapt<List<BaseSorterShelfRuleEntity>>();
            await _baseSorterShelfRuleRep.InsertRangeAsync(addInputList);
        }
        #endregion
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [AdminTransaction]
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _baseSorterRep.GetAsync(id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("分拣仪器不存在！");

        var sorterCode = entity.SorterCode;
        await _baseSorterShelfRep
                .SetColumnUpdateable(v => v.IsDeleted == true)
                .Where(v => v.SorterCode == sorterCode)
                .ExecuteCommandAsync();
        await _baseSorterShelfRuleRep
                .SetColumnUpdateable(v => v.IsDeleted == true)
                .Where(v => v.SorterCode == sorterCode)
                .ExecuteCommandAsync();

        return await _baseSorterRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 获取所有分拣仪
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    public async Task<List<BaseSorterDto>> GetAll()
    {
        var list = await _baseSorterRep.GetListAsync();
        var ret = list.Adapt<List<BaseSorterDto>>();
        return ret;
    }
}