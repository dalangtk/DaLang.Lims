using DaLang.Lims.BaseData.Contracts.InstrumentItem;
using DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;
using DaLang.Lims.BaseData.Domain.InstrumentItem;
using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.BaseInstrumentItem;

/// <summary>
/// 上机项目服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseInstrumentItemService : BaseService, IBaseInstrumentItemService, IDynamicApi
{
    private IBaseInstrumentItemRepository _baseInstrumentItemRep;
    private IBaseInstrumentItemDetailRepository _baseInstrumentItemDetailRep;
    private IBasePurposeRepository _basePurposeRep;

    public BaseInstrumentItemService(IBaseInstrumentItemRepository baseInstrumentItemRep,
        IBaseInstrumentItemDetailRepository baseInstrumentItemDetailRep,
        IBasePurposeRepository basePurposeRep)
    {
        _baseInstrumentItemRep = baseInstrumentItemRep;
        _baseInstrumentItemDetailRep = baseInstrumentItemDetailRep;
        _basePurposeRep = basePurposeRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseInstrumentItemDto> GetAsync(long id)
    {
        var output = await _baseInstrumentItemRep.GetAsync(id);
        return output.Adapt<BaseInstrumentItemDto>();
    }
    /// <summary>
    /// 根据上机项目代码查询
    /// </summary>
    /// <param name="codeList"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseInstrumentItemDto>> GetByCodeAsync(List<string> codeList)
    {
        if (codeList == null || codeList.Count == 0)
            throw ResultOutput.Exception("参数有误");
        var output = await _baseInstrumentItemRep.GetListAsync(a => codeList.Contains(a.InstrumentItemCode));
        return output.Adapt<List<BaseInstrumentItemDto>>();
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseInstrumentItemGetListDto>> GetPageAsync(PageInput<BaseInstrumentItemQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseInstrumentItemRep.GetQueryable(dynamicCondition)
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.GroupCode), a => a.GroupCode == filter.GroupCode)
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.InstrumentItemCode), a => a.InstrumentItemCode == filter.InstrumentItemCode)
            .OrderBy(c => c.Sort)
            .Select<BaseInstrumentItemGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseInstrumentItemGetListDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseInstrumentItemDto input)
    {
        if (string.IsNullOrWhiteSpace(input?.InstrumentItemCode))
            throw ResultOutput.Exception("上机项目代码不可为空");
        input.InstrumentItemCode = input.InstrumentItemCode.ToUpper().Trim();
        var isExists = await _baseInstrumentItemRep.IsAnyAsync(a => a.InstrumentItemCode == input.InstrumentItemCode);
        if (isExists)
            throw ResultOutput.Exception($"上机项目代码{input.InstrumentItemCode}已存在！");

        var entity = Mapper.Map<BaseInstrumentItemEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseInstrumentItemRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseInstrumentItemRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseInstrumentItemDto input)
    {
        var entity = await _baseInstrumentItemRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("上机项目不存在！");

        Mapper.Map(input, entity);
        await _baseInstrumentItemRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [AdminTransaction]
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _baseInstrumentItemRep.GetAsync(id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("上机项目不存在！");

        await _baseInstrumentItemDetailRep.AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.InstrumentItemCode == entity.InstrumentItemCode)
            .ExecuteCommandAsync();

        return await _baseInstrumentItemRep
            .AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取上机项目及项目明细
    /// </summary>
    /// <param name="purCodes"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<InstrumentItemInfoDto>> GetInstrumntItemInfo(List<string> purCodes)
    {
        var items = await _basePurposeRep.AsQueryable()
             .InnerJoin<BasePurposeDetailEntity>((a, b) => a.PurCode == b.PurCode && b.IsValid && !b.IsDeleted)
             .InnerJoin<BaseInstrumentItemEntity>((a, b, c) => b.InstrumentItemCode == c.InstrumentItemCode && c.IsValid && !c.IsDeleted)
             .InnerJoin<BaseItemEntity>((a, b, c, d) => d.ItemCode == b.ItemCode && d.IsValid && !d.IsDeleted)
             .LeftJoin<BaseItemPersonalizeEntity>((a, b, c, d, e) => e.ItemCode == d.ItemCode && e.IsValid && !e.IsDeleted)
             .Where((a, b) => a.IsValid && !a.IsDeleted && purCodes.Contains(a.PurCode))
             .Select((a, b, c, d, e) =>
                new InstrumentItemInfoDto
                {
                    InstrumentItemCode = b.InstrumentItemCode,
                    ItemCode = d.ItemCode,
                    ItemName = d.ItemName,
                    ItemEN = d.ItemNameEN,
                    ItemAB = d.ItemNameAB,
                    ItemUnit = e.ItemUnit,
                    MethodCode = e.MethodCode,
                    IsReportShow = e.IsReportShow,
                    ReportOrder = c.PrintOrder,
                    ItemReportOrder = d.Sort.ToString(),
                    MethodBasis = e.MethodBasis,
                    ResultType = d.ResultType,
                    IsCalculate = e.IsCalculcate,
                    CalcExpression = e.CalcExpression,
                    DefaultValue = string.IsNullOrWhiteSpace(b.DefaultValue) ? e.DefaultValue : b.DefaultValue
                })
             .ToListAsync();

        //var items = await _basePurposeRep.AsQueryable()
        //     .InnerJoin<BasePurposeDetailEntity>((a, b) => a.PurCode == b.PurCode && b.IsValid && !b.IsDeleted)
        //     .InnerJoin<BaseInstrumentItemEntity>((a, b, c) => b.InstrumentItemCode == c.InstrumentItemCode && c.IsValid && !c.IsDeleted)
        //     .InnerJoin<BaseInstrumentItemDetailEntity>((a, b, c, d) => d.InstrumentItemCode == c.InstrumentItemCode && d.IsValid && !d.IsDeleted)
        //     .InnerJoin<BaseItemEntity>((a, b, c, d, e) => e.ItemCode == d.ItemCode && e.IsValid && !e.IsDeleted)
        //     .LeftJoin<BaseItemPersonalizeEntity>((a, b, c, d, e, f) => e.ItemCode == f.ItemCode && f.IsValid && !f.IsDeleted)
        //     .Where((a, b) => a.IsValid && !a.IsDeleted && purCodes.Contains(a.PurCode))
        //     .Select((a, b, c, d, e, f) =>
        //        new InstrumentItemInfoDto
        //        {
        //            InstrumentItemCode = b.InstrumentItemCode,
        //            ItemCode = e.ItemCode,
        //            ItemName = e.ItemName,
        //            ItemEN = e.ItemNameEN,
        //            ItemAB = e.ItemNameAB,
        //            ItemUnit = f.ItemUnit,
        //            MethodCode = f.MethodCode,
        //            IsReportShow = f.IsReportShow,
        //            ReportOrder = c.PrintOrder,
        //            ItemReportOrder = d.Sort.ToString(),
        //            MethodBasis = f.MethodBasis,
        //            ResultType = e.ResultType,
        //            IsCalculate = f.IsCalculcate,
        //            CalcExpression = f.CalcExpression,
        //            DefaultValue = string.IsNullOrWhiteSpace(b.DefaultValue) ? f.DefaultValue : b.DefaultValue
        //        })
        //     .ToListAsync();
        return items;
    }
}