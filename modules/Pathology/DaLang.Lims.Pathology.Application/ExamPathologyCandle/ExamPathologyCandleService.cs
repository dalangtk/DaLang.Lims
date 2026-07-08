using DaLang.Lims.Pathology.Contracts.ExamPathologyCandle;
using DaLang.Lims.Pathology.Contracts.ExamPathologyCandle.Dto;
using DaLang.Lims.Pathology.Contracts.ExamPathologySection.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.ExamPathologyCandle;
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
using Yitter.IdGenerator;

namespace DaLang.Lims.Pathology.Application.ExamPathologyCandle;

/// <summary>
/// 蜡块服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class ExamPathologyCandleService : BaseService, IExamPathologyCandleService, IDynamicApi
{
    private IExamPathologyCandleRepository _examPathologyCandleRep;
    private IExamPathologySectionRepository _examPathologySectionRep;

    public ExamPathologyCandleService(IExamPathologyCandleRepository examPathologyCandleRep,
        IExamPathologySectionRepository examPathologySectionRep)
    {
        _examPathologyCandleRep = examPathologyCandleRep;
        _examPathologySectionRep = examPathologySectionRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamPathologyCandleDto> GetAsync(long id)
    {
        var output = await _examPathologyCandleRep.GetAsync(id);
        return output.Adapt<ExamPathologyCandleDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ExamPathologyCandleDto>> GetPageAsync(PageInput<ExamPathologyCandleQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _examPathologyCandleRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.CandleNo)
            .Select<ExamPathologyCandleDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<ExamPathologyCandleDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ExamPathologyCandleAddInput input)
    {
        var entity = Mapper.Map<ExamPathologyCandleEntity>(input);
        var id = await _examPathologyCandleRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ExamPathologyCandleUpdateInput input)
    {
        var entity = await _examPathologyCandleRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("蜡块不存在！");

        Mapper.Map(input, entity);
        await _examPathologyCandleRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _examPathologyCandleRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取蜡块
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    public async Task<List<ExamPathologyCandleDto>> GetCandlesAsync(long examInfoId)
    {
        var list = await _examPathologyCandleRep.AsQueryable()
            .Where(a => a.ExamInfoId == examInfoId && !a.IsDeleted)
            .OrderBy(c => c.CandleNo)
            .Select<ExamPathologyCandleDto>()
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 保存蜡块
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<bool> SaveCandles(List<ExamPathologyCandleDto> input)
    {
        var addList = input.FindAll(v => v.Id <= 0);
        if (addList.Any())
        {
            List<ExamPathologySectionDto> addSectionList = new();
            foreach (var item in addList)
            {
                var id = YitIdHelper.NextId();
                item.Id = id;

                addSectionList.Add(new ExamPathologySectionDto
                {
                    CandleNo = item.CandleNo,
                    CandleId = id,
                    Position = item.Position,
                    ExamInfoId = item.ExamInfoId,
                    OriginalCandleNo = item.OriginalCandleNo,
                    IsMedicalAdvice = false
                });
            }
            await _examPathologySectionRep.InsertRangeAsync(addSectionList.Adapt<List<ExamPathologySectionEntity>>());
            await _examPathologyCandleRep.InsertRangeAsync(addList.Adapt<List<ExamPathologyCandleEntity>>());
        }

        var updateList = input.FindAll(v => v.Id > 0);
        if (updateList.Any())
        {
            var candleIds = updateList.Select(v => v.Id).ToList();
            var sectionList = await _examPathologySectionRep.GetListAsync(v => candleIds.Contains(v.CandleId!.Value));
            foreach (var item in sectionList)
            {
                var currCandle = updateList.FirstOrDefault(v => v.Id == item.CandleId);
                if (currCandle == null)
                    item.IsDeleted = true;

                item.CandleNo = currCandle?.CandleNo;
                item.Position = currCandle?.Position;
                item.OriginalCandleNo = currCandle?.OriginalCandleNo;
            }
            await _examPathologySectionRep.UpdateRangeAsync(sectionList.Adapt<List<ExamPathologySectionEntity>>());
            await _examPathologyCandleRep.UpdateRangeAsync(updateList.Adapt<List<ExamPathologyCandleEntity>>());
        }

        return true;
    }
}