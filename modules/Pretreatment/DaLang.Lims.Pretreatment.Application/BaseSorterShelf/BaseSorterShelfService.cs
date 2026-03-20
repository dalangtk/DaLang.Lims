using DaLang.Lims.Pretreatment.Contracts.BaseSorterShelf;
using DaLang.Lims.Pretreatment.Contracts.BaseSorterShelf.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.BaseSorterShelf;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;


namespace DaLang.Lims.Pretreatment.Services.BaseSorterShelf;

/// <summary>
/// 分拣架子服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class BaseSorterShelfService : BaseService, IBaseSorterShelfService, IDynamicApi
{
    private IBaseSorterShelfRepository _baseSorterShelfRep;

    public BaseSorterShelfService(IBaseSorterShelfRepository baseSorterShelfRep)
    {
        _baseSorterShelfRep = baseSorterShelfRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseSorterShelfDto> GetAsync(long id)
    {
        var output = await _baseSorterShelfRep.GetAsync(id);
        return output.Adapt<BaseSorterShelfDto>();
    }

    /// <summary>
    /// 查询分拣仪明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseSorterShelfGetListDto>> GetSorterDetailAsync(BaseSorterShelfQueryInput input)
    {
        var list = await _baseSorterShelfRep.AsQueryable()
            .Where(a => a.SorterCode == input.SorterCode)
            .Select<BaseSorterShelfGetListDto>()
            .OrderBy(a => a.ShelfPosition)
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseSorterShelfDto input)
    {
        var entity = Mapper.Map<BaseSorterShelfEntity>(input);
        var id = await _baseSorterShelfRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseSorterShelfDto input)
    {
        var entity = await _baseSorterShelfRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("分拣架子不存在！");

        Mapper.Map(input, entity);
        await _baseSorterShelfRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseSorterShelfRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}