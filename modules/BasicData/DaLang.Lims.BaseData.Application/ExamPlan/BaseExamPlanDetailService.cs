using DaLang.Lims.BaseData.Contracts.ExamPlan.Dto;
using DaLang.Lims.BaseData.Contracts.ExamPlanDetail;
using DaLang.Lims.BaseData.Domain.ExamPlan;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;


namespace DaLang.Lims.BaseData.Services.ExamPlanDetail;

/// <summary>
/// 检测计划明细服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseExamPlanDetailService : BaseService, IBaseExamPlanDetailService, IDynamicApi
{
    private IBaseExamPlanDetailRepository _baseExamPlanDetailRep;

    public BaseExamPlanDetailService(IBaseExamPlanDetailRepository baseExamPlanDetailRep)
    {
        _baseExamPlanDetailRep = baseExamPlanDetailRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseExamPlanDetailDto> GetAsync(long id)
    {
        var output = await _baseExamPlanDetailRep.GetAsync(id);
        return output.Adapt<BaseExamPlanDetailDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="examPlanCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BaseExamPlanDetailDto>> GetAllAsync(string examPlanCode)
    {
        var list = await _baseExamPlanDetailRep.GetListAsync(a => a.ExamPlanCode == examPlanCode);
        return list.Adapt<List<BaseExamPlanDetailDto>>();
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseExamPlanDetailDto input)
    {
        var entity = Mapper.Map<BaseExamPlanDetailEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseExamPlanDetailRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseExamPlanDetailRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseExamPlanDetailDto input)
    {
        var entity = await _baseExamPlanDetailRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("检测计划明细不存在！");

        Mapper.Map(input, entity);
        await _baseExamPlanDetailRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseExamPlanDetailRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}