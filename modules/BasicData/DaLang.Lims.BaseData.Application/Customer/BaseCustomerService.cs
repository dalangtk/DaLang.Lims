using DaLang.Lims.BaseData.Contracts.Customer;
using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Dict;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.BaseData.Services.Customer;

/// <summary>
/// 客户服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseCustomerService : BaseService, IBaseCustomerService, IDynamicApi
{
    private IBaseCustomerRepository _baseCustomerRep;
    private IBaseCustomerReportExtendRepository _baseCustomerReportExtendRep;
    private IDictService _dictService;

    public BaseCustomerService(IBaseCustomerRepository baseCustomerRep,
        IBaseCustomerReportExtendRepository baseCustomerReportExtendRep,
        IDictService dictService)
    {
        _baseCustomerRep = baseCustomerRep;
        _baseCustomerReportExtendRep = baseCustomerReportExtendRep;
        _dictService = dictService;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<GetCustomerOutput> GetAsync(long id)
    {
        var output = new GetCustomerOutput();
        var customer = await _baseCustomerRep.AsQueryable()
            .Where(v => v.Id == id)
            .Select<BaseCustomerDto>()
            .FirstAsync();
        output.Customer = customer;

        var reportExtend = await _baseCustomerReportExtendRep.AsQueryable()
            .Where(v => v.CustomerCode == customer.CustomerCode)
            .Select<BaseCustomerReportExtendDto>()
            .FirstAsync();
        output.ReportExtend = reportExtend;

        return output;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseCustomerDto>> GetPageAsync(PageInput<BaseCustomerQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseCustomerRep.GetQueryable(dynamicCondition)
            .WhereIF(filter?.CustomerCode != null, a => a.CustomerCode!.StartsWith(filter!.CustomerCode!.ToUpper()) || a.CustomerName.Contains(filter!.CustomerCode))
            .OrderBy(c => c.Sort)
            .Select<BaseCustomerDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseCustomerDto> { List = list.Items.ToList(), Total = list.Total };

        if (data.List.Any())
        {
            var dicts = await _dictService.GetListAsync(["CustomerLevel", "CustomerType", "CustomerClassification","InputType"]);
            foreach (var customer in data.List)
            {
                customer.CustomerLevel = dicts["CustomerLevel"].FirstOrDefault(v => v.Value.Equals(customer.CustomerLevel))?.Name;
                customer.CustomerType = dicts["CustomerType"].FirstOrDefault(v => v.Value.Equals(customer.CustomerType))?.Name;
                customer.CustomerClassification = dicts["CustomerClassification"].FirstOrDefault(v => v.Value.Equals(customer.CustomerClassification))?.Name;
                customer.DoubleInputType = dicts["InputType"].FirstOrDefault(v => v.Value.Equals(customer.DoubleInputType))?.Name;
            }
        }

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<long> AddAsync(AddCustomerInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Customer.CustomerCode))
            throw ResultOutput.Exception("客户代码不可为空！");
        var isExists = await _baseCustomerRep.IsAnyAsync(v => v.CustomerCode == input.Customer.CustomerCode);
        if (isExists)
            throw ResultOutput.Exception($"客户{input.Customer.CustomerCode}已存在！");

        var entity = Mapper.Map<BaseCustomerEntity>(input.Customer);
        if (entity.Sort == 0)
        {
            var sort = await _baseCustomerRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        entity.BelongToTenant = AppInfo.User.TenantId;
        var id = await _baseCustomerRep.InsertReturnSnowflakeIdAsync(entity);

        input.ReportExtend.CustomerCode = input.Customer.CustomerCode;
        var reportExtend = Mapper.Map<BaseCustomerReportExtendEntity>(input.ReportExtend);
        await _baseCustomerReportExtendRep.InsertAsync(reportExtend);

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [AdminTransaction]
    public async Task UpdateAsync(UpdateCustomerInput input)
    {
        var entity = await _baseCustomerRep.GetAsync(input.Customer.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("客户不存在！");

        Mapper.Map(input.Customer, entity);
        await _baseCustomerRep.UpdateAsync(entity);

        var reportExtend = await _baseCustomerReportExtendRep.GetFirstAsync(v => v.CustomerCode == entity.CustomerCode);
        if (reportExtend == null)
        {
            reportExtend = new BaseCustomerReportExtendEntity
            {
                CustomerCode = entity.CustomerCode
            };
            await _baseCustomerReportExtendRep.InsertAsync(reportExtend);
        }
        else
        {
            Mapper.Map(input.ReportExtend, reportExtend);
            var ret = await _baseCustomerReportExtendRep.UpdateAsync(reportExtend);
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseCustomerRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 根据客户代码获取客户信息
    /// </summary>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    public async Task<BaseCustomerDto> GetCustomerInfoByCodeAsync(string customerCode)
    {
        if (string.IsNullOrWhiteSpace(customerCode))
            throw ResultOutput.Exception("参数有误！");

        var output = await _baseCustomerRep.GetFirstAsync(a => a.CustomerCode == customerCode && a.IsValid);
        return output.Adapt<BaseCustomerDto>();
    }
}