using DaLang.Lims.BaseData.Contracts.SampleType;
using DaLang.Lims.BaseData.Contracts.SampleType.Dto;
using DaLang.Lims.BaseData.Domain.SampleType;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.BaseData.Services.SampleType;

/// <summary>
/// 标本类型服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseSampleTypeService : BaseService, IBaseSampleTypeService, IDynamicApi
{
    private IBaseSampleTypeRepository _baseSampleTypeRep;
    private ICacheTool _cacheTool;

    public BaseSampleTypeService(IBaseSampleTypeRepository baseSampleTypeRep,
         ICacheTool cacheTool)
    {
        _baseSampleTypeRep = baseSampleTypeRep;
        _cacheTool = cacheTool;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseSampleTypeDto> GetAsync(long id)
    {
        var output = await _baseSampleTypeRep.GetAsync(id);
        return output.Adapt<BaseSampleTypeDto>();
    }
    /// <summary>
    /// 获取所有
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BaseSampleTypeDto>> GetAllAsync()
    {
        var output = await _cacheTool.GetOrSetAsync(BaseDataCacheKeys.SampleType, async () =>
        {
            var list = await _baseSampleTypeRep.GetListAsync();
            return list;
        }, TimeSpan.FromHours(24));

        return output.Adapt<List<BaseSampleTypeDto>>();
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseSampleTypeGetListDto>> GetPageAsync(PageInput<BaseSampleTypeQueryInput> input)
    {
        var filter = input.Filter;
        var list = await _baseSampleTypeRep.AsQueryable()
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.SampleTypeCode),
            a => a.SampleTypeCode == filter.SampleTypeCode || a.SampleTypeName == filter.SampleTypeCode)
            .OrderBy(c => c.Sort)
            .Select<BaseSampleTypeGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseSampleTypeGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseSampleTypeDto input)
    {
        var entity = Mapper.Map<BaseSampleTypeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseSampleTypeRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseSampleTypeRep.InsertReturnSnowflakeIdAsync(entity);

        await _cacheTool.DelAsync(BaseDataCacheKeys.SampleType);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseSampleTypeDto input)
    {
        var entity = await _baseSampleTypeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("标本类型不存在！");

        Mapper.Map(input, entity);
        await _baseSampleTypeRep.UpdateAsync(entity);
        await _cacheTool.DelAsync(BaseDataCacheKeys.SampleType);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        await _cacheTool.DelAsync(BaseDataCacheKeys.SampleType);
        return await _baseSampleTypeRep
            .AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}