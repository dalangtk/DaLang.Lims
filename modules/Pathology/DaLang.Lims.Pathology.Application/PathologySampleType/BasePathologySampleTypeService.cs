using DaLang.Lims.Pathology.Contracts.PathologySampleType;
using DaLang.Lims.Pathology.Contracts.PathologySampleType.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.PathologySampleType;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Pathology.Application.PathologySampleType;

/// <summary>
/// 病理标本服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class BasePathologySampleTypeService : BaseService, IBasePathologySampleTypeService, IDynamicApi
{
    private IBasePathologySampleTypeRepository _basePathologySampleTypeRep;

    public BasePathologySampleTypeService(IBasePathologySampleTypeRepository basePathologySampleTypeRep)
    {
        _basePathologySampleTypeRep = basePathologySampleTypeRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePathologySampleTypeDto> GetAsync(long id)
    {
        var output = await _basePathologySampleTypeRep.GetAsync(id);
        return output.Adapt<BasePathologySampleTypeDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BasePathologySampleTypeDto>> GetPageAsync(PageInput<BasePathologySampleTypeQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePathologySampleTypeRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select(c => new BasePathologySampleTypeDto
            {
                Children = SqlFunc.Subqueryable<BasePathologySampleTypeEntity>().Where(s => s.ParentCode == c.SampleTypeCode).ToList<BasePathologySampleTypeDto>()
            }, true)
            .Where(v => v.TypeGrade == 1)
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BasePathologySampleTypeDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 分页查询（不包含子节点）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BasePathologySampleTypeDto>> GetPageWithoutChildrenAsync(PageInput<BasePathologySampleTypeQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePathologySampleTypeRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BasePathologySampleTypeDto>()
            .Where(v => v.TypeGrade == 1)
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BasePathologySampleTypeDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BasePathologySampleTypeAddInput input)
    {
        var entity = Mapper.Map<BasePathologySampleTypeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePathologySampleTypeRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePathologySampleTypeRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BasePathologySampleTypeUpdateInput input)
    {
        var entity = await _basePathologySampleTypeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("病理标本不存在！");

        Mapper.Map(input, entity);
        await _basePathologySampleTypeRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePathologySampleTypeRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取标本类型列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BasePathologySampleTypeDto>> GetSampleTypeList(BasePathologySampleTypeQueryInput input)
    {
        var list = await _basePathologySampleTypeRep
            .AsQueryable()
            .WhereIF(input.TypeGrade != null, v => v.TypeGrade == input.TypeGrade)
            .WhereIF(input.SampleTypeCodes != null && input.SampleTypeCodes.Any(), v => input.SampleTypeCodes!.Contains(v.SampleTypeCode!))
            .OrderBy(v => v.SampleTypeCode)
            .Select<BasePathologySampleTypeDto>()
            .ToListAsync();
        return list;
    }
}