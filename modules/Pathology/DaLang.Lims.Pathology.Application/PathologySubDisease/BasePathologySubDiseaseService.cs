using DaLang.Lims.Pathology.Contracts.BasePathologySubDisease;
using DaLang.Lims.Pathology.Contracts.BasePathologySubDisease.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.BasePathologySubDisease;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Pathology.Application.BasePathologySubDisease;

/// <summary>
/// 子疾病服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class BasePathologySubDiseaseService : BaseService, IBasePathologySubDiseaseService, IDynamicApi
{
    private IBasePathologySubDiseaseRepository _basePathologySubDiseaseRep;

    public BasePathologySubDiseaseService(IBasePathologySubDiseaseRepository basePathologySubDiseaseRep)
    {
        _basePathologySubDiseaseRep = basePathologySubDiseaseRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePathologySubDiseaseDto> GetAsync(long id)
    {
        var output = await _basePathologySubDiseaseRep.GetAsync(id);
        return output.Adapt<BasePathologySubDiseaseDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BasePathologySubDiseaseDto>> GetPageAsync(PageInput<BasePathologySubDiseaseQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePathologySubDiseaseRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.SubDiseaseCode), c => c.SubDiseaseCode == filter.SubDiseaseCode || c.SubDiseaseName!.StartsWith(filter.SubDiseaseCode!))
            .OrderBy(c => c.Sort)
            .Select<BasePathologySubDiseaseDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BasePathologySubDiseaseDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BasePathologySubDiseaseAddInput input)
    {
        var maxItemCode = await _basePathologySubDiseaseRep.AsQueryable().MaxAsync(v => v.SubDiseaseCode);
        if (string.IsNullOrWhiteSpace(maxItemCode))
            maxItemCode = "0000";

        var next = (maxItemCode.ToInt() + 1).ToString().PadLeft(4, '0');
        input.SubDiseaseCode = next;

        var entity = Mapper.Map<BasePathologySubDiseaseEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePathologySubDiseaseRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePathologySubDiseaseRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BasePathologySubDiseaseUpdateInput input)
    {
        var entity = await _basePathologySubDiseaseRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("子疾病不存在！");

        Mapper.Map(input, entity);
        await _basePathologySubDiseaseRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePathologySubDiseaseRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}