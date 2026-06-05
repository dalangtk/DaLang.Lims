using DaLang.Lims.Pathology.Contracts.PathologyDisease;
using DaLang.Lims.Pathology.Contracts.PathologyDisease.Dto;
using DaLang.Lims.Pathology.Contracts.PathologyDiseaseDetail.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.PathologyDisease;
using DaLang.Lims.Pathology.Domain.PathologyDiseaseDetail;
using DaLang.Lims.Pathology.Domain.BasePathologySubDisease;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pathology.Application.PathologyDisease;

/// <summary>
/// 疾病服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class BasePathologyDiseaseService : BaseService, IBasePathologyDiseaseService, IDynamicApi
{
    private IBasePathologyDiseaseRepository _basePathologyDiseaseRep;
    private IBasePathologyDiseaseDetailRepository _basePathologyDiseaseDetailRep;

    public BasePathologyDiseaseService(IBasePathologyDiseaseRepository basePathologyDiseaseRep,
        IBasePathologyDiseaseDetailRepository basePathologyDiseaseDetailRep)
    {
        _basePathologyDiseaseRep = basePathologyDiseaseRep;
        _basePathologyDiseaseDetailRep = basePathologyDiseaseDetailRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePathologyDiseaseDto> GetAsync(long id)
    {
        var output = await _basePathologyDiseaseRep.GetAsync(id);
        return output.Adapt<BasePathologyDiseaseDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BasePathologyDiseaseDto>> GetPageAsync(PageInput<BasePathologyDiseaseQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePathologyDiseaseRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BasePathologyDiseaseDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BasePathologyDiseaseDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BasePathologyDiseaseAddInput input)
    {
        var maxItemCode = await _basePathologyDiseaseRep.AsQueryable().MaxAsync(v => v.DiseaseCode);
        if (string.IsNullOrWhiteSpace(maxItemCode))
            maxItemCode = "0000";

        var next = (maxItemCode.ToInt() + 1).ToString().PadLeft(4, '0');
        input.DiseaseCode = next;
        var entity = Mapper.Map<BasePathologyDiseaseEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePathologyDiseaseRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePathologyDiseaseRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BasePathologyDiseaseUpdateInput input)
    {
        var entity = await _basePathologyDiseaseRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("疾病管理不存在！");

        Mapper.Map(input, entity);
        await _basePathologyDiseaseRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [AdminTransaction]
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _basePathologyDiseaseRep.GetAsync(id);
        if (entity == null)
            throw ResultOutput.Exception("diease not exists！");

        await _basePathologyDiseaseDetailRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.DiseaseCode == entity.DiseaseCode)
            .ExecuteCommandAsync();

        return await _basePathologyDiseaseRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取疾病明细
    /// </summary>
    /// <param name="diseaseCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BasePathologyDiseaseDetailDto>> GetDiseaseDetails(string diseaseCode)
    {
        if (string.IsNullOrWhiteSpace(diseaseCode))
            throw ResultOutput.Exception("diseaseCode can not be null！");

        var ret = await _basePathologyDiseaseDetailRep.AsQueryable()
             .InnerJoin<BasePathologyDiseaseEntity>((a, b) => a.DiseaseCode == b.DiseaseCode && b.IsValid && !b.IsDeleted)
             .InnerJoin<BasePathologySubDiseaseEntity>((a, b, c) => a.SubDiseaseCode == c.SubDiseaseCode && c.IsValid && !c.IsDeleted)
             .Where((a, b, c) => a.DiseaseCode == diseaseCode && a.IsValid && !a.IsDeleted)
             .OrderBy((a, b, c) => c.SubDiseaseName)
             .Select((a, b, c) => new BasePathologyDiseaseDetailDto
             {
                 Id = a.Id,
                 DiseaseCode = a.DiseaseCode!,
                 SubDiseaseCode = a.SubDiseaseCode!,
                 DiseaseName = b.DiseaseName,
                 SubDiseaseName = c.SubDiseaseName
             })
             .ToListAsync();

        return ret;
    }

    /// <summary>
    /// 新增疾病明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddDiseaseDetail(List<BasePathologyDiseaseDetailAddInput> input)
    {
        if (input == null || input.Count == 0)
            throw ResultOutput.Exception("input can not be null！");
        var entityList = input.Adapt<List<BasePathologyDiseaseDetailEntity>>();
        var ret = await _basePathologyDiseaseDetailRep.InsertRangeAsync(entityList);
        return ret;
    }

    /// <summary>
    /// 删除疾病明细
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<bool> DeleteDiseaseDetail(long id)
    {
        var ret = await _basePathologyDiseaseDetailRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync();

        return ret > 0;
    }

    /// <summary>
    /// 获取疾病列表
    /// </summary>
    /// <param name="diseaseCodes"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BasePathologyDiseaseDto>> GetDiseaseList(List<string> diseaseCodes)
    {
        var ret = await _basePathologyDiseaseRep.AsQueryable()
            .WhereIF(diseaseCodes != null && diseaseCodes.Any(), a => diseaseCodes!.Contains(a.DiseaseCode!))
            .Select<BasePathologyDiseaseDto>()
            .ToListAsync();
        return ret;
    }
}