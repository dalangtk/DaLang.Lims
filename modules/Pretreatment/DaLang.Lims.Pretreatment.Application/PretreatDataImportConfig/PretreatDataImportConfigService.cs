using DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig;
using DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.PretreatDataImportConfig;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pretreatment.Services.PretreatDataImportConfig;

/// <summary>
/// 导入配置服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class PretreatDataImportConfigService : BaseService, IPretreatDataImportConfigService, IDynamicApi
{
    private IPretreatDataImportConfigRepository _pretreatDataImportConfigRep;

    public PretreatDataImportConfigService(IPretreatDataImportConfigRepository pretreatDataImportConfigRep)
    {
        _pretreatDataImportConfigRep = pretreatDataImportConfigRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PretreatDataImportConfigDto> GetAsync(long id)
    {
        var output = await _pretreatDataImportConfigRep.GetAsync(id);
        return output.Adapt<PretreatDataImportConfigDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PretreatDataImportConfigGetListDto>> GetPageAsync(PageInput<PretreatDataImportConfigQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _pretreatDataImportConfigRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Id)
            .Select<PretreatDataImportConfigGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<PretreatDataImportConfigGetListDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(PretreatDataImportConfigDto input)
    {
        var entity = Mapper.Map<PretreatDataImportConfigEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _pretreatDataImportConfigRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _pretreatDataImportConfigRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(PretreatDataImportConfigDto input)
    {
        var entity = await _pretreatDataImportConfigRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("导入配置不存在！");

        Mapper.Map(input, entity);
        await _pretreatDataImportConfigRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _pretreatDataImportConfigRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取通用导入配置
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<PretreatDataImportConfigGetListDto>> GetGeneralImportConfig()
    {
        var generalConfigs = await _pretreatDataImportConfigRep.GetListAsync(a => string.IsNullOrWhiteSpace(a.CustomerCode));
        if (!generalConfigs.Any())
            throw ResultOutput.Exception("未配置通用导入设置！");

        return generalConfigs.Adapt<List<PretreatDataImportConfigGetListDto>>();
    }
}