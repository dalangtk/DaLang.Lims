using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Domain.Document;
using DaLang.Lims.Web.Framework.Domain.DocumentImage;
using DaLang.Lims.Web.Framework.Services.Document.Dto;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;

namespace DaLang.Lims.Web.Framework.Services.Document;

/// <summary>
/// 文档服务
/// </summary>
[Order(180)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DocumentService : BaseService, IDocumentService, IDynamicApi
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentImageRepository _documentImageRepository;
    private readonly Lazy<IFileService> _fileService;

    public DocumentService(
        IDocumentRepository DocumentRepository,
        IDocumentImageRepository documentImageRepository,
        Lazy<IFileService> fileService
    )
    {
        _documentRepository = DocumentRepository;
        _documentImageRepository = documentImageRepository;
        _fileService = fileService;
    }

    /// <summary>
    /// 查询分组
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocumentGetGroupOutput> GetGroupAsync(long id)
    {
        var ret = await _documentRepository.GetAsync(id);
        var result = ret.Adapt<DocumentGetGroupOutput>();
        return result;
    }

    /// <summary>
    /// 查询菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocumentGetMenuOutput> GetMenuAsync(long id)
    {
        var ret = await _documentRepository.GetAsync(id);
        var result = ret.Adapt<DocumentGetMenuOutput>();
        return result;
    }

    /// <summary>
    /// 查询文档内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocumentGetContentOutput> GetContentAsync(long id)
    {
        var ret = await _documentRepository.GetAsync(id);
        var result = ret.Adapt<DocumentGetContentOutput>();
        return result;
    }

    /// <summary>
    /// 查询文档列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public async Task<List<DocumentListOutput>> GetListAsync(string key, DateTime? start, DateTime? end)
    {
        if (end.HasValue)
        {
            end = end.Value.AddDays(1);
        }

        var data = await _documentRepository.AsQueryable()
            .WhereIF(key.NotNull(), a => a.Name.Contains(key) || a.Label.Contains(key))
            .WhereIF(start.HasValue && end.HasValue, a => SqlFunc.Between(a.ProTime, start.Value, end.Value))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync();

        var res = data.Adapt<List<DocumentListOutput>>();
        return res;
    }

    /// <summary>
    /// 查询图片列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<string>> GetImageListAsync(long id)
    {
        var result = await _documentImageRepository.AsQueryable()
            .Where(a => a.DocumentId == id)
            .OrderByDescending(a => a.Id)
            .ToListAsync(a => a.Url);

        return result;
    }

    /// <summary>
    /// 新增分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddGroupAsync(DocumentAddGroupInput input)
    {
        var entity = Mapper.Map<DocumentEntity>(input);
        await _documentRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddMenuAsync(DocumentAddMenuInput input)
    {
        var entity = Mapper.Map<DocumentEntity>(input);
        await _documentRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增图片
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddImageAsync(DocumentAddImageInput input)
    {
        var entity = Mapper.Map<DocumentImageEntity>(input);
        await _documentImageRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateGroupAsync(DocumentUpdateGroupInput input)
    {
        var entity = await _documentRepository.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _documentRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateMenuAsync(DocumentUpdateMenuInput input)
    {
        var entity = await _documentRepository.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _documentRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改文档内容
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateContentAsync(DocumentUpdateContentInput input)
    {
        var entity = await _documentRepository.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _documentRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除文档
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _documentRepository.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 彻底删除图片
    /// </summary>
    /// <param name="documentId"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task DeleteImageAsync(long documentId, string url)
    {
        await _documentImageRepository.DeleteAsync(m => m.DocumentId == documentId && m.Url == url);
    }

    /// <summary>
    /// 删除文档
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        var entity = await _documentRepository.GetAsync(id);
        entity.IsDeleted = true;
        await _documentRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 查询精简文档列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<dynamic>> GetPlainListAsync()
    {
        var documents = await _documentRepository.AsQueryable()
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Opened });

        var menus = documents
            .Where(a => (new[] { DocumentType.Group, DocumentType.Markdown }).Contains(a.Type))
            .Select(a => new
            {
                a.Id,
                a.ParentId,
                a.Label,
                a.Type,
                a.Opened
            });

        return menus;
    }

    /// <summary>
    /// 上传文档图片
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> UploadImage([FromForm] DocumentUploadImageInput input)
    {
        var fileInfo = await _fileService.Value.UploadFileAsync(input.File);
        //保存文档图片
        await AddImageAsync(new DocumentAddImageInput
        {
            DocumentId = input.Id,
            Url = fileInfo.LinkUrl
        });
        return fileInfo.LinkUrl;
    }
}