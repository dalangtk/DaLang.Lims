using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Domain.Dto;
using DaLang.Lims.Web.Framework.Services.Dto;

namespace DaLang.Lims.Web.Framework.Services;

/// <summary>
/// 文件接口
/// </summary>
public interface IFileService
{
    Task<PageOutput<FileGetPageOutput>> GetPageAsync(PageInput<FileGetPageDto> input);

    Task DeleteAsync(FileDeleteInput input);

    Task<FileEntity> UploadFileAsync(IFormFile file, string fileDirectory = "", bool fileReName = true, string bussinessPrefix = "", bool saveToDb = true);

    Task<List<FileEntity>> UploadFilesAsync([Required] IFormFileCollection files, string fileDirectory = "", bool fileReName = true);
}