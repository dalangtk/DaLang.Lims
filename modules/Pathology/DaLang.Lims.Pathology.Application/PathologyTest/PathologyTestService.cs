using DaLang.Lims.BaseData.Contracts.AuditRule;
using DaLang.Lims.BaseData.Contracts.AuditRule.Dto;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.BaseData.Domain.Group;
using DaLang.Lims.BaseData.Domain.UserGroup;
using DaLang.Lims.Exam.Contracts.ExamUnAuditLog.Dto;
using DaLang.Lims.Exam.Contracts.Pathology.Dto;
using DaLang.Lims.Exam.Contracts.ReportTask.Dto;
using DaLang.Lims.Exam.Contracts.SampleTest;
using DaLang.Lims.Exam.Contracts.SampleTest.Dto;
using DaLang.Lims.Exam.Domain.ExamUnAuditLog;
using DaLang.Lims.Exam.Domain.ReportFiles;
using DaLang.Lims.Exam.Domain.ReportTask;
using DaLang.Lims.Pathology.Application.PathologySetting;
using DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot.Dto;
using DaLang.Lims.Pathology.Contracts.PathologySamplingSpotDetail.Dto;
using DaLang.Lims.Pathology.Contracts.PathologyTest;
using DaLang.Lims.Pathology.Contracts.PathologyTest.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Domain.ExamPathologySamplingSpot;
using DaLang.Lims.Pathology.Domain.PathologySampleType;
using DaLang.Lims.Pathology.Domain.PathologySamplingSpot;
using DaLang.Lims.Pathology.Domain.PathologySetting;
using DaLang.Lims.Pathology.Domain.PathologyTemplate;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;
using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;
using DaLang.Lims.Shared.Contracts.ExamSpecialResult.Dto;
using DaLang.Lims.Shared.Contracts.ExamTask.Dto;
using DaLang.Lims.Shared.Contracts.ExamTaskDetail.Dto;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyItem;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Shared.Domain.ExamResult;
using DaLang.Lims.Shared.Domain.ExamSampleTrack;
using DaLang.Lims.Shared.Domain.ExamSpecialResult;
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
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Parameter;
using DaLang.Lims.Web.Framework.Services.User.Dto;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SqlSugar;
using Yitter.IdGenerator;

namespace DaLang.Lims.Exam.Application.Pathology;

