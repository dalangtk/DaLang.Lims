using AngleSharp.Dom;
using DaLang.Lims.Pathology.Contracts.PathologyTemplate;
using DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;
using DaLang.Lims.Pathology.Core.Consts;
using DaLang.Lims.Pathology.Core.Enum;
using DaLang.Lims.Pathology.Domain.PathologySampleType;
using DaLang.Lims.Pathology.Domain.PathologyTemplate;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pathology.Application.PathologyTemplate;

/// <summary>
/// 诊断模板服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class BasePathologyTemplateService : BaseService, IBasePathologyTemplateService, IDynamicApi
{
    private IBasePathologyTemplateRepository _basePathologyTemplateRep;
    private IBasePathologySampleTypeRepository _basePathologySampleTypeRep;

    public BasePathologyTemplateService(IBasePathologyTemplateRepository basePathologyTemplateRep, IBasePathologySampleTypeRepository basePathologySampleTypeRep)
    {
        _basePathologyTemplateRep = basePathologyTemplateRep;
        _basePathologySampleTypeRep = basePathologySampleTypeRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PathologyTemplateDto> GetAsync(long id)
    {
        var output = await _basePathologyTemplateRep.GetAsync(id);

        if (output != null && !string.IsNullOrWhiteSpace(output.TemplateContent))
            output.TemplateContent = DesEncrypt.Decrypt(output.TemplateContent);

        return output.Adapt<PathologyTemplateDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PathologyTemplateDto>> GetPageAsync(PageInput<PathologyTemplateQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePathologyTemplateRep.GetQueryable(dynamicCondition)
            .Where(v => v.WFCode == filter.WFCode)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.TemplateCode), v => v.TemplateCode.Contains(filter.TemplateCode) || v.TemplateName.Contains(filter.TemplateCode))
            .WhereIF(filter.TemplateType != null, v => v.TemplateType == filter.TemplateType)
            .OrderBy(c => c.Sort)
            .Select<PathologyTemplateDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<PathologyTemplateDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 根据工作流获取所有模板
    /// </summary>
    /// <param name="wfCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<PathologyTemplateDto>> GetListByWfCode(string wfCode)
    {
        if (string.IsNullOrWhiteSpace(wfCode))
            throw ResultOutput.Exception("invalid wfCode.");

        var list = await _basePathologyTemplateRep
            .AsQueryable()
            .IgnoreColumns(v => v.TemplateContent)
            .Where(v => v.WFCode == wfCode)
            .OrderBy(v => v.Sort)
            .ToListAsync();

        return list.Adapt<List<PathologyTemplateDto>>();
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(PathologyTemplateDto input)
    {
        var entity = Mapper.Map<BasePathologyTemplateEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePathologyTemplateRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        var maxPurCode = await _basePathologyTemplateRep.AsQueryable().Where(v => v.WFCode == input.WFCode).MaxAsync(v => v.TemplateCode);
        if (string.IsNullOrWhiteSpace(maxPurCode))
            maxPurCode = "0000";
        else
            maxPurCode = maxPurCode.Replace(input.WFCode, "");

        var next = input.WFCode + (maxPurCode.ToInt() + 1).ToString().PadLeft(4, '0');
        entity.TemplateCode = next;

        if (!string.IsNullOrWhiteSpace(entity.TemplateContent))
            entity.TemplateContent = DesEncrypt.Encrypt(entity.TemplateContent);

        var id = await _basePathologyTemplateRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(PathologyTemplateDto input)
    {
        if (input == null)
            throw ResultOutput.Exception("input can not be null.");

        if (!string.IsNullOrWhiteSpace(input.TemplateContent))
            input.TemplateContent = DesEncrypt.Encrypt(input.TemplateContent);

        var entity = await _basePathologyTemplateRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("诊断模板不存在！");

        Mapper.Map(input, entity);
        if (!string.IsNullOrWhiteSpace(input.TemplateContent))
            input.TemplateContent = DesEncrypt.Encrypt(input.TemplateContent);
        await _basePathologyTemplateRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePathologyTemplateRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id).ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 根据模板代码获取模板列表
    /// </summary>
    /// <param name="templateCodes"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<LabelValueDto>> GetPathologyTemplateList(List<string> templateCodes)
    {
        var ret = await _basePathologyTemplateRep
            .AsQueryable()
            .WhereIF(templateCodes != null && templateCodes.Any(), v => templateCodes.Contains(v.TemplateCode))
            .Select(v => new LabelValueDto
            {
                Label = v.TemplateName,
                Value = v.TemplateCode
            })
            .ToListAsync();
        return ret;
    }

    /// <summary>
    /// 获取巨检模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<GrossExaminationTemplateDto>> GetGrossExaminationTemplateAsync(GrossExaminationTemplateQueryInput input)
    {
        if (input.SampleTypeCodes == null || !input.SampleTypeCodes.Any())
            throw ResultOutput.Exception("invalid sampleTypeCodes.");

        var list = await _basePathologySampleTypeRep.AsQueryable()
            .LeftJoin<BasePathologyTemplateEntity>((a, b) => a.TemplateCode.Contains(b.TemplateCode) && b.TemplateType == (int)TemplateTypeEnum.GrossExamination && b.IsValid && !b.IsDeleted)
            .Where((a, b) => input.SampleTypeCodes.Contains(a.SampleTypeCode) || input.SampleTypeCodes.Contains(a.ParentCode))
            .Where((a, b) => !string.IsNullOrWhiteSpace(a.TemplateCode))
            .Select((a, b) => new GrossExaminationTemplateDto
            {
                SampleTypeName = string.IsNullOrWhiteSpace(a.ParentCode) ? a.SampleTypeName : "",
                TemplateCode = b.TemplateCode,
                TemplateName = b.TemplateName,
                TemplateContent = b.TemplateContent
            })
            .ToListAsync();

        list.RemoveAll(v => string.IsNullOrWhiteSpace(v.TemplateContent));
        foreach (var item in list)
        {
            if (!string.IsNullOrWhiteSpace(item.TemplateContent))
                item.TemplateContent = DesEncrypt.Decrypt(item.TemplateContent);
        }
        return list;
    }
}
