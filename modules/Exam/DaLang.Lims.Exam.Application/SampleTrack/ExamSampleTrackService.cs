using DaLang.Lims.Exam.Core.Consts;
using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;
using DaLang.Lims.Shared.Domain.ExamSampleTrack;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.Exam.Services.ExamSampleTrack;

/// <summary>
/// 样本跟踪服务
/// </summary>
[DynamicApi(Area = ExamConsts.AreaName)]
public class ExamSampleTrackService : BaseService, IExamSampleTrackService, IDynamicApi
{
    private AdminRepositoryBase<ExamSampleTrackEntity> _examSampleTrackRep;

    public ExamSampleTrackService(AdminRepositoryBase<ExamSampleTrackEntity> examSampleTrackRep)
    {
        _examSampleTrackRep = examSampleTrackRep;
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<ExamSampleTrackGetListDto>> GetListAsync(ExamSampleTrackGetListInput input)
    {
        var list = await _examSampleTrackRep.AsQueryable()
            .Where(v => v.Barcode == input.Barcode)
            .Select<ExamSampleTrackGetListDto>()
            .OrderByDescending(v => v.ProTime)
            .ToListAsync();
        return list;
    }
}