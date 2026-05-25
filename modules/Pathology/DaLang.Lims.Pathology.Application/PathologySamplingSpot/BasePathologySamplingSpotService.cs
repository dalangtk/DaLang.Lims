using DaLang.Lims.Pathology.Contracts.PathologySamplingSpot;
using DaLang.Lims.Pathology.Contracts.PathologySamplingSpot.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.PathologySamplingSpot;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Pathology.Application.PathologySamplingSpot;

/// <summary>
/// 取材部位服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class BasePathologySamplingSpotService : BaseService, IBasePathologySamplingSpotService, IDynamicApi
{
    private IBasePathologySamplingSpotRepository _basePathologySamplingSpotRep;

    public BasePathologySamplingSpotService(IBasePathologySamplingSpotRepository basePathologySamplingSpotRep)
    {
        _basePathologySamplingSpotRep = basePathologySamplingSpotRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SamplingSpotDto> GetAsync(long id)
    {
        var output = await _basePathologySamplingSpotRep.GetAsync(id);
        return output.Adapt<SamplingSpotDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<SamplingSpotDto>> GetPageAsync(PageInput<SamplingSpotQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePathologySamplingSpotRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrEmpty(filter.SamplingSpotName), a => a.SamplingSpotName == filter.SamplingSpotName)
            .OrderBy(a => a.Sort)
            .Select<SamplingSpotDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<SamplingSpotDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(SamplingSpotAddInput input)
    {
        var entity = Mapper.Map<BasePathologySamplingSpotEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePathologySamplingSpotRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePathologySamplingSpotRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(SamplingSpotUpdateInput input)
    {
        var entity = await _basePathologySamplingSpotRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("取材部位不存在！");

        Mapper.Map(input, entity);
        await _basePathologySamplingSpotRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePathologySamplingSpotRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}