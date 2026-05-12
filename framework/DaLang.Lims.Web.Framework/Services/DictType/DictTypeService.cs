using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Dict;
using DaLang.Lims.Web.Framework.Domain.DictType;
using DaLang.Lims.Web.Framework.Domain.DictType.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services.DictType.Dto;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.DictType;

/// <summary>
/// 数据字典类型服务
/// </summary>
[Order(61)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DictTypeService : BaseService, IDictTypeService, IDynamicApi
{
    private readonly AdminRepositoryBase<DictTypeEntity> _dictTypeRep;
    private readonly AdminRepositoryBase<DictEntity> _dictRep;
    private ICacheTool _cache;

    public DictTypeService(AdminRepositoryBase<DictTypeEntity> dictTypeRep,
        AdminRepositoryBase<DictEntity> dictRep,
        ICacheTool cache)
    {
        _dictTypeRep = dictTypeRep;
        _dictRep = dictRep;
        _cache = cache;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DictTypeGetOutput> GetAsync(long id)
    {
        var ret = await _dictTypeRep.GetAsync(id);
        var result = ret.Adapt<DictTypeGetOutput>();
        return result;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<DictTypeGetPageOutput>> GetPageAsync(PageInput<DictTypeGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _dictTypeRep.AsQueryable()
        .WhereIF(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
        .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<DictTypeGetPageOutput>()
        {
            List = list.Items.Adapt<List<DictTypeGetPageOutput>>(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(DictTypeAddInput input)
    {
        if (await _dictTypeRep.AsQueryable().AnyAsync(a => a.Name == input.Name))
        {
            throw ResultOutput.Exception($"字典类型已存在");
        }

        if (input.Code.NotNull() && await _dictTypeRep.AsQueryable().AnyAsync(a => a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典类型编码已存在");
        }

        var entity = Mapper.Map<DictTypeEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _dictTypeRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _dictTypeRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(DictTypeUpdateInput input)
    {
        var entity = await _dictTypeRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("数据字典不存在");
        }

        if (await _dictTypeRep.AsQueryable().AnyAsync(a => a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"字典类型已存在");
        }

        if (input.Code.NotNull() && await _dictTypeRep.AsQueryable().AnyAsync(a => a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"字典类型编码已存在");
        }

        Mapper.Map(input, entity);
        await _dictTypeRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var dictList = await _dictRep.AsQueryable().Where(a => a.DictTypeId == id).ToListAsync();
        var dictType = await _dictTypeRep.GetAsync(id);
        dictList.ForEach(a => a.IsDeleted = true);
        dictType.IsDeleted = true;
        await _dictRep.UpdateRangeAsync(dictList);
        await _dictTypeRep.UpdateAsync(dictType);

        await _cache.DelAsync(CacheKeys.SystemDict + dictType.Code);
    }
}