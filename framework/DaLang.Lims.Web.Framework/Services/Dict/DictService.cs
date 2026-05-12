using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Dict;
using DaLang.Lims.Web.Framework.Domain.Dict.Dto;
using DaLang.Lims.Web.Framework.Domain.DictType;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services.Dict.Dto;
using Magicodes.ExporterAndImporter.Excel;
using Magicodes.ExporterAndImporter.Excel.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.Dict;

/// <summary>
/// 数据字典服务
/// </summary>
[Order(60)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictService : BaseService, IDictService, IDynamicApi
{
    private readonly AdminRepositoryBase<DictEntity> _dictRep;
    private readonly AdminRepositoryBase<DictTypeEntity> _dictTypeRep;
    private ICacheTool _cache;

    public DictService(AdminRepositoryBase<DictEntity> dictRep, ICacheTool cache, AdminRepositoryBase<DictTypeEntity> dictTypeRep)
    {
        _dictRep = dictRep;
        _dictTypeRep = dictTypeRep;
        _cache = cache;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictGetOutput> GetAsync(long id)
    {
        var ret = await _dictRep.GetAsync(id);
        var result = ret.Adapt<DictGetOutput>();
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DictGetPageOutput>> GetPageAsync(PageInput<DictGetPageDto> input)
    {
        var key = input.Filter?.Name;
        var dictTypeId = input.Filter?.DictTypeId;
        var list = await _dictRep.AsQueryable()
        .WhereIF(dictTypeId.HasValue && dictTypeId.Value > 0, a => a.DictTypeId == dictTypeId)
        .WhereIF(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<DictGetPageOutput>()
        {
            List = list.Items.Adapt<List<DictGetPageOutput>>(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="codes">字典类型编码列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListDto>>> GetListAsync(string[] codes)
    {
        var codeList = codes.ToList();
        var list = await _dictRep.AsQueryable()
            .InnerJoin<DictTypeEntity>((a, b) => codeList.Contains(b.Code) && a.IsValid == true && a.DictTypeId == b.Id)
        //.Where(a => codes.Contains(a.DictType.Code) && a.DictType.IsValid == true && a.IsValid == true)
        .OrderBy(a => a.Sort)
        .Select((a, b) => new DictGetListDto { DictTypeCode = b.Code }, true)
        .ToListAsync();

        var dicts = new Dictionary<string, List<DictGetListDto>>();
        foreach (var code in codes)
        {
            if (code.NotNull())
                dicts[code] = list.FindAll(a => a.DictTypeCode == code);
        }

        return dicts;
    }

    /// <summary>
    /// 查询字典类型字典列表
    /// </summary>
    /// <param name="names">字典类型名称列表</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<Dictionary<string, List<DictGetListDto>>> GetListByNamesAsync(string[] names)
    {
        var nameList = names.ToList();
        var list = await _dictRep.AsQueryable()
            .InnerJoin<DictTypeEntity>((a, b) => nameList.Contains(b.Name) && a.IsValid == true && a.DictTypeId == b.Id)
        //.Where(a => names.Contains(a.DictType.Name) && a.DictType.IsValid == true && a.IsValid == true)
        .OrderBy(a => a.Sort)
        .Select(a => new DictGetListDto { DictTypeName = a.DictType.Name })
        .ToListAsync();

        var dicts = new Dictionary<string, List<DictGetListDto>>();
        foreach (var name in names)
        {
            if (name.NotNull())
                dicts[name] = list.Where(a => a.DictTypeName == name).ToList();
        }

        return dicts;
    }
    /// <summary>
    /// 根据字典类型编码获取字典列表
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<List<DictGetListDto>> GetDictByTypeCodeAsync(string code)
    {
        var keyName = CacheKeys.SystemDict + code;
        var ret = await _cache.GetOrSetAsync(keyName, async () =>
        {
            return await _dictRep.AsQueryable()
                        .Where(a => code.Equals(a.DictType.Code) && a.DictType.IsValid == true && a.IsValid == true)
                        .OrderBy(a => a.Sort)
                        .Select(a => new DictGetListDto
                        {
                            DictTypeName = a.DictType.Name,
                            DictTypeCode = a.DictType.Code
                        }, true)
                        .ToListAsync();
        }, TimeSpan.FromHours(24));

        return ret;
    }

    /// <summary>
    /// 导出列表
    /// </summary>
    /// <returns></returns>
    [NonFormatResult]
    [HttpGet]
    public async Task<ActionResult> ExportListAsync()
    {
        //using var _ = _dictRep.DataFilter.DisableAll();

        var dataList = await _dictRep.AsQueryable().Where(a => 1 == 1).ToListAsync();

        //导出数据
        var result = await new ExcelExporter().Append(dataList).ExportAppendDataAsByteArray();

        return new XlsxFileResult(result, $"数据字典列表{DateTime.Now:yyyyMMddHHmm}.xlsx");
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictAddInput input)
    {
        if (await _dictRep.AsQueryable().AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"字典已存在");
        }

        if (input.Code.NotNull() && await _dictRep.AsQueryable().AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典编码已存在");
        }

        if (input.Code.NotNull() && await _dictRep.AsQueryable().AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典编码已存在");
        }

        if (input.Value.NotNull() && await _dictRep.AsQueryable().AnyAsync(a => a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception($"字典值已存在");
        }

        var entity = Mapper.Map<DictEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _dictRep.AsQueryable().Where(a => a.DictTypeId == input.DictTypeId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictRep.InsertAsync(entity);

        var dictType = await _dictTypeRep.GetFirstAsync(a => a.Id == entity.DictTypeId);
        if (dictType != null)
        {
            var keyName = CacheKeys.SystemDict + dictType.Code;
            var dictList = await _dictRep.GetListAsync(a => a.DictTypeId == dictType.Id && a.IsValid == true);
            await _cache.SetAsync(keyName, dictList, TimeSpan.FromHours(24));
        }

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictUpdateInput input)
    {
        var entity = await _dictRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("字典不存在");
        }

        if (await _dictRep.AsQueryable().AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"字典已存在");
        }

        if (input.Code.NotNull() && await _dictRep.AsQueryable().AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典编码已存在");
        }

        if (input.Value.NotNull() && await _dictRep.AsQueryable().AnyAsync(a => a.Id != input.Id && a.DictTypeId == input.DictTypeId && a.Value == input.Value))
        {
            throw ResultOutput.Exception($"字典值已存在");
        }
        var dictType = await _dictTypeRep.GetFirstAsync(a => a.Id == entity.DictTypeId);
        if (dictType == null)
        {
            throw ResultOutput.Exception($"字典类型不存在");
        }

        Mapper.Map(input, entity);
        var ret = await _dictRep.UpdateAsync(entity);

        if (ret)
        {
            var keyName = CacheKeys.SystemDict + dictType.Code;
            var dictList = await _dictRep.GetListAsync(a => a.DictTypeId == dictType.Id && a.IsValid == true);
            await _cache.SetAsync(keyName, dictList, TimeSpan.FromHours(24));
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        var dict = await _dictRep.GetAsync(id);
        dict.IsDeleted = true;
        var ret = await _dictRep.UpdateAsync(dict);
        if (ret)
        {
            var dictType = await _dictTypeRep.GetFirstAsync(a => a.Id == dict.DictTypeId);
            if (dictType != null)
            {
                var keyName = CacheKeys.SystemDict + dictType.Code;
                var dictList = await _dictRep.GetListAsync(a => a.DictTypeId == dictType.Id && a.IsValid == true);
                await _cache.SetAsync(keyName, dictList, TimeSpan.FromHours(24));
            }
        }
    }
}