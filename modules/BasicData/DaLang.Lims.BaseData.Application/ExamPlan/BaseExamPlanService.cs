using DaLang.Lims.BaseData.Contracts.ExamPlan;
using DaLang.Lims.BaseData.Contracts.ExamPlan.Dto;
using DaLang.Lims.BaseData.Domain.ExamPlan;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.BaseData.Domain.BaseWorkDate;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.ExamPlan;

/// <summary>
/// 检测计划服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseExamPlanService : BaseService, IBaseExamPlanService, IDynamicApi
{
    private IBaseExamPlanRepository _baseExamPlanRep;
    private IBaseExamPlanDetailRepository _baseExamPlanDetailRep => LazyGetRequiredService<IBaseExamPlanDetailRepository>();
    private IBaseWorkDateRepository _workDateRep => LazyGetRequiredService<IBaseWorkDateRepository>();

    public BaseExamPlanService(IBaseExamPlanRepository baseExamPlanRep)
    {
        _baseExamPlanRep = baseExamPlanRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseExamPlanDto> GetAsync(long id)
    {
        var output = await _baseExamPlanRep.GetAsync(id);
        return output.Adapt<BaseExamPlanDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseExamPlanDto>> GetPageAsync(PageInput<BaseExamPlanQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseExamPlanRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.ExamPlanCode), a => a.ExamPlanCode.Contains(filter.ExamPlanCode) || a.ExamPlanName.Contains(filter.ExamPlanCode))
            .OrderBy(c => c.Sort)
            .Select<BaseExamPlanDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseExamPlanDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseExamPlanDto input)
    {
        if (string.IsNullOrWhiteSpace(input.ExamPlanCode))
            throw ResultOutput.Exception("检测计划代码不可为空！");

        var isExists = await _baseExamPlanRep.IsAnyAsync(v => v.ExamPlanCode == input.ExamPlanCode);
        if (isExists)
            throw ResultOutput.Exception($"检测计划{input.ExamPlanCode}已存在！");

        var entity = Mapper.Map<BaseExamPlanEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseExamPlanRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseExamPlanRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseExamPlanDto input)
    {
        var entity = await _baseExamPlanRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("检测计划不存在！");

        Mapper.Map(input, entity);
        await _baseExamPlanRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseExamPlanRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 计算检测计划
    /// </summary>
    /// <param name="planCode"></param>
    /// <param name="receiveTime"></param>
    /// <returns>根据检测计划和接收时间计算检测日期和报告日期,同时返回检测计划的接收时间点estestdate,esreporttime,receivetimepoint</returns>
    [HttpGet]
    public async Task<(DateTime?, DateTime?, TimeSpan)> CalcTestAndReportDate(string planCode, DateTime receiveTime)
    {
        var plan = await _baseExamPlanRep.GetFirstAsync(a => a.ExamPlanCode == planCode && a.IsValid);
        if (plan == null)
            throw ResultOutput.Exception($"未找到检测计划{planCode}！");

        var planDetail = await _baseExamPlanDetailRep.GetListAsync(a => a.ExamPlanCode == planCode && a.IsValid);
        if (!planDetail.Any())
            throw ResultOutput.Exception($"未找到检测计划{planCode}的明细！");
        var ret = await CalcTestDate(plan, planDetail, receiveTime);
        return ret;

        async Task<(DateTime?, DateTime?, TimeSpan)> CalcTestDate(BaseExamPlanEntity plan, List<BaseExamPlanDetailEntity> planDetail, DateTime receiveTime)
        {
            var receiveDate = receiveTime.Date;

            DateTime? estimatedTestDate = null;
            DateTime? estimatedReportTime = null;
            var receiveTimePoint = TimeSpan.Parse(receiveTime.ToString("HH:mm:ss"));

            var currDetail = planDetail.FirstOrDefault(v =>
            {
                var detailTime = v.ReceiveTime?.ToString("HH:mm:ss");
                var detailTimePoint = TimeSpan.Parse(detailTime);
                return detailTimePoint > receiveTimePoint;
            });

            if (currDetail == null)
                currDetail = planDetail.FirstOrDefault();

            var testInterval = currDetail.TestInterval ?? 0;
            var reportInterval = currDetail.ReportInterval ?? 0;
            string type = "Day";
            List<int> cycleList = new();
            if (plan.PlanType == "102")
            {
                if (string.IsNullOrWhiteSpace(plan.PlanCycle))
                    throw ResultOutput.Exception("未设置检测周期！");

                var dayOfWeek = (int)receiveTime.DayOfWeek;
                if (dayOfWeek == 0)
                    dayOfWeek = 7;

                cycleList = plan.PlanCycle!.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v =>
                {
                    if (!int.TryParse(v, out int a))
                        throw ResultOutput.Exception("检测周期必须为数字！");
                    if (a > 7 || a < 1)
                        throw ResultOutput.Exception("检测周期必须为1-7之间数字！");
                    return a;
                }).ToList();


                while (!cycleList.Contains(dayOfWeek))
                {
                    receiveTime = receiveTime.AddDays(1);
                    dayOfWeek = (int)receiveTime.DayOfWeek;
                    if (dayOfWeek == 0)
                        dayOfWeek = 7;
                }
                type = "Week";
            }
            else if (plan.PlanType == "103")
            {
                if (string.IsNullOrWhiteSpace(plan.PlanCycle))
                    throw ResultOutput.Exception("未设置检测周期！");

                var dayOfMonth = receiveTime.Date.Day;
                cycleList = plan.PlanCycle!.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v =>
                {
                    if (!int.TryParse(v, out int a))
                        throw ResultOutput.Exception("检测周期必须为数字！");
                    if (a > 31 || a < 1)
                        throw ResultOutput.Exception("检测周期必须为1-31之间数字！");
                    return a;
                }).ToList();

                while (!plan.PlanCycle.Split(',', StringSplitOptions.RemoveEmptyEntries).Contains(dayOfMonth.ToString()))
                {
                    receiveTime = receiveTime.AddDays(1);
                    dayOfMonth = receiveTime.Date.Day;
                }
                type = "Month";
            }
            estimatedTestDate = receiveTime.Date;
            estimatedTestDate = estimatedTestDate.Value.AddDays(testInterval);
            estimatedReportTime = estimatedTestDate.Value.AddDays(reportInterval);
            if (!plan.IsIncludeHoliday)
            {
                estimatedTestDate = await GetWorkDay(estimatedTestDate.Value, type, cycleList);
                estimatedReportTime = await GetWorkDay(estimatedReportTime.Value, type, cycleList);
                if (currDetail.TimePointType == "101")
                    estimatedReportTime = DateTime.Parse($"{estimatedReportTime:yyyy-MM-dd} {currDetail.ReportTimePoint}");
            }

            return (estimatedTestDate, estimatedReportTime, TimeSpan.Parse(currDetail.ReceiveTime.Value.ToString("HH:mm:ss")));

        }

        async Task<DateTime> GetWorkDay(DateTime date, string type = "Day", List<int> cycles = null)
        {
            var day = await _workDateRep.GetFirstAsync(v => v.WorkDate == date.Date);
            if (day == null)
                throw ResultOutput.Exception($"未设置工作日！");
            if (day.DateType == 1)
            {
                return date;
            }
            else
            {
                do
                {
                    if (type == "Week")
                    {
                        date = date.AddDays(1);
                        var dayOfWeek = (int)date.DayOfWeek;
                        if (dayOfWeek == 0)
                            dayOfWeek = 7;
                        if (!cycles.Contains(dayOfWeek))
                            continue;
                    }
                    else if (type == "Month")
                    {
                        date = date.AddDays(1);
                        var dayOfMonth = date.Day;
                        if (!cycles.Contains(dayOfMonth))
                            continue;
                    }
                    else
                        date = date.AddDays(1);
                    day = await _workDateRep.GetFirstAsync(v => v.WorkDate == date.Date);
                    if (day == null)
                        throw ResultOutput.Exception($"未设置工作日！");
                } while (day.DateType != 0);
                return date;
            }
        }

    }
}