/// <summary>
/// 病理服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class PathologyTestService : BaseService, IPathologyTestService, IDynamicApi
{
    private readonly AdminRepositoryBase<ApplyInfoEntity> _applyInfoRep;
    private readonly AdminRepositoryBase<ApplyPurposeEntity> _applyPurposeRep;
    private readonly AdminRepositoryBase<ApplyItemEntity> _applyItemRep;
    private readonly AdminRepositoryBase<ExamTaskEntity> _taskRep;
    private readonly AdminRepositoryBase<ExamTaskDetailEntity> _taskDetailRep;
    private readonly AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private readonly AdminRepositoryBase<ExamResultEntity> _examResultRep;
    private readonly AdminRepositoryBase<ExamSpecialResultEntity> _examSpecialResultRep;
    private readonly IParameterService _paramService;
    private readonly IBasePathologyTemplateRepository _templateRep;
    private readonly IBaseAuditRuleService _auditRuleService;
    private readonly ISampleTestService _sampleTestService;
    private readonly AdminRepositoryBase<ApplyPurposeEntity> _purposeRep;
    private readonly IReportTaskRepository _reportTaskRep;
    private readonly IBaseCustomerReportExtendRepository _reportExtendRep;
    private readonly AdminRepositoryBase<ExamSampleTrackEntity> _sampleTrackRep;
    private readonly IExamUnAuditLogRepository _unAuditLogRep;
    private readonly BasePathologySettingService _settingService;
    private readonly AdminRepositoryBase<ReportFilesEntity> _reportFileRep;
    private readonly IBaseGroupRepository _baseGroupRep;
    private readonly IBaseUserGroupRepository _userGroupRep;
    private readonly IUserRepository _userRep;
    private readonly IExamPathologySamplingSpotRepository _examSamplingSpotRep;

    public PathologyTestService(AdminRepositoryBase<ApplyInfoEntity> applyInfoRep,
        AdminRepositoryBase<ApplyPurposeEntity> applyPurposeRep,
        AdminRepositoryBase<ApplyItemEntity> applyItemRep,
        AdminRepositoryBase<ExamTaskEntity> taskRep,
        AdminRepositoryBase<ExamTaskDetailEntity> taskDetailRep,
        AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        AdminRepositoryBase<ExamResultEntity> examResultRep,
        AdminRepositoryBase<ExamSpecialResultEntity> examSpecialResultRep,
        IParameterService paramService,
        IBasePathologyTemplateRepository templateRep,
        IBaseAuditRuleService auditRuleService,
        ISampleTestService sampleTestService,
        AdminRepositoryBase<ApplyPurposeEntity> purposeRep,
        IReportTaskRepository reportTaskRep,
        IBaseCustomerReportExtendRepository reportExtendRep,
        AdminRepositoryBase<ExamSampleTrackEntity> sampleTrackRep,
        IExamUnAuditLogRepository unAuditLogRep,
        BasePathologySettingService settingService,
        AdminRepositoryBase<ReportFilesEntity> reportFileRep,
        IBaseGroupRepository baseGroupRep,
        IBaseUserGroupRepository userGroupRep,
        IUserRepository userRep,
        IExamPathologySamplingSpotRepository examSamplingSpotRep)
    {
        _applyInfoRep = applyInfoRep;
        _applyPurposeRep = applyPurposeRep;
        _applyItemRep = applyItemRep;
        _taskRep = taskRep;
        _taskDetailRep = taskDetailRep;
        _examInfoRep = examInfoRep;
        _examResultRep = examResultRep;
        _examSpecialResultRep = examSpecialResultRep;
        _paramService = paramService;
        _templateRep = templateRep;
        _auditRuleService = auditRuleService;
        _sampleTestService = sampleTestService;
        _purposeRep = purposeRep;
        _reportTaskRep = reportTaskRep;
        _reportExtendRep = reportExtendRep;
        _sampleTrackRep = sampleTrackRep;
        _unAuditLogRep = unAuditLogRep;
        _settingService = settingService;
        _reportFileRep = reportFileRep;
        _baseGroupRep = baseGroupRep;
        _userGroupRep = userGroupRep;
        _userRep = userRep;
        _examSamplingSpotRep = examSamplingSpotRep;
    }

    /// <summary>
    /// 获取病理检验列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ExamInfoDto>> GetPathologySampleListAsync(PathologySampleListQueryInput input)
    {
        if (input.ExamInfoId == null)
        {
            if (string.IsNullOrWhiteSpace(input.WFCode))
                throw ResultOutput.Exception("工作流不能为空！");

            if (string.IsNullOrWhiteSpace(input.GroupCode))
                throw ResultOutput.Exception("组别代码不能为空！");

            if (string.IsNullOrWhiteSpace(input.Barcode) && (input.BeginDate == null || input.EndDate == null))
                throw ResultOutput.Exception("请选择查询时间段！");
        }

        var query = _examInfoRep.AsQueryable();

        if (input.ExamInfoId == null)
        {
            query = query.Where(v => v.GroupCode == input.GroupCode)
                         .Where(v => v.WFCode == input.WFCode)
                         .WhereIF(!string.IsNullOrWhiteSpace(input.Barcode), v => v.Barcode == input.Barcode)
                         .WhereIF(string.IsNullOrWhiteSpace(input.Barcode), v => SqlFunc.Between(v.TestDate, input.BeginDate!.Value.Date, input.EndDate!.Value.Date));
        }
        else
        {
            query = query.Where(v => v.Id == input.ExamInfoId);
        }

        var ret = await query.ToListAsync();
        var infoList = ret.Adapt<List<ExamInfoDto>>();

        return infoList;
    }

    /// <summary>
    /// 病理登记
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<ExamInfoDto> PathologyReceive(PathologyReceiveInput input)
    {
        if (input == null)
            throw ResultOutput.Exception("input can not be null.");

        var applyInfo = await _applyInfoRep.GetFirstAsync(v => v.Barcode == input.Barcode);
        if (applyInfo == null)
            throw ResultOutput.Exception("no info.");

        var exists = await _examInfoRep.IsAnyAsync(v => v.SampleNo == input.SampleNo && v.WFCode == input.WFCode);
        if (exists)
            throw ResultOutput.Exception("sample no already exists.");

        //TODO only accept designated purposes,now just filter by group code and sample status, need to optimize later
        bool unSortedReceive = await _paramService.GetParamValue(LimsConsts.PathologyReceiveMode, "0", TimeSpan.FromMinutes(5)) == "0";
        List<ApplyPurposeDto> applyPurposes = new();
        if (unSortedReceive)
        {
            applyPurposes = await _applyPurposeRep
            .AsQueryable()
            .Where(v => v.Barcode == input.Barcode && v.GroupCode == LimsConsts.PathologyGroupCode && v.SampleStatus == (int)SampleStatusEnum.Confirmed && v.AddType != 2)
            .Select<ApplyPurposeDto>()
            .OrderBy(v => v.PurCode).ToListAsync();
        }
        else
        {
            applyPurposes = await _applyPurposeRep
            .AsQueryable()
            .Where(v => v.Barcode == input.Barcode && v.GroupCode == LimsConsts.PathologyGroupCode && v.SampleStatus == (int)SampleStatusEnum.Handovered && v.AddType != 2)
            .Select<ApplyPurposeDto>()
            .OrderBy(v => v.PurCode).ToListAsync();
        }

        if (!applyPurposes.Any())
            throw ResultOutput.Exception("no purpose.");

        var purposeIds = applyPurposes.Select(v => v.Id);
        var applyItems = await _applyItemRep.GetListAsync(v => purposeIds.Contains(v.ApplyPurposeId));
        if (!applyItems.Any())
            throw ResultOutput.Exception("no item.");

        List<ExamTaskDto> taskList = new();
        List<ExamTaskDetailDto> taskDetailList = new();
        if (unSortedReceive)
        {
            var id = YitIdHelper.NextId();
            taskList.Add(new ExamTaskDto
            {
                Id = id,
                Barcode = applyInfo.Barcode,
                SampleNo = input.SampleNo,
                WFCode = input.WFCode,
                GroupCode = applyPurposes.First().GroupCode,
                GroupName = applyPurposes.First().GroupName,
                CustomerCode = applyInfo.CustomerCode,
                CustomerName = applyInfo.CustomerName,
                PurCodes = string.Join(",", applyPurposes.Select(a => a.PurCode).Distinct()),
                PurNames = string.Join(",", applyPurposes.Select(a => string.IsNullOrWhiteSpace(a.PurNamePersonalize) ? a.PurName : a.PurNamePersonalize).Distinct()),
                SampleTypeCode = applyPurposes.First().SampleTypeCode,
                SampleTypeName = applyPurposes.First().SampleTypeName,
                SortRuleCode = "-1",
                EntrustStatus = 0,
                ReceiveTime = applyPurposes.First().ReceiveTime,
                ApplyPurposeIds = string.Join(",", applyPurposes.Select(a => a.Id).Distinct()),
                HandoverStatus = 0
            });

            foreach (var pur in applyPurposes)
            {
                pur.TaskId = id;
                //var currItems = applyItems.FindAll(v => v.ApplyPurposeId == pur.Id);
                foreach (var item in applyItems)
                {
                    var detailId = YitIdHelper.NextId();
                    taskDetailList.Add(new ExamTaskDetailDto
                    {
                        Id = detailId,
                        TaskId = id,
                        Barcode = pur.Barcode,
                        GroupCode = pur.GroupCode,
                        ApplyItemId = item.Id,
                        ComboCode = pur.ComboCode,
                        InstrumentItemCode = item.InstrumentItemCode,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ItemNamePersonalize = item.ItemNamePersonalize,
                        InTest = 0,
                        PurCode = pur.PurCode
                    });
                }
            }
        }
        else
        {
            var taskIds = applyPurposes.Select(v => v.TaskId).Distinct().ToList();
            taskList = await _taskRep.AsQueryable().Where(v => taskIds.Contains(v.Id)).Select<ExamTaskDto>().ToListAsync();
            taskDetailList = await _taskDetailRep.AsQueryable().Where(v => taskIds.Contains(v.Id)).Select<ExamTaskDetailDto>().ToListAsync();
        }

        List<ExamInfoDto> examInfoList = new();
        List<ExamResultDto> examResultList = new();
        var ageValue = AgeConvertHelper.CalculateAgeToMinute(applyInfo.Age1, applyInfo.AgeUnit1, applyInfo.Age2, applyInfo.AgeUnit2);

        long? approverId = null;
        string approverName = string.Empty;
        var setting = await _settingService.GetSettingByWfCode(input.WFCode);
        if (setting != null)
        {
            if (setting.ReviewUserId != null)
            {
                approverId = setting.ReviewUserId;
                approverName = setting.ReviewUserName;
            }
        }

        foreach (var currTask in taskList)
        {
            currTask.HandoverId = AppInfo.User.Id;
            currTask.HandoverName = AppInfo.User.Name;
            currTask.HandoverTime = DateTime.Now;
            currTask.HandoverStatus = 1;

            var examId = YitIdHelper.NextId();
            examInfoList.Add(new ExamInfoDto
            {
                Id = examId,
                TaskId = taskList.First().Id,
                GroupCode = taskList.First().GroupCode,
                GroupName = taskList.First().GroupName,
                Barcode = taskList.First().Barcode,
                SampleNo = taskList.First().SampleNo,
                CustomerCode = taskList.First().CustomerCode,
                CustomerName = applyInfo.CustomerName,
                CustomerBarcode = applyInfo.CustomerBarcode,
                TestDate = DateTime.Now.Date,
                WFCode = input.WFCode,
                PatientTypeCode = applyInfo.PatientTypeCode,
                PatientTypeName = applyInfo.PatientTypeName,
                PatientId = applyInfo.PatientId,
                PatientName = applyInfo.PatientName,
                GenderCode = applyInfo.GenderCode,
                GenderName = applyInfo.GenderName,
                Age1 = applyInfo.Age1,
                AgeUnit1 = applyInfo.AgeUnit1,
                AgeUnitName1 = applyInfo.AgeUnitName1,
                Age2 = applyInfo.Age2,
                AgeUnit2 = applyInfo.AgeUnit2,
                AgeUnitName2 = applyInfo.AgeUnitName2,
                AgeValue = ageValue,
                CardTypeCode = applyInfo.CardTypeCode,
                CardTypeName = applyInfo.CardTypeName,
                Phone = applyInfo.Phone,
                BirthDay = applyInfo.BirthDay,
                IsMenoPause = applyInfo.IsMenoPause,
                LastMenstrualPeriod = applyInfo.LastMenstrualPeriod,
                Height = applyInfo.Height,
                Weight = applyInfo.Weight,
                NTTestResult = applyInfo.NTTestResult,
                CRL = applyInfo.CRL,
                BPD = applyInfo.BPD,
                GestationalWeeks = applyInfo.GestationalWeeks,
                HomeAddress = applyInfo.HomeAddress,
                Department = applyInfo.Department,
                Ward = applyInfo.Ward,
                Doctor = applyInfo.Doctor,
                BedNo = applyInfo.BedNo,
                ClinicalDiagnosis = applyInfo.ClinicalDiagnosis,
                PurCodes = currTask.PurCodes,
                PurNames = currTask.PurNames,
                SampleTypeCode = currTask.SampleTypeCode,
                SampleTypeName = currTask.SampleTypeName,
                SamplePropertyCode = "101",
                SamplePropertyName = "未见异常",
                CollectTime = applyInfo.CollectTime,
                ReceiveTime = currTask.ReceiveTime,
                SampleStatus = SampleStatusEnum.Testing.ToInt(),
                SampleStatusName = SampleStatusEnum.Testing.ToDescription(),
                ChargeType = applyInfo.ChargeType,
                IsUrgent = applyInfo.IsUrgent,
                InTestTime = DateTime.Now,
                InspectorId = AppInfo.User.Id,
                InspectorAuthorizedId = AppInfo.User.Id,
                InspectorName = AppInfo.User.Name,
                EntrustHospitalCode = currTask.EntrustHospitalCode,
                EntrustHospitalName = currTask.EntrustHospitalName,
                EntrustStatus = currTask.EntrustStatus,
                ApproverId = approverId,
                ApproverAuthorizedId = approverId,
                ApproverName = approverName
            });

            foreach (var currDetail in taskDetailList)
            {
                currDetail.InTest = 1;
                var currPurpose = applyPurposes.FirstOrDefault(v => v.PurCode == currDetail.PurCode);
                examResultList.Add(new ExamResultDto
                {
                    TaskDetailId = currDetail.Id,
                    ExamInfoId = examId,
                    GroupCode = currTask.GroupCode,
                    GroupName = currTask.GroupName,
                    Barcode = currTask.Barcode,
                    SampleNo = currTask.SampleNo,
                    TestDate = currTask.EstimatedTestDate,
                    ComboCode = currDetail.ComboCode,
                    PurCode = currDetail.PurCode,
                    PurName = string.IsNullOrWhiteSpace(currPurpose.PurNamePersonalize) ? currPurpose.PurName : currPurpose.PurNamePersonalize,
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

        if (unSortedReceive)
        {
            await _taskRep.Context.Insertable(taskList.Adapt<List<ExamTaskEntity>>()).ExecuteCommandAsync();
            await _taskDetailRep.Context.Insertable(taskDetailList.Adapt<List<ExamTaskDetailEntity>>()).ExecuteCommandAsync();
        }
        else
        {
            await _taskRep.Context.Updateable(taskList.Adapt<List<ExamTaskEntity>>()).UpdateColumns(v => new
            {
                v.HandoverStatus,
                v.HandoverBatchNo,
                v.HandoverId,
                v.HandoverName,
                v.HandoverTime
            }, true).ExecuteCommandAsync();
            await _taskDetailRep.Context.Updateable(taskDetailList.Adapt<List<ExamTaskDetailEntity>>()).UpdateColumns(v => new
            {
                v.InTest
            }, true).ExecuteCommandAsync();
        }

        await _applyPurposeRep.Context.Updateable(applyPurposes.Adapt<List<ApplyPurposeEntity>>()).UpdateColumns(v => new
        {
            v.SampleStatus,
            v.SampleStatusName,
            v.TaskId
        }, true).ExecuteCommandAsync();

        await _examInfoRep.Context.Insertable(examInfoList.Adapt<List<ExamInfoEntity>>()).ExecuteCommandAsync();
        await _examResultRep.Context.Insertable(examResultList.Adapt<List<ExamResultEntity>>()).ExecuteCommandAsync();

        var initInput = new InitializeDefaultResultInput
        {
            ExamInfoId = examInfoList.First().Id,
            ResultType = 1,
            Barcode = input.Barcode,
            WFCode = input.WFCode,
            SampleNo = input.SampleNo,
            TestDate = examInfoList.First().TestDate!.Value
        };
        await InitializeDefaultResult(initInput);

        return examInfoList.First();
    }

    /// <summary>
    /// 获取特检结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ExamSpecialResultDto>> GetSpecialResultList(ExamSpecialResultQueryInput input)
    {
        if (input.ExamInfoId <= 0)
            throw ResultOutput.Exception("invalid examId.");
        if (input.ResultType <= 0)
            throw ResultOutput.Exception("invalid result type.");

        var resultList = await _examSpecialResultRep.GetListAsync(v => v.ExamInfoId == input.ExamInfoId && (v.ResultType == input.ResultType || v.ResultType == 3));
        if (input.ResultType == 2 && !resultList.Any(v => v.ResultType == 2))
        {
            var firstResultList = await _examSpecialResultRep.GetListAsync(v => v.ExamInfoId == input.ExamInfoId && v.ResultType == 1);
            firstResultList.ForEach(v =>
            {
                v.ProId = AppInfo.User.Id;
                v.ProName = AppInfo.User.Name;
                v.ProTime = DateTime.Now;
                v.ModId = null;
                v.ModName = null;
                v.ModTime = null;
                v.Id = 0;
                v.ResultType = 2;
            });

            await _examSpecialResultRep.InsertRangeAsync(firstResultList);
            resultList = await _examSpecialResultRep.GetListAsync(v => v.ExamInfoId == input.ExamInfoId && (v.ResultType == input.ResultType || v.ResultType == 3));
        }

        return resultList.Adapt<List<ExamSpecialResultDto>>();
    }

    /// <summary>
    /// 初始化默认结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [NonAction]
    public async Task InitializeDefaultResult(InitializeDefaultResultInput input)
    {
        var customerCode = input.Barcode.Substring(0, 6);
        var templateIds = await _templateRep.AsQueryable().Where(v => (v.CustomerCodes.Contains(customerCode) || string.IsNullOrWhiteSpace(v.CustomerCodes)) && v.WFCode == input.WFCode && v.IsDefaultResult == true).OrderByDescending(v => v.CustomerCodes).Select(v => v.Id).ToListAsync();
        if (templateIds != null && templateIds.Count > 0)
        {
            var template = await _templateRep.GetByIdAsync(templateIds.First());
            if (!string.IsNullOrWhiteSpace(template.TemplateContent))
            {
                var content = DesEncrypt.Decrypt(template.TemplateContent);

                var jobject = JsonHelper.Deserialize<JObject>(content);
                List<ExamSpecialResultDto> specialResultList = new();
                foreach (var property in jobject.Properties())
                {
                    specialResultList.Add(new ExamSpecialResultDto
                    {
                        ExamInfoId = input.ExamInfoId,
                        GroupCode = LimsConsts.PathologyGroupCode,
                        Barcode = input.Barcode,
                        SampleNo = input.SampleNo,
                        TestDate = input.TestDate,
                        FieldCode = property.Name,
                        FieldName = "",
                        FieldValue = property.Value.ToString(),
                        ResultType = PathologyConsts.OtherResultField.Contains(property.Name) ? input.ResultType : 3,
                    });
                }

                await _examSpecialResultRep.Context.Insertable(specialResultList.Adapt<List<ExamSpecialResultEntity>>()).ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 删除登记
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    [AdminTransaction]
    public async Task<List<long>> PathologyBack(PathologyBackInput input)
    {
        if (input == null)
            throw ResultOutput.Exception("input can not be null.");

        if (string.IsNullOrWhiteSpace(input.WFCode))
            throw ResultOutput.Exception("wfcode can not be null.");

        if (input.ExamInfoIdList == null || input.ExamInfoIdList.Count == 0)
            throw ResultOutput.Exception("examInfoIdList can not be null or empty.");

        var examInfos = await _examInfoRep.GetListAsync(v => input.ExamInfoIdList.Contains(v.Id));
        if (examInfos == null)
            throw ResultOutput.Exception("no exam info.");

        var examIds = examInfos.Select(v => v.Id).ToList();
        var taskIds = examInfos.Select(v => v.TaskId).Distinct().ToList();
        bool unSortedReceive = await _paramService.GetParamValue(LimsConsts.PathologyReceiveMode, "0", TimeSpan.FromMinutes(5)) == "0";

        var sampleStatus = unSortedReceive ? (int)SampleStatusEnum.Confirmed : (int)SampleStatusEnum.Handovered;
        var sampleStatusName = unSortedReceive ? SampleStatusEnum.Confirmed.ToDescription() : SampleStatusEnum.Handovered.ToDescription();

        if (unSortedReceive)
        {
            await _taskRep.AsUpdateable().SetColumns(v => v.IsDeleted == true).Where(v => taskIds.Contains(v.Id)).ExecuteCommandAsync();
            await _taskDetailRep.AsUpdateable().SetColumns(v => v.IsDeleted == true).Where(v => taskIds.Contains(v.TaskId)).ExecuteCommandAsync();
        }
        else
        {
            await _taskDetailRep.AsUpdateable().SetColumns(v => v.InTest == 0).Where(v => taskIds.Contains(v.TaskId)).ExecuteCommandAsync();
        }

        await _applyPurposeRep.AsUpdateable()
            .SetColumns(v => v.SampleStatus == sampleStatus)
            .SetColumns(v => v.SampleStatusName == sampleStatusName)
            .Where(v => taskIds.Contains(v.TaskId)).ExecuteCommandAsync();

        await _examInfoRep.AsUpdateable()
            .SetColumns(v => v.IsDeleted == true)
            .Where(v => examIds.Contains(v.Id))
            .ExecuteCommandAsync();

        await _examResultRep.AsUpdateable()
            .SetColumns(v => v.IsDeleted == true)
            .Where(v => examIds.Contains(v.ExamInfoId))
            .ExecuteCommandAsync();

        await _examSpecialResultRep.AsUpdateable()
            .SetColumns(v => v.IsDeleted == true)
            .Where(v => examIds.Contains(v.ExamInfoId))
            .ExecuteCommandAsync();

        return examIds;
    }

    /// <summary>
    /// 保存结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<bool> SaveResult(SaveResultInput input)
    {
        if (input == null)
            throw ResultOutput.Exception("input can not be null or empty.");

        if (input.ExamInfoId <= 0)
            throw ResultOutput.Exception("invalid examInfoId.");

        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("examInfo not found.");

        if (input.ResultType == 1
            && examInfo.SampleStatus != SampleStatusEnum.Testing.ToInt()
            && examInfo.SampleStatus != SampleStatusEnum.GiantInspection.ToInt())
        {
            throw ResultOutput.Exception($"当前样本状态为{((SampleStatusEnum)examInfo.SampleStatus).ToDescription()}，无法保存结果！");
        }

        if (input.ResultType == 2
            && examInfo.SampleStatus != SampleStatusEnum.Testing.ToInt()
            && examInfo.SampleStatus != SampleStatusEnum.GiantInspection.ToInt()
             && examInfo.SampleStatus != SampleStatusEnum.FirstCheck.ToInt())
        {
            throw ResultOutput.Exception($"当前样本状态为{((SampleStatusEnum)examInfo.SampleStatus).ToDescription()}，无法保存结果！");
        }

        var resultInput = input.SpecialResultList.Adapt<List<ExamSpecialResultEntity>>();
        if (resultInput != null && resultInput.Count > 0)
        {
            var updateResult = resultInput.FindAll(v => v.Id > 0);
            if (updateResult.Any())
            {
                var ret = await _examSpecialResultRep.Context.Updateable(resultInput).UpdateColumns(v => new
                {
                    v.FieldValue
                }, true).ExecuteCommandAsync();
            }

            var insertResult = resultInput.FindAll(v => v.Id == 0);
            if (insertResult.Any())
            {
                foreach (var item in insertResult)
                {
                    item.ExamInfoId = examInfo.Id;
                    item.GroupCode = examInfo.GroupCode;
                    item.Barcode = examInfo.Barcode;
                    item.SampleNo = examInfo.SampleNo;
                    item.TestDate = examInfo.TestDate!.Value;
                }
                var ret = await _examSpecialResultRep.Context.Insertable(insertResult).ExecuteCommandAsync();
            }
        }
        if (input.Doctor != null)
        {
            bool needUpdateExam = false;
            var updateable = _examInfoRep.AsUpdateable();
            if (examInfo.SecondAuditAuthorizedId != input.Doctor.SecondDoctorId)
            {
                updateable = updateable.SetColumns(v => v.SecondAuditAuthorizedId == input.Doctor.SecondDoctorId)
                    .SetColumns(v => v.SecondAuditId == AppInfo.User.Id)
                    .SetColumns(v => v.SecondAuditName == input.Doctor.SecondDoctor);

                needUpdateExam = true;
            }
            if (examInfo.ApproverAuthorizedId != input.Doctor.ReportDoctorId)
            {
                updateable = updateable.SetColumns(v => v.ApproverAuthorizedId == input.Doctor.ReportDoctorId)
                    .SetColumns(v => v.ApproverId == AppInfo.User.Id)
                    .SetColumns(v => v.ApproverName == input.Doctor.ReportDoctor);

                needUpdateExam = true;
            }

            if (examInfo.SecondAuditTime != input.Doctor.ReportTime)
            {
                updateable = updateable.SetColumns(v => v.SecondAuditTime == input.Doctor.ReportTime);
                needUpdateExam = true;
            }

            if (needUpdateExam)
            {
                var r = await updateable.Where(v => v.Id == input.ExamInfoId).ExecuteCommandAsync();
            }
        }

        return true;
    }

    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    [HttpPost]
    public async Task<AuditResultDto> PathologyAudit(AuditInput input)
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

        long uid = AppInfo.User.Id;
        if (input.AuditType == OperationTypeEnum.SecondCheck)
        {
            if (examInfo.ApproverAuthorizedId == null)
            {
                examInfo.ApproverAuthorizedId = AppInfo.User.Id;
                examInfo.ApproverId = AppInfo.User.Id;
                examInfo.ApproverName = AppInfo.User.Name;
            }

            if (examInfo.SecondAuditId == null)
            {
                examInfo.SecondAuditAuthorizedId = AppInfo.User.Id;
                examInfo.SecondAuditId = AppInfo.User.Id;
                examInfo.SecondAuditName = AppInfo.User.Name;
            }

            uid = examInfo.SecondAuditAuthorizedId!.Value;
        }

        ckResult = await _sampleTestService.CheckUserGroupPermission(uid, examInfo.WFCode!, input.AuditType);
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
            var currSetting = await _settingService.GetSettingByWfCode(examInfo.WFCode!);
            if (!currSetting.CanSameUserReport && examInfo.SecondAuditAuthorizedId == examInfo.ApproverAuthorizedId)
                throw ResultOutput.Exception("审核失败，审核人不能与审批人相同！");

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
    public async Task<ExamInfoDto> UnAudit(UnAuditInput input) => await _sampleTestService.UnAudit(input);

    /// <summary>
    /// 获取病理复诊医生
    /// </summary>
    /// <param name="wfCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<UserGetOptionDto>> GetPathologySecondAuditUsers(string wfCode)
    {
        var group = await _baseGroupRep.GetListAsync(v => v.GroupCode == LimsConsts.PathologyGroupCode || v.GroupCode == wfCode);
        var groupCodes = group.Select(v => v.GroupCode).ToList();

        var query = _userGroupRep.AsQueryable()
            .InnerJoin<UserEntity>((a, b) => a.UserId == b.Id)
            .Where(a => groupCodes.Contains(a.GroupCode))
            .Where(a => a.CanSecondCheck == 1);
        //switch (operType)
        //{
        //    case OperationTypeEnum.FirstCheck:
        //        query.Where(a => a.CanFirstCheck == 1 || a.CanSecondCheck == 1);
        //        break;
        //    case OperationTypeEnum.SecondCheck:
        //        query.Where(a => a.CanSecondCheck == 1);
        //        break;
        //}

        var userList = await query.Select((a, b) => new UserGetOptionDto
        {
            Id = a.UserId,
            Name = b.Name,
            UserName = b.UserName
        }).ToListAsync();

        return userList;
    }

    /// <summary>
    /// 保存取材部位明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> SaveSamplingSpotDetail(List<ExamPathologySamplingSpotUpdateInput> input)
    {
        var addList = input.FindAll(v => v.Id <= 0);
        if (addList.Any())
            await _examSamplingSpotRep.InsertRangeAsync(addList.Adapt<List<ExamPathologySamplingSpotEntity>>());

        var updateList = input.FindAll(v => v.Id > 0);
        if (updateList.Any())
            await _examSamplingSpotRep.UpdateRangeAsync(updateList.Adapt<List<ExamPathologySamplingSpotEntity>>());

        return true;
    }

    /// <summary>
    /// 获取取材部位明细
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    public async Task<List<ExamPathologySamplingSpotDto>> GetSamplingSpotDetail(long examInfoId)
    {
        if (examInfoId <= 0)
            throw ResultOutput.Exception("invalid examInfoId.");

        var ret = await _examSamplingSpotRep.AsQueryable()
            .InnerJoin<BasePathologySamplingSpotEntity>((a, b) => a.SamplingSpotCode == b.SamplingSpotCode && b.IsValid && !b.IsDeleted)
            .InnerJoin<BasePathologySampleTypeEntity>((a, b, c) => a.SampleTypeCode == c.SampleTypeCode && c.IsValid && !c.IsDeleted)
            .Where((a, b, c) => a.ExamInfoId == examInfoId)
            .Select((a, b, c) => new ExamPathologySamplingSpotDto
            {
                Id = a.Id,
                ExamInfoId = a.ExamInfoId,
                SampleTypeCode = a.SampleTypeCode,
                SampleTypeName = c.SampleTypeName,
                SamplingSpotCode = a.SamplingSpotCode,
                SamplingSpotName = b.SamplingSpotName
            }).ToListAsync();

        return ret;
    }
}
