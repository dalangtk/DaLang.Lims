using Mapster;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.Role;
using DaLang.Lims.Web.Framework.Domain.RoleOrg;
using DaLang.Lims.Web.Framework.Domain.UserOrg;
using DaLang.Lims.Web.Framework.Resources;
using DaLang.Lims.Web.Framework.Services.Org.Input;
using DaLang.Lims.Web.Framework.Services.Org.Output;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;

namespace DaLang.Lims.Web.Framework.Services.Org;

/// <summary>
/// 部门服务
/// </summary>
[Order(30)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class OrgService : BaseService, IOrgService, IDynamicApi
{
    private readonly IOrgRepository _orgRep;
    private readonly IUserOrgRepository _userOrgRep;
    private readonly IRoleOrgRepository _roleOrgRep;
    private readonly IStringLocalizer<AdminLocalizer> _localizer;

    public OrgService(
        IOrgRepository orgRep,
        IUserOrgRepository userOrgRep,
        IRoleOrgRepository roleOrgRep,
        IStringLocalizer<AdminLocalizer> localizer
    )
    {
        _orgRep = orgRep;
        _userOrgRep = userOrgRep;
        _roleOrgRep = roleOrgRep;
        _localizer = localizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrgGetOutput> GetAsync(long id)
    {
        var ret = await _orgRep.GetAsync(id);
        var result = ret.Adapt<OrgGetOutput>();
        return result;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<OrgListOutput>> GetListAsync(string key)
    {
        var dataPermission = User.DataPermission;
        var hasOrg = dataPermission.OrgIds.Count > 0;

        var select = _orgRep.AsQueryable()
            .WhereIF(hasOrg, a => dataPermission.OrgIds.Contains(a.Id))
            .WhereIF(dataPermission.DataScope == DataScope.Self, a => a.ProId == User.Id)
            .WhereIF(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key));

        if (hasOrg)
        {
            //select = select.AsTreeCte(up: true);
        }

        var data = await select
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .Select<OrgListOutput>()
            .ToListAsync();

        return hasOrg ? data.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList() : data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(OrgAddInput input)
    {
        if (input.ParentId == 0)
        {
            throw ResultOutput.Exception(_localizer["请选择上级部门"]);
        }

        if (await _orgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_localizer["此部门已存在"]);
        }

        if (input.Code.NotNull() && await _orgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_localizer["此部门编码已存在"]);
        }

        var entity = Mapper.Map<OrgEntity>(input);

        if (entity.Sort == 0)
        {
            var sort = await _orgRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _orgRep.InsertAsync(entity);
        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(OrgUpdateInput input)
    {
        if (input.ParentId == 0)
        {
            throw ResultOutput.Exception(_localizer["请选择上级部门"]);
        }

        var entity = await _orgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_localizer["部门不存在"]);
        }

        if (input.Id == input.ParentId)
        {
            throw ResultOutput.Exception(_localizer["上级部门不能是本部门"]);
        }

        if (await _orgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_localizer["此部门已存在"]);
        }

        if (input.Code.NotNull() && await _orgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_localizer["此部门编码已存在"]);
        }

        var childIdList = await _orgRep.GetChildIdListAsync(input.Id);
        if (childIdList.Contains(input.ParentId))
        {
            throw ResultOutput.Exception(_localizer["上级部门不能是下级部门"]);
        }

        Mapper.Map(input, entity);
        await _orgRep.UpdateAsync(entity);

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task DeleteAsync(long id)
    {
        //本部门下是否有员工
        if (await _userOrgRep.HasUser(id))
        {
            throw ResultOutput.Exception(_localizer["当前部门有员工无法删除"]);
        }

        var orgIdList = await _orgRep.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRep.HasUser(orgIdList))
        {
            throw ResultOutput.Exception(_localizer["本部门的下级部门有员工无法删除"]);
        }

        //删除部门角色
        await _roleOrgRep.DeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRep.DeleteAsync(a => orgIdList.Contains(a.Id));

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task SoftDeleteAsync(long id)
    {
        //本部门下是否有员工
        if (await _userOrgRep.HasUser(id))
        {
            throw ResultOutput.Exception(_localizer["当前部门有员工无法删除"]);
        }

        var orgIdList = await _orgRep.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRep.HasUser(orgIdList))
        {
            throw ResultOutput.Exception(_localizer["本部门的下级部门有员工无法删除"]);
        }

        //删除部门角色

        var roleOrgs = await _roleOrgRep.GetListAsync(a => orgIdList.Contains(a.OrgId));
        roleOrgs.ForEach(a => a.IsDeleted = true);
        await _roleOrgRep.UpdateRangeAsync(roleOrgs);

        var orgs = await _orgRep.GetListAsync(a => orgIdList.Contains(a.Id));
        orgs.ForEach(a => a.IsDeleted = true);
        await _orgRep.UpdateRangeAsync(orgs);

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }
}