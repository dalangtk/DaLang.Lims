using DaLang.Lims.BaseData.Contracts.WorkDate.Dto;
using DaLang.Lims.Web.BaseData.Contracts.BaseWorkDate;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.BaseData.Domain.BaseWorkDate;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Web.BaseData.Services.BaseWorkDate;

/// <summary>
/// 工作日服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseWorkDateService : BaseService, IBaseWorkDateService, IDynamicApi
{
    private IBaseWorkDateRepository _baseWorkDateRep;

    public BaseWorkDateService(IBaseWorkDateRepository baseWorkDateRep)
    {
        _baseWorkDateRep = baseWorkDateRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseWorkDateDto> GetAsync(long id)
    {
        var output = await _baseWorkDateRep.GetAsync(id);
        return output.Adapt<BaseWorkDateDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseWorkDateGetListDto>> GetPageAsync(PageInput<BaseWorkDateQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseWorkDateRep.GetQueryable(dynamicCondition)
            .OrderBy(c => new { c.Month, c.Day })
            .Select<BaseWorkDateGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseWorkDateGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseWorkDateDto input)
    {
        var entity = Mapper.Map<BaseWorkDateEntity>(input);
        var id = await _baseWorkDateRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseWorkDateDto input)
    {
        var entity = await _baseWorkDateRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("工作日不存在！");

        Mapper.Map(input, entity);
        await _baseWorkDateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseWorkDateRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    [NonAction]
    public async Task<bool> CheckIsWorkDay(DateTime date)
    {
        var day = await _baseWorkDateRep.GetFirstAsync(a => a.WorkDate == date.Date);
        return day.DateType == 0;
    }
}