using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.BaseData.Domain.TenantReportExtend;
using DaLang.Lims.ReportTemplate.Contracts.ReportTemplate.Dto;
using DaLang.Lims.ReportTemplate.Core.Consts;
using DaLang.Lims.ReportTemplate.Domain.ReportTemplate;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;
using DaLang.Lims.Shared.Contracts.ExamTask.Dto;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Shared.Domain.ExamResult;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.ReportTemplate.Services.ReportTemplate;

/// <summary>
/// 模板服务
/// </summary>
[DynamicApi(Area = ReportTemplateConsts.AreaName)]
public class ReportTemplateService : BaseService, IReportTemplateService, IDynamicApi
{
    private AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private AdminRepositoryBase<ExamResultEntity> _examResultRep;
    private IReportTemplateRepository _reportTemplateRep;
    private AdminRepositoryBase<BaseTenantReportExtendEntity> _tenantReportExtendRep;

    public ReportTemplateService(IReportTemplateRepository reportTemplateRep,
        AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        AdminRepositoryBase<ExamResultEntity> examResultRep,
        AdminRepositoryBase<BaseTenantReportExtendEntity> tenantReportExtendRep)
    {
        _reportTemplateRep = reportTemplateRep;
        _examInfoRep = examInfoRep;
        _examResultRep = examResultRep;
        _tenantReportExtendRep = tenantReportExtendRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ReportTemplateDto> GetAsync(long id)
    {
        var output = await _reportTemplateRep.GetAsync(id);
        return output.Adapt<ReportTemplateDto>();
    }

    /// <summary>
    /// 根据代码查询
    /// </summary>
    /// <param name="templateCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ReportTemplateDto> GetByCodeAsync(string templateCode)
    {
        var output = await _reportTemplateRep.GetFirstAsync(v => v.TemplateCode == templateCode);
        return output.Adapt<ReportTemplateDto>();
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ReportTemplateDto>> GetPageAsync(PageInput<ReportTemplateQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _reportTemplateRep.GetQueryable(dynamicCondition)
            .Where(c => c.TemplateType == filter.TemplateType)
            .IgnoreColumns(c => c.TemplateContent)
            .OrderBy(c => c.Id)
            .Select<ReportTemplateDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<ReportTemplateDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ReportTemplateDto input)
    {
        var exists = await _reportTemplateRep.IsAnyAsync(v => v.TemplateCode == input.TemplateCode);
        if (exists)
            throw ResultOutput.Exception("模板代码重复，无法新增！");

        var entity = Mapper.Map<ReportTemplateEntity>(input);
        var id = await _reportTemplateRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ReportTemplateDto input)
    {
        var entity = await _reportTemplateRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("模板不存在！");

        Mapper.Map(input, entity);
        await _reportTemplateRep.AsUpdateable(entity)
            .IgnoreColumns(v => v.TemplateContent)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新模板内容
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> UpdateContentAsync(ReportTemplateUpdateContentInput input)
    {
        var ret = await _reportTemplateRep
             .SetColumnUpdateable(v => v.TemplateContent == input.TemplateContent)
             .Where(v => v.Id == input.Id)
             .ExecuteCommandAsync();
        return ret > 0;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _reportTemplateRep.
            SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取报告数据源
    /// </summary>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    [HttpGet]
    public object GetReportData(string sourceName)
    {
        switch (sourceName)
        {
            case "Handover":
                return new ExamTaskDto();
        }

        return null;
    }
    /// <summary>
    /// 获取所有报告模板
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ReportTemplateDto>> GetAllTemplateList(ReportTemplateQueryInput input)
    {
        var list = await _reportTemplateRep.AsQueryable()
             //.IgnoreColumns(v => v.TemplateContent)
             .Where(v => v.TemplateType == input.TemplateType)
             .Select<ReportTemplateDto>()
             .OrderBy(v => v.Id)
             .ToListAsync();
        return list;
    }

    /// <summary>
    /// 获取检验报告单数据源
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamReportSourceDto> GetExamReportSource(long examInfoId)
    {
        if (examInfoId <= 0)
            return new ExamReportSourceDto();

        var examInfo = await _examInfoRep.AsQueryable()
            .Where(v => v.Id == examInfoId)
            .Select<ExamInfoDto>()
            .FirstAsync();

        var examResult = await _examResultRep.AsQueryable()
            .Where(v => v.ExamInfoId == examInfoId)
            .Select<ExamResultDto>()
            .ToListAsync();

        var ret = examInfo.Adapt<ExamReportSourceDto>();
        ret.ResultList = examResult;

        var reportExtend = await _tenantReportExtendRep.AsQueryable()
            .LeftJoin<BaseCustomerReportExtendEntity>((a, b) => a.TenantId == b.TenantId && b.CustomerCode == examInfo.CustomerCode)
            .Where((a, b) => a.TenantId == AppInfo.User.TenantId)
            .Select((a, b) => new ReportExtendDto
            {
                CustomerCode = b.CustomerCode,
                TenantLogo = a.ReportLogo,
                TenantSignature = a.ReportSignature,
                CustomerLogo = b.CustomerLogo,
                CustomerSignature = b.CustomerSignature,
                TenantMainTitle = a.ReportMainTitle,
                TenantSubTitle = a.ReportSubTitle,
                CustomerMainTitle = b.ReportMainTitle,
                CustomerSubTitle = b.ReportSubTitle,
                TenantAddress = a.Address,
                CustomerAddress = b.Address,
                TenantTelephone = a.Telephone,
                CustomerTelephone = b.Telephone,
                IsGenerateReport = b.IsGenerateReport,
                ReportPriority = b.ReportPriority,
                ImageDpi = b.ImageDpi,
                LanguageType = b.LanguageType,
                GenerateTitleType = b.GenerateTitleType,
                CanSearchReportChannel = b.CanSearchReportChannel,
                PaperType = b.PaperType,
            }).FirstAsync();

        ret.ReportExtend = reportExtend;
        return ret;
    }

    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<bool> CopyTemplate(long id)
    {
        var template = await _reportTemplateRep.GetAsync(id);
        if (template == null)
            throw ResultOutput.Exception("模板不存在！");

        template.Id = 0;
        template.IsValid = false;
        template.ModId = null;
        template.ModName = null;
        template.ModTime = null;
        template.TemplateCode = $"{template.TemplateCode}_Copy";
        template.TemplateName = $"{template.TemplateName}_复制";
        var exists = await _reportTemplateRep.IsAnyAsync(v => v.TemplateCode == template.TemplateCode);
        if (exists)
            throw ResultOutput.Exception("模板代码重复，无法复制！");

        await _reportTemplateRep.InsertAsync(template);
        return true;
    }
}