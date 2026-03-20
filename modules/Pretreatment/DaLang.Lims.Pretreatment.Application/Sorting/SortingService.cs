using DaLang.Lims.BaseData.Contracts.ExamPlan;
using DaLang.Lims.BaseData.Domain.BaseSorterShelfRule;
using DaLang.Lims.BaseData.Domain.ExamPlan;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.BaseData.Domain.Sequence;
using DaLang.Lims.Pretreatment.Contracts.Entrust;
using DaLang.Lims.Pretreatment.Contracts.Entrust.Dto;
using DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;
using DaLang.Lims.Pretreatment.Contracts.Sorting;
using DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Core.Enum;
using DaLang.Lims.Pretreatment.Domain.BaseSorter;
using DaLang.Lims.Pretreatment.Domain.BaseSorterShelf;
using DaLang.Lims.Pretreatment.Domain.PretreatSortSplitBlood;
using DaLang.Lims.Pretreatment.Domain.Sorting;
using DaLang.Lims.Pretreatment.Domain.SortRule;
using DaLang.Lims.Shared.Contracts.ApplyInfo.Dto;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;
using DaLang.Lims.Shared.Contracts.ExamTask.Dto;
using DaLang.Lims.Shared.Contracts.ExamTaskDetail.Dto;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyItem;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Shared.Domain.ExamSampleTrack;
using DaLang.Lims.Shared.Domain.ExamTask;
using DaLang.Lims.Shared.Domain.ExamTaskDetail;
using DaLang.Lims.Web.BaseData.Domain.BaseWorkDate;
using DaLang.Lims.Web.Common.Consts;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Helpers;
using DaLang.Lims.Web.Framework.Domain.Parameter;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Parameter;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Yitter.IdGenerator;

namespace DaLang.Lims.Pretreatment.Services.Sorting;

