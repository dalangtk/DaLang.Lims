using DaLang.Lims.BaseData.Contracts.BasePurpose;
using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.BaseData.Services.BasePurpose;

/// <summary>
/// 目的机构设置服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BasePurposeTenantSettingService : BaseService, IBasePurposeTenantSettingService, IDynamicApi
{
    private IBasePurposeTenantSettingRepository _basePurposeTenantSettingRep;

    public BasePurposeTenantSettingService(IBasePurposeTenantSettingRepository basePurposeTenantSettingRep)
    {
        _basePurposeTenantSettingRep = basePurposeTenantSettingRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePurposeTenantSettingDto> GetAsync(long id)
    {
        var output = await _basePurposeTenantSettingRep.GetAsync(id);
        return output.Adapt<BasePurposeTenantSettingDto>();
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BasePurposeTenantSettingDto input)
    {
        var entity = Mapper.Map<BasePurposeTenantSettingEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePurposeTenantSettingRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePurposeTenantSettingRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BasePurposeTenantSettingDto input)
    {
        var entity = await _basePurposeTenantSettingRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("基础数据不存在！");

        Mapper.Map(input, entity);
        await _basePurposeTenantSettingRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePurposeTenantSettingRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}