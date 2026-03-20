using DaLang.Lims.BaseData.Contracts.EntrustHospital;
using DaLang.Lims.BaseData.Contracts.EntrustHospital.Dto;
using DaLang.Lims.BaseData.Domain.EntrustHospital;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.BaseData.Services.EntrustHospital;

/// <summary>
/// 委托医院服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseEntrustHospitalService : BaseService, IBaseEntrustHospitalService, IDynamicApi
{
    private IBaseEntrustHospitalRepository _baseEntrustHospitalRep;

    public BaseEntrustHospitalService(IBaseEntrustHospitalRepository baseEntrustHospitalRep)
    {
        _baseEntrustHospitalRep = baseEntrustHospitalRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseEntrustHospitalDto> GetAsync(long id)
    {
        var output = await _baseEntrustHospitalRep.GetAsync(id);
        return output.Adapt<BaseEntrustHospitalDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseEntrustHospitalGetListDto>> GetPageAsync(PageInput<BaseEntrustHospitalQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseEntrustHospitalRep.GetQueryable(dynamicCondition)
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.EntrustHospitalCode), a => a.EntrustHospitalCode.Contains(filter.EntrustHospitalCode) || a.EntrustHospitalName.Contains(filter.EntrustHospitalCode))
            .OrderBy(c => c.Sort)
            .Select<BaseEntrustHospitalGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseEntrustHospitalGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseEntrustHospitalDto input)
    {
        if (string.IsNullOrWhiteSpace(input?.EntrustHospitalCode))
            throw ResultOutput.Exception("委托医院代码不可为空");
        input.EntrustHospitalCode = input.EntrustHospitalCode.ToUpper().Trim();
        var isExists = await _baseEntrustHospitalRep.IsAnyAsync(a => a.EntrustHospitalCode == input.EntrustHospitalCode);
        if (isExists)
            throw ResultOutput.Exception($"委托医院{input.EntrustHospitalCode}已存在！");

        var entity = Mapper.Map<BaseEntrustHospitalEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseEntrustHospitalRep.AsQueryable().MaxAsync(a => a.Sort);
            //entity.Sort = sort + GetSortGrowStep(typeof(BaseEntrustHospitalEntity));
            entity.SetSort(sort);
        }
        var id = await _baseEntrustHospitalRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseEntrustHospitalDto input)
    {
        var entity = await _baseEntrustHospitalRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("委托医院不存在！");

        Mapper.Map(input, entity);
        await _baseEntrustHospitalRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseEntrustHospitalRep
            .AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}