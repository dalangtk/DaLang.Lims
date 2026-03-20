using DaLang.Lims.BaseData.Contracts.BaseSequence;
using DaLang.Lims.BaseData.Contracts.Sequence.Dto;
using DaLang.Lims.BaseData.Domain.Sequence;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.BaseSequence;

/// <summary>
/// 序列服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseSequenceService : BaseService, IBaseSequenceService, IDynamicApi
{
    private IBaseSequenceRepository _baseSequenceRep;

    public BaseSequenceService(IBaseSequenceRepository baseSequenceRep)
    {
        _baseSequenceRep = baseSequenceRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseSequenceDto> GetAsync(long id)
    {
        var output = await _baseSequenceRep.GetAsync(id);
        return output.Adapt<BaseSequenceDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<BaseSequenceGetListDto>> GetListAsync(BaseSequenceQueryInput input)
    {
        var list = await _baseSequenceRep.AsQueryable()
            .OrderByDescending(a => a.Id)
            .Select<BaseSequenceGetListDto>()
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseSequenceGetListDto>> GetPageAsync(PageInput<BaseSequenceQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseSequenceRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrWhiteSpace(filter?.SequenceCode), c => c.SequenceCode!.Contains(filter!.SequenceCode!) || c.SequenceName!.Contains(filter.SequenceCode!))
            .OrderBy(c => c.Sort)
            .Select<BaseSequenceGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        var data = new PageOutput<BaseSequenceGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseSequenceDto input)
    {
        var entity = Mapper.Map<BaseSequenceEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseSequenceRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseSequenceRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseSequenceDto input)
    {
        var entity = await _baseSequenceRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("序列不存在！");

        Mapper.Map(input, entity);
        await _baseSequenceRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseSequenceRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}