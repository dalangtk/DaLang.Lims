using DaLang.Lims.Shared.Contracts.ApplyInfo.Dto;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Statistics.Contracts.SampleQuery;
using DaLang.Lims.Statistics.Contracts.SampleQuery.Dto;
using DaLang.Lims.Statistics.Core.ReportQuery;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Statistics.Application.SampleQuery;

/// <summary>
/// 样本查询服务
/// </summary>
[DynamicApi(Area = StatisticsConsts.AreaName)]
public class SampleQueryService : BaseService, ISampleQueryService, IDynamicApi
{
    private readonly AdminRepositoryBase<ApplyInfoEntity> _applyInfoRep;
    private readonly AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private readonly AdminRepositoryBase<ApplyPurposeEntity> _applyPurposeRep;

    public SampleQueryService(AdminRepositoryBase<ApplyInfoEntity> applyInfoRep,
        AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        AdminRepositoryBase<ApplyPurposeEntity> applyPurposeRep)
    {
        _applyInfoRep = applyInfoRep;
        _examInfoRep = examInfoRep;
        _applyPurposeRep = applyPurposeRep;
    }

    /// <summary>
    /// 样本查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ApplyInfoDto>> QuerySample(PageInput<SampleQueryInput> input)
    {
        var filter = input.Filter;
        var query = _applyInfoRep.AsQueryable();
        if (string.IsNullOrWhiteSpace(filter.Barcode))
        {
            if (filter.BeginTime == null || filter.EndTime == null)
                throw ResultOutput.Exception("time can not be null.");

            if (!string.IsNullOrWhiteSpace(filter.PatientName))
                query = query.Where(v => v.PatientName == filter.PatientName);

            if (!string.IsNullOrWhiteSpace(filter.CustomerCode))
                query = query.Where(v => v.CustomerCode == filter.CustomerCode);

            query = query.Where(v => SqlFunc.Between(v.ReceiveTime, filter.BeginTime, filter.EndTime) || SqlFunc.Between(v.ProTime, filter.BeginTime, filter.EndTime));
        }
        else
            query = query.Where(v => v.Barcode == filter.Barcode);

        var list = await query
            .OrderBy(c => new { c.ReceiveTime, c.CustomerCode })
            .Select<ApplyInfoDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<ApplyInfoDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 根据条码获取检验列表
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ExamInfoDto>> GetExamList(string barcode)
    {
        var list = await _examInfoRep.GetListAsync(v => v.Barcode == barcode);
        return list.Adapt<List<ExamInfoDto>>();
    }

    /// <summary>
    /// 根据条码获取申请单目的列表
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ApplyPurposeDto>> GetPurposeList(string barcode)
    {
        var list = await _applyPurposeRep.GetListAsync(v => v.Barcode == barcode);
        return list.Adapt<List<ApplyPurposeDto>>();
    }
}
