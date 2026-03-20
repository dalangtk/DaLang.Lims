using DaLang.Lims.Pretreatment.Contracts.Handover;
using DaLang.Lims.Pretreatment.Contracts.Handover.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Shared.Contracts.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;
using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;
using DaLang.Lims.Shared.Contracts.ExamTask.Dto;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Shared.Domain.ExamResult;
using DaLang.Lims.Shared.Domain.ExamSampleTrack;
using DaLang.Lims.Shared.Domain.ExamTask;
using DaLang.Lims.Shared.Domain.ExamTaskDetail;
using DaLang.Lims.Web.Common.Consts;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DotNetCore.CAP;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Yitter.IdGenerator;

namespace DaLang.Lims.Pretreatment.Services.Handover;

/// <summary>
/// 交接服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class HandoverService : BaseService, IHandoverService, IDynamicApi
{
    private AdminRepositoryBase<ExamTaskEntity> _taskRep;
    private AdminRepositoryBase<ExamTaskDetailEntity> _taskDetailRep;
    private AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private AdminRepositoryBase<ExamResultEntity> _examResultRep;
    private AdminRepositoryBase<ApplyInfoEntity> _applyInfoRep;
    private AdminRepositoryBase<ApplyPurposeEntity> _applyPurposeRep;
    private AdminRepositoryBase<ExamSampleTrackEntity> _sampleTrackRep;
    private ICapPublisher _publisher;
    public HandoverService(AdminRepositoryBase<ExamTaskEntity> taskRep,
        AdminRepositoryBase<ExamTaskDetailEntity> taskDetailRep,
        AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        AdminRepositoryBase<ExamResultEntity> examResultRep,
        AdminRepositoryBase<ApplyInfoEntity> applyInfoRep,
        AdminRepositoryBase<ApplyPurposeEntity> applyPurposeRep,
        AdminRepositoryBase<ExamSampleTrackEntity> sampleTrackRep,
        ICapPublisher publisher)
    {
        _taskRep = taskRep;
        _taskDetailRep = taskDetailRep;
        _examInfoRep = examInfoRep;
        _examResultRep = examResultRep;
        _applyInfoRep = applyInfoRep;
        _applyPurposeRep = applyPurposeRep;
        _sampleTrackRep = sampleTrackRep;
        _publisher = publisher;
    }
    /// <summary>
    /// 获取交接任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    [HttpPost]
    public async Task<GetTaskOutput<List<ExamTaskDto>>> GetTaskAsync(ExamTaskQueryInput input)
    {
        var result = new GetTaskOutput<List<ExamTaskDto>>();
        if (string.IsNullOrWhiteSpace(input?.GroupCode))
            throw ResultOutput.Exception("group code can not be null.");
        if (string.IsNullOrWhiteSpace(input.Barcode) && input.BeginDate == null && input.EndDate == null)
            throw ResultOutput.Exception("请输入条码或时间！");

        var list = await _taskRep.AsQueryable()
             .Where(v => v.GroupCode.Equals(input.GroupCode) && v.HandoverStatus == input.Status)
             .WhereIF(!string.IsNullOrWhiteSpace(input.Barcode), v => v.Barcode.Equals(input.Barcode))
             .WhereIF(!string.IsNullOrWhiteSpace(input.SampleTypeCode), v => v.SampleTypeCode.Equals(input.SampleTypeCode))
             .WhereIF(input.BeginDate != null && input.EndDate != null, v => SqlFunc.Between(v.ReceiveTime, input.BeginDate, input.EndDate))
             .ToListAsync();

        if (!list.Any())
            throw ResultOutput.Exception("未找到数据！");

        if (list.Any() && !string.IsNullOrWhiteSpace(input.Barcode))
        {
            if (list.Select(v => v.SampleTypeCode).Distinct().Count() > 1)
            {
                result.Status = 0;
                var sampleTypeList = list.Select(a => new CodeNameDto
                {
                    Code = a.SampleTypeCode,
                    Name = a.SampleTypeName
                }).ToDistinct((a, b) => a.Code == b.Code);

                result.SampleTypeList = sampleTypeList.ToList();
                return result;
            }
        }

        var sampleTrackList = new List<ExamSampleTrackAddInput>();
        foreach (var task in list)
        {
            if (string.IsNullOrWhiteSpace(task.SampleNo))
            {
                var pDate = new SugarParameter("@i_Date", task.EstimatedTestDate);
                var pSeqCode = new SugarParameter("@i_SequenceCode", task.SequenceCode);
                var pTenant = new SugarParameter("@i_TenantId", AppInfo.User.TenantId);
                var pOperId = new SugarParameter("@i_OperId", AppInfo.User.Id);
                var pOperName = new SugarParameter("@i_OperName", AppInfo.User.Name);
                var pIsTest = new SugarParameter("@i_IsTest", 0);
                var oSeqVal = new SugarParameter("@o_SeqVal", null, true);
                var oCode = new SugarParameter("@o_Code", null, true);
                var oMsg = new SugarParameter("@o_ErrMsg", null, true);
                await _taskRep.Context.Ado.UseStoredProcedure().ExecuteCommandAsync("PR_GetSequence",
                    pDate, pSeqCode, pTenant, pOperId, pOperName, pIsTest,
                    oSeqVal, oCode, oMsg);
                if (oCode.Value?.ToString() == "-1")
                    throw ResultOutput.Exception($"{task.SequenceCode}取流水号出错：{oMsg.Value?.ToString()}");
                var sampleNo = oSeqVal.Value?.ToString();
                task.SampleNo = sampleNo;
                sampleTrackList.Add(new ExamSampleTrackDto
                {
                    Barcode = task.Barcode,
                    GroupCode = task.GroupCode,
                    GroupName = task.GroupName,
                    SampleNo = sampleNo,
                    OperationType = OperationTypeEnum.Handover,
                    TrackContent = $"取流水号：目的：{task.PurNames}",
                });
            }
        }
        if (sampleTrackList.Any())
        {
            await _sampleTrackRep.InsertRangeAsync(sampleTrackList.Adapt<List<ExamSampleTrackEntity>>());
            await _taskRep.Context.Updateable(list).UpdateColumns(v => new { v.SampleNo }, true).ExecuteCommandAsync();
        }

        result.Status = 1;
        result.OutputList = list.Adapt<List<ExamTaskDto>>();
        return result;
    }
    /// <summary>
    /// 交接
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    //[AdminTransaction]
    public async Task<bool> Handover(HandoverInput input)
    {
        if (input.TaskIds == null || input.TaskIds.Count == 0)
            throw ResultOutput.Exception("task ids can not be null.");

        var tasks = await _taskRep.AsQueryable()
            .Where(v => input.TaskIds.Contains(v.Id) && v.HandoverStatus == 0)
            .ToListAsync();
        if (!tasks.Any())
            throw ResultOutput.Exception("所选数据已交接！");

        var time = DateTime.Now;
        var handoverBatchNo = YitIdHelper.NextId();
        var taskDetails = await _taskDetailRep.GetListAsync(v => input.TaskIds.Contains(v.TaskId) && v.InTest == 0);
        tasks.ForEach(v =>
        {
            v.HandoverStatus = 1;
            v.HandoverBatchNo = handoverBatchNo;
            v.HandoverId = AppInfo.User.Id;
            v.HandoverName = AppInfo.User.Name;
            v.HandoverTime = time;
        });

        List<ExamInfoUpdateInput> examList = new();
        List<ExamResultAddInput> resultList = new();

        var barcodes = tasks.Select(a => a.Barcode).Distinct().ToList();
        var applyInfos = await _applyInfoRep.GetListAsync(v => barcodes.Contains(v.Barcode));
        var purposeIds = tasks.SelectMany(v => v.ApplyPurposeIds.Split(',').Select(i => long.Parse(i))).Distinct().ToList();

        var purposeList = await _applyPurposeRep.GetListAsync(v => purposeIds.Contains(v.Id));

        var sampleTrackList = new List<ExamSampleTrackAddInput>();
        List<RefreshExamInfoInput> refreshInfoList = new();
        foreach (var currTask in tasks)
        {
            var currApplyInfo = applyInfos.FirstOrDefault(v => v.Barcode == currTask.Barcode);
            if (currApplyInfo == null)
                throw ResultOutput.Exception($"条码{currTask.Barcode},未找到申请单信息！");

            var currTaskDetail = taskDetails.FindAll(v => v.TaskId == currTask.Id);
            if (!currTaskDetail.Any())
                throw ResultOutput.Exception($"条码{currTask.Barcode},目的{currTask.PurNames}未找到任务明细！");

            var examInfoId = YitIdHelper.NextId();
            var ageValue = AgeConvertHelper.CalculateAgeToMinute(currApplyInfo.Age1, currApplyInfo.AgeUnit1, currApplyInfo.Age2, currApplyInfo.AgeUnit2);
            examList.Add(new ExamInfoUpdateInput
            {
                Id = examInfoId,
                TaskId = currTask.Id,
                GroupCode = currTask.GroupCode,
                GroupName = currTask.GroupName,
                Barcode = currTask.Barcode,
                SampleNo = currTask.SampleNo,
                CustomerCode = currApplyInfo.CustomerCode,
                CustomerName = currApplyInfo.CustomerName,
                CustomerBarcode = currApplyInfo.CustomerBarcode,
                TestDate = currTask.EstimatedTestDate,
                WFCode = currTask.WFCode,
                PatientTypeCode = currApplyInfo.PatientTypeCode,
                PatientTypeName = currApplyInfo.PatientTypeName,
                PatientId = currApplyInfo.PatientId,
                PatientName = currApplyInfo.PatientName,
                GenderCode = currApplyInfo.GenderCode,
                GenderName = currApplyInfo.GenderName,
                Age1 = currApplyInfo.Age1,
                AgeUnit1 = currApplyInfo.AgeUnit1,
                AgeUnitName1 = currApplyInfo.AgeUnitName1,
                Age2 = currApplyInfo.Age2,
                AgeUnit2 = currApplyInfo.AgeUnit2,
                AgeUnitName2 = currApplyInfo.AgeUnitName2,
                AgeValue = ageValue,
                CardTypeCode = currApplyInfo.CardTypeCode,
                CardTypeName = currApplyInfo.CardTypeName,
                Phone = currApplyInfo.Phone,
                BirthDay = currApplyInfo.BirthDay,
                IsMenoPause = currApplyInfo.IsMenoPause,
                LastMenstrualPeriod = currApplyInfo.LastMenstrualPeriod,
                Height = currApplyInfo.Height,
                Weight = currApplyInfo.Weight,
                NTTestResult = currApplyInfo.NTTestResult,
                CRL = currApplyInfo.CRL,
                BPD = currApplyInfo.BPD,
                GestationalWeeks = currApplyInfo.GestationalWeeks,
                HomeAddress = currApplyInfo.HomeAddress,
                Department = currApplyInfo.Department,
                Ward = currApplyInfo.Ward,
                Doctor = currApplyInfo.Doctor,
                BedNo = currApplyInfo.BedNo,
                ClinicalDiagnosis = currApplyInfo.ClinicalDiagnosis,
                PurCodes = currTask.PurCodes,
                PurNames = currTask.PurNames,
                SampleTypeCode = currTask.SampleTypeCode,
                SampleTypeName = currTask.SampleTypeName,
                SamplePropertyCode = "101",
                SamplePropertyName = "未见异常",
                CollectTime = currApplyInfo.CollectTime,
                ReceiveTime = currTask.ReceiveTime,
                SampleStatus = SampleStatusEnum.Testing.ToInt(),
                SampleStatusName = SampleStatusEnum.Testing.ToDescription(),
                ChargeType = currApplyInfo.ChargeType,
                IsUrgent = currApplyInfo.IsUrgent,
                InTestTime = DateTime.Now,
                InspectorId = AppInfo.User.Id,
                InspectorAuthorizedId = AppInfo.User.Id,
                InspectorName = AppInfo.User.Name,
                EntrustHospitalCode = currTask.EntrustHospitalCode,
                EntrustHospitalName = currTask.EntrustHospitalName,
                EntrustStatus = currTask.EntrustStatus
            });
            refreshInfoList.Add(new RefreshExamInfoInput
            {
                ExamInfoId = examInfoId,
                IsForce = 0,
                IsReceive = true
            });

            var currPurposeIds = currTask.ApplyPurposeIds.Split(',').Select(i => long.Parse(i));
            var currPurposes = purposeList.FindAll(v => currPurposeIds.Contains(v.Id)).ToList();
            foreach (var p in currPurposes)
            {
                p.SampleStatus = SampleStatusEnum.Testing.ToInt();
                p.SampleStatusName = SampleStatusEnum.Testing.ToDescription();

                var currDetails = taskDetails.FindAll(v => v.TaskId == currTask.Id && v.PurCode == p.PurCode);
                foreach (var currDetail in currDetails)
                {
                    currDetail.InTest = 1;
                    resultList.Add(new ExamResultAddInput
                    {
                        TaskDetailId = currDetail.Id,
                        ExamInfoId = examInfoId,
                        GroupCode = currTask.GroupCode,
                        GroupName = currTask.GroupName,
                        Barcode = currTask.Barcode,
                        SampleNo = currTask.SampleNo,
                        TestDate = currTask.EstimatedTestDate,
                        ComboCode = p.ComboCode,
                        PurCode = p.PurCode,
                        PurName = string.IsNullOrWhiteSpace(p.PurNamePersonalize) ? p.PurName : p.PurNamePersonalize,
                        InstrumentItemCode = currDetail.InstrumentItemCode,
                        ItemCode = currDetail.ItemCode,
                        ItemName = currDetail.ItemName,
                        ItemNamePersonalize = currDetail.ItemNamePersonalize,
                        ResultSource = 0,
                        InTestCount = 0,
                        EntrustStatus = currTask.EntrustStatus,
                        AddType = 0,
                        ReviewStatus = 0
                    });
                }
            }

            sampleTrackList.Add(new ExamSampleTrackDto
            {
                Barcode = currTask.Barcode,
                GroupCode = currTask.GroupCode,
                GroupName = currTask.GroupName,
                SampleNo = currTask.SampleNo,
                OperationType = OperationTypeEnum.Testing,
                TrackContent = $"检验接收：目的：{currTask.PurNames}",
            });
        }
        try
        {
            _taskRep.Context.Ado.BeginTran();
            await _examInfoRep.InsertRangeAsync(examList.Adapt<List<ExamInfoEntity>>());
            await _examResultRep.InsertRangeAsync(resultList.Adapt<List<ExamResultEntity>>());

            await _applyPurposeRep.Context.Updateable(purposeList).UpdateColumns(v => new
            {
                v.SampleStatus,
                v.SampleStatusName
            }, true).ExecuteCommandAsync();

            await _taskDetailRep.Context.Updateable(taskDetails).UpdateColumns(v => new
            {
                v.InTest
            }, true).ExecuteCommandAsync();

            await _taskRep.Context.Updateable(tasks).UpdateColumns(v => new
            {
                v.HandoverStatus,
                v.HandoverBatchNo,
                v.HandoverId,
                v.HandoverName,
                v.HandoverTime
            }, true).ExecuteCommandAsync();

            await _sampleTrackRep.InsertRangeAsync(sampleTrackList.Adapt<List<ExamSampleTrackEntity>>());

            _taskRep.Context.Ado.CommitTran();

            //事物提交后再发布订阅
            foreach (var r in refreshInfoList)
                _publisher.Publish(LimsConsts.RefreshInfoEvent, r);
        }
        catch (Exception ex)
        {
            _taskRep.Context.Ado.RollbackTran();
            throw;
        }

        return true;
    }
    /// <summary>
    /// 获取交接批次
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GetTaskOutput<List<QueryHandoverBatchNoOutput>>> GetHandoverBatchNo(ExamTaskQueryInput input)
    {
        var result = new GetTaskOutput<List<QueryHandoverBatchNoOutput>>();
        var ret = await _taskRep.AsQueryable()
            .Where(v => v.GroupCode.Equals(input.GroupCode) && SqlFunc.Between(v.HandoverTime, input.BeginDate.Value.Date, input.EndDate.Value.AddDays(1).AddSeconds(-1)) && v.HandoverStatus == 1 && !string.IsNullOrWhiteSpace(v.PurCodes))
            .GroupBy(v => new { v.HandoverBatchNo, v.HandoverId, v.HandoverName, v.HandoverTime })
            .Select(v => new QueryHandoverBatchNoOutput
            {
                BatchNo = v.HandoverBatchNo.Value,
                HandoverId = v.HandoverId.Value,
                HandoverName = v.HandoverName,
                HandoverTime = v.HandoverTime.Value,
                Total = SqlFunc.AggregateCount(v.Id)
            })
            .Distinct()
            .ToListAsync();
        result.Status = 1;
        result.OutputList = ret;
        return result;
    }
    /// <summary>
    /// 获取批次明细
    /// </summary>
    /// <param name="batchNo"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ExamTaskDto>> GetTaskListByBatchNo(long batchNo)
    {
        var ret = await _taskRep.AsQueryable()
            .Where(v => v.HandoverBatchNo.Equals(batchNo))
            .Select<ExamTaskDto>()
            .ToListAsync();
        return ret;
    }
}
