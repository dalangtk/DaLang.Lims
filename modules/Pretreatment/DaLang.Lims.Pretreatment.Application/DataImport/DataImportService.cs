using DaLang.Lims.BaseData.Contracts.Customer;
using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.Pretreatment.Contracts.DataImport;
using DaLang.Lims.Pretreatment.Contracts.DataImport.Dto;
using DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig.Dto;
using DaLang.Lims.Pretreatment.Contracts.PretreatSampleInfo.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.PretreatDataImport;
using DaLang.Lims.Pretreatment.Domain.PretreatDataImportConfig;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleAmount;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleInfo;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Dict;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DaLang.Lims.Pretreatment.Services.DataImport;

/// <summary>
/// 数据导入
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class DataImportService : BaseService, IDataImportService, IDynamicApi
{
    private IFileService _fileService;
    private IPretreatDataImportConfigRepository _dataImportConfigRep;
    private IDictService _dictService;
    private IBaseCustomerService _customerService;
    private IPretreatDataImportRepository _dataImportRep;
    private AdminRepositoryBase<PretreatSampleAmountEntity> _sampleAmountRep;
    public DataImportService(IFileService fileService,
        IPretreatDataImportConfigRepository dataImportConfigRep,
        IDictService dictService,
        IBaseCustomerService customerService,
        IPretreatDataImportRepository dataImportRep,
        AdminRepositoryBase<PretreatSampleAmountEntity> sampleAmountRep)
    {
        _fileService = fileService;
        _dataImportConfigRep = dataImportConfigRep;
        _dictService = dictService;
        _customerService = customerService;
        _dataImportRep = dataImportRep;
        _sampleAmountRep = sampleAmountRep;
    }

    [HttpPost]
    public async Task<List<PretreatDataImportConfigDto>> DataPreImport([Required] IFormFile file)
    {
        var stream = file.OpenReadStream();
        var table = ExcelHelper.QueryTableByStream(stream);
        if (table == null || table.Rows.Count == 0 || table.Columns.Count == 0)
            throw ResultOutput.Exception("表格无数据！");

        var generalConfigs = await _dataImportConfigRep.AsQueryable().Where(a => string.IsNullOrWhiteSpace(a.CustomerCode))
            .Select<PretreatDataImportConfigDto>().ToListAsync();
        if (!generalConfigs.Any())
            throw ResultOutput.Exception("未配置通用导入设置！");

        var curConfigs = new List<PretreatDataImportConfigDto>();
        var barcodeConfig = generalConfigs.FirstOrDefault(a => a.FieldName == "Barcode");
        if (barcodeConfig != null)
        {
            List<PretreatDataImportConfigDto> customerImportConfigs;
            if (table.Columns.Contains(barcodeConfig.CellName))
            {
                var firstCustomerCode = table.Rows[0][barcodeConfig.CellName]?.ToString()?.ToUpper();
                customerImportConfigs = await _dataImportConfigRep.AsQueryable()
                    .Where(a => a.CustomerCode == firstCustomerCode)
                    .Select<PretreatDataImportConfigDto>()
                    .ToListAsync();
                if (customerImportConfigs.Any())
                {
                    var customerFields = customerImportConfigs.Select(a => a.FieldName);
                    generalConfigs.RemoveAll(a => customerFields.Contains(a.FieldName));
                    generalConfigs.AddRange(customerImportConfigs);
                }
            }
        }

        foreach (DataColumn dc in table.Columns)
        {
            var cur = generalConfigs.FirstOrDefault(a => a.CellName.ToUpper() == dc.ColumnName.ToUpper());
            if (cur != null)
            {
                cur.IsSystemCompare = true;
                curConfigs.Add(cur);
            }
            else
            {
                curConfigs.Add(new PretreatDataImportConfigDto
                {
                    CellName = dc.ColumnName,
                    IsSystemCompare = false
                });
            }
        }
        return curConfigs;
    }
    [HttpPost]
    public async Task<List<PretreatSampleInfoDto>> DataImport([Required] IFormFile file, string importConfigsJson = "")
    {
        var env = LazyGetRequiredService<IWebHostEnvironment>();
        var fileEntity = await _fileService.UploadFileAsync(file, bussinessPrefix: "PretreatDataImport");

        var stream = file.OpenReadStream();
        var table = ExcelHelper.QueryTableByStream(stream);
        if (table == null || table.Rows.Count == 0 || table.Columns.Count == 0)
            throw ResultOutput.Exception("表格无数据！");

        var generalConfigs = await _dataImportConfigRep.AsQueryable().Where(a => string.IsNullOrWhiteSpace(a.CustomerCode))
           .Select<PretreatDataImportConfigDto>().ToListAsync();
        if (!generalConfigs.Any())
            throw ResultOutput.Exception("未配置通用导入设置！");

        var importConfigs = new List<PretreatDataImportConfigDto>();
        if (!string.IsNullOrWhiteSpace(importConfigsJson))
            importConfigs = JsonHelper.Deserialize<List<PretreatDataImportConfigDto>>(importConfigsJson);

        if (importConfigs != null && importConfigs.Any())
        {
            var customerImportConfigs = new List<PretreatDataImportConfigDto>();
            if (importConfigs.Exists(a => a.FieldName == "Barcode"))
            {
                var barcodeConfig = importConfigs.FirstOrDefault(a => a.FieldName == "Barcode");
                if (table.Columns.Contains(barcodeConfig.CellName))
                {
                    var firstCustomerCode = table.Rows[0][barcodeConfig.CellName]?.ToString()?.ToUpper();
                    customerImportConfigs = await _dataImportConfigRep.AsQueryable()
                        .Where(a => a.CustomerCode == firstCustomerCode)
                        .Select<PretreatDataImportConfigDto>()
                        .ToListAsync();
                }
            }
            else
            {
                var barcodeConfig = generalConfigs.FirstOrDefault(a => a.FieldName == "Barcode");
                if (barcodeConfig != null)
                {
                    if (table.Columns.Contains(barcodeConfig.CellName))
                    {
                        var firstCustomerCode = table.Rows[0][barcodeConfig.CellName]?.ToString()?.ToUpper();
                        customerImportConfigs = await _dataImportConfigRep.AsQueryable()
                            .Where(a => a.CustomerCode == firstCustomerCode)
                            .Select<PretreatDataImportConfigDto>()
                            .ToListAsync();
                    }
                }
            }

            if (customerImportConfigs.Any())
            {
                var cusFields = customerImportConfigs.Select(a => a.FieldName);
                generalConfigs.RemoveAll(a => cusFields.Contains(a.FieldName));
                generalConfigs.AddRange(customerImportConfigs);
            }

            var importFields = importConfigs.Select(a => a.FieldName);
            generalConfigs.RemoveAll(a => importFields.Contains(a.FieldName));
            generalConfigs.AddRange(importConfigs);
        }

        foreach (DataColumn dc in table.Columns)
        {
            if (string.IsNullOrWhiteSpace(dc.ColumnName))
                continue;
            var currField = generalConfigs.FirstOrDefault(a => a.CellName == dc.ColumnName);
            if (currField != null && !string.IsNullOrWhiteSpace(currField.FieldName))
            {
                dc.ColumnName = currField.FieldName;
                currField.IsExcelExists = true;
            }
        }
        if (generalConfigs.Exists(a => a.IsMustExists && !a.IsExcelExists))
            throw ResultOutput.Exception($"{generalConfigs.First(a => a.IsMustExists && !a.IsExcelExists).CellName}未匹配表格列！");

        var sysDictList = await _dictService.GetListAsync(["Gender", "AgeUnit"]);

        for (int i = 1; i <= table.Rows.Count; i++)
        {
            var currInfoRow = table.Rows[i - 1];

            var purCodes = currInfoRow["PurCodes"]?.ToString();
            var purNames = currInfoRow["PurNames"]?.ToString();
            if (string.IsNullOrWhiteSpace(purCodes))
                throw ResultOutput.Exception($"第{i}行目的代码不能为空！");

            if (string.IsNullOrWhiteSpace(purNames))
                throw ResultOutput.Exception($"第{i}行目的名称不能为空！");

            if (purCodes.Split(",", StringSplitOptions.RemoveEmptyEntries).Length != purNames.Split(",", StringSplitOptions.RemoveEmptyEntries).Length)
                throw ResultOutput.Exception($"第{i}行目的代码和目的名称数量不一致！");

            var barcode = currInfoRow["Barcode"]?.ToString();
            if (barcode.CheckNull())
                throw ResultOutput.Exception($"第{i}行条码不能为空！");

            if (!barcode!.IsLetterAndNumber())
                throw ResultOutput.Exception($"第{i}行条码有误，条码只能由字母+数字组成！");

            if (barcode!.Length <= 6)
                throw ResultOutput.Exception($"第{i}行条码有误，条码不能短于6位！");
            //TODO: 其他校验，加子表，每个字段设置正则？
        }

        var sampleInfos = table.ToEntityList<PretreatSampleInfoEntity>();
        if (sampleInfos == null || !sampleInfos.Any())
            throw ResultOutput.Exception("无数据！");

        var existsBarcodes = await CheckBarcodeUsed(sampleInfos.Select(a => a.Barcode).ToList());
        if (!existsBarcodes.CheckNull())
            throw ResultOutput.Exception($"以下条码已存在！{string.Join(',', existsBarcodes)}");

        var importedConfig = new PretreatExcelImportedConfigEntity
        {
            FileName = fileEntity.Id,
            ConfigJson = JsonHelper.Serialize(generalConfigs)
        };
        //保存当前文件的的对照信息
        var configId = await _dataImportRep.Context.Insertable(importedConfig).ExecuteReturnSnowflakeIdAsync();

        var customerList = new List<BaseCustomerDto>();
        foreach (var item in sampleInfos)
        {
            item.OriginalPurCodes = item.PurCodes;
            item.OriginalPurNames = item.PurNames;
            //将括号内的逗号替换为顿号
            item.PurCodes = item.PurCodes.Replace(@"\(([^)]*?)\)", ",", "、");
            item.PurNames = item.PurNames.Replace(@"\(([^)]*?)\)", ",", "、");

            item.DataSource = (int)SampleDataSourceEnum.Excel;
            item.ImportConfig = configId;
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
                var currCustomer = await _customerService.GetCustomerInfoByCodeAsync(customerCode);
                if (currCustomer != null)
                    customerList.Add(currCustomer);
                else
                    throw ResultOutput.Exception($"未找到客户{customerCode}！");
            }
            item.CustomerCode = customerCode;
            item.CustomerName = customerList.FirstOrDefault(a => a.CustomerCode == customerCode)!.CustomerName;
            item.FileName = fileEntity.Id;
        }

        var idList = await _dataImportRep.InsertListReturnPKAsync(sampleInfos);

        var sampleAmountList = sampleInfos.Select(v => new PretreatSampleAmountEntity
        {
            Barcode = v.Barcode,
            CustomerCode = v.CustomerCode,
            SampleCnt = sampleInfos.FirstOrDefault(a => a.Barcode == v.Barcode)!.SampleCnt ?? 0,
            DataSource = v.DataSource,
            InfoStatus = 4,
            ItemStatus = 4
        }).ToList();
        await _sampleAmountRep.InsertRangeAsync(sampleAmountList);

        var list = await _dataImportRep.AsQueryable().Where(a => idList.Contains(a.Id)).Select<PretreatSampleInfoDto>().ToListAsync();
        return list;
    }
    [HttpPost]
    public async Task<List<string>> CheckBarcodeUsed(List<string> barcodes)
    {
        if (barcodes.CheckNull())
            throw ResultOutput.Exception("参数有误！");
        var list = await _dataImportRep.AsQueryable()
            .Where(a => barcodes.Contains(a.Barcode))
            .Take(20)
            .Select(a => a.Barcode)
            .ToListAsync();
        return list;
    }
    [HttpPost]
    [AdminTransaction]
    public async Task<PageOutput<PretreatSampleInfoDto>> GetImportedSampleInfoAsync(PageInput<QueryImportDataInput> pageInput)
    {
        if (pageInput == null)
            throw ResultOutput.Exception("参数有误！");

        var dynamicCondition = ChangeConditon(pageInput.DynamicFilter);
        var input = pageInput.Filter;
        input.Begin = input.Begin.Date;
        input.End = input.End.Date.AddDays(1).AddSeconds(-1);
        var list = await _dataImportRep.GetQueryable(dynamicCondition)
             .Where(a => SqlFunc.Between(a.ProTime, input.Begin, input.End))
             .WhereIF(input.OnlyMySelf, a => a.ProId == AppInfo.User.Id)
             .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), a => a.CustomerCode == input.CustomerCode)
             .WhereIF(!string.IsNullOrWhiteSpace(input.Barcode), a => a.Barcode == input.Barcode)
             .Select<PretreatSampleInfoDto>()
             .ToPagedListAsync(pageInput.CurrentPage, pageInput.PageSize);

        var data = new PageOutput<PretreatSampleInfoDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    [HttpPost]
    public async Task<bool> DeleteInfoAsync(List<long> delIds)
    {
        if (delIds == null || delIds.Count == 0)
            throw ResultOutput.Exception("参数有误！");

        var ret = await _dataImportRep
            .SetUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => delIds.Contains(a.Id))
            .ExecuteCommandAsync();
        return ret > 0;
    }

    private async Task<List<PretreatDataImportConfigDto>> GetGeneralConfigs(string customerCode)
    {
        var generalConfigs = await _dataImportConfigRep.AsQueryable().Where(a => string.IsNullOrWhiteSpace(a.CustomerCode))
           .Select<PretreatDataImportConfigDto>().ToListAsync();
        if (!generalConfigs.Any())
            throw ResultOutput.Exception("未配置通用导入设置！");

        var curConfigs = new List<PretreatDataImportConfigDto>();
        if (!string.IsNullOrWhiteSpace(customerCode))
        {
            var customerImportConfigs = await _dataImportConfigRep.AsQueryable()
                      .Where(a => a.CustomerCode == customerCode)
                      .Select<PretreatDataImportConfigDto>()
                      .ToListAsync();
            if (customerImportConfigs.Any())
            {
                var customerFields = customerImportConfigs.Select(a => a.FieldName);
                generalConfigs.RemoveAll(a => customerFields.Contains(a.FieldName));
                generalConfigs.AddRange(customerImportConfigs);
            }
        }

        return curConfigs;
    }
}
