using DaLang.Lims.Pathology.Contracts.ExamPathologyDigitalSlicing;
using DaLang.Lims.Pathology.Contracts.ExamPathologyDigitalSlicing.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.ExamPathologyDigitalSlicing;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pathology.Application.ExamPathologyDigitalSlicing;

/// <summary>
/// 数字切片服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class ExamPathologyDigitalSlicingService : BaseService, IExamPathologyDigitalSlicingService, IDynamicApi
{
    private IExamPathologyDigitalSlicingRepository _examPathologyDigitalSlicingRep;

    public ExamPathologyDigitalSlicingService(IExamPathologyDigitalSlicingRepository examPathologyDigitalSlicingRep)
    {
        _examPathologyDigitalSlicingRep = examPathologyDigitalSlicingRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamPathologyDigitalSlicingDto> GetAsync(long id)
    {
        var output = await _examPathologyDigitalSlicingRep.GetAsync(id);
        return output.Adapt<ExamPathologyDigitalSlicingDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<ExamPathologyDigitalSlicingDto>> GetListAsync(ExamPathologyDigitalSlicingQueryInput input)
    {
        if (input.ExamInfoId == null)
            throw ResultOutput.Exception("查询条件不可为空！");

        var list = await _examPathologyDigitalSlicingRep.AsQueryable()
            .WhereIF(input.ExamInfoId != null, a => a.ExamInfoId == input.ExamInfoId)
            .IgnoreColumns(v => v.SlicingPath)
            .OrderByDescending(a => a.Id)
            .Select<ExamPathologyDigitalSlicingDto>()
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ExamPathologyDigitalSlicingDto>> GetPageAsync(PageInput<ExamPathologyDigitalSlicingQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _examPathologyDigitalSlicingRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Query), a => a.Barcode == filter.Query || a.SampleNo == filter.Query)
            .OrderBy(c => c.SlicingName)
            .Select<ExamPathologyDigitalSlicingDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<ExamPathologyDigitalSlicingDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ExamPathologyDigitalSlicingAddInput input)
    {
        var entity = Mapper.Map<ExamPathologyDigitalSlicingEntity>(input);
        var id = await _examPathologyDigitalSlicingRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ExamPathologyDigitalSlicingUpdateInput input)
    {
        var entity = await _examPathologyDigitalSlicingRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("数字切片不存在！");

        Mapper.Map(input, entity);
        await _examPathologyDigitalSlicingRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _examPathologyDigitalSlicingRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id).ExecuteCommandAsync() > 0;
    }
}