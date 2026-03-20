using DaLang.Lims.BaseData.Contracts.AuditRule;
using DaLang.Lims.BaseData.Contracts.AuditRule.Dto;
using DaLang.Lims.BaseData.Core.RuleExtension;
using DaLang.Lims.BaseData.Domain.AuditRule;
using DaLang.Lims.BaseData.Domain.Group;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Shared.Domain.ExamResult;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.ClayObject;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using RulesEngine.Models;
using SqlSugar;
using System.ComponentModel;

namespace DaLang.Lims.BaseData.Services.AuditRule;

/// <summary>
/// 审核规则服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseAuditRuleService : BaseService, IBaseAuditRuleService, IDynamicApi
{
    private IBaseAuditRuleRepository _baseAuditRuleRep;
    private IBaseGroupRepository _baseGroupRep;
    private AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private AdminRepositoryBase<ExamResultEntity> _examResultRep;

    public BaseAuditRuleService(IBaseAuditRuleRepository baseAuditRuleRep,
        AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        AdminRepositoryBase<ExamResultEntity> examResultRep,
        IBaseGroupRepository baseGroupRep)
    {
        _baseAuditRuleRep = baseAuditRuleRep;
        _examInfoRep = examInfoRep;
        _examResultRep = examResultRep;
        _baseGroupRep = baseGroupRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseAuditRuleDto> GetAsync(long id)
    {
        var output = await _baseAuditRuleRep.GetAsync(id);
        return output.Adapt<BaseAuditRuleDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseAuditRuleDto>> GetPageAsync(PageInput<BaseAuditRuleQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseAuditRuleRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseAuditRuleDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var groups = await _baseGroupRep.GetListAsync();
        var ruleList = list.Items.ToList();
        foreach (var r in ruleList)
        {
            if (!string.IsNullOrWhiteSpace(r.GroupCode))
            {
                var groupCodes = r.GroupCode.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var groupNames = groups.Where(g => groupCodes.Contains(g.GroupCode)).Select(g => g.GroupName).ToList();
                r.GroupName = string.Join(',', groupNames);
            }
        }
        var data = new PageOutput<BaseAuditRuleDto> { List = ruleList, Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseAuditRuleDto input)
    {
        if (string.IsNullOrWhiteSpace(input.RuleCode))
            throw ResultOutput.Exception("规则代码不可为空！");

        var isExists = await _baseAuditRuleRep.IsAnyAsync(v => v.RuleCode == input.RuleCode);
        if (isExists)
            throw ResultOutput.Exception($"规则{input.RuleCode}已存在！");

        var entity = Mapper.Map<BaseAuditRuleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseAuditRuleRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseAuditRuleRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseAuditRuleDto input)
    {
        var entity = await _baseAuditRuleRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("审核规则不存在！");

        Mapper.Map(input, entity);
        await _baseAuditRuleRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseAuditRuleRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 校验规则
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<TestRuleResultDto> TestRule(TestRuleInput input)
    {
        if (input.ExamInfoId <= 0)
            throw ResultOutput.Exception("id有误");

        var ret = new TestRuleResultDto();

        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == input.ExamInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception($"不存在id为{input.ExamInfoId}的检验信息！");

        var queryable = _baseAuditRuleRep.AsQueryable().Where(v => v.IsValid && v.AuditType == input.ExecuteType.ToInt());
        if (!string.IsNullOrWhiteSpace(input.RuleCode))
            queryable = queryable.Where(v => v.RuleCode == input.RuleCode);
        else
        {
            queryable = queryable.Where(v => v.GroupCode.Contains(examInfo.GroupCode) || SqlFunc.IsNullOrEmpty(v.GroupCode));
            queryable = queryable.Where(v => v.WorkFlowType.Equals(examInfo.WFCode) || SqlFunc.IsNullOrEmpty(v.WorkFlowType));
        }
        if (!input.IgnoreRuleCodes.CheckNull())
        {
            queryable = queryable.Where(v => !input.IgnoreRuleCodes.Contains(v.RuleCode));
            ret.IgnoreRules = input.IgnoreRuleCodes;
        }

        var auditRules = await queryable.Select<BaseAuditRuleDto>().ToListAsync();
        if (!auditRules.Any())
            return new TestRuleResultDto();

        var examResults = await _examResultRep.GetListAsync(v => v.ExamInfoId == examInfo.Id);
        if (auditRules.Exists(v => !string.IsNullOrEmpty(v.ItemCodes)))
        {
            var resultItemCodes = examResults.Select(v => v.ItemCode).Distinct().ToList();
            auditRules.RemoveAll(v =>
            {
                if (string.IsNullOrEmpty(v.ItemCodes))
                    return false;
                var ruleItemCodes = v.ItemCodes.Split(',').ToList();
                return !resultItemCodes.All(ric => ruleItemCodes.Contains(ric));
            });
        }
        if (auditRules.Exists(v => !string.IsNullOrWhiteSpace(v.PurCodes)))
        {
            var examPurCodes = examInfo.PurCodes.Split(',').ToList();
            auditRules.RemoveAll(v =>
            {
                if (string.IsNullOrWhiteSpace(v.PurCodes))
                    return false;
                var rulePurCodes = v.PurCodes.Split(',').ToList();
                return !rulePurCodes.All(rpc => examPurCodes.Contains(rpc));
            });
        }
        if (!auditRules.Any())
            return new TestRuleResultDto();

        auditRules.Sort((a, b) => a.RuleProperty.CompareTo(b.RuleProperty));

        var workflows = new List<Workflow>();
        var workflow = new Workflow();
        workflow.WorkflowName = "Exam Workflow Rule";

        var clayObj = Clay.Parse(examInfo);
        var dict = (Dictionary<string, object>)clayObj.ToDictionary()!;

        dynamic dynObj = new System.Dynamic.ExpandoObject();
        foreach (var entry in dict)
            (dynObj as ICollection<KeyValuePair<string, object>>)!.Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
        var ruleParam = new RuleParameter("input", dynObj);

        var reSettings = new ReSettings
        {
            CustomTypes = [typeof(RuleExtensionUtil)]
        };
        List<RuleResultTree> resultList = new();
        var infoRule = new List<Rule>();
        var itemRule = new List<Rule>();
        foreach (var r in auditRules)
        {
            var rule = new Rule();
            rule.RuleName = r.RuleCode;
            rule.SuccessEvent = r.NoticeMessage;
            rule.Expression = r.RuleExpression;
            rule.RuleExpressionType = RuleExpressionType.LambdaExpression;

            if (r.JudgeType == 1)
                infoRule.Add(rule);
            else
                itemRule.Add(rule);
        }
        workflow.Rules = infoRule;
        workflows.Add(workflow);
        RulesEngine.RulesEngine bre;
        if (infoRule.Any())
        {
            bre = new RulesEngine.RulesEngine(workflows.ToArray(), reSettings);
            resultList.AddRange(await ExecuteRule(bre, ruleParam));
        }
        if (itemRule.Any())
        {
            workflow.Rules = itemRule;
            workflows.Clear();
            workflows.Add(workflow);
            bre = new RulesEngine.RulesEngine(workflows.ToArray(), reSettings);
            foreach (var r in examResults)
            {
                dict = clayObj.ToDictionary()!;
                GetResultDictionary(dict, r);

                dynObj = new System.Dynamic.ExpandoObject();
                foreach (var entry in dict)
                    (dynObj as ICollection<KeyValuePair<string, object>>)!.Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
                ruleParam = new RuleParameter("input", dynObj);

                var tmp = await ExecuteRule(bre, ruleParam);
                resultList.AddRange(tmp);
            }
        }

        resultList.ForEach(v =>
        {
            if (ret.TriggerRules.Exists(a => a.RuleCode == v.Rule.RuleName))
                return;

            var currRule = auditRules.Find(a => a.RuleCode == v.Rule.RuleName);
            if (v.IsSuccess)
            {
                var successRule = currRule.Adapt<TestRuleDto>();
                successRule.IsTrigger = true;
                ret.TriggerRules.Add(successRule);
            }
            else if (v.IsSuccess == false && !string.IsNullOrWhiteSpace(v.ExceptionMessage))
            {
                var exceptionRule = currRule.Adapt<TestRuleDto>();
                exceptionRule.IsException = true;
                exceptionRule.NoticeMessage = v.ExceptionMessage;
                ret.TriggerRules.Add(exceptionRule);
            }
            else if (input.ShowDetail)
            {
                var failRule = currRule.Adapt<TestRuleDto>();
                failRule.IsTrigger = false;
                ret.TriggerRules.Add(failRule);
            }
        });
        return ret;

        async Task<List<RuleResultTree>> ExecuteRule(RulesEngine.RulesEngine re, params RuleParameter[] rulePamas)
        {
            List<RuleResultTree> testResult = await re.ExecuteAllRulesAsync("Exam Workflow Rule", rulePamas);
            return testResult;
        }
    }

    /// <summary>
    /// 获取规则字段
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetRuleField(long examInfoId)
    {
        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == examInfoId);
        if (examInfo == null)
            throw ResultOutput.Exception("检验信息不存在！");

        var examResults = await _examResultRep.GetListAsync(v => v.ExamInfoId == examInfo.Id);
        var clayObj = Clay.Parse(examInfo);
        var dict = (Dictionary<string, object>)clayObj.ToDictionary()!;
        GetResultDictionary(dict, examResults.FirstOrDefault());

        return dict.Keys.ToList();
    }

    /// <summary>
    /// 获取自定义方法列表
    /// </summary>
    /// <returns></returns>
    public List<CustomMethodDto> GetCustomMethodsAsync()
    {
        var type = typeof(RuleExtensionUtil);

        var methods = type.GetMethods();
        var list = new List<CustomMethodDto>();
        foreach (var m in methods)
        {
            var methodParams = m.GetParameters();
            var attrs = m.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            list.Add(new CustomMethodDto
            {
                MethodName = m.Name,
                MethodDescription = attrs != null ? attrs.Description : string.Empty
            });
        }

        return list;
    }

    [NonAction]
    private Dictionary<string, object> GetResultDictionary(Dictionary<string, object> dict, ExamResultEntity currResult)
    {
        if (currResult == null)
            return dict;
        var itemCode = currResult.ItemCode;
        dict.Add($"ItemCode", $"{itemCode}");
        dict.Add($"ItemName", $"{currResult.ItemName}");
        dict.Add($"ItemNameEN", $"{currResult.ItemNameEN}");
        dict.Add($"ItemNameAB", $"{currResult.ItemNameAB}");
        dict.Add($"ItemUnit", $"{currResult.ItemUnit}");
        dict.Add($"ItemResult", $"{currResult.ItemResult}");
        dict.Add($"HLFlag", $"{currResult.HLFlag}");
        dict.Add($"ItemReference", $"{currResult.ItemReference}");
        dict.Add($"DisplayRange", $"{currResult.DisplayRange}");
        dict.Add($"ResultSource", $"{currResult.ResultSource}");
        dict.Add($"IsReportShow", $"{currResult.IsReportShow}");
        dict.Add($"ResultType", $"{currResult.ResultType}");
        return dict;
    }
}