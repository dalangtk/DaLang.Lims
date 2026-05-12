using DaLang.Lims.Exam.Contracts.ReportFiles.Dto;
using DaLang.Lims.Exam.Contracts.SampleTest.Dto;
using DaLang.Lims.Shared.Contracts.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;
using DaLang.Lims.Web.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.Exam.Contracts.SampleTest;

/// <summary>
/// 标本检验服务
/// </summary>
public interface ISampleTestService
{
    /// <summary>
    /// 获取检验列表
    /// </summary>
    Task<List<ExamInfoDto>> GetSampleListAsync(ExamListQueryInput input);

    /// <summary>
    /// 获取检验信息
    /// </summary>
    Task<ExamInfoDto> GetExamInfoAsync(long id);

    /// <summary>
    /// 获取检验结果
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<List<ExamResultDto>> GetResultListAsync(long examInfoId);

    /// <summary>
    /// 保存结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ExamResultDto> SaveItemResult(ExamResultUpdateInput input);
    /// <summary>
    /// 新增检验信息
    /// </summary>
    Task<long> AddExamInfoAsync(ExamInfoDto input);

    /// <summary>
    /// 更新检验信息
    /// </summary>
    Task UpdateExamInfoAsync(ExamInfoDto input);

    /// <summary>
    /// 更新病人基本信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdatePatientInfo(UpdatePatientInfoInput input);
    /// <summary>
    /// 删除检验信息
    /// </summary>
    Task<bool> DeleteExamInfoAsync(long id);
    /// <summary>
    /// 刷新项目信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> RefreshItemInfo(RefreshExamInfoInput input);
    /// <summary>
    /// 计算高低标记
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ExamResultDto> CalcAbnormal(ExamResultUpdateInput input);
    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<AuditResultDto> Audit(AuditInput input);
    /// <summary>
    /// 反审核
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ExamInfoDto> UnAudit(UnAuditInput input);
    /// <summary>
    /// 增项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ExamInfoDto> AddItem(AddOrDeletePurposeInput input);
    /// <summary>
    /// 退项
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ExamInfoDto> BackItem(AddOrDeletePurposeInput input);
    /// <summary>
    /// 取消检测
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ExamInfoDto> CancelTest(CancelTestInput input);
    /// <summary>
    /// 报告预览
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<FileResult> RptPreview(long examInfoId);
    /// <summary>
    /// 获取报告文件
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<List<ReportFilesDto>> GetReportFiles(long examInfoId);

    /// <summary>
    /// 判断用户组别权限
    /// </summary>
    /// <param name="id"></param>
    /// <param name="groupCode"></param>
    /// <param name="operType"></param>
    /// <returns></returns>
    Task<string> CheckUserGroupPermission(long id, string groupCode, OperationTypeEnum operType);
}