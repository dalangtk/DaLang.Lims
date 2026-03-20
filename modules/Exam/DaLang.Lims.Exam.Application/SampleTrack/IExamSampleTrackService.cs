using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;

namespace DaLang.Lims.Exam.Services.ExamSampleTrack;

/// <summary>
/// 样本跟踪服务
/// </summary>
public interface IExamSampleTrackService
{
    /// <summary>
    /// 列表查询
    /// </summary>
    Task<IEnumerable<ExamSampleTrackGetListDto>> GetListAsync(ExamSampleTrackGetListInput input);
}