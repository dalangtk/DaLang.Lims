using DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood;
using DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Core.Enum;
using DaLang.Lims.Pretreatment.Domain.PretreatSortSplitBlood;
using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Shared.Domain.ExamSampleTrack;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pretreatment.Services.PretreatSortSplitBlood;

/// <summary>
/// 标本分血服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class PretreatSortSplitBloodService : BaseService, IPretreatSortSplitBloodService, IDynamicApi
{
    private IPretreatSortSplitBloodRepository _splitBloodRep;
    private IPretreatSortSplitBloodDetailRepository _splitBlooldDetailRep;
    private AdminRepositoryBase<ApplyPurposeEntity> _purposeRep;
    private AdminRepositoryBase<ExamSampleTrackEntity> _sampleTrackRep;

    public PretreatSortSplitBloodService(IPretreatSortSplitBloodRepository pretreatSortSplitBloodRep,
        IPretreatSortSplitBloodDetailRepository splitBlooldDetailRep,
        AdminRepositoryBase<ApplyPurposeEntity> purposeRep,
        AdminRepositoryBase<ExamSampleTrackEntity> sampleTrackRep)
    {
        _splitBloodRep = pretreatSortSplitBloodRep;
        _splitBlooldDetailRep = splitBlooldDetailRep;
        _purposeRep = purposeRep;
        _sampleTrackRep = sampleTrackRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PretreatSortSplitBloodDto> GetAsync(long id)
    {
        var output = await _splitBloodRep.GetAsync(id);
        return output.Adapt<PretreatSortSplitBloodDto>();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PretreatSortSplitBloodGetListDto>> GetPageAsync(PageInput<PretreatSortSplitBloodQueryInput> input)
    {
        var filter = input.Filter;
        var barcode = filter.Barcode;
        var list = await _splitBloodRep.AsQueryable()
            .WhereIF(string.IsNullOrWhiteSpace(barcode), c => SqlFunc.Between(c.ReceiveTime, filter.BeginDate, filter.EndDate))
            .WhereIF(!string.IsNullOrWhiteSpace(barcode), c => c.Barcode.Equals(barcode))
            .OrderBy(c => c.SplitBloodTime)
            .Select<PretreatSortSplitBloodGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<PretreatSortSplitBloodGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 获取分血明细
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<PretreatSortSplitBloodDetailDto>> GetDetailAsync(long id)
    {
        var details = await _splitBlooldDetailRep.GetListAsync(a => a.SplitBloodId == id);
        return details.Adapt<List<PretreatSortSplitBloodDetailDto>>();
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(PretreatSortSplitBloodDto input)
    {
        var entity = Mapper.Map<PretreatSortSplitBloodEntity>(input);
        var id = await _splitBloodRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(PretreatSortSplitBloodDto input)
    {
        var entity = await _splitBloodRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("标本分血不存在！");

        Mapper.Map(input, entity);
        await _splitBloodRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _splitBloodRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 标本分血
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<SplitBloodOutput> SplitBlood(SplitBloodInput input)
    {
        SplitBloodOutput result = new();
        if (string.IsNullOrWhiteSpace(input.Barcode))
            throw ResultOutput.Exception("条码不可为空！");

        var splitBloods = await _splitBloodRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SampleTypeCode), v => v.SampleTypeCode == input.SampleTypeCode)
            .Where(v => v.Barcode == input.Barcode && v.SplitBloodStatus == 0)
            .ToListAsync();
        if (!splitBloods.Any())
            throw ResultOutput.Exception($"条码{input.Barcode}不存在未分血数据！");

        if (splitBloods.GroupBy(v => v.SampleTypeCode).Count() > 1)
        {
            result.Status = SplitBloodStatusEnum.SelectSortSampleType;
            var sampleTypeList = splitBloods.Select(a => new CodeNameDto
            {
                Code = a.SampleTypeCode,
                Name = a.SampleTypeName
            }).ToDistinct((a, b) => a.Code == b.Code);

            result.SampleTypeList = sampleTypeList.ToList();
            return result;
        }
        else
        {
            var splitDetails = await _splitBlooldDetailRep.GetListAsync(a => a.SplitBloodId == splitBloods.First().Id);

            var purCodes = splitDetails.SelectMany(v => v.PurCodes!.Split(',')).Distinct().ToList();

            var purposeList = await _purposeRep.AsQueryable()
                                   .Where(a => a.Barcode == input.Barcode && a.AddType != 2 && !a.IsDeleted)
                                   .Where(a => purCodes.Contains(a.PurCode) && a.SampleStatus == SampleStatusEnum.WaitSplitBlood.ToInt())
                                   .ToListAsync();

            purposeList.ForEach(v =>
            {
                v.SampleStatus = SampleStatusEnum.SplitBlood.ToInt();
                v.SampleStatusName = SampleStatusEnum.SplitBlood.ToDescription();
            });

            await _purposeRep.Context.Updateable(purposeList)
                .UpdateColumns(v => new { v.SampleStatus, v.SampleStatusName }, true)
                .ExecuteCommandAsync();

            await _splitBloodRep.AsUpdateable()
                .SetColumns(a => new PretreatSortSplitBloodEntity
                {
                    SplitBloodStatus = 1,
                    SplitBloodTime = DateTime.Now,
                    SplitBloodUserId = AppInfo.User.Id,
                    SplitBloodUserName = AppInfo.User.Name
                }, true)
                .Where(v => v.Id == splitBloods.First().Id)
                .ExecuteCommandAsync();

            var track = new ExamSampleTrackDto
            {
                Barcode = input.Barcode,
                OperationType = OperationTypeEnum.SplitBlood,
                TrackContent = $"标本分血-，标本类型：{purposeList.First().SampleTypeName}，目的：{string.Join(",", purposeList.OrderBy(a => a.PurCode).Select(a => $"{a.PurName}({a.PurCode})").Distinct())}",
            };
            await _sampleTrackRep.InsertAsync(track.Adapt<ExamSampleTrackEntity>());

            result.Status = SplitBloodStatusEnum.SplitBloodSuccess;
            result.SplitBlood = splitBloods.Adapt<List<PretreatSortSplitBloodDto>>();
            result.SplitBloodDetail = splitDetails.Adapt<List<PretreatSortSplitBloodDetailDto>>();
        }
        return result;
    }
}