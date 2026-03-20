using DaLang.Lims.BaseData.Contracts.Purpose;
using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.BaseData.Domain.SampleType;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.Purpose;

/// <summary>
/// 目的定制服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BasePurposePersonalizeService : BaseService, IBasePurposePersonalizeService, IDynamicApi
{
    private IBasePurposePersonalizeRepository _basePurposePersonalizeRep;

    public BasePurposePersonalizeService(IBasePurposePersonalizeRepository basePurposePersonalizeRep)
    {
        _basePurposePersonalizeRep = basePurposePersonalizeRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePurposePersonalizeDto> GetAsync(long id)
    {
        var output = await _basePurposePersonalizeRep.GetAsync(id);
        return output.Adapt<BasePurposePersonalizeDto>();
    }

    /// <summary>
    /// 根据目的代码查询定制
    /// </summary>
    /// <param name="purcode"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<BasePurposePersonalizeDto>> GetPurposePersonalizeList(string purcode)
    {
        if (string.IsNullOrWhiteSpace(purcode))
            throw ResultOutput.Exception("参数有误");
        var ret = await _basePurposePersonalizeRep.AsQueryable()
                .Where(a => a.PurCode == purcode)
               .Select(a => new BasePurposePersonalizeEntity
               {
                   SampleTypeName = string.IsNullOrWhiteSpace(a.SampleTypeCode) ? "" :
                   SqlFunc.Subqueryable<BaseSampleTypeEntity>()
                   .Where(z => SqlFunc.SplitIn(a.SampleTypeCode, z.SampleTypeCode))
                   .SelectStringJoin(z => z.SampleTypeName, ",")
               }, true).ToListAsync();

        return ret.Adapt<List<BasePurposePersonalizeDto>>();
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BasePurposePersonalizeDto input)
    {
        var entity = Mapper.Map<BasePurposePersonalizeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePurposePersonalizeRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePurposePersonalizeRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BasePurposePersonalizeDto input)
    {
        var entity = await _basePurposePersonalizeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("目的定制不存在！");

        Mapper.Map(input, entity);
        await _basePurposePersonalizeRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePurposePersonalizeRep
            .AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}