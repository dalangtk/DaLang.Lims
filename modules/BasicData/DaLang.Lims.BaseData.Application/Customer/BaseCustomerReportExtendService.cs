using DaLang.Lims.BaseData.Contracts.Customer;
using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.BaseData.Services.Customer;

/// <summary>
/// 客户报告扩展服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseCustomerReportExtendService : BaseService, IBaseCustomerReportExtendService, IDynamicApi
{
    private IBaseCustomerReportExtendRepository _baseCustomerReportExtendRep;

    public BaseCustomerReportExtendService(IBaseCustomerReportExtendRepository baseCustomerReportExtendRep)
    {
        _baseCustomerReportExtendRep = baseCustomerReportExtendRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseCustomerReportExtendDto> GetAsync(long id)
    {
        var output = await _baseCustomerReportExtendRep.GetAsync(id);
        return output.Adapt<BaseCustomerReportExtendDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseCustomerReportExtendDto>> GetPageAsync(PageInput<BaseCustomerReportExtendQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseCustomerReportExtendRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.CustomerCode)
            .Select<BaseCustomerReportExtendDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseCustomerReportExtendDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseCustomerReportExtendDto input)
    {
        var entity = Mapper.Map<BaseCustomerReportExtendEntity>(input);
        var id = await _baseCustomerReportExtendRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseCustomerReportExtendDto input)
    {
        var entity = await _baseCustomerReportExtendRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("客户报告扩展不存在！");

        Mapper.Map(input, entity);
        await _baseCustomerReportExtendRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseCustomerReportExtendRep.SetColumnUpdateable(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
    }
}