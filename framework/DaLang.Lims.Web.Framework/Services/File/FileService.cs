using AngleSharp.Text;
using DaLang.Lims.Web.Common.Files;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Helpers;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Domain.Dto;
using DaLang.Lims.Web.Framework.Services.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnceMi.AspNetCore.OSS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services;

/// <summary>
/// 文件服务
/// </summary>
[Order(110)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class FileService : BaseService, IFileService, IDynamicApi
{
    private readonly IFileRepository _fileRep;
    private readonly IOSSServiceFactory _oSSServiceFactory;
    private readonly OSSConfig _oSSConfig;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileService(
        IFileRepository fileRep,
        IOSSServiceFactory oSSServiceFactory,
        IOptions<OSSConfig> oSSConfig,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _fileRep = fileRep;
        _oSSServiceFactory = oSSServiceFactory;
        _oSSConfig = oSSConfig.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<FileGetPageOutput>> GetPageAsync(PageInput<FileGetPageDto> input)
    {
        var fileName = input.Filter?.FileName;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _fileRep.GetQueryable(dynamicCondition)
        .WhereIF(fileName.NotNull(), a => a.FileName.Contains(fileName))
        .Select(o => new FileGetPageOutput { FileName = o.SaveFileName, ProviderName = o.Provider.ToString() }, isAutoFill: true)
        .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<FileGetPageOutput>()
        {
            List = (IList<FileGetPageOutput>)list.Items,
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task DeleteAsync(FileDeleteInput input)
    {
        var file = await _fileRep.GetAsync(input.Id);
        if (file == null)
        {
            return;
        }

        var shareFile = await _fileRep.AsQueryable().Where(a => a.Id != input.Id && a.LinkUrl == file.LinkUrl).AnyAsync();
        if (!shareFile)
        {
            if (file.Provider.HasValue)
            {
                var oSSService = _oSSServiceFactory.Create(file.Provider.ToString());
                var oSSOptions = _oSSConfig.OSSConfigs.Where(a => a.Enable && a.Provider == file.Provider).FirstOrDefault();
                var enableOss = oSSOptions != null && oSSOptions.Enable;
                if (enableOss)
                {
                    var filePath = Path.Combine(file.FileDirectory, file.SaveFileName + file.Extension).ToPath();
                    await oSSService.RemoveObjectAsync(file.BucketName, filePath);
                }
            }
            else
            {
                var env = LazyGetRequiredService<IWebHostEnvironment>();
                var filePath = Path.Combine(env.WebRootPath, file.FileDirectory, file.SaveFileName + file.Extension).ToPath();
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        file.IsDeleted = true;
        await _fileRep.UpdateAsync(file);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">文件</param>
    /// <param name="fileDirectory">文件目录</param>
    /// <param name="fileReName">文件重命名</param>
    /// <returns></returns>
    public async Task<FileEntity> UploadFileAsync([Required] IFormFile file, string fileDirectory = "", bool fileReName = true, string bussinessPrefix = "", bool saveToDb = true)
    {
        var localUploadConfig = _oSSConfig.LocalUploadConfig;

        var extention = Path.GetExtension(file.FileName).ToLower();
        var hasIncludeExtension = localUploadConfig.IncludeExtension?.Length > 0;
        if (hasIncludeExtension && !localUploadConfig.IncludeExtension.Contains(extention))
        {
            throw new Exception($"不允许上传{extention}文件格式");
        }
        var hasExcludeExtension = localUploadConfig.ExcludeExtension?.Length > 0;
        if (hasExcludeExtension && localUploadConfig.ExcludeExtension.Contains(extention))
        {
            throw new Exception($"不允许上传{extention}文件格式");
        }

        var fileLenth = file.Length;
        if (fileLenth > localUploadConfig.MaxSize)
        {
            throw new Exception($"文件大小不能超过{new FileSize(localUploadConfig.MaxSize)}");
        }

        var oSSOptions = _oSSConfig.OSSConfigs.Where(a => a.Enable && a.Provider == _oSSConfig.Provider).FirstOrDefault();
        var enableOss = oSSOptions != null && oSSOptions.Enable;
        var enableMd5 = enableOss ? oSSOptions.Md5 : localUploadConfig.Md5;
        var md5 = string.Empty;
        if (enableMd5)
        {
            md5 = MD5Encrypt.GetHash(file.OpenReadStream());
            var md5FileEntity = await _fileRep.AsQueryable().WhereIF(enableOss, a => a.Provider == oSSOptions.Provider).Where(a => a.Md5 == md5).FirstAsync();
            if (md5FileEntity != null)
            {
                var sameFileEntity = new FileEntity
                {
                    Provider = md5FileEntity.Provider,
                    BucketName = md5FileEntity.BucketName,
                    FileGuid = Guid.NewGuid(),
                    SaveFileName = md5FileEntity.SaveFileName,
                    FileName = Path.GetFileNameWithoutExtension(file.FileName),
                    Extension = extention,
                    FileDirectory = md5FileEntity.FileDirectory,
                    Size = md5FileEntity.Size,
                    SizeFormat = md5FileEntity.SizeFormat,
                    LinkUrl = md5FileEntity.LinkUrl,
                    Md5 = md5,
                };
                sameFileEntity = await _fileRep.InsertReturnEntityAsync(sameFileEntity);
                return sameFileEntity;
            }
        }

        if (fileDirectory.IsNull())
        {
            fileDirectory = localUploadConfig.Directory;
            if (localUploadConfig.DateTimeDirectory.NotNull())
            {
                fileDirectory = Path.Combine(fileDirectory, bussinessPrefix, DateTime.Now.ToString(localUploadConfig.DateTimeDirectory)).ToPath();
            }
        }

        var fileSize = new FileSize(fileLenth);
        var fileEntity = new FileEntity
        {
            Provider = oSSOptions?.Provider,
            BucketName = oSSOptions?.BucketName,
            FileGuid = Guid.NewGuid(),
            FileName = Path.GetFileNameWithoutExtension(file.FileName),
            Extension = extention,
            FileDirectory = fileDirectory,
            Size = fileSize.Size,
            SizeFormat = fileSize.ToString(),
            Md5 = md5
        };
        fileEntity.SaveFileName = fileReName ? fileEntity.FileGuid.ToString() : fileEntity.FileName;

        var filePath = Path.Combine(fileDirectory, fileEntity.SaveFileName + fileEntity.Extension).ToPath();
        var url = string.Empty;
        if (enableOss)
        {
            url = oSSOptions.Url;
            if (url.IsNull())
            {
                url = oSSOptions.Provider switch
                {
                    OSSProvider.Minio => $"{oSSOptions.Endpoint}/{oSSOptions.BucketName}",
                    OSSProvider.Aliyun => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
                    OSSProvider.QCloud => $"{oSSOptions.BucketName}-{oSSOptions.Endpoint}.cos.{oSSOptions.Region}.myqcloud.com",
                    OSSProvider.Qiniu => $"{oSSOptions.BucketName}.{oSSOptions.Region}.qiniup.com",
                    OSSProvider.HuaweiCloud => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
                    _ => ""
                };

                if (url.IsNull())
                {
                    throw ResultOutput.Exception($"请配置{oSSOptions.Provider}的Url参数");
                }

                var urlProtocol = oSSOptions.IsEnableHttps ? "https" : "http";
                fileEntity.LinkUrl = $"{urlProtocol}://{url}/{filePath}";
            }
            else
            {
                fileEntity.LinkUrl = $"{url}/{filePath}";
            }
        }
        else
        {
            //fileEntity.LinkUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/{filePath}";
            string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            string domainName = $"{scheme}://{host}";

            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedProto) &&
                _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHost))
            {
                domainName = $"{forwardedProto.FirstOrDefault()}://{forwardedHost.FirstOrDefault()}";
            }

            //fileEntity.LinkUrl = $"{(domainName.EndsWith("/") ? domainName : domainName + "/")}{filePath}";
            fileEntity.LinkUrl = filePath;
        }

        if (enableOss)
        {
            var oSSService = _oSSServiceFactory.Create(_oSSConfig.Provider.ToString());
            var ret = await oSSService.PutObjectAsync(oSSOptions.BucketName, filePath, file.OpenReadStream());
        }
        else
        {
            var uploadHelper = LazyGetRequiredService<UploadHelper>();
            var env = LazyGetRequiredService<IWebHostEnvironment>();
            fileDirectory = Path.Combine(env.WebRootPath, fileDirectory).ToPath();
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            filePath = Path.Combine(env.WebRootPath, filePath).ToPath();
            await uploadHelper.SaveAsync(file, filePath);
        }

        if (saveToDb)
            fileEntity = await _fileRep.InsertReturnEntityAsync(fileEntity);

        return fileEntity;
    }

    /// <summary>
    /// 上传多文件
    /// </summary>
    /// <param name="files">文件列表</param>
    /// <param name="fileDirectory">文件目录</param>
    /// <param name="fileReName">文件重命名</param>
    /// <returns></returns>
    public async Task<List<FileEntity>> UploadFilesAsync([Required] IFormFileCollection files, string fileDirectory = "", bool fileReName = true)
    {
        var fileList = new List<FileEntity>();
        foreach (var file in files)
        {
            fileList.Add(await UploadFileAsync(file, fileDirectory, fileReName));
        }
        return fileList;
    }

    //[AllowAnonymous]
    //[NonFormatResult]
    //public async Task<FileResult> GetFile(string fileName)
    //{
    //    var localUploadConfig = _oSSConfig.LocalUploadConfig;
    //    var oSSOptions = _oSSConfig.OSSConfigs.Where(a => a.Enable && a.Provider == _oSSConfig.Provider).FirstOrDefault();
    //    var enableOss = oSSOptions != null && oSSOptions.Enable;

    //    var fileDirectory = localUploadConfig.Directory;
    //    var filePath = Path.Combine(fileDirectory, fileName).ToPath();
    //    var url = string.Empty;
    //    var fileFullPath = filePath;
    //    if (enableOss)
    //    {
    //        url = oSSOptions.Url;
    //        if (url.IsNull())
    //        {
    //            url = oSSOptions.Provider switch
    //            {
    //                OSSProvider.Minio => $"{oSSOptions.Endpoint}/{oSSOptions.BucketName}",
    //                OSSProvider.Aliyun => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
    //                OSSProvider.QCloud => $"{oSSOptions.BucketName}-{oSSOptions.Endpoint}.cos.{oSSOptions.Region}.myqcloud.com",
    //                OSSProvider.Qiniu => $"{oSSOptions.BucketName}.{oSSOptions.Region}.qiniup.com",
    //                OSSProvider.HuaweiCloud => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
    //                _ => ""
    //            };

    //            if (url.IsNull())
    //            {
    //                throw ResultOutput.Exception($"请配置{oSSOptions.Provider}的Url参数");
    //            }

    //            var urlProtocol = oSSOptions.IsEnableHttps ? "https" : "http";
    //            fileFullPath = $"{urlProtocol}://{url}/{filePath}";
    //        }
    //        else
    //        {
    //            fileFullPath = $"{url}/{filePath}";
    //        }
    //    }

    //    var fileInfo = new FileInfo(filePath);
    //    var name = fileInfo.Name;
    //    if (enableOss)
    //    {
    //        var oSSService = _oSSServiceFactory.Create(_oSSConfig.Provider.ToString());
    //        Stream stream = null;
    //        await oSSService.GetObjectAsync(oSSOptions.BucketName, filePath, (s) =>
    //        {
    //            stream = s;
    //        });
    //        var actionresult = new FileStreamResult(stream, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/csv"));
    //        actionresult.FileDownloadName = name;
    //        return actionresult;
    //    }
    //    else
    //    {
    //        if (!File.Exists(fileName))
    //            return null;
    //        var stream = File.OpenRead(fileName);
    //        var actionresult = new FileStreamResult(stream, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/csv"));
    //        actionresult.FileDownloadName = name;
    //        return actionresult;
    //    }
    //}
}