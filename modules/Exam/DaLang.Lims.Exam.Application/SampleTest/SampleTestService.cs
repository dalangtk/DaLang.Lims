using AngleSharp.Dom;
using COSXML.Network;
using DaLang.Lims.BaseData.Contracts.AuditRule;
using DaLang.Lims.BaseData.Contracts.AuditRule.Dto;
using DaLang.Lims.BaseData.Contracts.BasePurpose;
using DaLang.Lims.BaseData.Contracts.Combo.Dto;
using DaLang.Lims.BaseData.Contracts.InstrumentItem;
using DaLang.Lims.BaseData.Domain.Combo;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.BaseData.Domain.Group;
using DaLang.Lims.BaseData.Domain.InstrumentItem;
using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.BaseData.Domain.ItemReference;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.BaseData.Domain.UserGroup;
using DaLang.Lims.Exam.Contracts.ExamCriticalValue.Dto;
using DaLang.Lims.Exam.Contracts.ExamUnAuditLog.Dto;
using DaLang.Lims.Exam.Contracts.ReportFiles.Dto;
using DaLang.Lims.Exam.Contracts.ReportTask.Dto;
using DaLang.Lims.Exam.Contracts.SampleTest;
using DaLang.Lims.Exam.Contracts.SampleTest.Dto;
using DaLang.Lims.Exam.Core.Consts;
using DaLang.Lims.Exam.Domain.ExamCriticalValue;
using DaLang.Lims.Exam.Domain.ExamUnAuditLog;
using DaLang.Lims.Exam.Domain.ReportFiles;
using DaLang.Lims.Exam.Domain.ReportTask;
using DaLang.Lims.Shared.Contracts.ApplyItem.Dto;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;
using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;
using DaLang.Lims.Shared.Contracts.ExamTaskDetail.Dto;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyItem;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Shared.Domain.ExamResult;
using DaLang.Lims.Shared.Domain.ExamSampleTrack;
using DaLang.Lims.Shared.Domain.ExamTask;
using DaLang.Lims.Shared.Domain.ExamTaskDetail;
using DaLang.Lims.Web.Common.Consts;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Core.Exceptions;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Dict;
using DaLang.Lims.Web.Framework.Services.Parameter;
using DotNetCore.CAP;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using Yitter.IdGenerator;


namespace DaLang.Lims.Exam.Application.SampleTest;

/// <summary>
/// 标本检验服务
/// </summary>
[DynamicApi(Area = ExamConsts.AreaName)]
public class SampleTestService : BaseService, ISampleTestService, IDynamicApi, ICapSubscribe
{
    private readonly AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private readonly AdminRepositoryBase<ExamResultEntity> _examResultRep;
    private readonly IBaseItemRepository _itemRep;
    private readonly IBaseInstrumentItemRepository _instrumentItemRep;
    private readonly IBaseInstrumentItemService _instrumentItemService;
    private readonly IBaseItemReferenceRepository _referenceRep;
    private readonly IDictService _dictService;
    private readonly IBaseItemPersonalizeRepository _itemPersonalizeRep;
    private readonly AdminRepositoryBase<ExamTaskEntity> _taskRep;
    private readonly AdminRepositoryBase<ExamTaskDetailEntity> _taskDetailRep;
    private readonly AdminRepositoryBase<ApplyInfoEntity> _applyInfoRep;
    private readonly AdminRepositoryBase<ApplyPurposeEntity> _purposeRep;
    private readonly AdminRepositoryBase<ApplyItemEntity> _applyItemRep;
    private readonly ICacheTool _cache;
    private readonly IReportTaskRepository _reportTaskRep;
    private readonly IBaseCustomerReportExtendRepository _reportExtendRep;
    private readonly AdminRepositoryBase<ExamSampleTrackEntity> _sampleTrackRep;
    private readonly IExamUnAuditLogRepository _unAuditLogRep;
    private readonly IBaseUserGroupRepository _userGroupRep;
    private readonly IBasePurposeService _basePurposeService;
    private readonly ICapPublisher _publisher;
    private readonly IBaseComboRepository _baseComboRep;
    private readonly IBaseAuditRuleService _auditRuleService;
    private readonly IBasePurposeRepository _basePurposeRep;
    private readonly IExamCriticalValueRepository _criticalValueRep;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IParameterService _param;
    private readonly AdminRepositoryBase<ReportFilesEntity> _reportFileRep;
    private readonly IBaseGroupRepository _baseGroupRep;

