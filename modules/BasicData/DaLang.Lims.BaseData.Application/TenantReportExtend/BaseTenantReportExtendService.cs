using DaLang.Lims.BaseData.Contracts.TenantReportExtend;
using DaLang.Lims.BaseData.Contracts.TenantReportExtend.Dto;
using DaLang.Lims.BaseData.Domain.TenantReportExtend;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.BaseData.Application.TenantReportExtend;

/// <summary>
/// 机构报告设置服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseTenantReportExtendService : BaseService, IBaseTenantReportExtendService, IDynamicApi
{
    private AdminRepositoryBase<BaseTenantReportExtendEntity> _baseTenantReportExtendRep;

    public BaseTenantReportExtendService(AdminRepositoryBase<BaseTenantReportExtendEntity> baseTenantReportExtendRep)
    {
        _baseTenantReportExtendRep = baseTenantReportExtendRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseTenantReportExtendDto> GetAsync(long id)
    {
        var output = await _baseTenantReportExtendRep.GetAsync(id);
        return output.Adapt<BaseTenantReportExtendDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseTenantReportExtendDto>> GetPageAsync(PageInput<BaseTenantReportExtendQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseTenantReportExtendRep.GetQueryable(dynamicCondition)
            .Select<BaseTenantReportExtendDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseTenantReportExtendDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseTenantReportExtendDto input)
    {
        var entity = Mapper.Map<BaseTenantReportExtendEntity>(input);
        var id = await _baseTenantReportExtendRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseTenantReportExtendDto input)
    {
        var entity = await _baseTenantReportExtendRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("机构设置不存在！");

        Mapper.Map(input, entity);
        await _baseTenantReportExtendRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseTenantReportExtendRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}
