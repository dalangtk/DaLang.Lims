using DaLang.Lims.Exam.Contracts.ExamImages.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.Exam.Contracts.ExamImages;

/// <summary>
/// 检验图片服务
/// </summary>
public interface IExamImagesService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ExamImagesDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<ExamImagesDto>> GetPageAsync(PageInput<ExamImagesQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ExamImagesDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ExamImagesDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取所有图片
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<List<ExamImagesDto>> GetAll(long examInfoId);

    /// <summary>
    /// 上传检验图片
    /// </summary>
    /// <param name="file"></param>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<FileEntity> UploadExamImage([Required] IFormFile file, long examInfoId);
}