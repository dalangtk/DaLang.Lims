
using DaLang.Lims.Exam.Contracts.ExamCriticalValue;
using DaLang.Lims.Exam.Contracts.ExamCriticalValue.Dto;
using DaLang.Lims.Exam.Core.Consts;
using DaLang.Lims.Exam.Domain.ExamCriticalValue;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Exam.Application.ExamCriticalValue;

/// <summary>
/// 危急值服务
/// </summary>
[DynamicApi(Area = ExamConsts.AreaName)]
public class ExamCriticalValueService : BaseService, IExamCriticalValueService, IDynamicApi
{
    private IExamCriticalValueRepository _examCriticalValueRep;

    public ExamCriticalValueService(IExamCriticalValueRepository examCriticalValueRep)
    {
        _examCriticalValueRep = examCriticalValueRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamCriticalValueDto> GetAsync(long id)
    {
        var output = await _examCriticalValueRep.GetAsync(id);
        return output.Adapt<ExamCriticalValueDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<ExamCriticalValueDto>> GetListAsync(ExamCriticalValueQueryInput input)
    {
        var list = await _examCriticalValueRep.AsQueryable()
            .OrderByDescending(a => a.Id)
            .Select<ExamCriticalValueDto>()
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ExamCriticalValueDto>> GetPageAsync(PageInput<ExamCriticalValueQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _examCriticalValueRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.ProTime)
            .Select<ExamCriticalValueDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<ExamCriticalValueDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ExamCriticalValueDto input)
    {
        var entity = Mapper.Map<ExamCriticalValueEntity>(input);
        var id = await _examCriticalValueRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ExamCriticalValueDto input)
    {
        var entity = await _examCriticalValueRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("危急值不存在！");

        Mapper.Map(input, entity);
        await _examCriticalValueRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _examCriticalValueRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}