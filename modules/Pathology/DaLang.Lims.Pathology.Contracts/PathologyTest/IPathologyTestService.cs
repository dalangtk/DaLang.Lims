using DaLang.Lims.Exam.Contracts.Pathology.Dto;
using DaLang.Lims.Exam.Contracts.SampleTest.Dto;
using DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamSpecialResult.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologyTest;

public interface IPathologyTestService
{
    /// <summary>
    /// 获取病理检验列表
    /// </summary>
    Task<List<ExamInfoDto>> GetPathologySampleListAsync(PathologySampleListQueryInput input);
    /// <summary>
    /// 病理登记
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ExamInfoDto> PathologyReceive(PathologyReceiveInput input);
    /// <summary>
    /// 获取特检结果
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Task<List<ExamSpecialResultDto>> GetSpecialResultList(ExamSpecialResultQueryInput input);
    /// <summary>
    /// 删除登记
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<long>> PathologyBack(PathologyBackInput input);
    /// <summary>
    /// 保存特检结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SaveSpecialResult(List<ExamSpecialResultDto> input);
    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<AuditResultDto> PathologyAudit(AuditInput input);
}
