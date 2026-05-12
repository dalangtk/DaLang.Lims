using DaLang.Lims.Pathology.Contracts.PathologyTemplate;
using DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;
using DaLang.Lims.Pathology.Core.Consts;
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

namespace DaLang.Lims.Pathology.Application.PathologyTemplate;

/// <summary>
/// 诊断模板服务
/// </summary>
[DynamicApi(Area = PathologyConsts.AreaName)]
public class BasePathologyTemplateService : BaseService, IBasePathologyTemplateService, IDynamicApi
{
    private IBasePathologyTemplateRepository _basePathologyTemplateRep;

    public BasePathologyTemplateService(IBasePathologyTemplateRepository basePathologyTemplateRep)
    {
        _basePathologyTemplateRep = basePathologyTemplateRep;
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

        if (!string.IsNullOrWhiteSpace(input.TemplateContent))
            input.TemplateContent = DesEncrypt.Encrypt(input.TemplateContent);

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
}
