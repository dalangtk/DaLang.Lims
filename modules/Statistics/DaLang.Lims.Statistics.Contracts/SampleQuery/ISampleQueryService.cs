using DaLang.Lims.Shared.Contracts.ApplyInfo.Dto;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Statistics.Contracts.SampleQuery.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Statistics.Contracts.SampleQuery;

public interface ISampleQueryService
{
    /// <summary>
    /// 样本查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<ApplyInfoDto>> QuerySample(PageInput<SampleQueryInput> input);

    /// <summary>
    /// 根据条码获取检验列表
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    Task<List<ExamInfoDto>> GetExamList(string barcode);

    /// <summary>
    /// 根据条码获取申请单目的列表
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    Task<List<ApplyPurposeDto>> GetPurposeList(string barcode);
}