    public SampleTestService(AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        AdminRepositoryBase<ExamResultEntity> examResultRep,
        IBaseItemRepository itemRep,
        IBaseInstrumentItemRepository instrumentItemRep,
        IBaseItemReferenceRepository referenceRep,
        ICacheTool cache,
        IDictService dictService,
        IBaseItemPersonalizeRepository itemPersonalizeRep,
        AdminRepositoryBase<ExamTaskEntity> taskRep,
        AdminRepositoryBase<ExamTaskDetailEntity> taskDetailRep,
        AdminRepositoryBase<ApplyPurposeEntity> purposeRep,
        IReportTaskRepository reportTaskRep,
        IBaseCustomerReportExtendRepository reportExtendRep,
        AdminRepositoryBase<ExamSampleTrackEntity> sampleTrackRep,
        IExamUnAuditLogRepository unAuditLogRep,
        IBaseUserGroupRepository userGroupRep,
        IBasePurposeService basePurposeService,
        AdminRepositoryBase<ApplyInfoEntity> applyInfoRep,
        ICapPublisher publisher,
        AdminRepositoryBase<ApplyItemEntity> applyItemRep,
        IBaseComboRepository baseComboRep,
        IBaseAuditRuleService auditRuleService,
        IBasePurposeRepository basePurposeRep,
        IBaseInstrumentItemService instrumentItemService,
        IExamCriticalValueRepository criticalValueRep,
        IHttpClientFactory httpFactory,
        IParameterService param,
        AdminRepositoryBase<ReportFilesEntity> reportFileRep,
        IBaseGroupRepository baseGroupRep)
    {
        _examInfoRep = examInfoRep;
        _examResultRep = examResultRep;
        _itemRep = itemRep;
        _instrumentItemRep = instrumentItemRep;
        _referenceRep = referenceRep;
        _cache = cache;
        _dictService = dictService;
        _itemPersonalizeRep = itemPersonalizeRep;
        _taskRep = taskRep;
        _taskDetailRep = taskDetailRep;
        _purposeRep = purposeRep;
        _reportTaskRep = reportTaskRep;
        _reportExtendRep = reportExtendRep;
        _sampleTrackRep = sampleTrackRep;
        _unAuditLogRep = unAuditLogRep;
        _userGroupRep = userGroupRep;
        _basePurposeService = basePurposeService;
        _applyInfoRep = applyInfoRep;
        _publisher = publisher;
        _applyItemRep = applyItemRep;
        _baseComboRep = baseComboRep;
        _auditRuleService = auditRuleService;
        _basePurposeRep = basePurposeRep;
        _instrumentItemService = instrumentItemService;
        _criticalValueRep = criticalValueRep;
        _httpFactory = httpFactory;
        _param = param;
        _reportFileRep = reportFileRep;
        _baseGroupRep = baseGroupRep;
    }
    /// <summary>
    /// 获取检验列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ExamInfoDto>> GetSampleListAsync(ExamListQueryInput input)
    {
        if (input.ExamInfoId == null)
        {
            if (string.IsNullOrWhiteSpace(input.GroupCode))
                throw ResultOutput.Exception("组别代码不能为空！");

            if (string.IsNullOrWhiteSpace(input.Barcode) && (input.BeginDate == null || input.EndDate == null))
                throw ResultOutput.Exception("请选择查询时间段！");
        }

        var query = _examInfoRep.AsQueryable();

        if (input.ExamInfoId == null)
        {
            query = query.Where(v => v.GroupCode == input.GroupCode)
                         .WhereIF(!string.IsNullOrWhiteSpace(input.Barcode), v => v.Barcode == input.Barcode)
                         .WhereIF(string.IsNullOrWhiteSpace(input.Barcode), v => SqlFunc.Between(v.TestDate, input.BeginDate!.Value.Date, input.EndDate!.Value.Date));
        }
        else
        {
            query = query.Where(v => v.Id == input.ExamInfoId);
        }

        var ret = await query
                        .Select(v => new ExamInfoDto
                        {
                            HasCritical = SqlFunc.Subqueryable<ExamCriticalValueEntity>().Where(a => v.Id == a.ExamInfoId && a.ProcessStatus == 0).Count() > 0,
                        }, true)
                        .ToListAsync();
        var infoList = ret.Adapt<List<ExamInfoDto>>();

        return infoList;
    }
    /// <summary>
    /// 获取检验信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamInfoDto> GetExamInfoAsync(long id)
    {
        var output = await _examInfoRep.GetAsync(id);
        return output.Adapt<ExamInfoDto>();
    }
    /// <summary>
    /// 查询检验结果
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ExamResultDto>> GetResultListAsync(long examInfoId)
    {
        var resultList = await _examResultRep.AsQueryable()
            .Where(v => v.ExamInfoId == examInfoId)
            .Select<ExamResultDto>()
            .ToListAsync();

        return resultList;
    }
    /// <summary>
    /// 保存检验结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<ExamResultDto> SaveItemResult(ExamResultUpdateInput input)
    {
        var result = await _examResultRep.GetFirstAsync(v => v.Id == input.Id);
        if (result == null)
            throw ResultOutput.Exception("数据不存在，请刷新后重试！");

        await _examResultRep.AsUpdateable()
             .SetColumns(v => v.ItemResult == input.ItemResult)
             .Where(v => v.Id == input.Id)
             .ExecuteCommandAsync();

        var r = await CalcAbnormal(input);
        if (r.IsCriticalValue)
        {
            var isExists = await _criticalValueRep.IsAnyAsync(v => v.ExamResultId == input.Id);
            if (!isExists)
            {
                var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
                var criticalValue = new ExamCriticalValueAddInput
                {
                    GroupCode = examInfo.GroupCode,
                    GroupName = examInfo.GroupName,
                    ExamInfoId = examInfo.Id,
                    ExamResultId = result.Id,
                    Barcode = examInfo.Barcode,
                    SampleNo = examInfo.SampleNo,
                    TestDate = examInfo.TestDate,
                    PurCode = result.PurCode,
                    PurName = result.PurName,
                    InstrumentItemCode = result.InstrumentItemCode,
                    ItemCode = result.ItemCode,
                    ItemName = StringHelper.NVL(result.ItemNamePersonalize, result.ItemName),
                    ItemResult = result.ItemResult,
                    CriticalContent = r.JudgeCondition
                };
                await _criticalValueRep.InsertAsync(criticalValue.Adapt<ExamCriticalValueEntity>());
            }
        }
        return r;
    }
    /// <summary>
    /// 新增检验信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddExamInfoAsync(ExamInfoDto input)
    {
        var entity = Mapper.Map<ExamInfoEntity>(input);

        var id = await _examInfoRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新检验信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateExamInfoAsync(ExamInfoDto input)
    {
        var entity = await _examInfoRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("标本检验不存在！");

        Mapper.Map(input, entity);
        var ageValue = AgeConvertHelper.CalculateAgeToMinute(entity.Age1, entity.AgeUnit1, entity.Age2, entity.AgeUnit2);
        if (ageValue != entity.AgeValue)
            entity.AgeValue = ageValue;

        await _examInfoRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 更新病人基本信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task UpdatePatientInfo(UpdatePatientInfoInput input)
    {
        var examInfo = input.ExamInfo;
        var updateFields = input.UpdateFields;
        if (updateFields == null || updateFields.Count == 0)
            return;

        var isExists = await _examInfoRep.IsAnyAsync(v => v.Id == examInfo.Id);
        if (!isExists)
            throw ResultOutput.Exception("检验信息不存在！");

        var entity = Mapper.Map<ExamInfoEntity>(examInfo);
        await _examInfoRep.GetUpdateable(entity).UpdateColumns(input.UpdateFields.ToArray()).EnableDiffLogEvent().ExecuteCommandAsync();
        if (input.UpdateFields.Exists(v => v.ToLower().Contains("age")))
        {
            var info = await _examInfoRep.GetFirstAsync(v => v.Id == entity.Id);
            info.AgeValue = AgeConvertHelper.CalculateAgeToMinute(info.Age1, info.AgeUnit1, info.Age2, info.AgeUnit2);
            await _examInfoRep.AsUpdateable(info).EnableDiffLogEvent().ExecuteCommandAsync();
            await RefreshItemInfo(new RefreshExamInfoInput
            {
                ExamInfoId = info.Id,
                IsForce = 1,
                IsReceive = false
            });
        }
    }

    /// <summary>
    /// 删除检验信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteExamInfoAsync(long id)
    {
        return await _examInfoRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 刷新项目信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [CapSubscribe(LimsConsts.RefreshInfoEvent)]
    public async Task<bool> RefreshItemInfo(RefreshExamInfoInput input)
    {
        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("检验信息不存在！");

        if (examInfo.SampleStatus != SampleStatusEnum.Testing.ToInt())
            throw ResultOutput.Exception("仅检验中可刷新项目信息！");

        var resultList = await _examResultRep.GetListAsync(v => v.ExamInfoId == examInfo.Id);
        if (!resultList.Any())
            return true;

        if (input.IsForce == 0 && !string.IsNullOrWhiteSpace(examInfo.EntrustHospitalCode))
            return true;

        var methods = await _dictService.GetDictByTypeCodeAsync($"Method");
        var ageMinutes = examInfo.AgeValue;
        foreach (var currInstrumentItem in resultList.GroupBy(v => v.InstrumentItemCode))
        {
            var instrumentItemCode = currInstrumentItem.Key;
            var purCodes = resultList.FindAll(v => v.InstrumentItemCode == instrumentItemCode).Select(v => v.PurCode).Distinct().ToList();

            var items = await _instrumentItemService.GetInstrumntItemInfo(purCodes);

            //var items = await _instrumentItemRep.AsQueryable()
            //     .InnerJoin<BaseInstrumentItemDetailEntity>((a, b) => a.InstrumentItemCode == b.InstrumentItemCode)
            //     .InnerJoin<BaseItemEntity>((a, b, c) => b.ItemCode == c.ItemCode)
            //     .InnerJoin<BaseItemPersonalizeEntity>((a, b, c, d) => c.ItemCode == d.ItemCode)
            //     .Where((a, b, c, d) => a.InstrumentItemCode == instrumentItemCode)
            //     .Select((a, b, c, d) =>
            //     new InstrumentItemInfo
            //     {
            //         InstrumentItemCode = a.InstrumentItemCode,
            //         InstrumentItemName = a.InstrumentItemName,
            //         ItemCode = c.ItemCode,
            //         ItemName = c.ItemName,
            //         ItemEN = c.ItemNameEN,
            //         ItemAB = c.ItemNameAB,
            //         ItemUnit = d.ItemUnit,
            //         MethodCode = d.MethodCode,
            //         IsReportShow = d.IsReportShow,
            //         ReportOrder = a.PrintOrder,
            //         ItemReportOrder = b.Sort.ToString(),
            //         MethodBasis = d.MethodBasis,
            //         ResultType = c.ResultType,
            //         IsCalculate = d.IsCalculcate,
            //         CalcExpression = d.CalcExpression
            //     })
            //     .ToListAsync();

            foreach (var i in items)
            {
                i.ReportOrder += i.ItemReportOrder.PadLeft(3, '0');
                if (!string.IsNullOrWhiteSpace(i.MethodCode))
                {
                    if (!methods.Exists(v => v.Code == i.MethodCode))
                    {
                        if (!input.IsReceive)
                            throw ResultOutput.Exception($"项目{i.ItemCode}-{i.ItemName}方法学{i.MethodCode}不存在，请重新设置项目方法学！");
                    }
                    else
                        i.MethodName = methods.FirstOrDefault(v => v.Code == i.MethodCode).Name;
                }
            }

            foreach (var item in currInstrumentItem)
            {
                var itemReference = await _referenceRep.AsQueryable()
                    .Where(v => v.ItemCode == item.ItemCode)
                    .Where(v => string.IsNullOrEmpty(v.CustomerCode) || v.CustomerCode == examInfo.CustomerCode)
                    .Where(v => string.IsNullOrEmpty(v.InstrumentCode) || v.InstrumentCode == item.InstrumentCode)
                    .Where(v => string.IsNullOrEmpty(v.ReagentCode) || v.ReagentCode == item.ReagentCode)
                    .Where(v => string.IsNullOrEmpty(v.SampleTypeCode) || v.SampleTypeCode == examInfo.SampleTypeCode)
                    .Where(v => string.IsNullOrEmpty(v.GenderCode) || v.GenderCode == examInfo.GenderCode)
                    .Where(v => string.IsNullOrEmpty(v.EntrustHospitalCode) || v.EntrustHospitalCode == examInfo.EntrustHospitalCode)
                    .Where(v => v.AgeLowValue <= ageMinutes && v.AgeUpperValue > ageMinutes)
                    .OrderBy(v => v.Sort)
                    .FirstAsync();

                var currBaseItem = items.FirstOrDefault(v => v.InstrumentItemCode == instrumentItemCode && v.ItemCode == item.ItemCode);
                if (currBaseItem != null)
                {
                    item.ItemNameEN = currBaseItem.ItemEN;
                    item.ItemNameAB = currBaseItem.ItemAB;
                    item.ItemUnit = currBaseItem.ItemUnit;
                    item.MethodCode = currBaseItem.MethodCode;
                    item.MethodName = currBaseItem.MethodName;
                    item.IsReportShow = currBaseItem.IsReportShow;
                    item.ReportOrder = currBaseItem.ReportOrder;
                    item.MethodBasis = currBaseItem.MethodBasis;
                    item.ResultType = currBaseItem.ResultType;
                    item.IsCalculate = currBaseItem.IsCalculate;
                    item.CalcExpression = currBaseItem.CalcExpression;
                    item.ItemReference = itemReference?.ReferenceRange;
                    item.WarningRange = itemReference?.WarningRange;
                    item.CriticalRange = itemReference?.CriticalRange;
                    item.DisplayRange = itemReference?.DisplayRange;
                    if (input.IsReceive)
                        item.ItemResult = currBaseItem.DefaultValue;
                }
            }
        }

        await _examResultRep.AsUpdateable(resultList)
            .UpdateColumns(v =>
            new
            {
                v.ItemNameEN,
                v.ItemNameAB,
                v.ItemUnit,
                v.MethodName,
                v.MethodCode,
                v.IsReportShow,
                v.ReportOrder,
                v.MethodBasis,
                v.ResultType,
                v.IsCalculate,
                v.CalcExpression,
                v.ItemReference,
                v.WarningRange,
                v.CriticalRange,
                v.DisplayRange
            }, true)
            .ExecuteCommandAsync();

        return true;
    }
    /// <summary>
    /// 计算高低标记
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ExamResultDto> CalcAbnormal(ExamResultUpdateInput input)
    {
        var result = await _examResultRep.GetFirstAsync(v => v.Id == input.Id);
        if (result == null)
            throw ResultOutput.Exception($"未找到结果{input.Id}");

        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == result.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception($"未找到检验信息{result.ExamInfoId}");

        string abnormalFlag = "";
        string judgeCondition = "";
        bool saveResult = false;
        if (!string.IsNullOrWhiteSpace(result.ItemResult))
        {
            if (result.ResultType == "101" && !string.IsNullOrWhiteSpace(result.ItemReference))//定量
            {
                var itemAccuracy = 0;
                var itemPersonal = await _itemPersonalizeRep.GetFirstAsync(v => v.ItemCode == input.ItemCode);
                if (itemPersonal != null && itemPersonal.ResultAccuracy != null)
                    itemAccuracy = itemPersonal.ResultAccuracy!.Value;

                var tmpResult = ItemReferenceHelper.ConvertResultAccuracy(result.ItemResult!, itemAccuracy);
                if (result.ItemResult != tmpResult)
                {
                    result.ItemResult = tmpResult;
                    saveResult = true;
                }

                var refRange = StringHelper.ReplaceReferenceOperator(result.ItemReference);
                var tmp = ItemReferenceHelper.CalcQuantitativeAbnormal(result.ItemResult!, refRange);
                abnormalFlag = tmp.Item1;
                judgeCondition = tmp.Item2;

                if (!string.IsNullOrWhiteSpace(result.CriticalRange))
                {
                    refRange = StringHelper.ReplaceReferenceOperator(result.CriticalRange);
                    tmp = ItemReferenceHelper.CalcQuantitativeAbnormal(result.ItemResult!, refRange, "Critical");
                    var tmpAbnormal = tmp.Item1;
                    if (!string.IsNullOrEmpty(tmpAbnormal))
                    {
                        abnormalFlag = tmpAbnormal;
                        judgeCondition = tmp.Item2;
                    }
                }
                if (string.IsNullOrWhiteSpace(abnormalFlag) && !string.IsNullOrWhiteSpace(result.WarningRange))
                {
                    refRange = StringHelper.ReplaceReferenceOperator(result.WarningRange);
                    tmp = ItemReferenceHelper.CalcQuantitativeAbnormal(refRange, result.ItemResult!);
                    abnormalFlag = tmp.Item1;
                    judgeCondition = tmp.Item2;
                }
            }
            else if (result.ResultType == "102")//定性
            {
                if ((result.ItemResult!.Contains("+")
                            || result.ItemResult!.Contains("阳性")
                            || result.ItemResult!.Contains("检出"))
                             && !result.ItemResult!.Contains('-')
                             && !result.ItemResult!.Contains("阴性")
                             && !result.ItemResult!.Contains("灰区")
                             && !result.ItemResult!.Contains("已报告")
                             && !result.ItemResult!.Contains("未检出")
                             && !result.ItemResult!.Contains("/"))
                {
                    abnormalFlag = "H";
                }
            }
            else if (result.ResultType == "103")//字符型
            {

            }
            else if (result.ResultType == "104" && !string.IsNullOrWhiteSpace(result.ItemReference))//固定结果
            {
                if (!result.ItemResult.Equals(result.ItemReference))
                    abnormalFlag = "H";
            }
        }

        result.HLFlag = abnormalFlag;

        await _examResultRep.AsUpdateable()
            .SetColumns(v => v.HLFlag == abnormalFlag)
            .SetColumnsIF(saveResult, v => v.ItemResult == result.ItemResult)
            .Where(v => v.Id == result.Id)
            .ExecuteCommandAsync();

        if (abnormalFlag != "W" && !string.IsNullOrWhiteSpace(abnormalFlag))
        {
            await _examInfoRep.AsUpdateable()
                .SetColumns(v => v.IsExceptionResult == 1)
                .Where(v => v.Id == examInfo.Id)
                .ExecuteCommandAsync();
        }
        var ret = result.Adapt<ExamResultDto>();
        if (ret.IsCriticalValue)
            ret.JudgeCondition = judgeCondition;
        return ret;
    }
    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task<AuditResultDto> Audit(AuditInput input)
    {
        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("检验信息不存在！");
        if (string.IsNullOrWhiteSpace(examInfo.PurCodes))
            throw ResultOutput.Exception("目的不可为空！");

        var ret = new AuditResultDto();
        var ckResult = SampleStatusHelper.CheckStatus(examInfo.SampleStatus, input.AuditType);
        if (!string.IsNullOrWhiteSpace(ckResult))
            throw ResultOutput.Exception(ckResult);

        if (!examInfo.IsPdfReport)
        {
            var resultList = await _examResultRep.GetListAsync(v => v.ExamInfoId == examInfo.Id);
            if (resultList.Exists(v => string.IsNullOrWhiteSpace(v.ItemResult)))
                throw ResultOutput.Exception($"检验结果不可为空！");
        }

        var hasCriticalValue = await _criticalValueRep.IsAnyAsync(v => v.ExamInfoId == examInfo.Id && v.ProcessStatus == 0);
        if (hasCriticalValue)
            throw ResultOutput.Exception($"存在危急值未处理！");

        var uid = input.TeacherId == null ? AppInfo.User.Id : input.TeacherId.Value;
        ckResult = await CheckUserGroupPermission(uid, examInfo.GroupCode, input.AuditType);
        if (!string.IsNullOrWhiteSpace(ckResult))
            throw ResultOutput.Exception(ckResult);

        var forceRuleContent = string.Empty;
        if (input.IgnoreAuditRuleCodes.CheckNull() && input.AuditType == OperationTypeEnum.FirstCheck)
        {
            var testRuleResult = await _auditRuleService.TestRule(new TestRuleInput
            {
                ExamInfoId = examInfo.Id,
                ExecuteType = input.ExecuteType,
                IgnoreRuleCodes = input.IgnoreAuditRuleCodes
            });
            if (!testRuleResult.TriggerRules.CheckNull())
            {
                ret.TriggerRules = testRuleResult.TriggerRules;
                return ret;
            }
        }
        else
        {
            if (!input.IgnoreAuditRuleCodes.CheckNull())
                forceRuleContent = string.Join(",", input.IgnoreAuditRuleCodes);
        }

        List<long> purposeIds = new List<long>();
        var task = await _taskRep.GetFirstAsync(v => v.Id == examInfo.TaskId);
        purposeIds = task.ApplyPurposeIds.Split(',').Select(long.Parse).ToList();

        var purposeList = await _purposeRep.GetListAsync(v => purposeIds.Contains(v.Id) && v.AddType != 2 && v.SampleStatus != SampleStatusEnum.ReportCancel.ToInt());

        var reportPriority = 70;
        var reportExtend = await _reportExtendRep.GetFirstAsync(v => v.CustomerCode == examInfo.CustomerCode);
        if (reportExtend != null)
            reportPriority = reportExtend.ReportPriority ?? 70;

        if (input.AuditType == OperationTypeEnum.FirstCheck)
        {
            purposeList.ForEach(v =>
            {
                v.SampleStatus = SampleStatusEnum.FirstCheck.ToInt();
                v.SampleStatusName = SampleStatusEnum.FirstCheck.ToDescription();
            });
            examInfo.SampleStatus = SampleStatusEnum.FirstCheck.ToInt();
            examInfo.SampleStatusName = SampleStatusEnum.FirstCheck.ToDescription();
            examInfo.FirstAuditId = AppInfo.User.Id;
            examInfo.FirstAuditName = AppInfo.User.Name;
            examInfo.FirstAuditAuthorizedId = AppInfo.User.Id;
            examInfo.FirstAuditTime = DateTime.Now;
        }
        else if (input.AuditType == OperationTypeEnum.SecondCheck)
        {
            purposeList.ForEach(v =>
            {
                v.SampleStatus = SampleStatusEnum.SecondCheck.ToInt();
                v.SampleStatusName = SampleStatusEnum.SecondCheck.ToDescription();
            });
            examInfo.SampleStatus = SampleStatusEnum.SecondCheck.ToInt();
            examInfo.SampleStatusName = SampleStatusEnum.SecondCheck.ToDescription();
            examInfo.SecondAuditId = AppInfo.User.Id;
            examInfo.SecondAuditName = AppInfo.User.Name;
            examInfo.SecondAuditAuthorizedId = AppInfo.User.Id;
            examInfo.SecondAuditTime = DateTime.Now;
            examInfo.ReviewCount += 1;
        }
        await _examInfoRep.AsUpdateable(examInfo)
            .SetUpdateable()
            .UpdateColumnsIF(input.AuditType == OperationTypeEnum.FirstCheck, v => new
            {
                v.SampleStatus,
                v.SampleStatusName,
                v.FirstAuditId,
                v.FirstAuditName,
                v.FirstAuditAuthorizedId,
                v.FirstAuditTime
            })
            .UpdateColumnsIF(input.AuditType == OperationTypeEnum.SecondCheck, v => new
            {
                v.SampleStatus,
                v.SampleStatusName,
                v.SecondAuditId,
                v.SecondAuditName,
                v.SecondAuditAuthorizedId,
                v.SecondAuditTime,
                v.ReviewCount
            })
            .ExecuteCommandAsync();

        string content = $"标本审核，操作类型:{input.AuditType.ToDescription()}";
        if (!string.IsNullOrWhiteSpace(forceRuleContent))
            content += $",忽略规则：{forceRuleContent}";
        var sampleTrack = new ExamSampleTrackDto
        {
            Barcode = examInfo.Barcode,
            GroupCode = examInfo.GroupCode,
            GroupName = examInfo.GroupName,
            TestDate = examInfo.TestDate,
            SampleNo = examInfo.SampleNo,
            OperationType = input.AuditType,
            TrackContent = content,
        };

        await _purposeRep.AsUpdateable(purposeList)
            .UpdateColumns(v => new { v.SampleStatus, v.SampleStatusName })
            .ExecuteCommandAsync();

        await _sampleTrackRep.InsertAsync(sampleTrack.Adapt<ExamSampleTrackEntity>());

        if (input.AuditType == OperationTypeEnum.SecondCheck)
        {
            var rptTask = new ReportTaskAddInput
            {
                ExamInfoId = examInfo.Id,
                Barcode = examInfo.Barcode,
                TaskType = ReportTaskTypeEnum.Normal.ToInt(),
                TaskPriority = 70
            };
            await _reportTaskRep.InsertAsync(rptTask.Adapt<ReportTaskEntity>());
        }

        ret.ExamInfo = examInfo.Adapt<ExamInfoDto>();

        return ret;
    }
    /// <summary>
    /// 反审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<ExamInfoDto> UnAudit(UnAuditInput input)
    {
        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("检验信息不存在！");

        var ckResult = SampleStatusHelper.CheckStatus(examInfo.SampleStatus, OperationTypeEnum.UnChecked);
        if (!string.IsNullOrWhiteSpace(ckResult))
            throw ResultOutput.Exception(ckResult);

        var operType = examInfo.SampleStatus == SampleStatusEnum.Printed.ToInt() || examInfo.DownloadFlag == 1 ? OperationTypeEnum.PrintedUnChecked : OperationTypeEnum.UnChecked;

        var groupCode = examInfo.GroupCode == LimsConsts.PathologyGroupCode ? examInfo.WFCode : examInfo.GroupCode;
        ckResult = await CheckUserGroupPermission(AppInfo.User.Id, groupCode, operType);
        if (!string.IsNullOrWhiteSpace(ckResult))
            throw ResultOutput.Exception(ckResult);

        var taskId = examInfo.TaskId;
        var originalSampleStatus = examInfo.SampleStatus.ToInt();
        bool isFirstUnCheck = examInfo.SampleStatus == SampleStatusEnum.FirstCheck.ToInt();

        if (examInfo.GroupCode != LimsConsts.PathologyGroupCode
            || (examInfo.GroupCode == LimsConsts.PathologyGroupCode && isFirstUnCheck)
            || (examInfo.GroupCode == LimsConsts.PathologyGroupCode && !isFirstUnCheck && examInfo.FirstAuditId is null))
        {
            examInfo.FirstAuditId = null;
            examInfo.FirstAuditName = null;
            examInfo.FirstAuditAuthorizedId = null;
            examInfo.FirstAuditTime = null;
            examInfo.SampleStatus = SampleStatusEnum.Testing.ToInt();
            examInfo.SampleStatusName = SampleStatusEnum.Testing.ToDescription();
        }
        else if (examInfo.GroupCode == LimsConsts.PathologyGroupCode && !isFirstUnCheck && examInfo.FirstAuditId is not null)
        {
            examInfo.SampleStatus = SampleStatusEnum.FirstCheck.ToInt();
            examInfo.SampleStatusName = SampleStatusEnum.FirstCheck.ToDescription();
        }

        examInfo.SecondAuditId = null;
        examInfo.SecondAuditName = null;
        examInfo.SecondAuditAuthorizedId = null;
        examInfo.SecondAuditTime = null;
        examInfo.CreateReportTime = null;
        examInfo.PrintReportTime = null;
        examInfo.DownloadFlag = null;

        await _purposeRep.AsUpdateable()
            .SetColumns(v => v.SampleStatus == examInfo.SampleStatus)
            .SetColumns(v => v.SampleStatusName == examInfo.SampleStatusName)
            .Where(v => v.Barcode == examInfo.Barcode && v.TaskId == examInfo.TaskId && v.AddType != 2 && v.SampleStatus == originalSampleStatus)
            .ExecuteCommandAsync();

        await _examInfoRep.AsUpdateable(examInfo)
            .UpdateColumns(v => new
            {
                v.SampleStatus,
                v.SampleStatusName,
                v.FirstAuditId,
                v.FirstAuditName,
                v.FirstAuditAuthorizedId,
                v.FirstAuditTime,
                v.SecondAuditId,
                v.SecondAuditName,
                v.SecondAuditAuthorizedId,
                v.SecondAuditTime,
                v.CreateReportTime,
                v.PrintReportTime,
                v.DownloadFlag
            })
            .Where(v => v.Id == examInfo.Id)
            .ExecuteCommandAsync();

        var content = isFirstUnCheck ? "初审反审核" : "复审反审核";
        var sampleTrack = new ExamSampleTrackDto
        {
            Barcode = examInfo.Barcode,
            GroupCode = examInfo.GroupCode!,
            GroupName = examInfo.GroupName!,
            TestDate = examInfo.TestDate,
            SampleNo = examInfo.SampleNo,
            OperationType = OperationTypeEnum.UnChecked,
            TrackContent = $"{content}{(string.IsNullOrWhiteSpace(input.ReasonContent) ? "" : "，" + input.ReasonContent)}"
        };

        var unAuditLog = new ExamUnAuditLogAddInput
        {
            ExamInfoId = examInfo.Id,
            PurCodes = examInfo.PurCodes,
            PurNames = examInfo.PurNames,
            ReasonCode = input.ReasonCode,
            ReasonContent = input.ReasonContent,
            UnAuditType = isFirstUnCheck ? 0 : 1
        };

        await _reportFileRep
            .SetColumnUpdateable(v => v.IsValid == false)
            .Where(v => v.ExamInfoId == examInfo.Id)
            .ExecuteCommandAsync();

        await _sampleTrackRep.InsertAsync(sampleTrack.Adapt<ExamSampleTrackEntity>());
        await _unAuditLogRep.InsertAsync(unAuditLog.Adapt<ExamUnAuditLogEntity>());

        return examInfo.Adapt<ExamInfoDto>();
    }
    /// <summary>
    /// 增项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ExamInfoDto> AddItem(AddOrDeletePurposeInput input)
    {
        if (input.PurCodes.CheckNull() && input.ComboCodes.CheckNull())
            throw ResultOutput.Exception("请传入目的！");

        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("检验信息不存在！");

        var purCodes = input.PurCodes;
        var comboCodes = input.ComboCodes;
        if (!comboCodes.CheckNull())
        {
            var comboDetails = await _baseComboRep.AsQueryable()
                                .InnerJoin<BaseComboDetailEntity>((a, b) => a.ComboCode == b.ComboCode && b.IsValid)
                                .Select((a, b) => new BaseComboDetailDto
                                {
                                    ComboCode = a.ComboCode,
                                    PurCode = b.PurCode
                                })
                                .ToListAsync();

            var groupedDetails = comboDetails
                                .GroupBy(cd => cd.PurCode)
                                .Where(g => g.Select(x => x.ComboCode).Distinct().Count() > 1)
                                .Select(g => new
                                {
                                    PurCode = g.Key,
                                    ComboCodes = g.Select(x => x.ComboCode).Distinct().ToList(),
                                    Records = g.ToList()
                                })
                                .ToList();

            if (groupedDetails.Any())
            {
                var first = groupedDetails.First();
                throw ResultOutput.Exception($"组套{first.ComboCodes}中存在{first.PurCode}重复！");
            }

            var comboPurCodes = comboDetails.Select(v => v.PurCode).Distinct().ToList();
            if (purCodes.Intersect(comboCodes).Any())
            {
                var intersect = purCodes.Intersect(comboCodes);
                throw ResultOutput.Exception($"目的{intersect.First()}重复！");
            }

            purCodes.AddRange(comboPurCodes);
        }

        var allPurposes = await _purposeRep.GetListAsync(v => v.Barcode == examInfo.Barcode && v.IsDeleted == false && v.SampleStatus != SampleStatusEnum.ReportCancel.ToInt() && v.AddType != 2);
        var allPurCodes = allPurposes.Select(v => v.PurCode).Distinct().ToList();

        var notExistsPurpose = purCodes.Except(allPurCodes).ToList();
        if (notExistsPurpose == null || notExistsPurpose.Count == 0)
            throw ResultOutput.Exception("增项失败，目的已存在！");

        var ckResult = SampleStatusHelper.CheckStatus(examInfo.SampleStatus, OperationTypeEnum.AddItem);
        if (!string.IsNullOrWhiteSpace(ckResult))
            throw ResultOutput.Exception(ckResult);

        var applyInfo = await _applyInfoRep.GetFirstAsync(v => v.Barcode == examInfo.Barcode);

        var currApplyPurpose = await _purposeRep.AsQueryable()
            .ClearFilter<IDeletedFilter>()
            .Where(a => a.TaskId == examInfo.TaskId)
            .ToListAsync();

        var purposeDetail = await _basePurposeService.GetPurposeDetail(notExistsPurpose, examInfo.CustomerCode!);
        var examTask = await _taskRep.GetFirstAsync(v => v.Id == examInfo.TaskId);

        List<ExamTaskDetailDto> taskDetailList = new();
        List<ApplyPurposeDto> applyPurposeList = new List<ApplyPurposeDto>();
        List<ApplyItemDto> applyItemList = new List<ApplyItemDto>();
        List<ExamSampleTrackDto> sampleTrackList = new();

        List<ExamResultAddInput> resultList = new List<ExamResultAddInput>();
        foreach (var currBasePurpose in purposeDetail.GroupBy(v => v.PurCode))
        {
            if (!applyInfo.PurCodes.Split(',').Contains(currBasePurpose.Key))
            {
                applyInfo.PurCodes += $",{currBasePurpose.Key}";
                if (!string.IsNullOrWhiteSpace(currBasePurpose.First().PurNamePersonalize))
                    applyInfo.PurNames += $",{currBasePurpose.First().PurNamePersonalize}";
                else
                    applyInfo.PurNames += $",{currBasePurpose.First().PurName}";
            }

            examTask.PurCodes += $",{currBasePurpose.Key}";
            examInfo.PurCodes += $",{currBasePurpose.Key}";
            if (!string.IsNullOrWhiteSpace(currBasePurpose.First().PurNamePersonalize))
            {
                examTask.PurNames += $",{currBasePurpose.First().PurNamePersonalize}";
                examInfo.PurNames += $",{currBasePurpose.First().PurNamePersonalize}";
            }
            else
            {
                examTask.PurNames += $",{currBasePurpose.First().PurName}";
                examInfo.PurNames += $",{currBasePurpose.First().PurName}";
            }

            string comboCode = null;
            string comboName = null;
            if (currApplyPurpose.Exists(v => !string.IsNullOrWhiteSpace(v.ComboCode) && v.PurCode == currBasePurpose.First().PurCode))
            {
                var firstPurpose = currApplyPurpose.First(v => !string.IsNullOrWhiteSpace(v.ComboCode) && v.PurCode == currBasePurpose.First().PurCode);
                comboCode = firstPurpose.ComboCode;
                comboName = firstPurpose.ComboName;
            }

            long purposeId = YitIdHelper.NextId();
            examTask.ApplyPurposeIds += $",{purposeId}";

            applyPurposeList.Add(new ApplyPurposeDto
            {
                Id = purposeId,
                GroupCode = examInfo.GroupCode,
                GroupName = examInfo.GroupName,
                Barcode = examInfo.Barcode,
                ComboCode = comboCode,
                ComboName = comboName,
                PurCode = currBasePurpose.Key,
                PurName = currBasePurpose.First().PurName,
                PurNamePersonalize = currBasePurpose.First().PurNamePersonalize,
                PurAmount = currApplyPurpose.First().PurAmount,
                InstrumentItemCodes = currBasePurpose.First().InstrumentItemCode,
                WorkFlowType = "Routine",
                SampleTypeCode = examInfo.SampleTypeCode,
                SampleTypeName = examInfo.SampleTypeName,
                EstimateTestDate = currApplyPurpose.First().EstimateTestDate,
                EstimateReportTime = currApplyPurpose.First().EstimateReportTime,
                EntrustHospitalCode = examInfo.EntrustHospitalCode,
                EntrustHospitalName = examInfo.EntrustHospitalName,
                EntrustStatus = examInfo.EntrustStatus,
                EstimateAskTime = currApplyPurpose.First().EstimateAskTime,
                OriginalGroupCode = currBasePurpose.First().GroupCode,
                OriginalGroupName = currBasePurpose.First().GroupName,
                AddType = 1,
                SampleStatus = examInfo.SampleStatus,
                SampleStatusName = examInfo.SampleStatusName,
                ReceiveId = currApplyPurpose.First().ReceiveId,
                ReceiveName = currApplyPurpose.First().ReceiveName,
                ReceiveTime = currApplyPurpose.First().ReceiveTime,
                TestSeries = currApplyPurpose.First().TestSeries,
                SortStatus = 0,
                DataSource = currApplyPurpose.First().DataSource,
                BatchNo = currApplyPurpose.First().BatchNo,
                BatchScanOrder = currApplyPurpose.First().BatchScanOrder,
                TaskId = examInfo.TaskId
            });

            foreach (var baseItem in currBasePurpose)
            {
                long applyItemId = YitIdHelper.NextId();
                applyItemList.Add(new ApplyItemDto
                {
                    Id = applyItemId,
                    ApplyPurposeId = purposeId,
                    GroupCode = examInfo.GroupCode,
                    GroupName = examInfo.GroupName,
                    Barcode = examInfo.Barcode,
                    ComboCode = comboCode,
                    PurCode = currBasePurpose.Key,
                    InstrumentItemCode = baseItem.InstrumentItemCode,
                    ItemCode = baseItem.ItemCode,
                    ItemName = baseItem.ItemName,
                    ItemNamePersonalize = baseItem.ItemNamePersonalize
                });

                long taskDetailId = YitIdHelper.NextId();
                taskDetailList.Add(new ExamTaskDetailDto
                {
                    Id = taskDetailId,
                    Barcode = examInfo.Barcode,
                    GroupCode = examInfo.GroupCode,
                    TaskId = examInfo.TaskId,
                    ApplyItemId = applyItemId,
                    ComboCode = comboCode,
                    PurCode = currBasePurpose.Key,
                    InstrumentItemCode = baseItem.InstrumentItemCode,
                    ItemCode = baseItem.ItemCode,
                    ItemName = baseItem.ItemName,
                    ItemNamePersonalize = baseItem.ItemNamePersonalize,
                    InTest = 0
                });

                resultList.Add(new ExamResultAddInput
                {
                    TaskDetailId = taskDetailId,
                    ExamInfoId = examInfo.Id,
                    GroupCode = examInfo.GroupCode,
                    GroupName = examInfo.GroupName,
                    Barcode = examInfo.Barcode,
                    SampleNo = examInfo.SampleNo,
                    TestDate = examInfo.TestDate,
                    ComboCode = comboCode,
                    PurCode = currBasePurpose.Key,
                    PurName = baseItem.PurName,
                    InstrumentItemCode = baseItem.InstrumentItemCode,
                    ItemCode = baseItem.ItemCode,
                    ItemName = baseItem.ItemName,
                    ItemNamePersonalize = baseItem.ItemNamePersonalize
                });
            }
        }

        examInfo.PurCodes = examInfo.PurCodes.Trim(',');
        examInfo.PurNames = examInfo.PurNames.Trim(',');
        examTask.PurCodes = examTask.PurCodes.Trim(',');
        examTask.PurNames = examTask.PurNames.Trim(',');
        applyInfo.PurNames = applyInfo.PurNames.Trim(',');
        applyInfo.PurNames = applyInfo.PurNames.Trim(',');

        var names = purposeDetail.ToDistinct((a, b) => a.PurCode == b.PurCode).Select(v => (!string.IsNullOrWhiteSpace(v.PurNamePersonalize) ? v.PurNamePersonalize : v.PurName) + $"({v.PurCode})");

        sampleTrackList.Add(new ExamSampleTrackDto
        {
            Barcode = examInfo.Barcode,
            GroupCode = examInfo.GroupCode,
            GroupName = examInfo.GroupName,
            SampleNo = examInfo.SampleNo,
            TestDate = examInfo.TestDate,
            OperationType = OperationTypeEnum.AddItem,
            TrackContent = $"增项，目的：{string.Join(",", names)}",
        });

        _examInfoRep.Context.Ado.BeginTran();

        try
        {
            await _applyInfoRep.AsUpdateable(applyInfo)
                .UpdateColumns(v => new
                {
                    v.PurCodes,
                    v.PurNames
                }, true)
                .ExecuteCommandAsync();
            await _purposeRep.InsertRangeAsync(applyPurposeList.Adapt<List<ApplyPurposeEntity>>());
            await _applyItemRep.InsertRangeAsync(applyItemList.Adapt<List<ApplyItemEntity>>());

            await _taskRep.AsUpdateable(examTask)
                .UpdateColumns(v => new
                {
                    v.PurCodes,
                    v.PurNames,
                    v.ApplyPurposeIds
                }, true)
                .ExecuteCommandAsync();
            await _taskDetailRep.InsertRangeAsync(taskDetailList.Adapt<List<ExamTaskDetailEntity>>());

            await _examInfoRep.AsUpdateable(examInfo)
                .UpdateColumns(v => new
                {
                    v.PurCodes,
                    v.PurNames
                }, true)
                .ExecuteCommandAsync();
            await _examResultRep.InsertRangeAsync(resultList.Adapt<List<ExamResultEntity>>());

            await _sampleTrackRep.InsertRangeAsync(sampleTrackList.Adapt<List<ExamSampleTrackEntity>>());

            _examInfoRep.Context.Ado.CommitTran();
            _publisher.Publish(LimsConsts.RefreshInfoEvent, new RefreshExamInfoInput { ExamInfoId = examInfo.Id });
            _publisher.Publish(LimsConsts.GenerateFinanceData, examInfo);
        }
        catch (Exception ex)
        {
            _examInfoRep.Context.Ado.RollbackTran();
            throw;
        }
        return examInfo.Adapt<ExamInfoDto>();
    }

    /// <summary>
    /// 退项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ExamInfoDto> BackItem(AddOrDeletePurposeInput input)
    {
        if (input.PurCodes.CheckNull())
            throw ResultOutput.Exception("请传入目的！");

        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("检验信息不存在！");

        var ckResult = SampleStatusHelper.CheckStatus(examInfo.SampleStatus, OperationTypeEnum.DeleteItem);
        if (!string.IsNullOrWhiteSpace(ckResult))
            throw ResultOutput.Exception(ckResult);

        List<ExamSampleTrackDto> sampleTrackList = new();
        var applyInfo = await _applyInfoRep.GetFirstAsync(v => v.Barcode == examInfo.Barcode);
        var task = await _taskRep.GetFirstAsync(v => v.Id == examInfo.TaskId);
        var allPurposes = await _purposeRep.GetListAsync(v => v.Barcode == examInfo.Barcode && v.IsDeleted == false && v.SampleStatus != SampleStatusEnum.ReportCancel.ToInt());
        if (allPurposes.Any() && allPurposes.Exists(v => input.PurCodes!.Contains(v.PurCode)))
        {
            var needDelApplyPurposes = allPurposes.FindAll(v => v.TaskId == task.Id && input.PurCodes!.Contains(v.PurCode));
            if (needDelApplyPurposes.Any())
            {
                var delPurIds = needDelApplyPurposes.Select(v => v.Id).Distinct().ToList();
                var delPurCodes = needDelApplyPurposes.Select(v => v.PurCode).Distinct().ToList();
                var needDelApplyItems = await _applyItemRep.GetListAsync(v => delPurIds.Contains(v.ApplyPurposeId));

                var delApplyItemIds = needDelApplyItems.Select(v => v.Id).Distinct().ToList();

                var needDelTaskDetails = await _taskDetailRep.GetListAsync(v => v.TaskId == task.Id && delApplyItemIds.Contains(v.ApplyItemId));

                var delTaskDetailIds = needDelTaskDetails.Select(v => v.Id).Distinct().ToList();
                var needDelExamResults = await _examResultRep.GetListAsync(v => v.ExamInfoId == examInfo.Id && delTaskDetailIds.Contains(v.TaskDetailId));

                needDelApplyPurposes.ForEach(v => v.AddType = 2);
                needDelApplyItems.ForEach(v => v.IsDeleted = true);
                needDelTaskDetails.ForEach(v => v.IsDeleted = true);
                needDelExamResults.ForEach(v =>
                {
                    v.IsDeleted = true;
                    v.AddType = 2;
                });

                var inuseAllPurCodes = allPurposes.FindAll(v => v.AddType != 2);
                applyInfo.PurCodes = string.Join(",", inuseAllPurCodes.Select(v => v.PurCode).Distinct());
                applyInfo.PurNames = string.Join(",", inuseAllPurCodes.Select(v => !string.IsNullOrWhiteSpace(v.PurNamePersonalize) ? v.PurNamePersonalize : v.PurName).Distinct());

                var inuseCurrentPurpose = allPurposes.FindAll(v => v.TaskId == task.Id && v.AddType != 2);
                examInfo.PurCodes = task.PurCodes = string.Join(",", inuseCurrentPurpose.Select(v => v.PurCode).Distinct());
                examInfo.PurNames = task.PurNames = string.Join(",", inuseCurrentPurpose.Select(v => !string.IsNullOrWhiteSpace(v.PurNamePersonalize) ? v.PurNamePersonalize : v.PurName).Distinct());

                _examInfoRep.Context.Ado.BeginTran();
                try
                {
                    await _applyInfoRep.AsUpdateable(applyInfo)
                          .UpdateColumns(v => new
                          {
                              v.PurCodes,
                              v.PurNames
                          }, true)
                          .ExecuteCommandAsync();

                    await _purposeRep.AsUpdateable(needDelApplyPurposes)
                        .UpdateColumns(v => new { v.AddType }, true)
                        .ExecuteCommandAsync();

                    await _applyItemRep.AsUpdateable(needDelApplyItems)
                        .UpdateColumns(v => new { v.IsDeleted }, true)
                        .ExecuteCommandAsync();

                    await _taskRep.AsUpdateable(task)
                        .UpdateColumns(v => new
                        {
                            v.PurCodes,
                            v.PurNames
                        }, true)
                        .ExecuteCommandAsync();

                    await _taskDetailRep.AsUpdateable(needDelTaskDetails)
                        .UpdateColumns(v => new { v.IsDeleted }, true)
                        .ExecuteCommandAsync();

                    await _examInfoRep.AsUpdateable(examInfo)
                        .UpdateColumns(v => new
                        {
                            v.PurCodes,
                            v.PurNames
                        }, true)
                        .ExecuteCommandAsync();

                    await _examResultRep.AsUpdateable(needDelExamResults)
                        .UpdateColumns(v => new { v.IsDeleted }, true)
                        .ExecuteCommandAsync();

                    var names = needDelApplyPurposes.ToDistinct((a, b) => a.PurCode == b.PurCode).Select(v => (!string.IsNullOrWhiteSpace(v.PurNamePersonalize) ? v.PurNamePersonalize : v.PurName) + $"({v.PurCode})");
                    sampleTrackList.Add(new ExamSampleTrackDto
                    {
                        Barcode = examInfo.Barcode,
                        GroupCode = examInfo.GroupCode,
                        GroupName = examInfo.GroupName,
                        SampleNo = examInfo.SampleNo,
                        TestDate = examInfo.TestDate,
                        OperationType = OperationTypeEnum.AddItem,
                        TrackContent = $"退项，目的：{string.Join(",", names)}",
                    });
                    await _sampleTrackRep.InsertRangeAsync(sampleTrackList.Adapt<List<ExamSampleTrackEntity>>());

                    _examInfoRep.Context.Ado.CommitTran();
                    _publisher.Publish(LimsConsts.GenerateFinanceData, examInfo);
                }
                catch (Exception ex)
                {
                    _examInfoRep.Context.Ado.RollbackTran();
                    throw;
                }
            }
        }
        return examInfo.Adapt<ExamInfoDto>();
    }

    /// <summary>
    /// 取消检测
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task<ExamInfoDto> CancelTest(CancelTestInput input)
    {
        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("检验信息不存在！");

        var ckResult = SampleStatusHelper.CheckStatus(examInfo.SampleStatus, OperationTypeEnum.CancelTest);
        if (!string.IsNullOrWhiteSpace(ckResult))
            throw ResultOutput.Exception(ckResult);

        var task = await _taskRep.GetByIdAsync(examInfo.TaskId);
        var sampleStatus = SampleStatusEnum.ReportCancel.ToInt();
        var sampleStatusName = SampleStatusEnum.ReportCancel.ToDescription();
        examInfo.SampleStatus = sampleStatus;
        examInfo.SampleStatusName = sampleStatusName;

        await _purposeRep.SetUpdateable()
            .SetColumns(v => new ApplyPurposeEntity
            {
                SampleStatus = sampleStatus,
                SampleStatusName = sampleStatusName
            })
            .Where(v => v.Barcode == examInfo.Barcode && v.TaskId == task.Id)
            .ExecuteCommandAsync();

        await _examInfoRep
            .AsUpdateable(examInfo)
            .SetUpdateable()
            .UpdateColumns(v => new ExamInfoEntity
            {
                SampleStatus = sampleStatus,
                SampleStatusName = sampleStatusName
            })
            .ExecuteCommandAsync();

        var sampleTrack = new ExamSampleTrackDto
        {
            Barcode = examInfo.Barcode,
            GroupCode = examInfo.GroupCode,
            GroupName = examInfo.GroupName,
            TestDate = examInfo.TestDate,
            SampleNo = examInfo.SampleNo,
            OperationType = OperationTypeEnum.CancelTest,
            TrackContent = $"取消检测，原因:{input.ReasonContent}",
        };

        await _sampleTrackRep.InsertAsync(sampleTrack.Adapt<ExamSampleTrackEntity>());
        return examInfo.Adapt<ExamInfoDto>();
    }

    /// <summary>
    /// 校验组别权限
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="groupCode"></param>
    /// <param name="operType"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<string> CheckUserGroupPermission(long uid, string groupCode, OperationTypeEnum operType)
    {
        var allGroupCode = new List<string>();
        var group = await _baseGroupRep.GetListAsync(v => v.GroupCode == groupCode);
        if (group.Exists(v => !string.IsNullOrWhiteSpace(v.ParentCode)))
        {
            var parentCodeList = group.Select(v => v.ParentCode).Distinct().ToList();
            var parentGroup = await _baseGroupRep.GetListAsync(v => parentCodeList.Contains(v.GroupCode));
            allGroupCode = group.Select(v => v.GroupCode).Concat(parentGroup.Select(v => v.GroupCode)).ToList();
        }

        var query = _userGroupRep.AsQueryable()
            .Where(v => allGroupCode.Contains(v.GroupCode) && v.UserId == uid);
        switch (operType)
        {
            case OperationTypeEnum.FirstCheck:
                query.Where(a => a.CanFirstCheck == 1);
                break;
            case OperationTypeEnum.SecondCheck:
                query.Where(a => a.CanSecondCheck == 1);
                break;
            case OperationTypeEnum.UnChecked:
                query.Where(a => a.CanUnCheck == 1);
                break;
            case OperationTypeEnum.PrintedUnChecked:
                query.Where(a => a.CanPrintedUnCheck == 1);
                break;
        }

        var ret = await query.AnyAsync();
        if (!ret)
            return $"没有{operType.ToDescription()}权限！";

        return string.Empty;
    }

    /// <summary>
    /// 报告预览
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    [HttpPost]
    [NonFormatResult]
    public async Task<FileResult> RptPreview([FromQuery] long examInfoId)
    {
        try
        {
            var previewUrl = await _param.GetParamValue("PreviewUrl");
            if (string.IsNullOrWhiteSpace(previewUrl))
                return null;

            var url = previewUrl + "?examinfoid=" + examInfoId;
            var client = _httpFactory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var bytes = await response.Content.ReadAsByteArrayAsync();

            var ms = new MemoryStream(bytes);
            ms.Seek(0, SeekOrigin.Begin);
            var actionresult = new FileStreamResult(ms, "application/pdf");
            actionresult.FileDownloadName = $"{examInfoId}.pdf";
            return actionresult;
        }
        catch (Exception ex)
        {
            //if non formate result, must use custom exception,otherwise web can not catch the exception
            throw new AppException(ex.Message);
        }
    }

    /// <summary>
    /// 获取报告文件
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ReportFilesDto>> GetReportFiles(long examInfoId)
    {
        var isExists = await _examInfoRep.IsAnyAsync(v => v.Id == examInfoId);
        if (!isExists)
            throw ResultOutput.Exception("检验信息不存在！");

        var reportFiles = await _reportFileRep.GetListAsync(v => v.ExamInfoId == examInfoId && v.IsValid);
        return reportFiles.Adapt<List<ReportFilesDto>>();
    }
}