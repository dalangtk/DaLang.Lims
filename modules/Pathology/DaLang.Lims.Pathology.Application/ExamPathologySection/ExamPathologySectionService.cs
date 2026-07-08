using DaLang.Lims.Pathology.Contracts.ExamPathologySection;
using DaLang.Lims.Pathology.Contracts.ExamPathologySection.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.ExamPathologySection;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pathology.Application.ExamPathologySection;

/// <summary>
/// 切片服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class ExamPathologySectionService : BaseService, IExamPathologySectionService, IDynamicApi
{
    private IExamPathologySectionRepository _examPathologySectionRep;

    public ExamPathologySectionService(IExamPathologySectionRepository examPathologySectionRep)
    {
        _examPathologySectionRep = examPathologySectionRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamPathologySectionDto> GetAsync(long id)
    {
        var output = await _examPathologySectionRep.GetAsync(id);
        return output.Adapt<ExamPathologySectionDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ExamPathologySectionDto>> GetPageAsync(PageInput<ExamPathologySectionQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _examPathologySectionRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.SectionNo)
            .Select<ExamPathologySectionDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<ExamPathologySectionDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ExamPathologySectionAddInput input)
    {
        var entity = Mapper.Map<ExamPathologySectionEntity>(input);
        var id = await _examPathologySectionRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ExamPathologySectionUpdateInput input)
    {
        var entity = await _examPathologySectionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("切片不存在！");

        Mapper.Map(input, entity);
        await _examPathologySectionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _examPathologySectionRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取切片
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ExamPathologySectionDto>> GetSectionsAsync(long examInfoId)
    {
        var list = await _examPathologySectionRep.AsQueryable()
            .Where(a => a.ExamInfoId == examInfoId && !a.IsDeleted)
            .OrderBy(c => c.SectionNo)
            .Select<ExamPathologySectionDto>()
            .ToListAsync();
        return list;
    }
}