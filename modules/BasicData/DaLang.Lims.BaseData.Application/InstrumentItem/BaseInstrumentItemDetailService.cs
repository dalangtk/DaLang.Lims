using DaLang.Lims.BaseData.Contracts.InstrumentItem;
using DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;
using DaLang.Lims.BaseData.Domain.InstrumentItem;
using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.BaseInstrumentItem;

/// <summary>
/// 上机项目明细服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseInstrumentItemDetailService : BaseService, IBaseInstrumentItemDetailService, IDynamicApi
{
    private IBaseInstrumentItemDetailRepository _instrumentItemDetailRep;
    IBaseInstrumentItemRepository _instrumentItemRep;

    public BaseInstrumentItemDetailService(IBaseInstrumentItemDetailRepository instrumentItemDetailRep,
        IBaseInstrumentItemRepository instrumentItemRep)
    {
        _instrumentItemDetailRep = instrumentItemDetailRep;
        _instrumentItemRep = instrumentItemRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseInstrumentItemDetailDto> GetAsync(long id)
    {
        var output = await _instrumentItemDetailRep.AsQueryable()
            .LeftJoin<BaseItemEntity>((a, b) => a.ItemCode == b.ItemCode && b.IsValid && !b.IsDeleted)
            .Where((a, b) => a.IsValid == true && a.IsDeleted == false && a.Id == id)
            .Select((a, b) => new BaseInstrumentItemDetailDto { ItemName = b.ItemName }, true)
            .FirstAsync();
        return output.Adapt<BaseInstrumentItemDetailDto>();
    }

    /// <summary>
    /// 根据上机项目代码获取明细
    /// </summary>
    /// <param name="instrumentItemCodeList"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseInstrumentItemDetailDto>> GetListByInstrumentItemCodeAsync(List<string> instrumentItemCodeList)
    {
        if (instrumentItemCodeList == null || instrumentItemCodeList.Count == 0)
            throw ResultOutput.Exception("参数有误！");

        var output = await _instrumentItemRep.AsQueryable()
             .InnerJoin<BaseInstrumentItemDetailEntity>((a, b) => a.InstrumentItemCode == b.InstrumentItemCode && b.IsValid && !b.IsDeleted)
             .InnerJoin<BaseItemEntity>((a, b, c) => b.ItemCode == c.ItemCode && c.IsValid && !c.IsDeleted)
             .LeftJoin<BaseItemPersonalizeEntity>((a, b, c, d) => c.ItemCode == d.ItemCode && d.IsValid && !d.IsDeleted)
             .Where(a => instrumentItemCodeList.Contains(a.InstrumentItemCode))
             .Select((a, b, c, d) => new BaseInstrumentItemDetailDto
             {
                 Id = b.Id,
                 ItemName = string.IsNullOrWhiteSpace(d.ItemNamePersonalize) ? c.ItemName : d.ItemNamePersonalize,
                 ResultType = c.ResultType,
                 PrintOrder = d.PrintOrder ?? "",
                 Method = d.MethodCode,
                 InstrumentItemName = a.InstrumentItemName,
                 Sort = b.Sort
             }, true)
             .ToListAsync();

        //按打印排序显示
        output.Sort((a, b) => a.PrintOrder.PadLeft(20, '0').CompareTo(b.PrintOrder.PadLeft(20, '0')));
        return output.Adapt<List<BaseInstrumentItemDetailDto>>();
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseInstrumentItemDetailGetListDto>> GetPageAsync(PageInput<BaseInstrumentItemDetailQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _instrumentItemDetailRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseInstrumentItemDetailGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseInstrumentItemDetailGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseInstrumentItemDetailDto input)
    {
        var entity = Mapper.Map<BaseInstrumentItemDetailEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _instrumentItemDetailRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _instrumentItemDetailRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }
    /// <summary>
    /// 新增列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddListAsync(List<BaseInstrumentDetailAddInput> input)
    {
        var entity = Mapper.Map<List<BaseInstrumentItemDetailEntity>>(input);
        if (!entity.Any())
            throw ResultOutput.Exception("参数有误！");

        if (entity.Exists(a => a.Sort == 0))
        {
            var sort = await _instrumentItemDetailRep.AsQueryable()
                .Where(a => a.InstrumentItemCode == input.First().InstrumentItemCode)
                .MaxAsync(a => a.Sort);
            entity.FindAll(a => a.Sort == 0).ForEach(a =>
            {
                sort++;
                a.Sort = sort;
            });
        }
        return await _instrumentItemDetailRep.InsertRangeAsync(entity);
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseInstrumentItemDetailDto input)
    {
        var entity = await _instrumentItemDetailRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("上机项目明细不存在！");

        Mapper.Map(input, entity);
        await _instrumentItemDetailRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _instrumentItemDetailRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
    }
}