using AngleSharp;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolGood.Words.Pinyin;
using Yitter.IdGenerator;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Region;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;

namespace DaLang.Lims.Web.Framework.Services.Region;

/// <summary>
/// 地区服务
/// </summary>
[Order(220)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class RegionService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<RegionEntity> _regionRep;

    public RegionService(AdminRepositoryBase<RegionEntity> regionRep)
    {
        _regionRep = regionRep;
    }

    [NonAction]
    public async Task<List<long>> GetParentIdListAsync(long id)
    {
        return await _regionRep.AsQueryable()
        .Where(a => a.Id == id)
        //.AsTreeCte(up: true)
        .Select(a => a.Id)
        .ToListAsync();
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RegionGetOutput> GetAsync(long id)
    {
        var ouput = await _regionRep.AsQueryable()
        .Where(a => a.Id == id)
        .Select<RegionGetOutput>()
        .FirstAsync();

        ouput.ParentIdList = await GetParentIdListAsync(ouput.ParentId);

        return ouput;
    }

    /// <summary>
    /// 查询下级列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<RegionGetChildListOutput>> GetChildListAsync(RegionGetListInput input)
    {
        return await _regionRep.AsQueryable()
        .Where(a => a.ParentId == input.ParentId)
        .WhereIF(input.Hot.HasValue, a => a.Hot == input.Hot.Value)
        .WhereIF(input.IsValid.HasValue, a => a.IsValid == input.IsValid.Value)
        .OrderBy(a => a.Level)
        .OrderByDescending(a => a.Hot)
        .OrderBy(a => new { a.Sort, a.Code })
        .Select<RegionGetChildListOutput>()
        .ToListAsync();
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<RegionGetPageOutput>> GetPageAsync(PageInput<RegionGetPageInput> input)
    {
        var filter = input.Filter;

        var list = await _regionRep.AsQueryable()
        .WhereIF(filter != null && filter.Name.NotNull(), a => a.Name.Contains(filter.Name))
        .WhereIF(filter != null && filter.ParentId.HasValue, a => a.ParentId == filter.ParentId.Value)
        .WhereIF(filter != null && filter.Hot.HasValue, a => a.Hot == filter.Hot.Value)
        .WhereIF(filter != null && filter.Level.HasValue, a => a.Level == filter.Level.Value)
        .WhereIF(filter != null && filter.IsValid.HasValue, a => a.IsValid == filter.IsValid.Value)
        .Select<RegionGetPageOutput>()
        .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<RegionGetPageOutput>()
        {
            List = list.Items.ToList(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(RegionAddInput input)
    {
        if (await _regionRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此地区名已存在");
        }

        if (input.Code.NotNull() && await _regionRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此地区代码已存在");
        }

        var entity = Mapper.Map<RegionEntity>(input);
        entity.Pinyin = WordsHelper.GetPinyin(entity.Name);
        entity.PinyinFirst = WordsHelper.GetFirstPinyin(entity.Name);

        await _regionRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(RegionUpdateInput input)
    {
        var entity = await _regionRep.GetAsync(input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception("地区不存在");
        }

        if (await _regionRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此地区名已存在");
        }

        if (input.Code.NotNull() && await _regionRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此地区代码已存在");
        }

        Mapper.Map(input, entity);
        await _regionRep.UpdateAsync(entity);
    }

    [NonAction]
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await _regionRep.AsQueryable()
        .Where(a => a.Id == id)
        //.AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        var idList = await GetChildIdListAsync(id);
        await _regionRep.AsUpdateable().Where(a => idList.Contains(a.Id)).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(RegionSetEnableInput input)
    {
        var entity = await _regionRep.GetAsync(input.RegionId);
        entity.IsValid = input.IsValid;
        await _regionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 设置热门
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetHotAsync(RegionSetHotInput input)
    {
        var entity = await _regionRep.GetAsync(input.RegionId);
        entity.Hot = input.Hot;
        await _regionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 同步地区
    /// </summary>
    /// <param name="region"></param>
    /// <param name="selectors"></param>
    /// <param name="regionLevel"></param>
    /// <param name="url"></param>
    /// <param name="splitUrl"></param>
    /// <returns></returns>
    private async Task SyncRegionAsync(RegionEntity region,
        string selectors,
        RegionLevel regionLevel,
        string url = "",
        string splitUrl = "")
    {
        var isProvice = regionLevel == RegionLevel.Province;

        if (url.IsNull())
        {
            url = isProvice ? "http://www.stats.gov.cn/sj/tjbz/tjyqhdmhcxhfdm/2023/index.html" : "http://www.stats.gov.cn/sj/tjbz/tjyqhdmhcxhfdm/2023/";
        }

        if (splitUrl.IsNull())
            splitUrl = "tjyqhdmhcxhfdm/2023/";

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(url + region.Url);

        var elementList = document.QuerySelectorAll(selectors);
        var dataList = new List<RegionEntity>();
        int increment = isProvice ? 1 : 2;
        int index = isProvice ? 0 : 1;
        if (regionLevel == RegionLevel.Vilage)
        {
            increment = 3;
            index = 2;
        }
        for (var i = 0; i < elementList.Length; i += increment)
        {
            var element = elementList[i + index] as IHtmlAnchorElement;
            dataList.Add(new RegionEntity()
            {
                Id = YitIdHelper.NextId(),
                ParentId = region.Id,
                Name = element.TextContent,
                Level = regionLevel,
                Code = isProvice ? element.Href.Split(splitUrl).Last().Replace(".html", "") : elementList[i].TextContent,
                Url = element.Href.Split(splitUrl).Last(),
                Pinyin = WordsHelper.GetPinyin(element.TextContent),
                PinyinFirst = WordsHelper.GetFirstPinyin(element.TextContent),
            });
        }

        var codeList = dataList.Select(a => a.Code).ToList();
        var existsDataList = await _regionRep.AsQueryable().Where(a => a.ParentId == region.Id && codeList.Contains(a.Code)).ToListAsync();
        var existsDataCodeList = existsDataList.Select(a => a.Code).ToList();
        var insertDataList = dataList.Where(a => !existsDataCodeList.Contains(a.Code));

        if (insertDataList.Any())
        {
            await _regionRep.InsertRangeAsync(insertDataList.ToList());
        }

        if (existsDataList.Any())
        {
            foreach (var item in existsDataList)
            {
                var updateItem = dataList.Where(a => a.Code == item.Code).First();
                item.Name = updateItem.Name;
                item.Url = updateItem.Url;
                item.Pinyin = updateItem.Pinyin;
            }

            await _regionRep.UpdateRangeAsync(existsDataList);
        }
    }

    /// <summary>
    /// 同步数据
    /// </summary>
    /// <param name="regionLevel">地区级别</param>
    /// <returns></returns>
    public async Task SyncDataAsync(RegionLevel regionLevel = RegionLevel.City)
    {
        //同步省份
        await SyncRegionAsync(new RegionEntity
        {
            Id = 0,
            Url = "",
        }, "table.provincetable tr.provincetr td a", RegionLevel.Province);
        WordsHelper.ClearCache();

        if (regionLevel == RegionLevel.Province)
        {
            return;
        }

        var provinceList = await _regionRep.AsQueryable().Where(a => a.Level == RegionLevel.Province).ToListAsync();
        foreach (var province in provinceList)
        {
            //同步城市
            if (province.Url.NotNull())
            {
                await SyncRegionAsync(province, "table.citytable tr.citytr td a", RegionLevel.City);
                WordsHelper.ClearCache();
            }

            if (regionLevel == RegionLevel.City)
            {
                continue;
            }

            //同步县/区
            var cityList = await _regionRep.AsQueryable().Where(a => a.Level == RegionLevel.City).ToListAsync();
            foreach (var city in cityList)
            {
                if (city.Url.NotNull())
                {
                    await SyncRegionAsync(city, "table.countytable tr.countytr td a", RegionLevel.County);
                    WordsHelper.ClearCache();
                }

                if (regionLevel == RegionLevel.County)
                {
                    continue;
                }

                //同步镇/乡/街道
                var townList = await _regionRep.AsQueryable().Where(a => a.Level == RegionLevel.Town).ToListAsync();
                foreach (var town in townList)
                {
                    if (town.Url.NotNull())
                    {
                        await SyncRegionAsync(town, "table.towntable tr.towntr td a", RegionLevel.Town);
                        WordsHelper.ClearCache();
                    }

                    if (regionLevel == RegionLevel.Town)
                    {
                        continue;
                    }

                    //同步 村/社区/居委会/村委会
                    var vilageList = await _regionRep.AsQueryable().Where(a => a.Level == RegionLevel.Vilage).ToListAsync();
                    foreach (var vilage in vilageList)
                    {
                        if (vilage.Url.NotNull())
                        {
                            await SyncRegionAsync(vilage, "table.villagetable tr.villagetr td", RegionLevel.Vilage);
                            WordsHelper.ClearCache();
                        }
                    }
                }
            }
        }
    }
}