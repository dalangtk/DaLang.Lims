using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Parameter;
using DaLang.Lims.Web.Framework.Services.Parameter.Dto;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace DaLang.Lims.Web.Framework.Services.Parameter;

/// <summary>
/// 系统参数服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class ParameterService : BaseService, IParameterService, IDynamicApi
{
    private IParameterRepository _parameterRep;
    private ICacheTool _cacheTool;
    public ParameterService(IParameterRepository parameterRep,
        ICacheTool cacheTool)
    {
        _parameterRep = parameterRep;
        _cacheTool = cacheTool;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ParameterDto> GetAsync(long id)
    {
        var output = await _parameterRep.GetAsync(id);
        return output.Adapt<ParameterDto>();
    }
    /// <summary>
    /// 根据参数名称查询
    /// </summary>
    /// <param name="paramName"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ParameterDto> GetParamByName(string paramName)
    {
        var output = await _parameterRep.GetFirstAsync(a => a.ParamName == paramName);
        return output.Adapt<ParameterDto>();
    }

    /// <summary>
    /// 查询参数值 sys_param:
    /// </summary>
    /// <param name="paramName"></param>
    /// <param name="defaultValue"></param>
    /// <param name="expireTime"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<string> GetParamValue(string paramName, string defaultValue = "", TimeSpan? expireTime = null)
    {
        var keyName = CacheKeys.SystemParam + paramName;
        var paramValue = await _cacheTool.GetOrSetAsync(keyName, async () =>
        {
            var param = await _parameterRep.GetFirstAsync(a => a.ParamName == paramName);
            if (string.IsNullOrWhiteSpace(param?.ParamValue))
                return defaultValue;
            return param?.ParamValue;
        }, expireTime);

        return string.IsNullOrWhiteSpace(paramValue) ? defaultValue : paramValue;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ParameterGetListDto>> GetPageAsync(PageInput<ParameterQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _parameterRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<ParameterGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<ParameterGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ParameterDto input)
    {
        var entity = Mapper.Map<ParameterEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _parameterRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _parameterRep.InsertReturnSnowflakeIdAsync(entity);
        var keyName = CacheKeys.SystemParam + input.ParamName;
        _cacheTool.Set(keyName, input.ParamValue);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ParameterDto input)
    {
        var entity = await _parameterRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("系统参数不存在！");

        Mapper.Map(input, entity);
        await _parameterRep.UpdateAsync(entity);
        var keyName = CacheKeys.SystemParam + entity.ParamName;
        await _cacheTool.SetAsync(keyName, entity.ParamValue);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _parameterRep.GetAsync(id);
        if (entity == null)
            throw ResultOutput.Exception("系统参数不存在！");

        var keyName = CacheKeys.SystemParam + entity.ParamName;
        await _cacheTool.DelAsync(keyName);

        var ret = await _parameterRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
        return ret;
    }
}