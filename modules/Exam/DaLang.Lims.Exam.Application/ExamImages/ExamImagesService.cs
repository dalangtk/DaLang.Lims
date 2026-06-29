using DaLang.Lims.Exam.Contracts.ExamImages;
using DaLang.Lims.Exam.Contracts.ExamImages.Dto;
using DaLang.Lims.Exam.Core.Consts;
using DaLang.Lims.Exam.Domain.ExamImages;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Dict;
using DaLang.Lims.Web.Framework.Services.Dict.Dto;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.Exam.Application.ExamImages;

/// <summary>
/// 检验图片服务
/// </summary>
[DynamicApi(Area = ExamConsts.AreaName)]
public class ExamImagesService : BaseService, IExamImagesService, IDynamicApi
{
    private IExamImagesRepository _examImagesRep;
    private IFileService _fileService;
    private AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private IDictService _dictService;

    public ExamImagesService(IExamImagesRepository examImagesRep,
        IFileService fileService,
        AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        IDictService dictService)
    {
        _examImagesRep = examImagesRep;
        _fileService = fileService;
        _examInfoRep = examInfoRep;
        _dictService = dictService;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExamImagesDto> GetAsync(long id)
    {
        var output = await _examImagesRep.GetAsync(id);
        return output.Adapt<ExamImagesDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ExamImagesDto>> GetPageAsync(PageInput<ExamImagesQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _examImagesRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<ExamImagesDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<ExamImagesDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(ExamImagesDto input)
    {
        var entity = Mapper.Map<ExamImagesEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _examImagesRep.AsQueryable()
                .Where(v => v.ExamInfoId == input.ExamInfoId)
                .MaxAsync(a => a.Sort);
            entity.Sort = sort + 10;
        }
        var id = await _examImagesRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ExamImagesDto input)
    {
        var entity = await _examImagesRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("检验图片不存在！");

        await _examImagesRep.AsUpdateable()
            .SetColumns(v => new ExamImagesEntity
            {
                AntiBodyCode = input.AntiBodyCode,
                AntiBodyName = input.AntiBodyName,
                ZoomCode = input.ZoomCode,
                ZoomName = input.ZoomName,
                IsShow = input.IsShow
            })
            .Where(v => v.Id == input.Id)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _examImagesRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取所有图片
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <param name="isGrossExamination"></param>
    /// <returns></returns>
    public async Task<List<ExamImagesDto>> GetAll(long examInfoId, bool isGrossExamination = false)
    {
        var imageType = isGrossExamination ? 1 : 0;
        var output = await _examImagesRep.GetListAsync(v => v.ExamInfoId == examInfoId && v.ImageType == imageType);
        return output.Adapt<List<ExamImagesDto>>();
    }

    /// <summary>
    /// 上传检验图片
    /// </summary>
    /// <param name="file"></param>
    /// <param name="examInfoId"></param>
    /// <param name="isGrossExamination"></param>
    /// <returns></returns>
    public async Task<FileEntity> UploadExamImage([Required] IFormFile file, long examInfoId, bool isGrossExamination = false)
    {
        var fileRet = await _fileService.UploadFileAsync(file, "lims\\exam\\examimage", true, "", false);
        var image = new ExamImagesDto
        {
            ExamInfoId = examInfoId,
            FileName = fileRet.FileName,
            FileUrl = fileRet.LinkUrl,
            ImageType = isGrossExamination ? 1 : 0,
            IsShow = true
        };

        var antiBody = await GetImageDefaultDict(examInfoId, "AntiBody");
        if (antiBody != null)
        {
            image.AntiBodyCode = antiBody.Code;
            image.AntiBodyCode = antiBody.Name;
        }

        var zoom = await GetImageDefaultDict(examInfoId, "Zoom");
        if (zoom != null)
        {
            image.ZoomCode = zoom.Code;
            image.ZoomName = zoom.Name;
        }

        await _examImagesRep.InsertAsync(image.Adapt<ExamImagesEntity>());
        return fileRet;
    }
    private async Task<DictGetListDto?> GetImageDefaultDict(long examInfoId, string dictCode)
    {
        var wfCode = await _examInfoRep.AsQueryable().Select(v => v.WFCode).FirstAsync();

        var dicts = await _dictService.GetDictByTypeCodeAsync(dictCode);
        if (dicts.Exists(v => v.Description?.Split(',').Contains(wfCode) == true))
            return dicts.First(v => v.Description?.Split(',').Contains(wfCode) == true);
        return null;
    }
}