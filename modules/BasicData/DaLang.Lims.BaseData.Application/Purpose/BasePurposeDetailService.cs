using DaLang.Lims.BaseData.Contracts.Purpose;
using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Dict;
using DaLang.Lims.Web.Framework.Domain.DictType;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.BasePurposeDetail;

/// <summary>
/// 目的明细服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BasePurposeDetailService : BaseService, IBasePurposeDetailService, IDynamicApi
{
    private IBasePurposeDetailRepository _basePurposeDetailRep;

    public BasePurposeDetailService(IBasePurposeDetailRepository basePurposeDetailRep)
    {
        _basePurposeDetailRep = basePurposeDetailRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePurposeDetailDto> GetAsync(long id)
    {
        var output = await _basePurposeDetailRep.GetAsync(id);
        return output.Adapt<BasePurposeDetailDto>();
    }
    /// <summary>
    /// 获取目的明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BasePurposeDetailGetListDto>> GetPurposeDetailAsync(BasePurposeDetailQueryInput input)
    {
        if (input == null)
            throw ResultOutput.Exception("参数有误！");
        var list = await _basePurposeDetailRep.AsQueryable()
            .InnerJoin<BaseItemEntity>((a, b) => a.ItemCode == b.ItemCode && b.IsValid && !b.IsDeleted)
            .LeftJoin<BaseItemPersonalizeEntity>((a, b, c) => b.ItemCode == c.ItemCode && c.IsValid && !c.IsDeleted)
            .LeftJoin<DictTypeEntity>((a, b, c, d) => d.IsValid && !d.IsDeleted && d.Code == "ResultType")
            .LeftJoin<DictEntity>((a, b, c, d, e) => b.ResultType == e.Value && e.IsValid && !e.IsDeleted && e.DictTypeId == d.Id)
            .Where(a => a.GroupCode == input.GroupCode && a.PurCode == input.PurCode)
            .Select((a, b, c, d, e) => new BasePurposeDetailGetListDto
            {
                ItemName = string.IsNullOrWhiteSpace(c.ItemNamePersonalize) ? b.ItemName : c.ItemNamePersonalize,
                PrintOrder = c.PrintOrder ?? "",
                ResultType = e.Name
            }, true)
            .ToListAsync();

        list.Sort((a, b) => a.PrintOrder.CompareTo(b.PrintOrder));
        return list;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BasePurposeDetailGetListDto>> GetPageAsync(PageInput<BasePurposeDetailQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePurposeDetailRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BasePurposeDetailGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BasePurposeDetailGetListDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BasePurposeDetailDto input)
    {
        var entity = Mapper.Map<BasePurposeDetailEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _basePurposeDetailRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _basePurposeDetailRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BasePurposeDetailDto input)
    {
        var entity = await _basePurposeDetailRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("目的明细不存在！");

        Mapper.Map(input, entity);
        await _basePurposeDetailRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePurposeDetailRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 删除目的明细
    /// </summary>
    /// <param name="purCode">目的代码</param>
    /// <param name="instrumentItemCode">上机项目带啊吗</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteByInstrumentItemCodeAsync(string purCode, string instrumentItemCode)
    {
        return await _basePurposeDetailRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.InstrumentItemCode == instrumentItemCode && a.PurCode == purCode)
            .ExecuteCommandAsync() > 0;
    }
}