using DaLang.Lims.BaseData.Contracts.EntrustPurpose.Dto;
using DaLang.Lims.Web.BaseData.Contracts.EntrustPurpose;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.BaseData.Domain.EntrustPurpose;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Web.BaseData.Services.EntrustPurpose;

/// <summary>
/// 委托目的服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseEntrustPurposeService : BaseService, IBaseEntrustPurposeService, IDynamicApi
{
    private IBaseEntrustPurposeRepository _baseEntrustPurposeRep;

    public BaseEntrustPurposeService(IBaseEntrustPurposeRepository baseEntrustPurposeRep)
    {
        _baseEntrustPurposeRep = baseEntrustPurposeRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseEntrustPurposeDto> GetAsync(long id)
    {
        var output = await _baseEntrustPurposeRep.GetAsync(id);
        return output.Adapt<BaseEntrustPurposeDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<BaseEntrustPurposeGetListDto>> GetListAsync(BaseEntrustPurposeQueryInput input)
    {
        var list = await _baseEntrustPurposeRep.AsQueryable()
            .OrderByDescending(a => a.Id)
            .Select<BaseEntrustPurposeGetListDto>()
            .ToListAsync();
        return list;
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseEntrustPurposeGetListDto>> GetPageAsync(PageInput<BaseEntrustPurposeQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseEntrustPurposeRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseEntrustPurposeGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseEntrustPurposeGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseEntrustPurposeDto input)
    {
        var entity = Mapper.Map<BaseEntrustPurposeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseEntrustPurposeRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseEntrustPurposeRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseEntrustPurposeDto input)
    {
        var entity = await _baseEntrustPurposeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("委托目的不存在！");

        Mapper.Map(input, entity);
        await _baseEntrustPurposeRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseEntrustPurposeRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}