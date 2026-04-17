using DaLang.Lims.BaseData.Contracts.BasePurpose;
using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.BaseData.Services.SampleType;
using DaLang.Lims.Pretreatment.Contracts.Entrust;
using DaLang.Lims.Pretreatment.Contracts.Entrust.Dto;
using DaLang.Lims.Pretreatment.Contracts.SampleInput;
using DaLang.Lims.Pretreatment.Contracts.SampleInput.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleAmount;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleInfo;
using DaLang.Lims.Shared.Contracts.ApplyItem.Dto;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyItem;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Web.Common.Consts;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Parameter;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tsp;
using Yitter.IdGenerator;

namespace DaLang.Lims.Pretreatment.Application.Input;

/// <summary>
/// 录入服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class SampleInputService : BaseService, ISampleInputService, IDynamicApi
{
    private readonly AdminRepositoryBase<PretreatSampleInfoEntity> _sampleInfoRep;
    private readonly AdminRepositoryBase<PretreatSampleAmountEntity> _sampleAmountRep;
    private readonly AdminRepositoryBase<ApplyInfoEntity> _applyInfoRep;
    private readonly AdminRepositoryBase<ApplyPurposeEntity> _applyPurposeRep;
    private readonly AdminRepositoryBase<ApplyItemEntity> _applyItemRep;
    private readonly IBasePurposeService _basePurposeService;
    private readonly BaseSampleTypeService _sampleTypeService;
    private readonly IEntrustService _entrustService;
    private readonly IBaseCustomerRepository _customerRep;
    private IParameterService _paramService;

    public SampleInputService(AdminRepositoryBase<PretreatSampleInfoEntity> sampleInfoRep,
        AdminRepositoryBase<PretreatSampleAmountEntity> sampleAmountRep,
        AdminRepositoryBase<ApplyInfoEntity> applyInfoRep,
        AdminRepositoryBase<ApplyPurposeEntity> applyPurposeRep,
        AdminRepositoryBase<ApplyItemEntity> applyItemRep,
        IBasePurposeService basePurposeService,
         BaseSampleTypeService sampleTypeService,
         IEntrustService entrustService,
         IBaseCustomerRepository customerRep,
          IParameterService paramService)
    {
        _sampleInfoRep = sampleInfoRep;
        _sampleAmountRep = sampleAmountRep;
        _applyInfoRep = applyInfoRep;
        _applyPurposeRep = applyPurposeRep;
        _applyItemRep = applyItemRep;
        _basePurposeService = basePurposeService;
        _sampleTypeService = sampleTypeService;
        _entrustService = entrustService;
        _customerRep = customerRep;
        _paramService = paramService;
    }
    /// <summary>
    /// 判断条码是否已使用
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    public async Task<bool> CheckBarcodeInUse(string barcode)
    {
        var exists = await _sampleAmountRep.AsQueryable().AnyAsync(a => a.Barcode == barcode);
        if (!exists)
            exists = await _sampleInfoRep.AsQueryable().AnyAsync(a => a.Barcode == barcode);

        return exists;
    }

    /// <summary>
    /// 保存信息和项目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<InputSaveSuccessOutput> SaveInfoAndItem(InputSaveDto input)
    {
        if (input.InputType == InputTypeEnum.Item || input.InputType == InputTypeEnum.DirectInput)
        {
            if (input.InputItems == null || input.InputItems?.Count == 0)
                throw ResultOutput.Exception("项目不能为空！");
        }

        if (input.InputType != InputTypeEnum.Item && string.IsNullOrWhiteSpace(input.InputInfo.PatientName))
            throw ResultOutput.Exception("姓名不能为空！");

        if (string.IsNullOrWhiteSpace(input.InputInfo?.Barcode) || input.InputInfo.Barcode.Length < 12)
            throw ResultOutput.Exception($"条码位数有误！");

        if (input.InputType == InputTypeEnum.DirectInput)
        {
            var customerCode = input.InputInfo.Barcode.Substring(0, 6);
            var customer = await _customerRep.AsQueryable()
                .Where(v => v.CustomerCode == customerCode)
                .Select(v => new BaseCustomerDto
                {
                    CustomerCode = customerCode,
                    CustomerName = v.CustomerName,
                }).FirstAsync();

            if (customer == null)
                throw ResultOutput.Exception($"客户不存在！");

            var paramValue = await _paramService.GetParamValue(LimsConsts.PushForwardTime, "7");
            if (!int.TryParse(paramValue, out int pushForwardTime))
                throw ResultOutput.Exception("PushForwardTime参数设置有误！");

            var sampleTypeList = await _sampleTypeService.GetAllAsync();
            var purCodes = input.InputItems.Select(v => v.PurCode).ToList();
            var allPurposes = await _basePurposeService.GetPurposeDetail(purCodes, input.InputInfo.Barcode.Substring(0, 6));

            #region check item is duplicated
            for (int i = 0; i < input.InputItems.Count; i++)
            {
                for (int j = i + 1; j < input.InputItems.Count; j++)
                {
                    if (input.InputItems[i].SampleTypeCode != input.InputItems[j].SampleTypeCode)
                        continue;

                    var itemList1 = allPurposes.FindAll(v => v.PurCode == input.InputItems[i].PurCode).Select(v => v.ItemCode);
                    var itemList2 = allPurposes.FindAll(v => v.PurCode == input.InputItems[j].PurCode).Select(v => v.ItemCode);

                    var tmp = itemList1.Union(itemList2).Distinct();

                    if (tmp.Count() == itemList1.Count() || tmp.Count() == itemList2.Count())
                    {
                        throw ResultOutput.Exception($"{input.InputItems[i].PurCode}-{input.InputItems[i].PurName}和{input.InputItems[j].PurCode}-{input.InputItems[j].PurName}项目完全重复！");
                    }

                }
            }
            #endregion

            var applyInfo = input.InputInfo.Adapt<ApplyInfoEntity>();
            applyInfo.CustomerCode = customerCode;
            applyInfo.CustomerName = customer.CustomerName;
            applyInfo.DataSource = 2;

            List<ApplyPurposeDto> applyPurposeList = new();
            List<ApplyItemDto> applyItemList = new();

            List<CalcEntrustDto> calcEntrustList = new();
            var purGroup = input.InputItems.GroupBy(v => v.SampleTypeCode);
            foreach (var purs in purGroup)
            {
                var param = new CalcEntrustInput
                {
                    CustomerCode = input.InputInfo.Barcode.Substring(0, 6),
                    PurCodeList = purs.Select(v => v.PurCode).ToList(),
                    ReceiveTime = DateTime.Now,
                    SampleTypeCode = purs.Key
                };
                var ret = await _entrustService.CalcEntrustAsync(param);
                calcEntrustList.AddRange(ret);
            }

            foreach (var item in input.InputItems)
            {
                long purposeId = YitIdHelper.NextId();
                var currPurpose = allPurposes.FindAll(v => v.PurCode == item.PurCode);

                var groupCode = currPurpose.First().GroupCode;
                var groupName = currPurpose.First().GroupName;

                var entrust = calcEntrustList.FirstOrDefault(v => v.PurCode == item.PurCode);
                applyPurposeList.Add(new ApplyPurposeDto
                {
                    Id = purposeId,
                    GroupCode = entrust?.IsEntrust == true ? LimsConsts.EntrustGroupCode : groupCode,
                    GroupName = entrust?.IsEntrust == true ? "外送" : groupName,
                    Barcode = input.InputInfo.Barcode,
                    ComboCode = item.ComboCode,
                    ComboName = item.ComboName,
                    PurCode = currPurpose.First().PurCode,
                    PurName = currPurpose.First().PurName,
                    PurNamePersonalize = currPurpose.First().PurNamePersonalize,
                    InstrumentItemCodes = string.Join(",", currPurpose.Select(a => a.InstrumentItemCode).Distinct().OrderBy(a => a)),
                    SampleTypeCode = item.SampleTypeCode,
                    SampleTypeName = sampleTypeList.FirstOrDefault(v => v.SampleTypeCode == item.SampleTypeCode)?.SampleTypeName,
                    OriginalGroupCode = currPurpose.First().GroupCode,
                    OriginalGroupName = currPurpose.First().GroupName,
                    AddType = 0,
                    SampleStatus = (int)SampleStatusEnum.Confirmed,
                    SampleStatusName = Enum.GetName(typeof(SampleStatusEnum), SampleStatusEnum.Confirmed),
                    DataSource = 1,
                    EntrustHospitalCode = entrust?.EntrustHospitalCode,
                    EntrustHospitalName = entrust?.EntrustHospitalName,
                    EntrustStatus = string.IsNullOrWhiteSpace(entrust?.EntrustHospitalCode) ? 0 : 1,
                    TestExamPlanCode = currPurpose.First().ExamPlan
                });

                foreach (var purDetail in currPurpose)
                {
                    applyItemList.Add(new ApplyItemDto
                    {
                        ApplyPurposeId = purposeId,
                        GroupCode = groupCode,
                        GroupName = groupName,
                        ComboCode = item.ComboCode,
                        Barcode = input.InputInfo.Barcode,
                        PurCode = purDetail.PurCode,
                        InstrumentItemCode = purDetail.InstrumentItemCode,
                        ItemCode = purDetail.ItemCode,
                        ItemName = purDetail.ItemName,
                        ItemNamePersonalize = purDetail.ItemNamePersonalize,
                    });
                }
            }

            var currBarcodePurpose = applyPurposeList.OrderBy(a => a.PurCode).ToList();
            var sampleTypeCodes = currBarcodePurpose.Select(a => a.SampleTypeCode);

            applyInfo.PurCodes = string.Join(",", currBarcodePurpose.Select(a => a.PurCode).Distinct());
            applyInfo.PurNames = string.Join(",", currBarcodePurpose.Select(a => a.PurName).Distinct());
            applyInfo.SampleTypeCode = sampleTypeCodes.Count() == 1 ? sampleTypeCodes.First() : "999";
            applyInfo.SampleTypeName = sampleTypeCodes.Count() == 1 ? currBarcodePurpose.First().SampleTypeName : "多种标本类型";
            if (applyInfo.CollectTime == null)
                applyInfo.CollectTime = DateTime.Now.AddHours(-pushForwardTime).Date.AddHours(8);

            var sampleAmount = new PretreatSampleAmountEntity
            {
                CustomerCode = applyInfo.CustomerCode,
                CustomerName = applyInfo.CustomerName,
                Barcode = applyInfo.Barcode,
                DataSource = 2,
                SampleCnt = applyPurposeList.GroupBy(v => v.SampleTypeCode).Count(),
            };

            await _applyInfoRep.InsertAsync(applyInfo);
            await _applyPurposeRep.InsertRangeAsync(applyPurposeList.Adapt<List<ApplyPurposeEntity>>());
            await _applyItemRep.InsertRangeAsync(applyItemList.Adapt<List<ApplyItemEntity>>());
            await _sampleAmountRep.InsertAsync(sampleAmount);
        }
        else
        {
            //TODO 双输
        }

        return null;
    }
}
