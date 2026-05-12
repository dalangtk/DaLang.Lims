using DaLang.Lims.Pathology.Contracts.PathologySetting;
using DaLang.Lims.Pathology.Contracts.PathologySetting.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.PathologySetting;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.Pathology.Application.PathologySetting;

/// <summary>
/// 病理配置服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class BasePathologySettingService : BaseService, IBasePathologySettingService, IDynamicApi
{
    private IBasePathologySettingRepository _basePathologySettingRep;
    private ICacheTool _cacheTool;

    public BasePathologySettingService(IBasePathologySettingRepository basePathologySettingRep)
    {
        _basePathologySettingRep = basePathologySettingRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PathologySettingDto> GetAsync(long id)
    {
        var output = await _basePathologySettingRep.GetAsync(id);
        return output.Adapt<PathologySettingDto>();
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PathologySettingDto>> GetPageAsync(PageInput<PathologySettingQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePathologySettingRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<PathologySettingDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<PathologySettingDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(PathologySettingAddInput input)
    {
        var entity = Mapper.Map<BasePathologySettingEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePathologySettingRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePathologySettingRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(PathologySettingUpdateInput input)
    {
        var entity = await _basePathologySettingRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("病理配置不存在！");

        Mapper.Map(input, entity);
        await _basePathologySettingRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePathologySettingRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 根据工作流编码查询病理配置
    /// </summary>
    /// <param name="wfCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PathologySettingDto> GetSettingByWfCode(string wfCode)
    {
        var keyName = PathologyCacheKeys.PathologySettingKey + wfCode;
        var ret = await _cacheTool.GetOrSetAsync(keyName, async () =>
        {
            var setting = await _basePathologySettingRep.GetFirstAsync(v => v.WFCode == wfCode);
            return setting;
        }, TimeSpan.FromMinutes(240));

        return ret.Adapt<PathologySettingDto>();
    }
}
