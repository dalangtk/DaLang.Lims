using DaLang.Lims.Api.Contracts;
using DaLang.Lims.Api.Contracts.Dto;
using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleAmount;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleInfo;
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
    public EntrustService(AdminRepositoryBase<PretreatSampleAmountEntity> sampleAmountRep,
        IDictService dictService,
        AdminRepositoryBase<BaseCustomerEntity> customerService,
        AdminRepositoryBase<PretreatSampleInfoEntity> pretreatmentSampleInfoRep)
    {
        _sampleAmountRep = sampleAmountRep;
        _dictService = dictService;
        _customerService = customerService;
        _pretreatmentSampleInfoRep = pretreatmentSampleInfoRep;
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
}
