using DaLang.Lims.Api.Contracts;
using DaLang.Lims.Api.Contracts.Dto;
using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleAmount;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleInfo;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Shared.Domain.ExamResult;
using DaLang.Lims.Statistics.Core.ReportQuery;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Dict;
using Mapster;

namespace DaLang.Lims.Api.Application;

/// <summary>
/// 
/// </summary>
[DynamicApi(Area = ApiConsts.AreaName)]
public class EntrustService : BaseService, IDynamicApi, IEntrustService
{
    private AdminRepositoryBase<PretreatSampleAmountEntity> _sampleAmountRep;
    private IDictService _dictService;
    private AdminRepositoryBase<BaseCustomerEntity> _customerService;
    private AdminRepositoryBase<PretreatSampleInfoEntity> _pretreatmentSampleInfoRep;
    private AdminRepositoryBase<ExamInfoEntity> _examRep;
    private AdminRepositoryBase<ExamResultEntity> _examResultRep;
    public EntrustService(AdminRepositoryBase<PretreatSampleAmountEntity> sampleAmountRep,
        IDictService dictService,
        AdminRepositoryBase<BaseCustomerEntity> customerService,
        AdminRepositoryBase<PretreatSampleInfoEntity> pretreatmentSampleInfoRep,
        AdminRepositoryBase<ExamInfoEntity> examRep,
        AdminRepositoryBase<ExamResultEntity> examResultRep)
    {
        _sampleAmountRep = sampleAmountRep;
        _dictService = dictService;
        _customerService = customerService;
        _pretreatmentSampleInfoRep = pretreatmentSampleInfoRep;
        _examRep = examRep;
        _examResultRep = examResultRep;
    }

    /// <summary>
    /// 推送数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<bool> PushEntrustData(List<PushDataInput> input)
    {
        CheckHelper.ArgumentNullException(input, "推送数据不能为空");

        var barcodes = input.Select(a => a.Barcode).ToList();
        var existsBarcodes = await _sampleAmountRep.AsQueryable()
             .Where(a => barcodes.Contains(a.Barcode))
             .Take(20)
             .Select(a => a.Barcode)
             .ToListAsync();
        if (!existsBarcodes.CheckNull())
            throw ResultOutput.Exception($"以下条码已存在！{string.Join(',', existsBarcodes)}");

        //保存当前文件的的对照信息
        var sampleInfos = input.Adapt<List<PretreatSampleInfoEntity>>();
        if (sampleInfos == null || !sampleInfos.Any())
            throw ResultOutput.Exception("无数据！");


        var sysDictList = await _dictService.GetListAsync(["Gender", "AgeUnit"]);
        var customerList = new List<BaseCustomerDto>();
        foreach (var item in sampleInfos)
        {
            item.OriginalPurCodes = item.PurCodes;
            item.OriginalPurNames = item.PurNames;
            //将括号内的逗号替换为顿号
            item.PurCodes = item.PurCodes.Replace(@"\(([^)]*?)\)", ",", "、");
            item.PurNames = item.PurNames.Replace(@"\(([^)]*?)\)", ",", "、");

            item.DataSource = (int)SampleDataSourceEnum.Excel;
            //item.ImportConfig = configId;
            //TODO: 转换字典，年龄类型，人员类别，性别等
            var gender = item.GenderName;
            if (!string.IsNullOrWhiteSpace(gender))
            {
                var genderDict = sysDictList["Gender"].FirstOrDefault(a => a.Name.ToUpper().Split(',').Contains(gender.ToUpper()));
                if (genderDict != null)
                {
                    item.GenderCode = genderDict.Code;
                    item.GenderName = genderDict.Name;
                }
                else
                {
                    item.GenderCode = item.GenderName = string.Empty;
                }
            }

            var ageUnit = item.AgeUnitName1;
            if (!string.IsNullOrWhiteSpace(ageUnit))
            {
                var ageUnitDict = sysDictList["AgeUnit"].FirstOrDefault(a => a.Name.ToUpper().Split(',').Contains(ageUnit.ToUpper()));
                if (ageUnitDict != null)
                {
                    item.AgeUnit1 = ageUnitDict.Code;
                    item.AgeUnitName1 = ageUnitDict.Name;
                }
                else
                {
                    item.AgeUnit1 = item.AgeUnitName1 = string.Empty;
                }
            }
            else
            {
                var ageUnitDict = sysDictList["AgeUnit"].FirstOrDefault(a => a.Name == "岁");
                if (ageUnitDict != null)
                {
                    item.AgeUnit1 = ageUnitDict.Code;
                    item.AgeUnitName1 = ageUnitDict.Name;
                }
            }

            var customerCode = item.Barcode.Substring(0, 6);
            if (!customerList.Exists(a => a.CustomerCode == customerCode))
            {
                var currCustomer = await _customerService.GetFirstAsync(v => v.CustomerCode == customerCode);
                if (currCustomer != null)
                    customerList.Add(currCustomer.Adapt<BaseCustomerDto>());
                else
                    throw ResultOutput.Exception($"未找到客户{customerCode}！");
            }
            item.CustomerCode = customerCode;
            item.CustomerName = customerList.FirstOrDefault(a => a.CustomerCode == customerCode)!.CustomerName;
        }
        await _pretreatmentSampleInfoRep.InsertListReturnPKAsync(sampleInfos);
        return true;
    }

    /// <summary>
    /// 根据条码获取结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<EntrustResultDto>> GetResultByBarcode(EntrustResultQueryInpupt input)
    {
        CheckHelper.ArgumentNullException(input, "查询条件不能为空！");
        if (string.IsNullOrWhiteSpace(input.Barcode))
            throw ResultOutput.Exception("条码号不能为空！");

        var ret = new List<EntrustResultDto>();
        var examInfos = await _examRep.GetListAsync(v => v.Barcode == input.Barcode);
        foreach (var item in examInfos)
        {
            ret.Add(new EntrustResultDto
            {
                ExamInfo = item.Adapt<ExamInfoDto>(),
                ResultList = (await _examResultRep.GetListAsync(v => v.ExamInfoId == item.Id)).Adapt<List<ExamResultDto>>()
            });
        }
        return ret;
    }

    /// <summary>
    /// 根据时间段获取结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<EntrustResultDto>> GetResultByTime(EntrustResultQueryInpupt input)
    {
        CheckHelper.ArgumentNullException(input, "查询条件不能为空！");
        CheckHelper.ArgumentNullException(input.CustomerCode, "客户代码不能为空！");
        CheckHelper.ArgumentNullException(input.Begin, "起始时间不能为空！");
        CheckHelper.ArgumentNullException(input.End, "截止时间不能为空！");

        var ret = new List<EntrustResultDto>();
        var queryable = _examRep.AsQueryable().Where(v => v.CustomerCode == input.CustomerCode);
        if (input.TimeType == EntrustResultQueryTimeType.ReceiveTime)
            queryable = queryable.Where(v => v.ReceiveTime >= input.Begin && v.ReceiveTime <= input.End);
        else if (input.TimeType == EntrustResultQueryTimeType.ReportTime)
            queryable = queryable.Where(v => v.CreateReportTime >= input.Begin && v.CreateReportTime <= input.End);

        var examInfos = await queryable.ToListAsync();
        foreach (var item in examInfos)
        {
            ret.Add(new EntrustResultDto
            {
                ExamInfo = item.Adapt<ExamInfoDto>(),
                ResultList = (await _examResultRep.GetListAsync(v => v.ExamInfoId == item.Id)).Adapt<List<ExamResultDto>>()
            });
        }
        return ret;
    }
}
