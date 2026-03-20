using AspectCore.DynamicProxy.Parameters;
using DaLang.Lims.BaseData.Contracts.Item.Dto;
using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.Web.BaseData.Contracts.Item;
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

namespace DaLang.Lims.Web.BaseData.Services.Item;

/// <summary>
/// 基础项目服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName, GroupNames = ["basedata"])]
public class BaseItemService : BaseService, IBaseItemService, IDynamicApi
{
    private IBaseItemRepository _baseItemRep;
    private IBaseItemPersonalizeRepository _baseItemPersonalRep;

    public BaseItemService(IBaseItemRepository baseItemRep, IBaseItemPersonalizeRepository baseItemPersonalRep)
    {
        _baseItemRep = baseItemRep;
        _baseItemPersonalRep = baseItemPersonalRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseItemWithPersonDto> GetAsync(long id)
    {
        var itemWithPersonal = new BaseItemWithPersonDto();
        var output = await _baseItemRep.AsQueryable()
            .Where(a => a.Id == id)
            .Includes(a => a.ItemPersonal)
            .FirstAsync();
        if (output == null || output.Id == 0)
            throw new Exception("未找到数据，请刷新后查看！");
        itemWithPersonal.BaseItem = output.Adapt<BaseItemDto>();
        itemWithPersonal.BaseItemPersonal = output.ItemPersonal.Adapt<BaseItemPersonalizeDto>() ?? new BaseItemPersonalizeDto();

        return itemWithPersonal;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseItemGetListDto>> GetPageAsync(PageInput<BaseItemQueryInput> input)
    {
        var filter = input.Filter;
        string itemCode = !string.IsNullOrWhiteSpace(filter.ItemCode) ? filter.ItemCode.ToUpper() : "";

        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseItemRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.GroupCode), a => a.GroupCode == filter.GroupCode)
            .WhereIF(!string.IsNullOrWhiteSpace(itemCode),
            a => a.ItemCode.ToUpper() == itemCode
            || a.ItemName.ToUpper().Contains(itemCode)
            || a.ItemNameAB.ToUpper().Contains(itemCode)
            || a.ItemNameEN.ToUpper().Contains(itemCode))
            .OrderBy(c => c.Sort)
            .Select<BaseItemGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseItemGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<long> AddAsync([NotNull] BaseItemWithPersonDto input)
    {
        if (input == null)
            throw ResultOutput.Exception("参数有误");

        var baseItem = input.BaseItem;
        var itemPersonal = input.BaseItemPersonal;

        if (string.IsNullOrWhiteSpace(baseItem?.ItemCode))
            throw ResultOutput.Exception("项目代码不可为空");
        baseItem.ItemCode = baseItem.ItemCode.ToUpper().Trim();
        var isExists = await _baseItemRep.IsAnyAsync(a => a.ItemCode == baseItem.ItemCode);
        if (isExists)
            throw ResultOutput.Exception($"项目代码{baseItem.ItemCode}已存在！");
        var entity = Mapper.Map<BaseItemEntity>(baseItem);
        if (entity.Sort == 0)
        {
            var sort = await _baseItemRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseItemRep.InsertReturnSnowflakeIdAsync(entity);
        var personalEntity = Mapper.Map<BaseItemPersonalizeEntity>(itemPersonal);
        await _baseItemPersonalRep.InsertAsync(personalEntity);

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync([NotNull] BaseItemWithPersonDto input)
    {
        if (input == null)
            throw ResultOutput.Exception("参数有误");

        var baseItem = input.BaseItem;
        var itemPersonal = input.BaseItemPersonal;

        var entity = await _baseItemRep.GetAsync(baseItem.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("基础数据不存在！");

        Mapper.Map(baseItem, entity);
        await _baseItemRep.UpdateAsync(entity);

        if (itemPersonal.Id <= 0)
        {
            var personalEntity = Mapper.Map<BaseItemPersonalizeEntity>(itemPersonal);
            await _baseItemPersonalRep.InsertAsync(personalEntity);
            //throw ResultOutput.Exception("个性化数据不存在！");
        }
        else
        {
            var entityPersonal = await _baseItemPersonalRep.GetAsync(itemPersonal.Id);
            Mapper.Map(itemPersonal, entityPersonal);
            await _baseItemPersonalRep.UpdateAsync(entityPersonal);
        }

    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseItemRep
            .AsUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}