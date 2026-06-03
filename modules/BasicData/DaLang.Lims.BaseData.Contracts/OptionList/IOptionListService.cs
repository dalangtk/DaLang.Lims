using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.OptionList;

/// <summary>
/// 选项服务
/// </summary>
public interface IOptionListService
{
    /// <summary>
    /// 获取客户选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetCustomerOptions(PageInput<string> input);
    /// <summary>
    /// 获取委托医院选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetEntrustHospitalOptions(PageInput<string> input);
    /// <summary>
    /// 获取检验计划选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetExamPlanOptions(PageInput<string> input);
    /// <summary>
    /// 获取项目选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetItemOptions(PageInput<string> input);
    /// <summary>
    /// 获取目的选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetPurposeOptions(PageInput<string> input);
    /// <summary>
    /// 获取样本类型选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetSampleTypeOptions(PageInput<string> input);

    /// <summary>
    /// 获取字典选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetDictOptions(PageInput<string> input);

    /// <summary>
    /// 获取问询规则选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetAskRuleOptions(PageInput<string> input);

    /// <summary>
    /// 获取用户选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<List<LabelValueDto>> GetUserOptions(PageInput<string> input);

    /// <summary>
    /// 获取疾病选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<LabelValueDto>> GetDiseaseOptions(PageInput<string> input);

    /// <summary>
    /// 获取病理标本类型选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<LabelValueDto>> GetPathologySampleTypeOptions(PageInput<string> input);

    /// <summary>
    /// 获取病理诊断模板选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<LabelValueDto>> GetPathologyDiagnosisTemplateOptions(PageInput<string> input);

    /// <summary>
    /// 获取病理巨检模板选项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<LabelValueDto>> GetPathologyGrossExaminationTemplateOptions(PageInput<string> input);
}
