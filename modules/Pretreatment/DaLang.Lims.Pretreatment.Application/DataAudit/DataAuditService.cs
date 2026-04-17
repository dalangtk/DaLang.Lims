using DaLang.Lims.Shared.Contracts.ApplyInfo.Dto;
using DaLang.Lims.Shared.Contracts.ApplyItem.Dto;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyItem;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.BaseData.Contracts.BasePurpose;
using DaLang.Lims.BaseData.Contracts.Combo.Dto;
using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.BaseData.Domain.Combo;
using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.BaseData.Domain.SampleType;
using DaLang.Lims.Pretreatment.Contracts.DataAudit;
using DaLang.Lims.Pretreatment.Contracts.DataAudit.Dto;
using DaLang.Lims.Pretreatment.Contracts.DataImport;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.PretreatCustomerPurposeMatch;
using DaLang.Lims.Pretreatment.Domain.PretreatDataImport;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.BaseData.Domain.EntrustPurpose;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Parameter;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Yitter.IdGenerator;
using DaLang.Lims.Web.Common.Consts;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleAmount;

namespace DaLang.Lims.Pretreatment.Services.DataAudit;

/// <summary>
/// 数据审核
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class DataAuditService : BaseService, IDataAuditService, IDynamicApi
{
    private IPretreatDataImportRepository _dataImportRep;
    private IPretreatCustomerPurposeMatchRepository _purMatchRep;
    private IBasePurposeRepository _basePurposeRep;
    private IBasePurposeDetailRepository _basePurposeDetailRep;
    private IBaseItemRepository _baseItemRep;
    private IBasePurposeService _basePurposeService;
    private AdminRepositoryBase<ApplyInfoEntity> _applyInfoRep;
    private AdminRepositoryBase<ApplyPurposeEntity> _applyPurposeRep;
    private AdminRepositoryBase<ApplyItemEntity> _applyItemRep;
    private ICacheTool _cacheTool;
    private IBaseSampleTypeRepository _baseSampleTypeRep;
    private IBaseEntrustPurposeRepository _baseEntrustPurposeRep;
    private IParameterService _paramService;
    private IBasePurposeTenantSettingRepository _purTenantSettingRep;
    private IBaseComboRepository _comboRep;
    private AdminRepositoryBase<PretreatSampleAmountEntity> _sampleAmountRep;
    public DataAuditService(IDataImportService dataImportService,
        IPretreatDataImportRepository dataImportRep,
        IPretreatCustomerPurposeMatchRepository purMatchRep,
        IBasePurposeRepository basePurposeRep,
        IBasePurposeDetailRepository basePurposeDetailRep,
        IBaseItemRepository baseItemRep,
        IBasePurposeService basePurposeService,
        AdminRepositoryBase<ApplyInfoEntity> applyInfoRep,
        AdminRepositoryBase<ApplyPurposeEntity> applyPurposeRep,
        AdminRepositoryBase<ApplyItemEntity> applyItemRep,
        IBaseSampleTypeRepository baseSampleTypeRep,
        ICacheTool cacheTool,
        IBaseEntrustPurposeRepository baseEntrustPurposeRep,
        IParameterService paramService,
        IBasePurposeTenantSettingRepository purTenantSettingRep,
        IBaseComboRepository comboRep,
        AdminRepositoryBase<PretreatSampleAmountEntity> sampleAmountRep)
    {
        _dataImportRep = dataImportRep;
        _purMatchRep = purMatchRep;
        _basePurposeRep = basePurposeRep;
        _basePurposeDetailRep = basePurposeDetailRep;
        _baseItemRep = baseItemRep;
        _basePurposeService = basePurposeService;
        _applyInfoRep = applyInfoRep;
        _applyPurposeRep = applyPurposeRep;
        _applyItemRep = applyItemRep;
        _cacheTool = cacheTool;
        _baseSampleTypeRep = baseSampleTypeRep;
        _baseEntrustPurposeRep = baseEntrustPurposeRep;
        _paramService = paramService;
        _purTenantSettingRep = purTenantSettingRep;
        _comboRep = comboRep;
        _sampleAmountRep = sampleAmountRep;
    }
    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="auditIds"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    [AdminTransaction]
    public async Task<List<DataAuditOutput>> DataAudit([Required] List<long> auditIds)
    {
        if (auditIds == null || auditIds.Count == 0)
            throw ResultOutput.Exception("参数有误！");

        var waitAuditList = await _dataImportRep.GetListAsync(a => auditIds.Contains(a.Id));
        if (!waitAuditList.Any())
            throw ResultOutput.Exception("未找到任何数据！");

        var paramValue = await _paramService.GetParamValue(LimsConsts.PushForwardTime, "7");
        if (!int.TryParse(paramValue, out int pushForwardTime))
            throw ResultOutput.Exception("PushForwardTime参数设置有误！");

        var sampleTypes = await _baseSampleTypeRep.GetListAsync(a => a.IsValid);
        var auditResultList = new List<DataAuditOutput>();
        var customerList = waitAuditList.Select(a => a.CustomerCode).Distinct();
        var purMatchs = await _purMatchRep.GetListAsync(a => customerList.Contains(a.CustomerCode));
        var applyInfoList = new List<ApplyInfoDto>();
        var applyPurposeList = new List<ApplyPurposeDto>();
        var applyItemList = new List<ApplyItemDto>();
        foreach (var currCustomerGroup in waitAuditList.GroupBy(a => a.CustomerCode))
        {
            var currCustomerPurDetailList = new List<PurposeWithDetailListDto>();
            foreach (var currInfo in currCustomerGroup)
            {
                if (currInfo.AuditStatus == 1)
                {
                    auditResultList.Add(new DataAuditOutput
                    {
                        Barcode = currInfo.Barcode,
                        AuditFailedType = 1,
                        AuditStatus = false,
                        ErrMsg = "已审核",
                        PatientName = currInfo.PatientName
                    });
                    continue;
                }
                var customerPurCodes = currInfo.PurCodes.Split(',').ToList();
                var customerPurNames = currInfo.PurNames.Split(',').ToList();
                if (customerPurCodes.Exists(a => !purMatchs.Select(b => b.CustomerPurCode).Contains(a)))
                {
                    var notMatchPurList = new List<CustomerPurMatchMainDto>();
                    var notMatchPurs = customerPurCodes.FindAll(a => !purMatchs.Select(b => b.CustomerPurCode).Contains(a));
                    foreach (var code in notMatchPurs)
                    {
                        notMatchPurList.Add(new CustomerPurMatchMainDto
                        {
                            CustomerCode = currInfo.CustomerCode,
                            CustomerPurCode = code,
                            CustomerPurName = customerPurNames.ElementAt(customerPurCodes.FindIndex(a => a == code))
                        });
                    }

                    auditResultList.Add(new DataAuditOutput
                    {
                        CustomerCode = currInfo.CustomerCode,
                        Barcode = currInfo.Barcode,
                        AuditFailedType = 0,
                        AuditStatus = false,
                        ErrMsg = "目的对照缺失",
                        PatientName = currInfo.PatientName,
                        NeedMatchPurList = notMatchPurList
                    });
                    continue;
                }
                else
                {
                    //TODO 套餐核验
                    List<BaseComboDetailDto> comboDetails = new();
                    var currPurMatchCombos = purMatchs.FindAll(a => customerPurCodes.Contains(a.CustomerPurCode) && a.IsCombo == 1);
                    if (currPurMatchCombos.Any())
                    {
                        var comboCodes = currPurMatchCombos.Select(v => v.CentralPurCode).Distinct().ToList();
                        comboDetails = await _comboRep.AsQueryable()
                            .InnerJoin<BaseComboDetailEntity>((a, b) => a.ComboCode == b.ComboCode && b.IsValid)
                            .Where((a, b) => comboCodes.Contains(a.ComboCode) && a.IsValid)
                            .Select((a, b) =>
                            new BaseComboDetailDto
                            {
                                ComboCode = a.ComboCode,
                                ComboName = a.ComboName,
                                PurCode = b.PurCode,
                                SampleTypeCode = a.SampleTypeCode ?? ""
                            })
                            .ToListAsync();

                        comboDetails.ForEach(v =>
                        {
                            v.SampleTypeName = sampleTypes.FirstOrDefault(s => s.SampleTypeCode == v.SampleTypeCode)?.SampleTypeName!;
                        });

                        var currCombos = comboDetails.Select(v => v.ComboCode).Distinct().ToList();
                        var notExistsCodes = comboCodes.FindAll(a => !currCombos.Contains(a));

                        if (notExistsCodes.Any())
                            throw ResultOutput.Exception($"套餐{string.Join(",", notExistsCodes)}不存在或已停用！");
                    }
                    var currPurMatchPurposes = purMatchs.FindAll(a => customerPurCodes.Contains(a.CustomerPurCode) && a.IsCombo != 1);
                    var centralPurCodes = currPurMatchPurposes.Select(a => a.CentralPurCode).Distinct().ToList();
                    var comboPurs = comboDetails.Select(v => v.PurCode).Distinct().ToList();
                    if (centralPurCodes.Intersect(comboPurs).Any())
                    {
                        var existsPurCode = centralPurCodes.Intersect(comboPurs);
                        throw ResultOutput.Exception($"套餐中{string.Join(",", existsPurCode)}目的重复！");
                    }

                    centralPurCodes.AddRange(comboDetails.Select(v => v.PurCode).Distinct().ToList());

                    var insPurpose = await _basePurposeRep//.GetListAsync(a => centralPurCodes.Contains(a.PurCode));
                                        .AsQueryable()
                                        .InnerJoin<BasePurposeTenantSettingEntity>((a, b) => a.PurCode == b.PurCode)
                                        .Where((a, b) => centralPurCodes.Contains(a.PurCode))
                                        .Select((a, b) =>
                                        new
                                        {
                                            b.ExamPlan,
                                            a.PurCode,
                                            a.PurName,
                                            b.IsValid,
                                        }).ToListAsync();
                    //var insPurpose = await _purTenantSettingRep.GetListAsync(a => centralPurCodes.Contains(a.PurCode));


                    if (insPurpose.Exists(a => a.IsValid == false))
                    {
                        auditResultList.Add(new DataAuditOutput
                        {
                            Barcode = currInfo.Barcode,
                            AuditFailedType = 1,
                            AuditStatus = false,
                            ErrMsg = $"目的{insPurpose.First(a => a.IsValid == false).PurName}已停用",
                            PatientName = currInfo.PatientName
                        });
                        continue;
                    }
                    else if (insPurpose.Exists(a => string.IsNullOrWhiteSpace(a.ExamPlan)))
                    {
                        auditResultList.Add(new DataAuditOutput
                        {
                            Barcode = currInfo.Barcode,
                            AuditFailedType = 1,
                            AuditStatus = false,
                            ErrMsg = $"目的{insPurpose.First(a => string.IsNullOrWhiteSpace(a.ExamPlan)).PurName}未设置检测计划",
                            PatientName = currInfo.PatientName
                        });
                        continue;
                    }
                    else
                    {
                        //purCodes.RemoveAll(a => currCustomerPurDetailList.Exists(b => b.PurCode == a));
                        var notExistsCodes = centralPurCodes.FindAll(a => !currCustomerPurDetailList.Exists(b => b.PurCode == a));
                        if (notExistsCodes.Any())
                        {
                            var purposeDetail = await _basePurposeService.GetPurposeDetail(centralPurCodes, currCustomerGroup.Key);
                            if (!purposeDetail.Any())
                            {
                                auditResultList.Add(new DataAuditOutput
                                {
                                    Barcode = currInfo.Barcode,
                                    AuditFailedType = 1,
                                    AuditStatus = false,
                                    ErrMsg = $"获取目的明细失败！",
                                    PatientName = currInfo.PatientName
                                });
                                continue;
                            }
                            else
                            {
                                currCustomerPurDetailList.AddRange(purposeDetail);
                            }
                        }

                        var currBarcodePurposeDetail = currCustomerPurDetailList.FindAll(a => centralPurCodes.Contains(a.PurCode));
                        currBarcodePurposeDetail.ForEach(a =>
                        {
                            if (!string.IsNullOrWhiteSpace(a.SampleTypeCode))
                            {
                                var typeList = a.SampleTypeCode.Split(',');
                                var typeNameList = new List<string>();
                                for (int i = 0; i < typeList.Length; i++)
                                    typeNameList.Add(sampleTypes.FirstOrDefault(a => a.SampleTypeCode == typeList[i])?.SampleTypeName);
                                a.SampleTypeName = string.Join(",", typeNameList);
                            }
                        });

                        var currPurposeGroup = currCustomerPurDetailList.FindAll(a => centralPurCodes.Contains(a.PurCode)).GroupBy(a => a.PurCode);
                        foreach (var currPurpose in currPurposeGroup)
                        {
                            string entrustHospitalCode = null;
                            string entrustHospitalName = null;
                            var curPurEntrustPurpose = await _baseEntrustPurposeRep.GetFirstAsync(v => v.PurCode == currPurpose.Key);
                            if (curPurEntrustPurpose != null)
                            {
                                if (curPurEntrustPurpose.BeginTime >= DateTime.Now
                                    && DateTime.Now <= curPurEntrustPurpose.EndTime)
                                {
                                    if ((!string.IsNullOrWhiteSpace(curPurEntrustPurpose.CustomerCode)
                                        && curPurEntrustPurpose.IsCustomerReverse == false
                                        && curPurEntrustPurpose.CustomerCode.Contains(currInfo.CustomerCode!))
                                        || (string.IsNullOrWhiteSpace(curPurEntrustPurpose.CustomerCode)
                                        && curPurEntrustPurpose.IsCustomerReverse == false)
                                        || (!string.IsNullOrWhiteSpace(curPurEntrustPurpose.CustomerCode)
                                        && curPurEntrustPurpose.IsCustomerReverse == true
                                        && !curPurEntrustPurpose.CustomerCode.Contains(currInfo.CustomerCode!)))
                                    {
                                        entrustHospitalCode = curPurEntrustPurpose.EntrustHospitalCode!;
                                        entrustHospitalName = curPurEntrustPurpose.EntrustHospitalName!;
                                    }
                                }
                            }

                            var groupCode = currPurpose.First().GroupCode;
                            var groupName = currPurpose.First().GroupName;
                            if (!string.IsNullOrWhiteSpace(entrustHospitalCode))
                            {
                                groupCode = LimsConsts.EntrustGroupCode;
                                groupName = "外送";
                            }

                            var currComboDetail = comboDetails.FirstOrDefault(v => v.PurCode == currPurpose.Key);

                            long purposeId = YitIdHelper.NextId();
                            applyPurposeList.Add(new ApplyPurposeDto
                            {
                                Id = purposeId,
                                GroupCode = groupCode,
                                GroupName = groupName,
                                Barcode = currInfo.Barcode,
                                ComboCode = currComboDetail != null ? currComboDetail.ComboCode : null,
                                ComboName = currComboDetail != null ? currComboDetail.ComboName : null,
                                PurCode = currPurpose.Key,
                                PurName = currPurpose.First().PurName,
                                PurNamePersonalize = currPurpose.First().PurNamePersonalize,
                                InstrumentItemCodes = string.Join(",", currPurpose.Select(a => a.InstrumentItemCode).Distinct().OrderBy(a => a)),
                                SampleTypeCode = !string.IsNullOrWhiteSpace(currComboDetail?.SampleTypeCode) ? currComboDetail.SampleTypeCode : currPurpose.First().SampleTypeCode,
                                SampleTypeName = !string.IsNullOrWhiteSpace(currComboDetail?.SampleTypeCode) ? currComboDetail.SampleTypeName : currPurpose.First().SampleTypeName,
                                OriginalGroupCode = currPurpose.First().GroupCode,
                                OriginalGroupName = currPurpose.First().GroupName,
                                AddType = 0,
                                SampleStatus = (int)SampleStatusEnum.Confirmed,
                                SampleStatusName = Enum.GetName(typeof(SampleStatusEnum), SampleStatusEnum.Confirmed),
                                DataSource = currInfo.DataSource,
                                EntrustHospitalCode = entrustHospitalCode,
                                EntrustHospitalName = entrustHospitalName,
                                EntrustStatus = string.IsNullOrWhiteSpace(entrustHospitalCode) ? 0 : 1,
                                TestExamPlanCode = currPurpose.First().ExamPlan
                            });

                            foreach (var purpose in currPurpose)
                            {
                                applyItemList.Add(new ApplyItemDto
                                {
                                    ApplyPurposeId = purposeId,
                                    GroupCode = groupCode,
                                    GroupName = groupName,
                                    ComboCode = currComboDetail != null ? currComboDetail.ComboCode : null,
                                    Barcode = currInfo.Barcode,
                                    PurCode = currPurpose.Key,
                                    InstrumentItemCode = purpose.InstrumentItemCode,
                                    ItemCode = purpose.ItemCode,
                                    ItemName = purpose.ItemName,
                                    ItemNamePersonalize = purpose.ItemNamePersonalize,
                                });
                            }
                        }

                        var tmpApplyInfo = currInfo.Adapt<ApplyInfoDto>();
                        tmpApplyInfo.Id = 0;
                        var currBarcodePurpose = applyPurposeList.FindAll(a => a.Barcode == currInfo.Barcode).OrderBy(a => a.PurCode).ToList();
                        var sampleTypeCodes = currBarcodePurpose.Select(a => a.SampleTypeCode);

                        tmpApplyInfo.PurCodes = string.Join(",", currBarcodePurpose.Select(a => a.PurCode).Distinct());
                        tmpApplyInfo.PurNames = string.Join(",", currBarcodePurpose.Select(a => a.PurName).Distinct());
                        tmpApplyInfo.SampleTypeCode = sampleTypeCodes.Count() == 1 ? sampleTypeCodes.First() : "999";
                        tmpApplyInfo.SampleTypeName = sampleTypeCodes.Count() == 1 ? currBarcodePurpose.First().SampleTypeName : "多种标本类型";
                        if (tmpApplyInfo.CollectTime == null)
                            tmpApplyInfo.CollectTime = DateTime.Now.AddHours(-pushForwardTime).Date.AddHours(8);

                        applyInfoList.Add(tmpApplyInfo);

                        currInfo.AuditStatus = 1;
                        currInfo.AuditId = AppInfo.User.Id;
                        currInfo.AuditName = AppInfo.User.Name;
                    }
                }
            }
        }

        var infoList = Mapper.Map<List<ApplyInfoEntity>>(applyInfoList);
        var ret = await _applyInfoRep.InsertRangeAsync(infoList);

        var sampleAmountList = infoList.Select(v => new PretreatSampleAmountEntity
        {
            Barcode = v.Barcode,
            CustomerCode = v.CustomerCode,
            SampleCnt = waitAuditList.FirstOrDefault(a => a.Barcode == v.Barcode)!.SampleCnt ?? 0,
            DataSource = v.DataSource,
            InfoStatus = 4,
            ItemStatus = 4
        }).ToList();
        await _sampleAmountRep.InsertRangeAsync(sampleAmountList);

        var purList = Mapper.Map<List<ApplyPurposeEntity>>(applyPurposeList);
        ret = await _applyPurposeRep.InsertRangeAsync(purList);

        var itemList = Mapper.Map<List<ApplyItemEntity>>(applyItemList);
        ret = await _applyItemRep.InsertRangeAsync(itemList);

        await _dataImportRep.Context.Updateable(waitAuditList.FindAll(a => a.AuditStatus == 1))
             .UpdateColumns(["AuditStatus", "AuditId", "AuditName", "ModId", "ModName", "ModTime"])
             .ExecuteCommandAsync();

        return auditResultList;
    }
}
