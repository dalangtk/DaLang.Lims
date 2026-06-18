using DaLang.Lims.BaseData.Contracts.OptionList;
using DaLang.Lims.BaseData.Domain.BaseAskRule;
using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.BaseData.Domain.EntrustHospital;
using DaLang.Lims.BaseData.Domain.ExamPlan;
using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.BaseData.Domain.SampleType;
using DaLang.Lims.Pathology.Domain.PathologyDisease;
using DaLang.Lims.Pathology.Domain.PathologySampleType;
using DaLang.Lims.Pathology.Domain.PathologySamplingSpot;
using DaLang.Lims.Pathology.Domain.PathologyTemplate;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Dict;
using DaLang.Lims.Web.Framework.Domain.DictType;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.BaseData.Application.OptionList;

/// <summary>
/// 选项服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class OptionListService : BaseService, IOptionListService, IDynamicApi
{
    private readonly IBaseCustomerRepository _customerRep;
    private readonly IBaseEntrustHospitalRepository _entrustHospitalRep;
    private readonly IBaseExamPlanRepository _examPlanRep;
    private readonly IBaseItemRepository _itemRep;
    private readonly IBasePurposeRepository _purposeRep;
    private readonly IBaseSampleTypeRepository _sampleTypeRep;
    private readonly AdminRepositoryBase<DictEntity> _dictRep;
    private readonly IBaseAskRuleRepository _askRuleRep;
    private readonly IUserRepository _userRep;
    private readonly IBasePathologyDiseaseRepository _pathologyDiseaseRep;
    private readonly IBasePathologySampleTypeRepository _pathologySampleTypeRep;
    private readonly IBasePathologyTemplateRepository _pathologyTemplateRep;
    private readonly IBasePathologySamplingSpotRepository _samplingSpotRep;
    public OptionListService(IBaseCustomerRepository customerRep,
        IBaseEntrustHospitalRepository entrustHospitalRep,
        IBaseExamPlanRepository examPlanRep,
        IBaseItemRepository itemRep,
        IBasePurposeRepository purposeRep,
        IBaseSampleTypeRepository sampleTypeRep,
        AdminRepositoryBase<DictEntity> dictRep,
        IBaseAskRuleRepository askRuleRep,
        IUserRepository userRep,
        IBasePathologyDiseaseRepository pathologyDiseaseRep,
        IBasePathologySampleTypeRepository pathologySampleTypeRep,
        IBasePathologyTemplateRepository pathologyTemplateRep,
        IBasePathologySamplingSpotRepository samplingSpotRep)
    {
        _customerRep = customerRep;
        _entrustHospitalRep = entrustHospitalRep;
        _examPlanRep = examPlanRep;
        _itemRep = itemRep;
        _purposeRep = purposeRep;
        _sampleTypeRep = sampleTypeRep;
        _dictRep = dictRep;
        _askRuleRep = askRuleRep;
        _userRep = userRep;
        _pathologyDiseaseRep = pathologyDiseaseRep;
        _pathologySampleTypeRep = pathologySampleTypeRep;
        _pathologyTemplateRep = pathologyTemplateRep;
        _samplingSpotRep = samplingSpotRep;
    }
    /// <summary>
    /// 获取客户选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetCustomerOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _customerRep.AsQueryable()
           .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.CustomerCode!.StartsWith(query.ToUpper()) || a.CustomerName!.Contains(query))
           .OrderBy(c => c.Sort)
           .Select(a => new LabelValueDto
           {
               Label = a.CustomerName,
               Value = a.CustomerCode
           })
           .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取委托医院选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetEntrustHospitalOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _entrustHospitalRep.AsQueryable()
           .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.EntrustHospitalCode!.StartsWith(query.ToUpper()) || a.EntrustHospitalName!.Contains(query))
           .OrderBy(c => c.Sort)
           .Select(a => new LabelValueDto
           {
               Label = a.EntrustHospitalName,
               Value = a.EntrustHospitalCode
           })
           .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取检验计划选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetExamPlanOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _examPlanRep.AsQueryable()
           .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.ExamPlanCode!.StartsWith(query.ToUpper()) || a.ExamPlanName!.Contains(query))
           .OrderBy(c => c.Sort)
           .Select(a => new LabelValueDto
           {
               Label = a.ExamPlanName,
               Value = a.ExamPlanCode
           })
           .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取项目选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetItemOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _itemRep.AsQueryable()
           .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.ItemCode!.StartsWith(query.ToUpper()) || a.ItemName!.Contains(query))
           .OrderBy(c => c.Sort)
           .Select(a => new LabelValueDto
           {
               Label = a.ItemName,
               Value = a.ItemCode
           })
           .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取目的选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetPurposeOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _purposeRep.AsQueryable()
           .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.PurCode!.StartsWith(query.ToUpper()) || a.PurName!.Contains(query))
           .OrderBy(c => c.Sort)
           .Select(a => new LabelValueDto
           {
               Label = a.PurName,
               Value = a.PurCode
           })
           .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取样本类型选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetSampleTypeOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _sampleTypeRep.AsQueryable()
           .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.SampleTypeCode!.StartsWith(query.ToUpper()) || a.SampleTypeName!.Contains(query))
           .OrderBy(c => c.Sort)
           .Select(a => new LabelValueDto
           {
               Label = a.SampleTypeName,
               Value = a.SampleTypeCode
           })
           .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取字典选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetDictOptions(PageInput<string> input)
    {
        var dictCode = input.Filter;
        var list = await _dictRep.AsQueryable()
            .InnerJoin<DictTypeEntity>((a, b) => b.Code.Contains(dictCode) && a.IsValid == true && a.DictTypeId == b.Id)
            .OrderBy(a => a.Sort)
            .Select(a => new LabelValueDto
            {
                Label = a.Name,
                Value = a.Value
            })
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取问询规则选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetAskRuleOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _askRuleRep.AsQueryable()
          .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.AskRuleCode!.StartsWith(query.ToUpper()) || a.AskRuleName!.Contains(query))
          .OrderBy(c => c.Sort)
          .Select(a => new LabelValueDto
          {
              Label = a.AskRuleName,
              Value = a.AskRuleCode
          })
          .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取用户选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetUserOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _userRep.AsQueryable()
          .WhereIF(!string.IsNullOrWhiteSpace(query), a => a.UserName!.StartsWith(query.ToUpper()) || a.Name!.Contains(query))
          .Select(a => new LabelValueDto
          {
              Label = a.Name,
              Value = a.Id.ToString()
          })
          .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取疾病选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetDiseaseOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _pathologyDiseaseRep.AsQueryable()
            .Where(a => a.DiseaseCode == query || a.DiseaseName!.Contains(query))
            .Select(a => new LabelValueDto
            {
                Label = a.DiseaseName,
                Value = a.DiseaseCode
            })
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取病理标本类型选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetPathologySampleTypeOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _pathologySampleTypeRep.AsQueryable()
            .Where(a => a.SampleTypeCode == query || a.SampleTypeName!.Contains(query))
            .Select(a => new LabelValueDto
            {
                Label = a.SampleTypeName,
                Value = a.SampleTypeCode
            })
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取病理诊断模板选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetPathologyDiagnosisTemplateOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _pathologyTemplateRep.AsQueryable()
            .Where(a => a.TemplateCode == query || a.TemplateName!.Contains(query))
            .Where(v => v.TemplateType == 1)
            .Select(a => new LabelValueDto
            {
                Label = a.TemplateName,
                Value = a.TemplateCode
            })
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取病理巨检模板选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetPathologyGrossExaminationTemplateOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _pathologyTemplateRep.AsQueryable()
            .Where(a => a.TemplateCode == query || a.TemplateName!.Contains(query))
            .Where(v => v.TemplateType == 2)
            .Select(a => new LabelValueDto
            {
                Label = a.TemplateName,
                Value = a.TemplateCode
            })
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = list.Items.ToList();
        return data;
    }

    /// <summary>
    /// 获取取材部位选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetSamplingSpotOptions(PageInput<string> input)
    {
        var query = input.Filter;
        var list = await _samplingSpotRep.AsQueryable()
            .Where(a => a.SamplingSpotCode == query || a.PinYin.Contains(query) || a.SamplingSpotName!.Contains(query))
            .Select(a => new LabelValueDto
            {
                Label = a.SamplingSpotName,
                Value = a.SamplingSpotCode
            })
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = list.Items.ToList();
        return data;
    }
}