/// <summary>
/// 分拣服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class SortingService : BaseService, ISortingService, IDynamicApi
{
    private AdminRepositoryBase<ApplyInfoEntity> _applyInfoRep;
    private AdminRepositoryBase<ApplyItemEntity> _applyItemRep;
    private AdminRepositoryBase<ApplyPurposeEntity> _applyPurposeRep;
    private IBaseExamPlanRepository _baseExamPlanRep;
    private IBaseExamPlanDetailRepository _baseExamPlanDetailRep;
    private IBaseWorkDateRepository _workDateRep;
    private IConfiguration _config;
    private IBaseSorterRepository _baseSorterRep;
    private IBaseSorterShelfRepository _baseSorterShelfRep;
    private IBaseSorterShelfRuleRepository _baseSorterShelfRuleRep;
    private IPretreatSortInfoRepository _sortInfoRep;
    private IPretreatSortDetailRepository _sortDetailRep;
    private ICacheTool _cache;
    private IPretreatSortSplitBloodRepository _bloodRep;
    private IBaseExamPlanService _examPlanService;
    private IParameterService _paramService;
    private AdminRepositoryBase<ExamTaskEntity> _taskRep;
    private AdminRepositoryBase<ExamTaskDetailEntity> _taskDetailRep;
    private AdminRepositoryBase<ExamSampleTrackEntity> _sampleTrackRep;
    private IEntrustService _entrustService;
    public SortingService(AdminRepositoryBase<ApplyInfoEntity> applyInfoRep,
        AdminRepositoryBase<ApplyItemEntity> applyItemRep,
        AdminRepositoryBase<ApplyPurposeEntity> applyPurposeRep,
        IBaseExamPlanRepository baseExamPlanRep,
        IBaseExamPlanDetailRepository baseExamPlanDetailRep,
        IConfiguration configuration,
        IBaseWorkDateRepository workDateRep,
        IParameterRepository parameterRep,
        IBaseSorterRepository baseSorterRep,
        IBaseSorterShelfRepository baseSorterShelfRep,
        IBaseSorterShelfRuleRepository baseSorterShelfRuleRep,
        IPretreatSortInfoRepository sortInfoRep,
        IPretreatSortDetailRepository sortDetailRep,
        ICacheTool cache,
        IPretreatSortSplitBloodRepository bloodRep,
        IBaseExamPlanService examPlanService,
        IParameterService paramService,
        AdminRepositoryBase<ExamTaskEntity> taskRep,
        AdminRepositoryBase<ExamTaskDetailEntity> taskDetailRep,
        AdminRepositoryBase<ExamSampleTrackEntity> sampleTrackRep,
        IEntrustService entrustService)
    {
        _applyInfoRep = applyInfoRep;
        _applyItemRep = applyItemRep;
        _applyPurposeRep = applyPurposeRep;
        _baseExamPlanRep = baseExamPlanRep;
        _baseExamPlanDetailRep = baseExamPlanDetailRep;
        _config = configuration;
        _workDateRep = workDateRep;
        _baseSorterRep = baseSorterRep;
        _baseSorterShelfRep = baseSorterShelfRep;
        _baseSorterShelfRuleRep = baseSorterShelfRuleRep;
        _sortInfoRep = sortInfoRep;
        _sortDetailRep = sortDetailRep;
        _bloodRep = bloodRep;
        _examPlanService = examPlanService;
        _paramService = paramService;
        _taskRep = taskRep;
        _cache = cache;
        _taskDetailRep = taskDetailRep;
        _sampleTrackRep = sampleTrackRep;
        _entrustService = entrustService;
    }
    /// <summary>
    /// 开始分拣
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<string> StartSorting(StartSortingInput input)
    {
        var sorterCode = input.SorterCode;
        if (string.IsNullOrWhiteSpace(sorterCode))
            throw ResultOutput.Exception("参数有误！");

        var sorter = await _baseSorterRep.GetFirstAsync(v => v.SorterCode == sorterCode && v.IsValid);
        if (sorter == null)
            throw ResultOutput.Exception($"分拣仪{sorterCode}不存在或未启用！");

        var sorterShelf = await _baseSorterShelfRep.GetListAsync(v => v.SorterCode == sorterCode && v.IsValid && !v.IsDeleted);
        if (!sorterShelf.Any())
            throw ResultOutput.Exception($"分拣仪{sorterCode}未配置架子！");

        var shelfRule = await _baseSorterShelfRuleRep.AsQueryable()
            .InnerJoin<BaseSortRuleEntity>((a, b) => a.SortRuleCode == b.RuleCode && b.IsValid)
            .InnerJoin<BaseSequenceEntity>((a, b, c) => b.SequenceCode == c.SequenceCode && c.IsValid)
            .Where((a, b) => a.SorterCode == sorterCode && a.IsValid && !a.IsDeleted)
            .Select((a, b, c) => new DetailShelfRule
            {
                ShelfPosition = a.ShelfPosition!.Value,
                RuleCode = b.RuleCode,
                RuleName = b.RuleName,
                RuleExpression = b.RuleExpression!,
                SequenceCode = b.SequenceCode!,
                SequenceName = c.SequenceName!,
                ItemCount = b.ItemCount ?? 0,
                IsSortAll = b.IsSortAll,
                IsBatch = b.IsBatch
            }).ToListAsync();

        var sortInfoCode = YitIdHelper.NextId();
        var sortInfo = new PretreatSortInfoDto
        {
            SortInfoCode = sortInfoCode,
            SorterCode = sorterCode,
            StartTime = DateTime.Now,
            IsShelfMode = 0,
            Status = 1
        };

        List<SortDetailWithRule> sortDetail = new List<SortDetailWithRule>();
        if (input.SortMode == SortModeEnum.UseShelf)
        {

        }
        else
        {
            var idx = 0;
            foreach (var currShelf in sorterShelf.OrderBy(v => v.ShelfPosition))
            {
                idx++;
                var currShelfRule = shelfRule.FindAll(v => v.ShelfPosition == currShelf.ShelfPosition).OrderBy(v => v.Sort);

                if (!currShelfRule.Any())
                    continue;

                sortDetail.Add(new SortDetailWithRule
                {
                    Id = YitIdHelper.NextId(),
                    SortInfoCode = sortInfoCode,
                    IsShelfMode = 0,
                    ShelfPosition = currShelf.ShelfPosition,
                    ShelfCode = $"Shelf{idx}",
                    ShelfName = currShelf.ShelfName,
                    ShelfType = currShelf.ShelfType,
                    RowHoleCount = 100000,
                    ColumnHoleCount = 100000,
                    UsedCount = 0,
                    ShelfRuleList = currShelfRule.ToList()
                });
            }
        }

        await _sortInfoRep.InsertAsync(sortInfo.Adapt<PretreatSortInfoEntity>());
        await _sortDetailRep.InsertRangeAsync(sortDetail.Adapt<List<PretreatSortDetailEntity>>());
        _cache.Set(PretreatmentCacheKeys.SortInfoCache + sortInfoCode, sortDetail, TimeSpan.FromHours(24));
        return sortInfoCode.ToString();
    }

    [NonAction]
    public async Task<bool> UpdatePuposeSampleType()
    {
        return true;
    }

    /// <summary>
    /// 分拣
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<SortingOutput> Sorting(SortingInput input)
    {
        if (input == null)
            throw ResultOutput.Exception("参数有误！");

        if (string.IsNullOrWhiteSpace(input.SortInfoCode))
            throw ResultOutput.Exception("分拣代码不能为空！");

        if (!long.TryParse(input.SortInfoCode, out long sortInfoCode))
            throw ResultOutput.Exception("分拣代码有误！");

        var sortDetailsWithRule = _cache.Get<List<SortDetailWithRule>>(PretreatmentCacheKeys.SortInfoCache + input.SortInfoCode);
        if (sortDetailsWithRule == null)
        {
            var sortInfo = await _sortInfoRep.GetFirstAsync(v => v.SortInfoCode == sortInfoCode && v.Status == 1);
            if (sortInfo == null)
                throw ResultOutput.Exception("分拣信息不存在或已结束！");

            var sortDetails = await _sortDetailRep.GetListAsync(v => v.SortInfoCode == sortInfoCode);
            if (!sortDetails.Any())
                throw ResultOutput.Exception("分拣明细不存在！");

            var shelfRule = await _baseSorterShelfRuleRep.AsQueryable()
                .InnerJoin<BaseSortRuleEntity>((a, b) => a.SortRuleCode == b.RuleCode && b.IsValid)
                .InnerJoin<BaseSequenceEntity>((a, b, c) => b.SequenceCode == c.SequenceCode && c.IsValid)
                .Where((a, b) => a.SorterCode == sortInfo.SorterCode && a.IsValid && !a.IsDeleted)
                .Select((a, b, c) => new DetailShelfRule
                {
                    ShelfPosition = a.ShelfPosition!.Value,
                    RuleCode = b.RuleCode,
                    RuleName = b.RuleName,
                    RuleExpression = b.RuleExpression!,
                    SequenceCode = b.SequenceCode!,
                    SequenceName = c.SequenceName!,
                    ItemCount = b.ItemCount ?? 0,
                    IsSortAll = b.IsSortAll,
                    IsBatch = b.IsBatch
                }).ToListAsync();

            sortDetailsWithRule = sortDetails.Select(v => new SortDetailWithRule
            {
                Id = v.Id,
                SortInfoCode = sortInfoCode,
                IsShelfMode = sortInfo.IsShelfMode,
                ShelfPosition = v.ShelfPosition,
                ShelfCode = v.ShelfCode,
                ShelfName = v.ShelfName,
                ShelfType = v.ShelfType,
                RowHoleCount = v.RowHoleCount,
                ColumnHoleCount = v.ColumnHoleCount,
                UsedCount = v.UsedCount ?? 0,
                ShelfRuleList = shelfRule.FindAll(a => a.ShelfPosition == v.ShelfPosition).OrderBy(a => a.Sort).ToList()
            }).ToList();

            _cache.Set(PretreatmentCacheKeys.SortInfoCache + sortInfoCode, sortDetailsWithRule, TimeSpan.FromHours(24));
        }

        var sortDetailList = sortDetailsWithRule.SelectMany(v => v.ShelfRuleList.OrderBy(a => a.Sort).Select(a => new
        {
            v.Id,
            v.SortInfoCode,
            v.IsShelfMode,
            v.ShelfPosition,
            v.ShelfCode,
            v.ShelfName,
            v.ShelfType,
            v.RowHoleCount,
            v.ColumnHoleCount,
            v.UsedCount,
            a.RuleCode,
            a.RuleName,
            a.RuleExpression,
            a.SequenceCode,
            a.SequenceName,
            a.ItemCount,
            a.IsSortAll,
            a.IsBatch,
        })).ToList();

        var barcode = input.Barcode?.Trim()?.ToUpper();
        var sampleType = input.SampleTypeCode;

        var paramValue = await _paramService.GetParamValue(LimsConsts.PushForwardTime, "7");
        if (!int.TryParse(paramValue, out int pushForwardTime))
            throw ResultOutput.Exception("PushForwardTime参数设置有误！");

        if (string.IsNullOrWhiteSpace(barcode))
            throw ResultOutput.Exception("条码不可为空！");

        var info = await _applyInfoRep.GetFirstAsync(a => a.Barcode == barcode);
        if (info == null)
            throw ResultOutput.Exception($"未找到条码为{barcode}的申请信息！");

        var sortingDto = new SortingOutput { Barcode = barcode };
        var isExistsUnSortPurpose = await _applyPurposeRep.AsQueryable()
            //.Where(a => a.Barcode == barcode && (a.SampleStatus == SampleStatusEnum.Confirmed.ToInt() || a.SampleStatus == SampleStatusEnum.SplitBlood.ToInt()) && a.AddType != 2)
            .Where(a => a.Barcode == barcode && a.SortStatus <= 0 && a.AddType != 2 && a.SampleStatus != SampleStatusEnum.ReportCancel.ToInt())
            .WhereIF(!string.IsNullOrWhiteSpace(sampleType), a => a.SampleTypeCode == sampleType)
            .AnyAsync();

        if (!isExistsUnSortPurpose)
        {
            sortingDto.Status = SortingStatusEnum.SortFailed;
            sortingDto.ApplyPurposeList = await GetAllPurpose(barcode);
            sortingDto.Message = $"{barcode}不存在未分拣目的！";
            return sortingDto;
        }

        var purposeList = await _applyPurposeRep.AsQueryable()
            .InnerJoin<ApplyInfoEntity>((a, b) => a.Barcode == b.Barcode)
            .InnerJoin<ApplyItemEntity>((a, b, c) => a.Barcode == c.Barcode && a.PurCode == c.PurCode && c.IsDeleted == false)
            .LeftJoin<BasePurposeTenantSettingEntity>((a, b, c, d) => a.PurCode == d.PurCode)
            .Where((a, b, c, d) => a.Barcode == barcode && a.SortStatus <= 0 && a.AddType != 2 && a.SampleStatus != SampleStatusEnum.ReportCancel.ToInt())
            .WhereIF(!string.IsNullOrWhiteSpace(sampleType), (a, b, c, d) => a.SampleTypeCode == sampleType)
            .Select((a, b, c, d) => new SortingPurpose
            {
                Id = a.Id,
                GroupCode = a.GroupCode,
                GroupName = a.GroupName,
                Barcode = b.Barcode,
                ComboCode = a.ComboCode,
                ComboName = a.ComboName,
                PurCode = a.PurCode,
                PurName = a.PurName,
                PurNamePersonalize = a.PurNamePersonalize,
                InstrumentItemCodes = a.InstrumentItemCodes,
                WorkFlowType = a.WorkFlowType,
                TestExamPlanCode = string.IsNullOrWhiteSpace(a.TestExamPlanCode) ? d.ExamPlan : a.TestExamPlanCode,
                SampleTypeCode = a.SampleTypeCode,
                SampleTypeName = a.SampleTypeName,
                EntrustHospitalCode = a.EntrustHospitalCode,
                OriginalGroupCode = a.OriginalGroupCode,
                OriginalGroupName = a.OriginalGroupName,
                TestShift = a.TestShift,
                TestSeries = a.TestSeries,
                BatchNo = a.BatchNo,
                BatchScanOrder = a.BatchScanOrder,
                PatientName = b.PatientName,
                CustomerCode = b.CustomerCode,
                CustomerName = b.CustomerName,
                PatientTypeCode = b.PatientTypeCode,
                GenderCode = b.GenderCode,
                InstrumentItemCode = c.InstrumentItemCode,
                ItemCode = c.ItemCode,
                ItemName = c.ItemName,
                ItemNamePersonalize = c.ItemNamePersonalize,
                ReceiveTime = a.ReceiveTime,
                ApplyItemId = c.Id,
                SampleStatus = a.SampleStatus
            })
            .ToListAsync();

        if (purposeList == null || purposeList.Count == 0)
        {
            var allPurpose = await GetAllPurpose(barcode);
            if (!allPurpose.Any())
            {
                throw ResultOutput.Exception($"{barcode}获取申请项目失败！");
            }
            else
            {
                sortingDto.Status = SortingStatusEnum.SortFailed;
                sortingDto.ApplyPurposeList = allPurpose;
                return sortingDto;
            }
        }

        if (purposeList.Exists(v => string.IsNullOrWhiteSpace(v.TestExamPlanCode)))
            throw ResultOutput.Exception($"{barcode}{purposeList.First(v => string.IsNullOrWhiteSpace(v.TestExamPlanCode)).PurName}未设置检测计划，请先设置！");

        sortingDto.ApplyInfo = info.Adapt<ApplyInfoDto>();

        if (purposeList.Exists(a => a.SampleTypeCode.Contains(',')))
        {
            #region 目的需要选择标本类型
            var selDtos = purposeList.FindAll(a => a.SampleTypeCode.Contains(',')).Adapt<List<ApplyPurposeDto>>().Select(a =>
                     {
                         var sampleTypeNameList = a.SampleTypeName?.Split(',');
                         var sampleTypes = a.SampleTypeCode.Split(',').ToList().Select((b, i) =>
                          {
                              var sampleTypeName = string.Empty;
                              if (sampleTypeNameList != null && sampleTypeNameList.Length > i)
                                  sampleTypeName = sampleTypeNameList[i];

                              return new CodeNameDto
                              {
                                  Code = b,
                                  Name = sampleTypeName
                              };
                          });

                         return new SelectPurSampleTypeDto
                         {
                             Purpose = a,
                             SampleTypeList = sampleTypes.ToList()
                         };
                     });

            sortingDto.Status = SortingStatusEnum.SelectPurposeSampleType;
            sortingDto.SelectPurSampleTypeList = selDtos.ToList();
            #endregion
        }
        else if (purposeList.Select(a => a.SampleTypeCode).Distinct().Count() > 1)
        {
            #region 选择标本类型分拣
            var typeList = purposeList.Select(a => new CodeNameDto { Code = a.SampleTypeCode, Name = a.SampleTypeName });
            typeList = typeList.ToDistinct((a, b) => a.Code == b.Code);
            sortingDto.SampleTypeList = typeList.ToList();
            //sortingDto.SelectSortSampleType = new SelectSortSampleTypeDto
            //{
            //    Barcode = barcode,
            //    SampleTypeList = typeList.ToList()
            //};
            sortingDto.Status = SortingStatusEnum.SelectSortSampleType;
            #endregion
        }
        else
        {
            sampleType = purposeList.Select(a => a.SampleTypeCode).First();
            var splitBlood = await _bloodRep.GetListAsync(v => v.SampleTypeCode == sampleType && v.Barcode == barcode);
            if (splitBlood.Exists(v => v.SplitBloodStatus == 0))
                throw ResultOutput.Exception("请先分血！");

            var purPlanList = new List<CalcExamPlanResult>();
            DateTime? esTestDate;
            DateTime? esReportTime;

            purposeList.FindAll(v => v.ReceiveTime == null).ForEach(v => v.ReceiveTime = DateTime.Now);

            #region process entrust data
            foreach (var purs in purposeList.FindAll(v => v.GroupCode == "8888").GroupBy(v => v.SampleTypeCode))
            {
                var param = new CalcEntrustInput
                {
                    CustomerCode = info.CustomerCode!,
                    ThrowNotExists = true,
                    ReceiveTime = purs.First().ReceiveTime!.Value,
                    PurCodeList = purs.Select(v => v.PurCode).ToList(),
                    SampleTypeCode = sampleType,
                };
                var ret = await _entrustService.CalcEntrustAsync(param);
                foreach (var p in purs)
                {
                    var currentEntrust = ret.FirstOrDefault(v => v.PurCode == p.PurCode);
                    if (currentEntrust != null)
                    {
                        p.EntrustHospitalCode = currentEntrust.EntrustHospitalCode;
                        p.EntrustHospitalName = currentEntrust.EntrustHospitalName;
                        p.EstimateAskTime = currentEntrust.EstimatedAskTime;
                        p.EstimateTestDate = DateTime.Now.Date;
                        p.EstimateReportTime = p.EstimateAskTime;
                        p.EntrustStatus = 1;
                    }
                    else
                    {
                        throw ResultOutput.Exception($"{p.PurCode}-{p.PurName}计算委托失败！");
                    }
                }
            }
            #endregion

            foreach (var purs in purposeList.FindAll(v => v.GroupCode != "8888").GroupBy(a => a.PurCode))
            {
                var currReceiveTime = purs.First().ReceiveTime!.Value.AddHours(-pushForwardTime);
                var receiveTimePoint = TimeSpan.Parse(purs.First().ReceiveTime.Value.ToString("HH:mm:ss"));

                var currPlan = purPlanList.FirstOrDefault(v => v.ExamPlanCode.Equals(purs.First().TestExamPlanCode) && currReceiveTime.Date == v.ReceiveDate && receiveTimePoint < v.ReceiveTimePoint);
                if (currPlan != null)
                {
                    esTestDate = currPlan.TestDate;
                    esReportTime = currPlan.ReportTime;
                }
                else
                {
                    var calcRet = await _examPlanService.CalcTestAndReportDate(purs.First().TestExamPlanCode!, currReceiveTime);
                    esTestDate = calcRet.Item1;
                    esReportTime = calcRet.Item2;
                    var rvTimePoint = calcRet.Item3;
                    if (!purPlanList.Exists(v => v.ExamPlanCode == purs.First().TestExamPlanCode && currReceiveTime.Date == v.ReceiveDate && v.ReceiveTimePoint.Equals(rvTimePoint)))
                    {
                        purPlanList.Add(new CalcExamPlanResult
                        {
                            ExamPlanCode = purs.First().TestExamPlanCode!,
                            ReceiveDate = currReceiveTime.Date,
                            ReceiveTimePoint = rvTimePoint,
                            TestDate = esTestDate,
                            ReportTime = esReportTime
                        });
                    }
                }

                foreach (var p in purs)
                {
                    p.EstimateTestDate = esTestDate;
                    p.EstimateReportTime = esReportTime;
                }
            }
            var firstNullTime = purposeList.FirstOrDefault(v => v.EstimateTestDate == null || v.EstimateReportTime == null);
            if (firstNullTime != null)
                throw ResultOutput.Exception($"{firstNullTime.Barcode}计算检测计划{firstNullTime.TestExamPlanCode}失败！");

            List<string> splitGroupList = new List<string>();
            foreach (var currRule in sortDetailList)
            {
                if (currRule.ShelfType == PretreatShelfTypeEnum.SplitBlood)
                    continue;

                if (string.IsNullOrWhiteSpace(currRule.RuleExpression))
                    continue;

                if (!purposeList.Exists(v => v.SampleStatus == SampleStatusEnum.Confirmed.ToInt()
                                            || v.SampleStatus == SampleStatusEnum.SplitBlood.ToInt()))
                    break;

                var currShelfUsedCount = sortDetailsWithRule.FirstOrDefault(v => v.ShelfPosition == currRule.ShelfPosition)?.UsedCount ?? 0;

                var expStr = currRule.RuleExpression;
                var filter = JsonHelper.Deserialize<DynamicFilterInfo>(expStr);
                var expression = DynamicFilterExpressionBuilder.BuildPredicate<SortingPurpose>(filter);
                var matchList = purposeList.Where(expression.Compile())?.ToList();
                if (matchList == null || matchList.Count <= 0)
                    continue;

                if (currRule.ItemCount != 0 && currRule.ItemCount != matchList.Count)
                    continue;

                var entrustHospitalCode = matchList.First().EntrustHospitalCode;
                if (!string.IsNullOrWhiteSpace(entrustHospitalCode))
                    expression = expression.And(v => v.EntrustHospitalCode == entrustHospitalCode);
                else
                    expression = expression.And(v => string.IsNullOrWhiteSpace(v.EntrustHospitalCode));

                matchList = purposeList.Where(expression.Compile())?.ToList();

                if (currRule.IsSortAll && string.IsNullOrWhiteSpace(entrustHospitalCode))
                    matchList = purposeList;

                var splitGroup = $"{matchList.First().GroupCode},{currRule.ShelfPosition},{entrustHospitalCode}";
                if (!splitGroupList.Contains(splitGroup))
                    splitGroupList.Add(splitGroup);

                var sortingGroups = matchList.Select(v => $"{v.GroupCode},{v.WorkFlowType},{v.EstimateTestDate:yyyy-MM-dd},{v.EstimateReportTime},{v.EntrustHospitalCode}").Distinct();

                foreach (var sg in sortingGroups)
                {
                    var groupCode = sg.Split(',')[0];
                    var workFlowType = sg.Split(',')[1];
                    var estTestDate = DateTime.Parse(sg.Split(',')[2]);
                    var estReportTime = DateTime.Parse(sg.Split(',')[3]);
                    var entrustCode = sg.Split(',')[4];

                    matchList.FindAll(v =>
                    v.GroupCode == groupCode && (v.WorkFlowType ?? "") == workFlowType && v.EstimateTestDate.Equals(estTestDate) && v.EstimateReportTime.Equals(estReportTime) && (v.EntrustHospitalCode ?? "").Equals(entrustCode)).ForEach(v =>
                    {
                        v.SampleStatus = SampleStatusEnum.Sorted.ToInt();
                        v.SortInfoCode = sortInfoCode;
                        v.SortRuleCode = currRule.RuleCode;
                        v.SortRuleName = currRule.RuleName;
                        v.SequenceCode = currRule.SequenceCode;
                        v.SequenceName = currRule.SequenceName;
                        v.SampleStatus = SampleStatusEnum.Sorted.ToInt();
                        v.SampleStatusName = SampleStatusEnum.Sorted.ToDescription();
                        //v.SortStatus = 1;
                        v.SortUserName = AppInfo.User.Name;
                        v.ShelfBarcode = currRule.ShelfCode;
                        v.ShelfName = currRule.ShelfName;
                        v.HolePosition = currShelfUsedCount + 1;
                        v.SortingGroup = sg;
                        v.SplitGroup = splitGroup;
                    });
                }

                //sortDetailsWithRule.FindAll(v => v.ShelfPosition == currRule.ShelfPosition).ForEach(v => v.UsedCount += 1);
            }
            var unSortPurs = purposeList.FindAll(v => v.SampleStatus == SampleStatusEnum.Confirmed.ToInt() || v.SampleStatus == SampleStatusEnum.SplitBlood.ToInt());
            if (unSortPurs.Any())
            {
                var errMsg = $"{string.Join(",", unSortPurs.Select(v => $"{v.ItemCode}-{v.ItemName}"))}未分拣，请维护分拣规则！";
                throw ResultOutput.Exception(errMsg);
            }

            var g = purposeList.GroupBy(v => new { v.PurCode, v.PurName });
            foreach (var gi in g)
            {
                var seqs = gi.Select(o => o.SortRuleCode).Distinct();
                if (seqs.Count() > 1)
                    throw new Exception($"目的{gi.Key.PurCode}-{gi.Key.PurName}被分到{string.Join(",", seqs)}规则上，请先调整分拣规则！");
            }
            var sampleTrackList = new List<ExamSampleTrackDto>();
            if (splitGroupList.Count > 1 && !splitBlood.Any())
            {
                var id = YitIdHelper.NextId();
                List<PretreatSortSplitBloodDto> splitList = new();
                List<PretreatSortSplitBloodDetailDto> splitDetailList = new();
                splitList.Add(new PretreatSortSplitBloodDto
                {
                    Id = id,
                    Barcode = purposeList.FirstOrDefault().Barcode,
                    SampleTypeCode = purposeList.FirstOrDefault().SampleTypeCode,
                    SampleTypeName = purposeList.FirstOrDefault().SampleTypeName,
                    PurCodes = string.Join(",", purposeList.OrderBy(a => a.PurCode).Select(a => a.PurCode).Distinct()),
                    PurNames = string.Join(",", purposeList.OrderBy(a => a.PurCode).Select(a => a.PurName).Distinct()),
                    SplitTubeCnt = splitGroupList.Count,
                    SplitBloodStatus = 0,
                    ReceiveTime = purposeList.FirstOrDefault().ReceiveTime
                });

                foreach (var sg in splitGroupList)
                {
                    var splitBloodDetailPurs = purposeList.FindAll(v => v.SplitGroup == sg).ToDistinct((a, b) => a.PurCode == b.PurCode).OrderBy(a => a.PurCode);
                    splitDetailList.Add(new PretreatSortSplitBloodDetailDto
                    {
                        SplitBloodId = id,
                        Barcode = splitBloodDetailPurs.First().Barcode,
                        GroupCode = splitBloodDetailPurs.First().GroupCode,
                        GroupName = splitBloodDetailPurs.First().GroupName,
                        SampleTypeCode = splitBloodDetailPurs.First().SampleTypeCode,
                        PurCodes = string.Join(",", splitBloodDetailPurs.Select(a => a.PurCode)),
                        PurNames = string.Join(",", splitBloodDetailPurs.Select(a => a.PurNamePersonalize ?? a.PurName)),
                        EntrustHospitalCode = splitBloodDetailPurs.First().EntrustHospitalCode,
                        EntrustHospitalName = splitBloodDetailPurs.First().EntrustHospitalName
                    });
                }

                purposeList.ForEach(v =>
                {
                    v.SampleStatus = SampleStatusEnum.WaitSplitBlood.ToInt();
                    v.SampleStatusName = SampleStatusEnum.WaitSplitBlood.ToDescription();
                });

                var masterList = splitList.Adapt<List<PretreatSortSplitBloodEntity>>();
                await _bloodRep.InsertRangeAsync(masterList);

                var detailList = splitDetailList.Adapt<List<PretreatSortSplitBloodDetailEntity>>();
                await _bloodRep.Context.GetSimpleClient<PretreatSortSplitBloodDetailEntity>().InsertRangeAsync(detailList);

                sampleTrackList.Add(new ExamSampleTrackDto
                {
                    Barcode = barcode,
                    OperationType = OperationTypeEnum.Sorting,
                    TrackContent = $"标本分拣-分血架，标本类型：{purposeList.First().SampleTypeName}，目的：{string.Join(",", purposeList.OrderBy(a => a.PurCode).Select(a => $"{a.PurName}({a.PurCode})").Distinct())}",
                });
                sortingDto.Status = SortingStatusEnum.SplitBlood;
            }
            else
            {
                List<ExamTaskDto> taskList = new();
                List<ExamTaskDetailDto> taskDetaiList = new();
                purposeList = purposeList.FindAll(v => v.SortingGroup == purposeList.First().SortingGroup).ToList();

                sortDetailsWithRule.FindAll(v => v.ShelfCode == purposeList.First().ShelfBarcode).ForEach(v => v.UsedCount += 1);

                foreach (var sg in purposeList.GroupBy(v => new { v.Barcode, v.SortingGroup }))
                {
                    var currPurs = sg.Select(v => v).OrderBy(v => v.PurCode).ToList();
                    var id = YitIdHelper.NextId();
                    taskList.Add(new ExamTaskDto
                    {
                        Id = id,
                        Barcode = currPurs.First().Barcode,
                        GroupCode = currPurs.First().GroupCode,
                        GroupName = currPurs.First().GroupName,
                        CustomerCode = currPurs.First().CustomerCode,
                        CustomerName = currPurs.First().CustomerName,
                        PurCodes = string.Join(",", currPurs.Select(a => a.PurCode).Distinct()),
                        PurNames = string.Join(",", currPurs.Select(a => string.IsNullOrWhiteSpace(a.PurNamePersonalize) ? a.PurName : a.PurNamePersonalize).Distinct()),
                        SampleTypeCode = currPurs.First().SampleTypeCode,
                        SampleTypeName = currPurs.First().SampleTypeName,
                        EstimatedTestDate = currPurs.First().EstimateTestDate,
                        EstimatedReportTime = currPurs.First().EstimateReportTime,
                        SortUserId = AppInfo.User.Id,
                        SortUserName = AppInfo.User.Name,
                        SortTime = DateTime.Now,
                        SortInfoCode = sortInfoCode,
                        ShelfBarcode = currPurs.First().ShelfBarcode,
                        ShelfName = currPurs.First().ShelfName,
                        HolePosition = currPurs.First().HolePosition,
                        SortRuleCode = currPurs.First().SortRuleCode,
                        SortRuleName = currPurs.First().SortRuleName,
                        SequenceCode = currPurs.First().SequenceCode,
                        SequenceName = currPurs.First().SequenceName,
                        EntrustHospitalCode = currPurs.First().EntrustHospitalCode,
                        EntrustHospitalName = currPurs.First().EntrustHospitalName,
                        EntrustStatus = !string.IsNullOrWhiteSpace(currPurs.First().EntrustHospitalCode) ? 1 : 0,
                        ReceiveTime = currPurs.First().ReceiveTime,
                        ApplyPurposeIds = string.Join(",", currPurs.Select(a => a.Id).Distinct()),
                        HandoverStatus = 0
                    });
                    foreach (var pur in currPurs)
                    {
                        pur.TaskId = id;
                        taskDetaiList.Add(new ExamTaskDetailDto
                        {
                            TaskId = id,
                            Barcode = pur.Barcode,
                            GroupCode = pur.GroupCode,
                            ApplyItemId = pur.ApplyItemId,
                            ComboCode = pur.ComboCode,
                            InstrumentItemCode = pur.InstrumentItemCode,
                            ItemCode = pur.ItemCode,
                            ItemName = pur.ItemName,
                            ItemNamePersonalize = pur.ItemNamePersonalize,
                            InTest = 0,
                            PurCode = pur.PurCode
                        });
                    }
                }

                var updateList = new List<PretreatSortDetailDto>();
                foreach (var item in sortDetailList.GroupBy(a => a.ShelfPosition))
                {
                    var id = item.First().Id;
                    var currShelf = sortDetailsWithRule.FirstOrDefault(v => v.Id == id);
                    if (currShelf!.UsedCount != item.FirstOrDefault()!.UsedCount)
                    {
                        updateList.Add(new PretreatSortDetailDto
                        {
                            Id = id,
                            UsedCount = currShelf.UsedCount
                        });
                    }
                }
                if (updateList.Any())
                {
                    var cacheKey = PretreatmentCacheKeys.SortInfoCache + input.SortInfoCode;
                    await _cache.SetAsync(cacheKey, sortDetailsWithRule, TimeSpan.FromHours(24));

                    await _sortDetailRep.AsUpdateable(updateList.Adapt<List<PretreatSortDetailEntity>>()).UpdateColumns(v => new { v.UsedCount }, true)
                        .ExecuteCommandAsync();
                }

                await _taskRep.InsertRangeAsync(taskList.Adapt<List<ExamTaskEntity>>());
                await _taskDetailRep.InsertRangeAsync(taskDetaiList.Adapt<List<ExamTaskDetailEntity>>());

                sampleTrackList.Add(new ExamSampleTrackDto
                {
                    Barcode = barcode,
                    OperationType = OperationTypeEnum.Sorting,
                    TrackContent = $"标本分拣-{purposeList.First().ShelfName}，标本类型：{purposeList.First().SampleTypeName}，目的：{string.Join(",", purposeList.OrderBy(a => a.PurCode).Select(a => $"{a.PurName}({a.PurCode})").Distinct())}",
                });

                var allTask = await _taskRep.GetListAsync(a => a.Barcode == barcode);
                sortingDto.ExamTaskList = allTask.Adapt<List<ExamTaskDto>>();
                sortingDto.ShelfName = purposeList.First().ShelfName;
                sortingDto.HolePosition = purposeList.First().HolePosition;
                sortingDto.Status = SortingStatusEnum.SortSuccess;
            }

            var updateApplyPurpose = purposeList.GroupBy(a => a.Id).Select(a => new ApplyPurposeEntity
            {
                Id = a.First().Id,
                SortStatus = a.First().SampleStatus == SampleStatusEnum.Sorted.ToInt() ? 1 : 0,
                SampleStatus = a.First().SampleStatus,
                SampleStatusName = a.First().SampleStatusName,
                ReceiveTime = a.First().ReceiveTime is null ? DateTime.Now : a.First().ReceiveTime,
                TaskId = a.First().TaskId
            }).ToList();

            await _applyPurposeRep.AsUpdateable(updateApplyPurpose)
                .UpdateColumns(v => new
                {
                    v.SortStatus,
                    v.SampleStatus,
                    v.SampleStatusName,
                    v.ReceiveTime,
                    v.TaskId
                }, true)
                .WhereColumns(v => new { v.Id })
                .ExecuteCommandAsync();

            if (info.ReceiveTime is null)
                await _applyInfoRep.AsUpdateable().SetColumns(v => v.ReceiveTime == DateTime.Now).ExecuteCommandAsync();

            await _sampleTrackRep.InsertRangeAsync(sampleTrackList.Adapt<List<ExamSampleTrackEntity>>());

            var allPurpose = await GetAllPurpose(barcode);
            sortingDto.ApplyPurposeList = allPurpose;
        }
        return sortingDto;
    }

    [NonAction]
    private async Task<List<ApplyPurposeDto>> GetAllPurpose(string barcode)
    {
        var ret = await _applyPurposeRep.AsQueryable()
                                   .Where(a => a.Barcode == barcode && a.AddType != 2 && a.SampleStatus != SampleStatusEnum.ReportCancel.ToInt() && !a.IsDeleted)
                                   .Select<ApplyPurposeDto>()
                                   .ToListAsync();
        return ret;
    }
}
