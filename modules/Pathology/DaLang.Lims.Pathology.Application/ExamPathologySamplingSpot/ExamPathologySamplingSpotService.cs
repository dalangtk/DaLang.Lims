using DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot;
using DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.ExamPathologySamplingSpot;
using DaLang.Lims.Pathology.Domain.PathologySampleType;
using DaLang.Lims.Pathology.Domain.PathologySamplingSpot;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Pathology.Application.ExamPathologySamplingSpot;

/// <summary>
/// 检验取材部位服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class ExamPathologySamplingSpotService : BaseService, IExamPathologySamplingSpotService, IDynamicApi
{
    private IExamPathologySamplingSpotRepository _examPathologySamplingSpotRep;

    public ExamPathologySamplingSpotService(IExamPathologySamplingSpotRepository examPathologySamplingSpotRep)
    {
        _examPathologySamplingSpotRep = examPathologySamplingSpotRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamPathologySamplingSpotDto> GetAsync(long id)
    {
        var output = await _examPathologySamplingSpotRep.GetAsync(id);
        return output.Adapt<ExamPathologySamplingSpotDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ExamPathologySamplingSpotDto>> GetListAsync(long examInfoId)
    {
        var list = await _examPathologySamplingSpotRep.AsQueryable()
            .LeftJoin<BasePathologySamplingSpotEntity>((a, b) => a.SamplingSpotCode == b.SamplingSpotCode)
            .LeftJoin<BasePathologySampleTypeEntity>((a, b, c) => a.SampleTypeCode == c.SampleTypeCode)
            .OrderByDescending(a => a.Sort)
            .Select((a, b, c) => new ExamPathologySamplingSpotDto
            {
                Id = a.Id,
                ExamInfoId = a.ExamInfoId,
                SampleTypeCode = c.SampleTypeCode,
                SampleTypeName = c.SampleTypeName,
                SamplingSpotCode = b.SamplingSpotCode,
                SamplingSpotName = b.SamplingSpotName,
            })
            .ToListAsync();
        return list;
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ExamPathologySamplingSpotAddInput input)
    {
        var entity = Mapper.Map<ExamPathologySamplingSpotEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _examPathologySamplingSpotRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _examPathologySamplingSpotRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ExamPathologySamplingSpotUpdateInput input)
    {
        var entity = await _examPathologySamplingSpotRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("检验取材部位不存在！");

        Mapper.Map(input, entity);
        await _examPathologySamplingSpotRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _examPathologySamplingSpotRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}