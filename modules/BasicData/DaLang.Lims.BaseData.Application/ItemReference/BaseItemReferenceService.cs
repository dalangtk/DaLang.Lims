using DaLang.Lims.BaseData.Contracts.ItemReference.Dto;
using DaLang.Lims.BaseData.Domain.ItemReference;
using DaLang.Lims.Web.BaseData.Contracts.ItemReference;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Web.BaseData.Services.ItemReference;

/// <summary>
/// 参考范围服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseItemReferenceService : BaseService, IBaseItemReferenceService, IDynamicApi
{
    private IBaseItemReferenceRepository _baseItemReferenceRep;

    public BaseItemReferenceService(IBaseItemReferenceRepository baseItemReferenceRep)
    {
        _baseItemReferenceRep = baseItemReferenceRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseItemReferenceDto> GetAsync(long id)
    {
        var output = await _baseItemReferenceRep.GetAsync(id);
        return output.Adapt<BaseItemReferenceDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseItemReferenceDto>> GetPageAsync(PageInput<BaseItemReferenceQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseItemReferenceRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseItemReferenceDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseItemReferenceDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }

    /// <summary>
    /// 获取单个项目参考范围
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseItemReferenceDto>> GetItemReferenceAsync(BaseItemReferenceQueryInput input)
    {
        if (string.IsNullOrWhiteSpace(input.ItemCode))
            throw ResultOutput.Exception("参数有误！");

        var list = await _baseItemReferenceRep.GetQueryable()
            .Where(a => a.ItemCode == input.ItemCode)
            .OrderBy(c => c.Sort)
            .Select<BaseItemReferenceDto>()
            .ToListAsync();

        return list;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseItemReferenceDto input)
    {
        if (input.AgeLowLimit == null || input.AgeUpperLimit == null)
            throw ResultOutput.Exception("年龄不能为空！");
        var entity = Mapper.Map<BaseItemReferenceEntity>(input);
        var ageLowValue = AgeConvertHelper.ConvertAgeToMinutes(input.AgeLowLimit.Value, input.AgeUnit);
        var ageUpperValue = AgeConvertHelper.ConvertAgeToMinutes(input.AgeUpperLimit.Value, input.AgeUnit);
        entity.AgeLowValue = ageLowValue;
        entity.AgeUpperValue = ageUpperValue;

        if (entity.Sort == 0)
        {
            var sort = await _baseItemReferenceRep.AsQueryable().Where(v => v.ItemCode == input.ItemCode).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        var id = await _baseItemReferenceRep.InsertReturnSnowflakeIdAsync(entity);

        return id;
    }

    /// <summary>
    /// 新增列表
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddListAsync(List<BaseItemReferenceDto> list)
    {
        var entity = Mapper.Map<List<BaseItemReferenceEntity>>(list);
        return await _baseItemReferenceRep.InsertRangeAsync(entity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(BaseItemReferenceDto input)
    {
        if (input.AgeLowLimit == null || input.AgeUpperLimit == null)
            throw ResultOutput.Exception("年龄不能为空！");

        var entity = await _baseItemReferenceRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("数据不存在！");

        Mapper.Map(input, entity);
        var ageLowValue = AgeConvertHelper.ConvertAgeToMinutes(input.AgeLowLimit.Value, input.AgeUnit);
        var ageUpperValue = AgeConvertHelper.ConvertAgeToMinutes(input.AgeUpperLimit.Value, input.AgeUnit);
        if (entity.AgeLowValue != ageLowValue)
            entity.AgeLowValue = ageLowValue;
        if (entity.AgeUpperValue != ageUpperValue)
            entity.AgeUpperValue = ageUpperValue;
        await _baseItemReferenceRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<bool> SaveListAsync(List<BaseItemReferenceDto> list)
    {
        var updateList = list.Adapt<List<BaseItemReferenceEntity>>();
        var ret = await _baseItemReferenceRep.Context.Storageable(updateList).ExecuteCommandAsync();
        if (ret != list.Count)
            throw ResultOutput.Exception("数据不存在！");

        return ret == list.Count;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseItemReferenceRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}