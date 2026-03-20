using DaLang.Lims.BaseData.Contracts.Group.Dto;
using DaLang.Lims.BaseData.Domain.Group;
using DaLang.Lims.Web.BaseData.Contracts.Group;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;


namespace DaLang.Lims.Web.BaseData.Services.Group;

/// <summary>
/// 组别服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName, GroupNames = ["basedata"])]
public class BaseGroupService : BaseService, IBaseGroupService, IDynamicApi
{
    private IBaseGroupRepository _baseGroupRep;

    public BaseGroupService(IBaseGroupRepository baseGroupRep)
    {
        _baseGroupRep = baseGroupRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseGroupDto> GetAsync(long id)
    {
        var output = await _baseGroupRep.GetAsync(id);
        return output.Adapt<BaseGroupDto>();
    }
    /// <summary>
    /// 获取所有
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BaseGroupDto>> GetAllAsync()
    {
        var output = await _baseGroupRep.GetListAsync();
        output.Sort((a, b) => a.GroupCode.CompareTo(b.GroupCode));
        return output.Adapt<List<BaseGroupDto>>();
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseGroupGetListDto>> GetPageAsync(PageInput<BaseGroupQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseGroupRep.GetQueryable(dynamicCondition)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.GroupCode), a => a.GroupCode == filter.GroupCode || a.GroupName == filter.GroupCode)
            .OrderBy(c => c.Sort)
            .Select<BaseGroupGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseGroupGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseGroupDto input)
    {
        if (string.IsNullOrWhiteSpace(input?.GroupCode))
            throw ResultOutput.Exception("组别代码不可为空");
        input.GroupCode = input.GroupCode.ToUpper().Trim();
        var isExists = await _baseGroupRep.IsAnyAsync(a => a.GroupCode == input.GroupCode);
        if (isExists)
            throw ResultOutput.Exception($"组别代码{input.GroupCode}已存在！");

        var entity = Mapper.Map<BaseGroupEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _baseGroupRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _baseGroupRep.InsertReturnSnowflakeIdAsync(entity);

        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(BaseGroupDto input)
    {
        var entity = await _baseGroupRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("组别不存在！");
        }

        Mapper.Map(input, entity);
        await _baseGroupRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseGroupRep
            .AsUpdateable()
            .SetColumns(a => a.IsDeleted == true, true)
            .SetUpdateable()
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
}